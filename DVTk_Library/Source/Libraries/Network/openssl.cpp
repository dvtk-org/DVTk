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
//  DESCRIPTION     :	Class to provide the interface to the OpenSSL library
//						Loads and maintains the function pointers into the OpenSSL DLL.
//						Also has mapping functions to each of the used functions.
//*****************************************************************************


//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#define _CRT_RAND_S

#include <stdlib.h>
#include <stdio.h>
#include <limits.h>

#include <iostream>
#include <fstream>

#include <sstream>
#include "openssl.h"

//*****************************************************************************
//  MACROS 
//	These macros are used to define each of the function pointers used and to
//	define the wrapper function used to call into the OpenSSL DLLs.
//*****************************************************************************

// function with no arguments
#define ARG0(rvType, functionName)				\
typedef rvType (*functionName##_TYPE)(void);	\
functionName##_TYPE functionName##_FP = NULL;	\
rvType functionName()							\
{												\
	return functionName##_FP();					\
}

// function with no arguments, no return value
#define ARG0void(functionName)					\
typedef void (*functionName##_TYPE)(void);		\
functionName##_TYPE functionName##_FP = NULL;	\
void functionName()								\
{												\
	functionName##_FP();						\
}

// function with 1 argument
#define ARG1(rvType, functionName, arg1Type)	\
typedef rvType (*functionName##_TYPE)(arg1Type);\
functionName##_TYPE functionName##_FP = NULL;	\
rvType functionName(arg1Type a)					\
{												\
	return functionName##_FP(a);				\
}

// function with 1 argument, no return value
#define ARG1void(functionName, arg1Type)		\
typedef void (*functionName##_TYPE)(arg1Type);	\
functionName##_TYPE functionName##_FP = NULL;	\
void functionName(arg1Type a)					\
{												\
	functionName##_FP(a);						\
}

// function with 2 arguments
#define ARG2(rvType, functionName, arg1Type, arg2Type)		\
typedef rvType (*functionName##_TYPE)(arg1Type, arg2Type);	\
functionName##_TYPE functionName##_FP = NULL;				\
rvType functionName(arg1Type a, arg2Type b)					\
{															\
	return functionName##_FP(a,b);							\
}

// function with 2 arguments, no return value
#define ARG2void(functionName, arg1Type, arg2Type)		\
typedef void (*functionName##_TYPE)(arg1Type, arg2Type);\
functionName##_TYPE functionName##_FP = NULL;			\
void functionName(arg1Type a, arg2Type b)				\
{														\
	functionName##_FP(a,b);								\
}

// function with 3 arguments
#define ARG3(rvType, functionName, arg1Type, arg2Type, arg3Type)	\
typedef rvType (*functionName##_TYPE)(arg1Type, arg2Type, arg3Type);\
functionName##_TYPE functionName##_FP = NULL;						\
rvType functionName(arg1Type a, arg2Type b, arg3Type c)				\
{																	\
	return functionName##_FP(a,b,c);								\
}

// function with 3 arguments, no return value
#define ARG3void(functionName, arg1Type, arg2Type, arg3Type)		\
typedef void (*functionName##_TYPE)(arg1Type, arg2Type, arg3Type);	\
functionName##_TYPE functionName##_FP = NULL;						\
void functionName(arg1Type a, arg2Type b, arg3Type c)				\
{																	\
	functionName##_FP(a,b,c);										\
}

// function with 4 arguments
#define ARG4(rvType, functionName, arg1Type, arg2Type, arg3Type, arg4Type)		\
typedef rvType (*functionName##_TYPE)(arg1Type, arg2Type, arg3Type, arg4Type);	\
functionName##_TYPE functionName##_FP = NULL;									\
rvType functionName(arg1Type a, arg2Type b, arg3Type c, arg4Type d)				\
{																				\
	return functionName##_FP(a,b,c,d);											\
}

// function with 4 arguments, no return value
#define ARG4void(functionName, arg1Type, arg2Type, arg3Type, arg4Type)		\
typedef void (*functionName##_TYPE)(arg1Type, arg2Type, arg3Type, arg4Type);\
functionName##_TYPE functionName##_FP = NULL;								\
void functionName(arg1Type a, arg2Type b, arg3Type c, arg4Type d)			\
{																			\
	functionName##_FP(a,b,c,d);												\
}

// function with 5 arguments
#define ARG5(rvType, functionName, arg1Type, arg2Type, arg3Type, arg4Type, arg5Type)		\
typedef rvType (*functionName##_TYPE)(arg1Type, arg2Type, arg3Type, arg4Type, arg5Type);	\
functionName##_TYPE functionName##_FP = NULL;												\
rvType functionName(arg1Type a, arg2Type b, arg3Type c, arg4Type d, arg5Type e)				\
{																							\
	return functionName##_FP(a,b,c,d, e);													\
}

// function with 5 arguments, no return value
#define ARG5void(functionName, arg1Type, arg2Type, arg3Type, arg4Type, arg5Type)		\
typedef void (*functionName##_TYPE)(arg1Type, arg2Type, arg3Type, arg4Type, arg5Type);	\
functionName##_TYPE functionName##_FP = NULL;											\
void functionName(arg1Type a, arg2Type b, arg3Type c, arg4Type d, arg5Type e)			\
{																						\
	functionName##_FP(a,b,c,d,e);														\
}

// function with 7 arguments
#define ARG7(rvType, functionName, arg1Type, arg2Type, arg3Type, arg4Type, arg5Type, arg6Type, arg7Type)		\
typedef rvType (*functionName##_TYPE)(arg1Type, arg2Type, arg3Type, arg4Type, arg5Type, arg6Type, arg7Type);	\
functionName##_TYPE functionName##_FP = NULL;																	\
rvType functionName(arg1Type a, arg2Type b, arg3Type c, arg4Type d, arg5Type e, arg6Type f, arg7Type g)			\
{																												\
	return functionName##_FP(a,b,c,d, e, f, g);																	\
}



//*****************************************************************************
//  Function Pointer Defintions and Wrapper Functions
//
//	To add a new function, add the function below using the correct macro (based
//	on the number of function arguments).
//	After adding the function below, uncomment the GetProcAddress line in the
//	appropriate load lib method below.
//*****************************************************************************
ARG3(char *, SSL_CIPHER_description, const SSL_CIPHER *, char *, int)
ARG1(const char *, SSL_CIPHER_get_name, SSL_CIPHER *)

ARG4(long, SSL_CTX_ctrl, SSL_CTX *, int, long, void *)
ARG1void(SSL_CTX_free, SSL_CTX *)
ARG3(int, SSL_CTX_load_verify_locations, SSL_CTX *, const char *,const char *)
ARG1(SSL_CTX *, SSL_CTX_new, const SSL_METHOD *)
ARG2(int, SSL_CTX_set_cipher_list, SSL_CTX *, const char *)
ARG2void(SSL_CTX_set_client_CA_list, SSL_CTX *, STACK_OF(X509_NAME) *)
ARG2void(SSL_CTX_set_default_passwd_cb, SSL_CTX *, pem_password_cb *)
ARG2void(SSL_CTX_set_default_passwd_cb_userdata, SSL_CTX *, void *)
ARG3(int, SSL_CTX_set_session_id_context, SSL_CTX *, const unsigned char *, unsigned int)
ARG2(long, SSL_CTX_set_timeout, SSL_CTX *, long)
ARG2(int, SSL_CTX_use_PrivateKey, SSL_CTX *, EVP_PKEY *)
ARG3(int, SSL_CTX_use_PrivateKey_file, SSL_CTX *, const char *, int)
ARG2(int, SSL_CTX_use_certificate, SSL_CTX *, X509 *)
ARG2(int, SSL_CTX_use_certificate_chain_file, SSL_CTX *, const char *)

ARG1void(SSL_SESSION_free, SSL_SESSION *)

ARG1(int, SSL_accept, SSL *)
ARG1(int, SSL_connect, SSL *)
ARG4(long, SSL_ctrl, SSL *, int, long, void *)
ARG1void(SSL_free, SSL *)
ARG1(SSL_SESSION *, SSL_get1_session, SSL *)
ARG1(const SSL_CIPHER *, SSL_get_current_cipher, const SSL *)
ARG2(int, SSL_get_error, const SSL *, int)
ARG1(X509 *, SSL_get_peer_certificate, const SSL *)
ARG1(int, SSL_get_rfd, const SSL *)
ARG1(long, SSL_get_verify_result, const SSL *)
ARG1(const char *, SSL_get_version, const SSL *)
ARG1(int, SSL_get_wfd, const SSL *)
ARG0(int, SSL_library_init)
ARG1(STACK_OF(X509_NAME) *, SSL_load_client_CA_file, const char *)
ARG0void(SSL_load_error_strings)
ARG1(SSL *, SSL_new, SSL_CTX *)
ARG1(int, SSL_pending, const SSL *)
ARG3(int, SSL_read, SSL *, void *, int)
ARG1void(SSL_set_accept_state, SSL *)
ARG3void(SSL_set_bio, SSL *, BIO *, BIO *)
ARG2(int, SSL_set_session, SSL *, SSL_SESSION *)
ARG1(int, SSL_shutdown, SSL *)
ARG3(int, SSL_write, SSL *,const void *, int)


ARG0(const SSL_METHOD *, SSLv23_server_method)

ARG0(const SSL_METHOD *, SSLv23_method)
ARG0(const SSL_METHOD *, SSLv3_method)
ARG0(const SSL_METHOD *, TLSv1_method)
ARG0(const SSL_METHOD *, TLSv1_1_method)
ARG0(const SSL_METHOD *, TLSv1_2_method)

ARG1(long, ASN1_INTEGER_get, const ASN1_INTEGER *)
ARG2(int, ASN1_INTEGER_set, ASN1_INTEGER *, long)
ARG1void(ASN1_TYPE_free, ASN1_TYPE*)
ARG4(ASN1_VALUE *, ASN1_item_d2i, ASN1_VALUE **, const unsigned char **, long, const ASN1_ITEM *)
ARG3(void *, ASN1_item_d2i_bio, const ASN1_ITEM *, BIO *, void *)

ARG4(long, BIO_ctrl, BIO *, int, long, void *)
ARG1(int, BIO_free, BIO *)
ARG1(BIO *, BIO_new, BIO_METHOD *)
ARG1(BIO *, BIO_new_accept, char *)
ARG1(BIO *, BIO_new_connect, char *)
ARG1(BIO *, BIO_pop, BIO *)
ARG0(BIO_METHOD *, BIO_s_file)

ARG3(BIGNUM * ,BN_bin2bn, const unsigned char *, int, BIGNUM *)
ARG1(int, BN_num_bits, const BIGNUM *)

ARG5(int, CRYPTO_add_lock, int *, int, int, const char *, int)
ARG1void(CRYPTO_free, void *)
ARG0(int, CRYPTO_num_locks)

ARG1void(DH_free, DH *)
ARG0(DH *, DH_new)

ARG1void(DSA_free, DSA *)
ARG1(int, DSA_generate_key, DSA *)

ARG0void(ERR_clear_error)
ARG3void(ERR_error_string_n, unsigned long, char *, size_t)
ARG0(unsigned long, ERR_get_error)
ARG4(unsigned long, ERR_get_error_line_data, const char **, int *, const char **, int *)
ARG0(unsigned long, ERR_peek_error)
ARG0(unsigned long, ERR_peek_last_error)
ARG5void(ERR_put_error, int, int, int, const char *, int)
ARG1void(ERR_remove_state, unsigned long)

ARG1(EVP_PKEY *, EVP_PKCS82PKEY, PKCS8_PRIV_KEY_INFO *)
ARG3(int, EVP_PKEY_assign, EVP_PKEY *, int, void *)
ARG1void(EVP_PKEY_free, EVP_PKEY *)
ARG0(EVP_PKEY *, EVP_PKEY_new)
ARG1(int, EVP_PKEY_type, int)
ARG0(const EVP_CIPHER *, EVP_des_ede3_cbc)
ARG1(int, EVP_add_cipher, const EVP_CIPHER *)
ARG0void(EVP_cleanup)
ARG0(const EVP_MD *, EVP_dss1)
ARG0(const EVP_MD *, EVP_sha1)

ARG4(STACK_OF(X509_INFO) *, PEM_X509_INFO_read_bio, BIO *, STACK_OF(X509_INFO) *, pem_password_cb *, void *)
ARG5(int, PEM_do_header, EVP_CIPHER_INFO *, unsigned char *, long *, pem_password_cb *, void *)
ARG2(int, PEM_get_EVP_CIPHER_INFO, char *, EVP_CIPHER_INFO *)
ARG5(int, PEM_read_bio, BIO *, char **, char **, unsigned char **, long *)
ARG4(EVP_PKEY *, PEM_read_bio_PrivateKey, BIO *, EVP_PKEY **, pem_password_cb *, void *)
ARG4(PKCS7 *, PEM_read_bio_PKCS7, BIO *, PKCS7 **, pem_password_cb *, void *)
ARG4(X509 *, PEM_read_bio_X509, BIO *, X509 **, pem_password_cb *, void *)
ARG2(int, PEM_write_bio_X509, BIO *, X509*)
ARG7(int, PEM_write_bio_PKCS8PrivateKey, BIO *, EVP_PKEY *, const EVP_CIPHER *, char *, int, pem_password_cb *, void *)

ARG1void(PKCS12_free, PKCS12*)
ARG5(int, PKCS12_parse, PKCS12 *, const char *, EVP_PKEY **, X509 **, STACK_OF(X509) **)
ARG3(int, PKCS12_verify_mac, PKCS12 *, const char *, int)

ARG1void(PKCS7_free, PKCS7*)

ARG1void(PKCS8_PRIV_KEY_INFO_free, PKCS8_PRIV_KEY_INFO*)
ARG3(PKCS8_PRIV_KEY_INFO *, PKCS8_decrypt, X509_SIG *, const char *, int)

ARG1(int, OBJ_obj2nid, const ASN1_OBJECT *)
ARG1(int, OBJ_sn2nid, const char *)

ARG0void(OPENSSL_add_all_algorithms_noconf)

ARG3void(RAND_add, const void *, int, double)
ARG2void(RAND_seed, const void *,int)
ARG0(int, RAND_status)

// MK: Added following three lines:
ARG2(const char *, RAND_file_name, char *, size_t)
ARG2(int, RAND_load_file, const char *, long)
ARG1(int, RAND_write_file, const char *)


ARG1void(RSA_free, RSA *)

ARG1void(X509_INFO_free, X509_INFO *)
ARG0(X509_INFO *, X509_INFO_new)
ARG5(X509_NAME_ENTRY *, X509_NAME_ENTRY_create_by_NID, X509_NAME_ENTRY **, int, int, unsigned char *, int);
ARG4(int, X509_NAME_add_entry, X509_NAME *, X509_NAME_ENTRY *, int, int)
ARG1void(X509_NAME_free, X509_NAME*)
ARG1(X509_NAME *, X509_NAME_dup, X509_NAME *)
ARG0(X509_NAME*, X509_NAME_new)
ARG3(char *, X509_NAME_oneline, X509_NAME *, char *, int)
ARG0(X509_PKEY *, X509_PKEY_new)
ARG1void(X509_SIG_free, X509_SIG*)
ARG1(X509 *, X509_STORE_CTX_get_current_cert, X509_STORE_CTX *)
ARG1(int, X509_STORE_CTX_get_error, X509_STORE_CTX *)
ARG1(int, X509_STORE_CTX_get_error_depth, X509_STORE_CTX *)
ARG1void(X509_free, X509 *)
ARG1(X509 *, X509_dup, X509 *)
ARG1(X509_NAME *, X509_get_issuer_name, X509 *)
ARG1(EVP_PKEY *, X509_get_pubkey, X509 *)
ARG1(ASN1_INTEGER *, X509_get_serialNumber, X509 *)
ARG1(X509_NAME *, X509_get_subject_name, X509 *)
ARG0(const ASN1_ITEM *, X509_it)
ARG0(X509 *, X509_new)
ARG2(int, X509_set_issuer_name, X509 *, X509_NAME *)
ARG2(int, X509_set_pubkey, X509 *, EVP_PKEY *)
ARG2(int, X509_set_subject_name, X509 *, X509_NAME *)
ARG2(int, X509_set_version, X509 *, long)
ARG3(int, X509_sign, X509 *, EVP_PKEY *, const EVP_MD *)
ARG3(ASN1_TIME *, X509_time_adj, ASN1_TIME *, long, time_t *)
ARG1(const char *, X509_verify_cert_error_string, long )

ARG3(ASN1_TYPE *, d2i_ASN1_TYPE, ASN1_TYPE **, const unsigned char **, long)
//ARG4(ASN1_STRING *, d2i_ASN1_TYPE_BYTES, ASN1_STRING **, const unsigned char **, long, int)

ARG4(EVP_PKEY *, d2i_PrivateKey, int, EVP_PKEY **, const unsigned char **, long)
ARG2(PKCS12 *, d2i_PKCS12_bio, BIO *, PKCS12 **)
ARG2(PKCS7 *, d2i_PKCS7_bio, BIO *, PKCS7 **)
ARG3(PKCS8_PRIV_KEY_INFO*, d2i_PKCS8_PRIV_KEY_INFO, PKCS8_PRIV_KEY_INFO**, const unsigned char **, long)
ARG3(X509 *, d2i_X509, X509 **, unsigned char **, long)
ARG3(X509_SIG *, d2i_X509_SIG, X509_SIG **, const unsigned char **, long)

ARG3(int, i2t_ASN1_OBJECT, char *, int, ASN1_OBJECT *)
  
ARG2(void *, sk_delete, _STACK *, int)
ARG3(int, sk_insert, _STACK *, void *, int)
ARG0(_STACK *, sk_new_null)
ARG1(int, sk_num, const _STACK *)
ARG1(void *, sk_pop, _STACK *)
ARG2(int, sk_push, _STACK *, void *)
ARG1(void *,sk_shift, _STACK *)
ARG2(void *, sk_value, const _STACK *, int)


//
//	Special Cases (function pointers as arguments)
//
typedef void (*SSL_CTX_set_msg_callback_TYPE)(SSL_CTX *, void (*)(int write_p, int version, int content_type, const void *buf, size_t len, SSL *ssl, void *arg));
SSL_CTX_set_msg_callback_TYPE SSL_CTX_set_msg_callback_FP = NULL;
void SSL_CTX_set_msg_callback(SSL_CTX *a, void (*b)(int write_p, int version, int content_type, const void *buf, size_t len, SSL *ssl, void *arg))
{
	SSL_CTX_set_msg_callback_FP(a, b);
}

typedef void (*SSL_CTX_set_tmp_dh_callback_TYPE)(SSL_CTX *, DH *(*)(SSL *ssl, int is_export, int keylength));
SSL_CTX_set_tmp_dh_callback_TYPE SSL_CTX_set_tmp_dh_callback_FP = NULL;
void SSL_CTX_set_tmp_dh_callback(SSL_CTX *a, DH *(*b)(SSL *ssl, int is_export, int keylength))
{
	SSL_CTX_set_tmp_dh_callback_FP(a, b);
}

typedef void (*SSL_CTX_set_verify_TYPE)(SSL_CTX *, int mode, int (*)(int, X509_STORE_CTX *));
SSL_CTX_set_verify_TYPE SSL_CTX_set_verify_FP = NULL;
void SSL_CTX_set_verify(SSL_CTX *a, int b, int (*c)(int, X509_STORE_CTX *))
{
	SSL_CTX_set_verify_FP(a, b, c);
}

typedef char *(*ASN1_d2i_bio_TYPE)(char *(*)(),char *(*)(),BIO *,unsigned char **);
ASN1_d2i_bio_TYPE ASN1_d2i_bio_FP = NULL;
char *ASN1_d2i_bio(char *(*a)(),char *(*b)(),BIO *c,unsigned char **d)
{
	return ASN1_d2i_bio_FP(a,b,c,d);
}

typedef void (*CRYPTO_set_dynlock_create_callback_TYPE)(struct CRYPTO_dynlock_value *(*)(const char *file, int line));
CRYPTO_set_dynlock_create_callback_TYPE CRYPTO_set_dynlock_create_callback_FP = NULL;
void CRYPTO_set_dynlock_create_callback(struct CRYPTO_dynlock_value *(*a)(const char *file, int line))
{
	CRYPTO_set_dynlock_create_callback_FP(a);
}

typedef void (*CRYPTO_set_dynlock_destroy_callback_TYPE)(void (*)(struct CRYPTO_dynlock_value *l, const char *file, int line));
CRYPTO_set_dynlock_destroy_callback_TYPE CRYPTO_set_dynlock_destroy_callback_FP = NULL;
void CRYPTO_set_dynlock_destroy_callback(void (*a)(struct CRYPTO_dynlock_value *l, const char *file, int line))
{
	CRYPTO_set_dynlock_destroy_callback_FP(a);
}

typedef void (*CRYPTO_set_dynlock_lock_callback_TYPE)(void (*)(int mode, struct CRYPTO_dynlock_value *l, const char *file, int line));
CRYPTO_set_dynlock_lock_callback_TYPE CRYPTO_set_dynlock_lock_callback_FP = NULL;
void CRYPTO_set_dynlock_lock_callback(void (*a)(int mode, struct CRYPTO_dynlock_value *l, const char *file, int line))
{
	CRYPTO_set_dynlock_lock_callback_FP(a);
}

typedef void (*CRYPTO_set_id_callback_TYPE)(unsigned long (*)(void));
CRYPTO_set_id_callback_TYPE CRYPTO_set_id_callback_FP = NULL;
void CRYPTO_set_id_callback(unsigned long (*a)(void))
{
	CRYPTO_set_id_callback_FP(a);
}

typedef void (*CRYPTO_set_locking_callback_TYPE)(void (*)(int mode,int type, const char *file,int line));
CRYPTO_set_locking_callback_TYPE CRYPTO_set_locking_callback_FP = NULL;
void CRYPTO_set_locking_callback(void (*a)(int mode,int type, const char *file,int line))
{
	CRYPTO_set_locking_callback_FP(a);
}

				
//typedef SSL_METHOD * (*SSLv23_method_TYPE)(void);	
//SSLv23_method_TYPE SSLv23_method_FP = NULL;	
//SSL_METHOD * SSLv23_method()							
//{												
//	return SSLv23_method_FP();					
//}
//
//typedef SSL_METHOD * (*SSLv3_method_TYPE)(void);	
//SSLv3_method_TYPE SSLv3_method_FP = NULL;	
//SSL_METHOD * SSLv3_method()							
//{												
//	return SSLv3_method_FP();					
//}
//
//typedef SSL_METHOD * (*TLSv1_method_TYPE)(void);	
//TLSv1_method_TYPE TLSv1_method_FP = NULL;	
//SSL_METHOD * TLSv1_method()							
//{												
//	return TLSv1_method_FP();					
//}





typedef DSA * (*DSA_generate_parameters_TYPE)(int, unsigned char *, int, int *, unsigned long *, void (*)(int, int, void *), void *);
DSA_generate_parameters_TYPE DSA_generate_parameters_FP = NULL;
DSA * DSA_generate_parameters(int a, unsigned char *b, int c, int *d, unsigned long *e, void (*f)(int, int, void *), void *g)
{
	return DSA_generate_parameters_FP(a,b,c,d,e,f,g);
}

typedef RSA * (*RSA_generate_key_TYPE)(int, unsigned long, void (*)(int,int,void *), void *);
RSA_generate_key_TYPE RSA_generate_key_FP = NULL;
RSA * RSA_generate_key(int a, unsigned long b, void (*c)(int,int,void *), void *d)
{
	return RSA_generate_key_FP(a,b,c,d);
}

typedef _STACK * (*d2i_ASN1_SET_TYPE)(_STACK **, const unsigned char **, long, BIO *, void (*)(void *), int, int);
d2i_ASN1_SET_TYPE d2i_ASN1_SET_FP = NULL;
_STACK *d2i_ASN1_SET(_STACK **a, const unsigned char **b, long c, BIO *d, void (*e)(void *), int f, int g)
{
	return d2i_ASN1_SET_FP(a,b,c,d,e,f,g);
}

typedef void (*sk_pop_free_TYPE)(_STACK *, void (*)(void *));
sk_pop_free_TYPE sk_pop_free_FP = NULL;
void sk_pop_free(_STACK *a, void (*b)(void *))
{
	sk_pop_free_FP(a,b);
}


//*****************************************************************************
//  OPENSSL_CLASS
//*****************************************************************************

// initialize the static members
OPENSSL_CLASS* OPENSSL_CLASS::instanceM_ptr = NULL;
MUTEX_TYPE* OPENSSL_CLASS::mutexMapM_ptr = NULL;


//>>===========================================================================

OPENSSL_CLASS* OPENSSL_CLASS::getInstance()

//  DESCRIPTION     : Return the pointer to the singleton.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool loaded = false;

	if (instanceM_ptr == NULL)
	{
		instanceM_ptr = new OPENSSL_CLASS(loaded);
		if (!loaded)
		{
			delete instanceM_ptr;
			instanceM_ptr = NULL;
		}
	}

	return instanceM_ptr;
}


//>>===========================================================================

OPENSSL_CLASS::OPENSSL_CLASS(bool& loaded)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	ssleayHandleM = NULL;
	libeayHandleM = NULL;

	libraryInitializedM = false;

	// load the DLLs
	loaded = loadSsleayLib() && loadLibeayLib();
	if (loaded)
	{
		loaded = libraryInitialize();
	}
}

		
//>>===========================================================================

OPENSSL_CLASS::~OPENSSL_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	if (libraryInitializedM)
	{
		EVP_cleanup();
	}

	if (mutexMapM_ptr != NULL)
	{
		delete mutexMapM_ptr;
		mutexMapM_ptr = NULL;
	}

	if (ssleayHandleM != NULL)
	{
		FreeLibrary(ssleayHandleM);
		ssleayHandleM = NULL;
	}

	if (libeayHandleM != NULL)
	{
		FreeLibrary(libeayHandleM);
		libeayHandleM = NULL;
	}
}


//>>===========================================================================

bool OPENSSL_CLASS::libraryInitialize()

//  DESCRIPTION     : Initializes the OpenSSL library.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : This is a static method
//<<===========================================================================
{
	// initialize the static thread locking functions
	mutexMapM_ptr = new MUTEX_TYPE[CRYPTO_num_locks()];

	for (int i = 0; i < CRYPTO_num_locks(); i++)
	{
#if defined(_WINDOWS)
		*(mutexMapM_ptr + i) = CreateMutex(NULL, FALSE, NULL);
#elif defined(_POSIX_THREADS)
		pthread_mutex_init((mutexMapM_ptr + i), NULL);
#endif
	}

	CRYPTO_set_locking_callback(threadLockingCallback);
	CRYPTO_set_id_callback(threadIdCallback);

	// initialize the dynamic thread locking functions
	CRYPTO_set_dynlock_create_callback(dynamicLockCreateCallback);
	CRYPTO_set_dynlock_lock_callback(dynamicLockingCallback);
	CRYPTO_set_dynlock_destroy_callback(dynamicLockDestroyCallback);

	// initialize the library
	if (!SSL_library_init())
	{
		return false;
	}

	// load the error strings
	SSL_load_error_strings();

	// load the ciphers to be used for encryption of keys
	OpenSSL_add_all_algorithms();

	// seed the random number generator
	seedPrng();

	libraryInitializedM = true;

	return true;
}


//>>===========================================================================

void OPENSSL_CLASS::seedPrng()

//  DESCRIPTION     : Seed the OpenSSL random number generator.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : This uses the C code random number generator to generate the seed.
//					: This would not be considered to be a good seed, but DVT is not really 
//					: secure anyway...
//					: This is a static method.
//<<===========================================================================
{
	int value;
	int i = 0;
	const double entropy = 15.0/8.0;

// seed the C random number generator
	srand(static_cast<unsigned>(time(NULL)));

	//bool use_rand_add_method = false;

	//// seed the C random number generator
	//// srand(static_cast<unsigned>(time(NULL)));


	////
	//// MK: Determine temp file path to write random data to.
	////

	//DWORD dwRetVal = 0;
	//UINT uRetVal   = 0;
	//TCHAR tempDirectoryPath[MAX_PATH];
	//TCHAR tempFilePath[MAX_PATH];

 //    //  Gets the temp path env string (no guarantee it's a valid path).
 //   dwRetVal = GetTempPath(MAX_PATH,          // length of the buffer
 //                          tempDirectoryPath); // buffer for path 
 //   if (dwRetVal > MAX_PATH || (dwRetVal == 0))
	//{
	//	use_rand_add_method = true;
	//}
	//else
	//{
	//	//  Generates a temporary file name. 
	//	uRetVal = GetTempFileName(tempDirectoryPath, // directory for tmp files
	//							  TEXT("RND"),     // temp file name prefix 
	//							  0,                // create unique name 
	//							  tempFilePath);  // buffer for name 

	//	if (uRetVal == 0)
	//	{
	//		use_rand_add_method = true;
	//	}
	//}


	////
	//// MK: Write random data to temp file and read it as random data for the OpenSSL library.
	////

	//if (!use_rand_add_method)
	//{
	//	// MK:
	//	// Write the random data and load it again, this should make sure that long delays because of the RAND_status/RAND_add method calls will not occur anymore.
	//	// This random data may not be such random anymore but this is the price to pay for removing the possible delay under Windows 7.

	//	// Write pseudo random bytes to file.
	//	const int numberOfRandomIntegers = 512;
	//	int randBuffer[numberOfRandomIntegers];
	//	unsigned int randomNumber;

	//	for (int randBufferIndex = 0; randBufferIndex < numberOfRandomIntegers; randBufferIndex++)
	//	{
	//		rand_s(&randomNumber);
	//		randBuffer[randBufferIndex] = randomNumber;
	//	}

	//	ofstream outputFileStream(tempFilePath);
	//	outputFileStream.write((char*) &randBuffer, numberOfRandomIntegers * 4);
	//	outputFileStream.close();

	//	// Read pseudo random bytes from file.
	//	RAND_load_file(tempFilePath, numberOfRandomIntegers * 4);

	//	// Remove generated temp file.
	//	DeleteFile(tempFilePath);
	//}


	//////
	////// MK: Check if OpenSLL library consideres the random data as random enough.
	//////
	//
	////// Check if enough randomness is present according to the library.
	////if (RAND_status() != 1)
	////{
	////	use_rand_add_method = true;
	////}


	////
	//// MK: If previous code does not work, do it the original which may cause a long delay under Windows 7.
	////

	//if (use_rand_add_method)
	//{
	//	while (RAND_status() != 1)
	//	{
	//		value = rand();
	//		RAND_add(&value, sizeof(value), entropy);

	//		if (i++ > 4000) // sanity check to make sure we don't get stuck in an infinite loop
	//		{
	//			break;
	//		}
	//	}		
	//}

	while (RAND_status() != 1)
	{
		value = rand();
		RAND_add(&value, sizeof(value), entropy);

		if (i++ > 4000) // sanity check to make sure we don't get stuck in an infinite loop
		{
			break;
		}
	}

}


//>>===========================================================================

void OPENSSL_CLASS::printError(LOG_CLASS* logger_ptr, UINT32 logLevel, const char *format_ptr, ...)

//  DESCRIPTION     : Generate an error message for the current Open SSL error.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	va_list	arguments;
	char buffer[1024];
	unsigned long error_code;
	char* data;
	char* file;
	int line;
	int flags;

	if (logger_ptr == NULL)
	{
		// nothing to do
		return;
	}

	// handle the variable arguments
	va_start(arguments, format_ptr);
	vsprintf(buffer, format_ptr, arguments);
	va_end(arguments);
	
	if (logger_ptr && (logger_ptr->getLogMask() & logLevel))
	{
		logger_ptr->text(logLevel, 1, "Secure Socket - Error %s", buffer);

		// loop reporting each of the OpenSSL errors
		while ((error_code = ERR_get_error_line_data(const_cast<const char**>(&file), &line, 
													 const_cast<const char**>(&data), &flags)) != 0)
		{
			ERR_error_string_n(error_code, buffer, sizeof(buffer));
			logger_ptr->text(LOG_NONE, 1, "    openssl %s:%s", buffer, (flags & ERR_TXT_STRING) ? data : "");
			if (error_code == 0x1408F10B)
			{
				// special error code that might indicate that the peer is attempting to connect un-secure
				logger_ptr->text(LOG_ERROR, 1, "The returned SSL error code %08X might indicate that the peer is trying to make an unsecure connection to DVT.", error_code);
			}
		}

		logger_ptr->text(LOG_NONE, 1, ""); // flush the output
	}
}


//>>===========================================================================

bool OPENSSL_CLASS::isCipherListValid(const char* cipherList)

//  DESCRIPTION     : Checks to see if the given cipher list selects at least one cipher.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	bool result;
	SSL_CTX* ctx_ptr;
	
	// setup a connection factory
	ctx_ptr = SSL_CTX_new(SSLv23_method());


	if (ctx_ptr == NULL)
	{
		return true; // we have other problems...
	}


	result = (SSL_CTX_set_cipher_list(ctx_ptr, cipherList) ? true : false);

	if (result)
	{
		if (ctx_ptr->cipher_list->stack.num == 0)
		{
			result = false;
		}
	}

	// free the connection factory
	SSL_CTX_free(ctx_ptr);

	return result;

}


//>>===========================================================================

bool OPENSSL_CLASS::isPasswordValid(const char* filename, const char* password, bool& unencryptedKeyFound)

//  DESCRIPTION     : Checks to see if the given password is valid.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	STACK_OF(X509_INFO)* pem_ptr = NULL;
	bool ret = true;

	if (readPemFile(filename, &pem_ptr, openSslPasswordCallback, (void *)password, NULL, 
					&unencryptedKeyFound) == MSG_INVALID_PASSWORD)
	{
		// only say it is invalid if it is definately invalid, all other conditions return true
		ret = false;
	}

	if (pem_ptr != NULL) sk_X509_INFO_pop_free(pem_ptr, X509_INFO_free);

	return ret;
}


//>>===========================================================================

bool OPENSSL_CLASS::asn1TimeToTm(ASN1_TIME* asn1Time_ptr, struct tm& tmTime)

//  DESCRIPTION     : Converts an ASN1_TIME structure to a tm time structure.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : Based on the OpenSSL library functions ASN1_TIME_print,
//					: ASN1_GENERALIZEDTIME_print, ASN1_UTCTIME_print
//					:
//					: Static Method
//<<===========================================================================
{
	char *v;
	int gmt=0;
	int i;
	int y=0,M=0,d=0,h=0,m=0,s=0;

	memset(&tmTime, 0, sizeof(struct tm));

	if (asn1Time_ptr == NULL)
	{
		return false;
	}

	if (asn1Time_ptr->type == V_ASN1_UTCTIME)
	{
		i=asn1Time_ptr->length;
		v=(char *)asn1Time_ptr->data;

		if (i < 10) return false;

		if (v[i-1] == 'Z') gmt=1;
		
		for (i=0; i<10; i++)
			if ((v[i] > '9') || (v[i] < '0')) return false;

		y= (v[0]-'0')*10+(v[1]-'0');
		if (y < 50) y+=100;
		
		M= (v[2]-'0')*10+(v[3]-'0');
		if ((M > 12) || (M < 1)) return false;
		
		d= (v[4]-'0')*10+(v[5]-'0');
		
		h= (v[6]-'0')*10+(v[7]-'0');
		
		m=  (v[8]-'0')*10+(v[9]-'0');
		
		if (	(v[10] >= '0') && (v[10] <= '9') &&
			(v[11] >= '0') && (v[11] <= '9'))
			s=  (v[10]-'0')*10+(v[11]-'0');
	}
	else if (asn1Time_ptr->type == V_ASN1_GENERALIZEDTIME)
	{
		i=asn1Time_ptr->length;
		v=(char *)asn1Time_ptr->data;

		if (i < 12) return false;
		
		if (v[i-1] == 'Z') gmt=1;
		
		for (i=0; i<12; i++)
			if ((v[i] > '9') || (v[i] < '0')) return false;

		y= (v[0]-'0')*1000+(v[1]-'0')*100 + (v[2]-'0')*10+(v[3]-'0');
		y-= 1900;
		if (y < 0) return false;
		
		M= (v[4]-'0')*10+(v[5]-'0');
		if ((M > 12) || (M < 1)) return false;
		
		d= (v[6]-'0')*10+(v[7]-'0');
		
		h= (v[8]-'0')*10+(v[9]-'0');
		
		m=  (v[10]-'0')*10+(v[11]-'0');
		
		if (	(v[12] >= '0') && (v[12] <= '9') &&
			(v[13] >= '0') && (v[13] <= '9'))
			s=  (v[12]-'0')*10+(v[13]-'0');
	}
	else
	{
		return false;
	}

	tmTime.tm_year = y;
	tmTime.tm_mon = M - 1;
	tmTime.tm_mday = d;
	tmTime.tm_hour = h;
	tmTime.tm_min = m;
	tmTime.tm_sec = s;

	return true;
}


//>>===========================================================================

bool OPENSSL_CLASS::serialNumberToString(ASN1_INTEGER* serialNumber_ptr, string& serialNumberString)

//  DESCRIPTION     : Converts an ASN1_INTEGER structure to a character string.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : Extracted and modified from the OpenSSL library function X509_print_ex
//					:
//					: Static Method
//<<===========================================================================
{
	ostringstream snStream;
	long value;

	serialNumberString = "";

	if (serialNumber_ptr == NULL)
	{
		return NULL;
	}

	snStream << "0x" << hex << right << uppercase;
	snStream.fill('0');

	if (serialNumber_ptr->length <= 4)
	{
		value = ASN1_INTEGER_get(serialNumber_ptr);
		snStream << value;
	}
	else
	{
		for (int i = 0; i < serialNumber_ptr->length; i++)
		{
			snStream.width(2);
			snStream << (int)serialNumber_ptr->data[i];
		}
	}

	serialNumberString = snStream.str();

	return true;
}


//>>===========================================================================

X509_NAME* OPENSSL_CLASS::onelineName2Name(const char *string_ptr)

//  DESCRIPTION     : Converts a character string to a name structure.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : The structure is "/tag=value/tag=value"
//<<===========================================================================
{
	string oneline = string_ptr;
	string sn;
	string value;
	X509_NAME* name_ptr;
	int slashPosition;
	int equalPosition;

	name_ptr = X509_NAME_new();
	if (name_ptr == NULL)
	{
		return NULL;
	}

	slashPosition = oneline.find('/');
	while (slashPosition != (int) string::npos)
	{
		equalPosition = oneline.find('=');
		if (equalPosition == (int) string::npos)
		{
			// problem, abort
			X509_NAME_free(name_ptr);
			return NULL;
		}

		// get the tag ("short name")
		sn = oneline.substr(slashPosition + 1, equalPosition - slashPosition - 1);

		// chop off the beginning of the string at the '='
		oneline = oneline.substr(equalPosition + 1, string::npos);

		slashPosition = oneline.find('/');

		// get the value
		if (slashPosition == (int) string::npos)
		{
			value = oneline;
		}
		else
		{
			value = oneline.substr(0, slashPosition);
		}

		// add the value to the name
		int nid = OBJ_sn2nid(sn.c_str());
		if (nid == NID_undef)
		{
			// unknown tag, abort
			X509_NAME_free(name_ptr);
			return NULL;
		}

		X509_NAME_ENTRY* entry_ptr = 
			X509_NAME_ENTRY_create_by_NID(NULL, nid, MBSTRING_ASC,(unsigned char*)value.c_str(), -1);
		if (entry_ptr == NULL)
		{
			// error, abort
			X509_NAME_free(name_ptr);
			return NULL;
		}

		if (X509_NAME_add_entry(name_ptr, entry_ptr, -1, 0) != 1)
		{
			// error, abort
			X509_NAME_free(name_ptr);
			return NULL;
		}
	}

	return name_ptr;
}

//>>===========================================================================

bool OPENSSL_CLASS::checkPemName(const char* readName_ptr, const char* name_ptr)

//  DESCRIPTION     : Checks to see if the read name (from a PEM file) matches the given name.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : Copied from  OpenSSL's private function check_pem()
//<<===========================================================================
{
	/* Normal matching readName_ptr and name_ptr */
	if (!strcmp(readName_ptr,name_ptr)) return true;

	/* Make PEM_STRING_EVP_PKEY match any private key */

	if(!strcmp(readName_ptr,PEM_STRING_PKCS8) &&
		!strcmp(name_ptr,PEM_STRING_EVP_PKEY)) return true;

	if(!strcmp(readName_ptr,PEM_STRING_PKCS8INF) &&
		 !strcmp(name_ptr,PEM_STRING_EVP_PKEY)) return true;

	if(!strcmp(readName_ptr,PEM_STRING_RSA) &&
		!strcmp(name_ptr,PEM_STRING_EVP_PKEY)) return true;

	if(!strcmp(readName_ptr,PEM_STRING_DSA) &&
		 !strcmp(name_ptr,PEM_STRING_EVP_PKEY)) return true;

	/* Permit older strings */

	if(!strcmp(readName_ptr,PEM_STRING_X509_OLD) &&
		!strcmp(name_ptr,PEM_STRING_X509)) return true;

	if(!strcmp(readName_ptr,PEM_STRING_X509_REQ_OLD) &&
		!strcmp(name_ptr,PEM_STRING_X509_REQ)) return true;

	/* Allow normal certs to be read as trusted certs */
	if(!strcmp(readName_ptr,PEM_STRING_X509) &&
		!strcmp(name_ptr,PEM_STRING_X509_TRUSTED)) return true;

	if(!strcmp(readName_ptr,PEM_STRING_X509_OLD) &&
		!strcmp(name_ptr,PEM_STRING_X509_TRUSTED)) return true;

	/* Some CAs use PKCS#7 with CERTIFICATE headers */
	if(!strcmp(readName_ptr, PEM_STRING_X509) &&
		!strcmp(name_ptr, PEM_STRING_PKCS7)) return true;

	return false;
}


//>>===========================================================================

DVT_STATUS OPENSSL_CLASS::readPemFile(const char* filename_ptr, STACK_OF(X509_INFO)** pem_ptrptr,
	pem_password_cb *passwordCallback_ptr, void *passswordCbUserData_ptr, LOG_CLASS* logger_ptr,
	bool* unencryptedKeyFound_ptr)

//  DESCRIPTION     : Reads a PEM file.
//  PRECONDITIONS   : 
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : The returned PEM stack must be freed with sk_X509_INFO_pop_free(, X509_INFO_free()) or similar.
//					: Returns MSK_OK, MSG_ERROR, MSG_FILE_NOT_EXIST, MSG_INVALID_PASSWORD
//<<===========================================================================
{
	DVT_STATUS ret = MSG_ERROR;
	bool unencryptedKeyFound = false;
	BIO* bio_ptr = NULL;
	unsigned long err;
	char *name_ptr = NULL;
	char *header_ptr = NULL;
	unsigned char *data_ptr = NULL;
	long len;
	EVP_CIPHER_INFO cipher;
	X509_INFO* x509Info_ptr = NULL;

	// clear the error queue
	ERR_clear_error();

	if (*pem_ptrptr != NULL)
	{
		sk_X509_INFO_pop_free(*pem_ptrptr, X509_INFO_free);
	}
	*pem_ptrptr = sk_X509_INFO_new_null();

	// open the file
	bio_ptr = BIO_new(BIO_s_file_internal());
	if (bio_ptr == NULL)
	{
		printError(logger_ptr, LOG_ERROR, "setting up to read PEM file");
		goto end;
	}
	if (BIO_read_filename(bio_ptr, filename_ptr) <= 0)
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
			printError(logger_ptr, LOG_ERROR, "opening PEM file for reading");
		}
		goto end;
	}

	// read PEM data structures until an error
	while (PEM_read_bio(bio_ptr, &name_ptr, &header_ptr, &data_ptr, &len))
	{
		if (!PEM_get_EVP_CIPHER_INFO(header_ptr, &cipher))
		{
			printError(logger_ptr, LOG_ERROR, "getting cipher information from PEM file");
			goto end;
		}
		if (!PEM_do_header(&cipher, data_ptr, &len, passwordCallback_ptr, passswordCbUserData_ptr)) 
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
				printError(logger_ptr, LOG_ERROR, "processing PEM header in PEM file");
			}
			goto end;
		}

		if (checkPemName(name_ptr, PEM_STRING_EVP_PKEY))
		{
			// private key read
			// convert PEM to private key [code copied and modified from PEM_read_bio_PrivateKey()]
			const unsigned char *tmpData_ptr = data_ptr; // don't modify data_ptr below
			EVP_PKEY *key_ptr = NULL;
			bool tryPkcs8 = false;

			if (strcmp(name_ptr, PEM_STRING_RSA) == 0)
			{
				// rsa unencrypted key
				key_ptr = d2i_PrivateKey(EVP_PKEY_RSA, NULL, &tmpData_ptr, len);
				if (key_ptr == NULL)
				{
					err = ERR_peek_error();
					if ((ERR_GET_LIB(err) == ERR_LIB_ASN1) && (ERR_GET_REASON(err) == ASN1_R_WRONG_TAG))
					{
						// invalid tag found, try reading it as PKCS#8 encoding
						tryPkcs8 = true;
						ERR_clear_error(); // eat any errors
					}
					else
					{
						printError(logger_ptr, LOG_ERROR, "decoding RSA Private Key from PEM file");
						goto end;
					}
				}
				else
				{
					unencryptedKeyFound = true;
				}
			}
			else if (strcmp(name_ptr, PEM_STRING_DSA) == 0)
			{
				// dsa unencrypted key
				key_ptr = d2i_PrivateKey(EVP_PKEY_DSA, NULL, &tmpData_ptr, len);
				if (key_ptr == NULL)
				{
					err = ERR_peek_error();
					if ((ERR_GET_LIB(err) == ERR_LIB_ASN1) && (ERR_GET_REASON(err) == ASN1_R_WRONG_TAG))
					{
						// invalid tag found, try reading it as PKCS#8 encoding
						tryPkcs8 = true;
						ERR_clear_error(); // eat any errors
					}
					else
					{
						printError(logger_ptr, LOG_ERROR, "decoding DSA Private Key from PEM file");
						goto end;
					}
				}
				else
				{
					unencryptedKeyFound = true;
				}
			}

			if (key_ptr != NULL)
			{
				// got a key above, don't need to try the following
			}
			else if (tryPkcs8 || (strcmp(name_ptr, PEM_STRING_PKCS8INF) == 0))
			{
				// PKCS8 unencrypted key
				PKCS8_PRIV_KEY_INFO *p8inf;

				p8inf = d2i_PKCS8_PRIV_KEY_INFO(NULL, &tmpData_ptr, len);
				if(p8inf == NULL)
				{
					PEMerr(PEM_F_PEM_ASN1_READ_BIO,ERR_R_ASN1_LIB);
					printError(logger_ptr, LOG_ERROR, "decoding PKCS8INF Private Key from PEM file");
					goto end;
				}
				key_ptr = EVP_PKCS82PKEY(p8inf);
				unencryptedKeyFound = true;

				PKCS8_PRIV_KEY_INFO_free(p8inf);
			}
			else if (strcmp(name_ptr, PEM_STRING_PKCS8) == 0) 
			{
				// PKCS8 encrypted key
				PKCS8_PRIV_KEY_INFO *p8inf;
				X509_SIG *p8;
				int klen;
				char psbuf[PEM_BUFSIZE];

				p8 = d2i_X509_SIG(NULL, &tmpData_ptr, len);
				if(p8 == NULL)
				{
					PEMerr(PEM_F_PEM_ASN1_READ_BIO,ERR_R_ASN1_LIB);
					printError(logger_ptr, LOG_ERROR, "decoding PKCS8 Private Key from PEM file");
					goto end;
				}
				klen = passwordCallback_ptr(psbuf, PEM_BUFSIZE, 0, passswordCbUserData_ptr);
				if (klen <= 0) 
				{
					// bad password
					ERR_clear_error(); // eat any errors
					ret = MSG_INVALID_PASSWORD;
					goto end;
				}
				p8inf = PKCS8_decrypt(p8, psbuf, klen);
				X509_SIG_free(p8);
				if(p8inf == NULL)
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
						PEMerr(PEM_F_PEM_ASN1_READ_BIO,ERR_R_ASN1_LIB);
						printError(logger_ptr, LOG_ERROR, "decrypting PKCS8 Private Key from PEM file");
					}
					goto end;
				}
				key_ptr = EVP_PKCS82PKEY(p8inf);

				PKCS8_PRIV_KEY_INFO_free(p8inf);
			}
			else
			{
				if (logger_ptr)
				{
					logger_ptr->text(LOG_ERROR, 1, "Unsupported private key type \"%s\" in PEM file",
										name_ptr);
				}
				goto end;
			}

			// save the key in an X509_INFO
			x509Info_ptr = X509_INFO_new();
			if (x509Info_ptr == NULL)
			{
				printError(logger_ptr, LOG_ERROR, "creating PEM info structure reading PEM file");
				EVP_PKEY_free(key_ptr);
				goto end;
			}

			x509Info_ptr->enc_data = NULL;
			x509Info_ptr->enc_len = 0;
			x509Info_ptr->x_pkey = X509_PKEY_new();
			if (x509Info_ptr->x_pkey == NULL)
			{
				printError(logger_ptr, LOG_ERROR, "creating private key structure reading PEM file");
				EVP_PKEY_free(key_ptr);
				goto end;
			}
			x509Info_ptr->x_pkey->dec_pkey = key_ptr;

			sk_X509_INFO_push(*pem_ptrptr, x509Info_ptr);
		}
		else if (checkPemName(name_ptr, PEM_STRING_X509))
		{
			X509* x509_ptr;

			// certificate read
			// decode the certificate
			unsigned char *tmpData_ptr = data_ptr; // don't modify data_ptr below

			x509_ptr = d2i_X509(NULL, &tmpData_ptr, len);
			if (x509_ptr == NULL)
			{
				printError(logger_ptr, LOG_ERROR, "decoding certificate from PEM file");
				goto end;
			}

			// save the certificate in an X509_INFO
			x509Info_ptr = X509_INFO_new();
			if (x509Info_ptr == NULL)
			{
				printError(logger_ptr, LOG_ERROR, "creating PEM info structure reading PEM file");
				X509_free(x509_ptr);
				goto end;
			}

			x509Info_ptr->x509 = x509_ptr;
			
			sk_X509_INFO_push(*pem_ptrptr, x509Info_ptr);
		}
		else
		{
			// ignore everything else in the file without notice
		}

		// free the memory for the next loop
		if (data_ptr != NULL)
		{
			OPENSSL_free(data_ptr);
			data_ptr = NULL;
		}
		if (name_ptr != NULL) 
		{
			OPENSSL_free(name_ptr);
			name_ptr = NULL;
		}
		if (header_ptr != NULL) 
		{
			OPENSSL_free(header_ptr);
			header_ptr = NULL;
		}
	} // end of while PEM_read_bio loop
	// check the error
	err = ERR_peek_error();
	if ((ERR_GET_LIB(err) == ERR_LIB_PEM) && (ERR_GET_REASON(err) == PEM_R_NO_START_LINE))
	{
		// end of data - this is normal
		ERR_clear_error();
	}
	else
	{
		printError(logger_ptr, LOG_ERROR, "reading PEM file");
		goto end;
	}

	// everything is OK
	ret = MSG_OK;

end:
	if (data_ptr != NULL) OPENSSL_free(data_ptr);
	if (name_ptr != NULL) OPENSSL_free(name_ptr);
	if (header_ptr != NULL) OPENSSL_free(header_ptr);
	if (bio_ptr != NULL) BIO_free(bio_ptr);

	if (ret != MSG_OK)
	{
		// error, free the return structure
		if (*pem_ptrptr != NULL)
		{
			sk_X509_INFO_pop_free(*pem_ptrptr, X509_INFO_free);
			*pem_ptrptr = NULL;
		}
	}

	if (unencryptedKeyFound_ptr != NULL)
	{
		*unencryptedKeyFound_ptr = unencryptedKeyFound;
	}

	return ret;
}


//>>===========================================================================

bool OPENSSL_CLASS::writePemFile(const char* filename_ptr,  STACK_OF(X509_INFO)* pem_ptr,
	const char* password_ptr, LOG_CLASS* logger_ptr)

//  DESCRIPTION     : Writes out a PEM file.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	bool ret = false;
	BIO* bio_ptr = NULL;
	const EVP_CIPHER *cipher;
	int i;
	int num;
	X509_INFO* x509Info_ptr;

	// clear the error queue
	ERR_clear_error();

	if ((password_ptr != NULL) && (strlen(password_ptr) > 0))
	{
		// we have a password, use 3DES to encrypt the key
		cipher = EVP_des_ede3_cbc();
	}
	else
	{
		// there is no password, don't encrypt the key
		cipher = NULL;
	}

	// open the file
	bio_ptr = BIO_new(BIO_s_file_internal());
	if (bio_ptr == NULL)
	{
		printError(logger_ptr, LOG_ERROR, "setting up to write PEM file");
		goto end;
	}
	if (BIO_write_filename(bio_ptr, (void *)filename_ptr) <= 0)
	{
		printError(logger_ptr, LOG_ERROR, "opening to write PEM file");
		goto end;
	}

	// write each of the PEM infos
	num = sk_X509_INFO_num(pem_ptr);
	for (i = 0; i < num; i++)
	{
		// get the PEM info
		x509Info_ptr = sk_X509_INFO_value(pem_ptr, i);
		if (x509Info_ptr == NULL)
		{
			printError(logger_ptr, LOG_ERROR, "getting PEM to write");
			goto end;
		}

		if (x509Info_ptr->x_pkey != NULL)
		{
			// private key, write it out
			if (PEM_write_bio_PKCS8PrivateKey(bio_ptr, x509Info_ptr->x_pkey->dec_pkey, cipher, 
												NULL, 0, NULL, (void *)password_ptr) != 1)
			{
				printError(logger_ptr, LOG_ERROR, "writing private key to PEM file");
				goto end;
			}
		}

		if (x509Info_ptr->x509 != NULL)
		{
			// certificate, write it out
			if (PEM_write_bio_X509(bio_ptr, x509Info_ptr->x509) != 1)
			{
				printError(logger_ptr, LOG_ERROR, "writing certificate to PEM file");
				goto end;
			}
		}
	}

	ret = true;

end:
	if (bio_ptr != NULL) BIO_free(bio_ptr);
	return ret;
}


//>>===========================================================================

DVT_STATUS OPENSSL_CLASS::readCredentialsFile(const char* filename_ptr, 
	EVP_PKEY** rsaPrivateKey_ptrptr, STACK_OF(X509)** rsaCertChain_ptrptr,
	EVP_PKEY** dsaPrivateKey_ptrptr, STACK_OF(X509)** dsaCertChain_ptrptr,
	pem_password_cb *passwordCallback_ptr, void *passswordCbUserData_ptr, LOG_CLASS* logger_ptr)

//  DESCRIPTION     : Reads a credentials file.
//  PRECONDITIONS   : 
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : The returned keys must be freed by the caller with EVP_PKEY_free().
//					: The returned certificate chains must be freed with sk_X509_pop_free() or similar.
//					: Returns MSK_OK, MSG_ERROR, MSG_FILE_NOT_EXIST
//<<===========================================================================
{
	DVT_STATUS ret = MSG_ERROR;
	BIO* bio_ptr = NULL;
	unsigned long err;
	char *name_ptr = NULL;
	char *header_ptr = NULL;
	unsigned char *data_ptr = NULL;
	long len;
	EVP_CIPHER_INFO cipher;

	bool waitingForCertificate = false; // indicates that the next PEM read must be a certificate
	bool rsaKeyRead = false;
	bool dsaKeyRead = false;
	bool lastKeyReadRsa = false; // indicates that we are working on the RSA key and certificates,
								 // otherwise, we are working on the DSA key and certificate.

	// set the return pointers to NULL
	*rsaPrivateKey_ptrptr = NULL;
	*dsaPrivateKey_ptrptr = NULL;
	*rsaCertChain_ptrptr = NULL;
	*dsaCertChain_ptrptr = NULL;

	// clear the error queue
	ERR_clear_error();

	// open the file
	bio_ptr = BIO_new(BIO_s_file_internal());
	if (bio_ptr == NULL)
	{
		printError(logger_ptr, LOG_ERROR, "setting up the read credentials file");
		goto end;
	}
	if (BIO_read_filename(bio_ptr, filename_ptr) <= 0)
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
			printError(logger_ptr, LOG_ERROR, "opening the credentials file for reading");
		}
		goto end;
	}

	// read PEM data structures until an error
	while (PEM_read_bio(bio_ptr, &name_ptr, &header_ptr, &data_ptr, &len))
	{
		if (!PEM_get_EVP_CIPHER_INFO(header_ptr, &cipher))
		{
			printError(logger_ptr, LOG_ERROR, "getting cipher information from credentials file");
			goto end;
		}
		if (!PEM_do_header(&cipher, data_ptr, &len, passwordCallback_ptr, passswordCbUserData_ptr)) 
		{
			printError(logger_ptr, LOG_ERROR, "processing PEM header in credentials file");
			goto end;
		}

		if (checkPemName(name_ptr, PEM_STRING_EVP_PKEY))
		{
			// private key read
			if (waitingForCertificate)
			{
				// read private key when we should have read a certificate
				if (logger_ptr)
				{
					logger_ptr->text(LOG_ERROR, 1, 
						"Private key does not have a corresponding certificate in credentials file");
				}
				goto end;
			}

			// convert PEM to private key [code copied from PEM_read_bio_PrivateKey()]
			const unsigned char *tmpData_ptr = data_ptr; // don't modify data_ptr below
			EVP_PKEY *key_ptr = NULL;

			if (strcmp(name_ptr, PEM_STRING_RSA) == 0)
			{
				key_ptr = d2i_PrivateKey(EVP_PKEY_RSA, NULL, &tmpData_ptr, len);
				if (key_ptr == NULL)
				{
					printError(logger_ptr, LOG_ERROR, "decoding RSA Private Key from credentials file");
					goto end;
				}
			}
			else if (strcmp(name_ptr, PEM_STRING_DSA) == 0)
			{
				key_ptr = d2i_PrivateKey(EVP_PKEY_DSA, NULL, &tmpData_ptr, len);
				if (key_ptr == NULL)
				{
					printError(logger_ptr, LOG_ERROR, "decoding DSA Private Key from credentials file");
					goto end;
				}
			}
			else if (strcmp(name_ptr, PEM_STRING_PKCS8INF) == 0) 
			{
				PKCS8_PRIV_KEY_INFO *p8inf;

				p8inf = d2i_PKCS8_PRIV_KEY_INFO(NULL, &tmpData_ptr, len);
				if(p8inf == NULL)
				{
					PEMerr(PEM_F_PEM_ASN1_READ_BIO,ERR_R_ASN1_LIB);
					printError(logger_ptr, LOG_ERROR, "decoding PKCS8INF Private Key from credentials file");
					goto end;
				}
				key_ptr = EVP_PKCS82PKEY(p8inf);

				PKCS8_PRIV_KEY_INFO_free(p8inf);
			}
			else if (strcmp(name_ptr, PEM_STRING_PKCS8) == 0) 
			{
				PKCS8_PRIV_KEY_INFO *p8inf;
				X509_SIG *p8;
				int klen;
				char psbuf[PEM_BUFSIZE];

				p8 = d2i_X509_SIG(NULL, &tmpData_ptr, len);
				if(p8 == NULL)
				{
					PEMerr(PEM_F_PEM_ASN1_READ_BIO,ERR_R_ASN1_LIB);
					printError(logger_ptr, LOG_ERROR, "decoding PKCS8 Private Key from credentials file");
					goto end;
				}
				klen = passwordCallback_ptr(psbuf, PEM_BUFSIZE, 0, passswordCbUserData_ptr);
				if (klen <= 0) 
				{
					PEMerr(PEM_F_PEM_ASN1_READ_BIO, PEM_R_BAD_PASSWORD_READ);
					printError(logger_ptr, LOG_ERROR, "decoding PKCS8 Private Key from credentials file");
					goto end;
				}
				p8inf = PKCS8_decrypt(p8, psbuf, klen);
				X509_SIG_free(p8);
				if(p8inf == NULL)
				{
					PEMerr(PEM_F_PEM_ASN1_READ_BIO,ERR_R_ASN1_LIB);
					printError(logger_ptr, LOG_ERROR, "decrypting PKCS8 Private Key from credentials file");
					goto end;
				}
				key_ptr = EVP_PKCS82PKEY(p8inf);

				PKCS8_PRIV_KEY_INFO_free(p8inf);
			}
			else
			{
				if (logger_ptr)
				{
					logger_ptr->text(LOG_ERROR, 1, "Unsupported private key type \"%s\" in credentials file",
										name_ptr);
				}
				goto end;
			}

			// determine the type of key
			if (key_ptr->type == EVP_PKEY_RSA)
			{
				// rsa key
				if (rsaKeyRead)
				{
					if (logger_ptr)
					{
						logger_ptr->text(LOG_ERROR, 1, "More than one RSA Private Key in credentials file");
					}
					goto end;
				}

				// save the key
				*rsaPrivateKey_ptrptr = key_ptr;

				// initialize the rsa certificate stack
				*rsaCertChain_ptrptr = sk_X509_new_null();

				rsaKeyRead = true;
				lastKeyReadRsa = true; // we are reading rsa keys now
			}
			else if (key_ptr->type == EVP_PKEY_DSA)
			{
				// dsa key
				if (dsaKeyRead)
				{
					if (logger_ptr)
					{
						logger_ptr->text(LOG_ERROR, 1, "More than one DSA Private Key in credentials file");
					}
					goto end;
				}

				// save the key
				*dsaPrivateKey_ptrptr = key_ptr;

				// initialize the dsa certificate stack
				*dsaCertChain_ptrptr = sk_X509_new_null();

				dsaKeyRead = true;
				lastKeyReadRsa = false; // we are reading dsa keys now
			}
			else
			{
				if (logger_ptr)
				{
					logger_ptr->text(LOG_ERROR, 1, "Unsupported private key type in credentials file");
				}
				EVP_PKEY_free(key_ptr);
				goto end;
			}

			waitingForCertificate = true;  // the next thing in the file needs to be the public key certificate
		}
		else if (checkPemName(name_ptr, PEM_STRING_X509))
		{
			X509* x509_ptr;

			// certificate read
			if (!rsaKeyRead && !dsaKeyRead)
			{
				// no private key read yet
				if (logger_ptr) 
				{
					logger_ptr->text(LOG_ERROR, 1, 
						"Credentials file starts with a certificate.  Must start with a private key.");
				}
				goto end;
			}

			// decode the certificate
			unsigned char *tmpData_ptr = data_ptr; // don't modify data_ptr below

			x509_ptr = d2i_X509(NULL, &tmpData_ptr, len);
			if (x509_ptr == NULL)
			{
				printError(logger_ptr, LOG_ERROR, "decoding certificate from credentials file");
				goto end;
			}

			// add the certificate to the appropriate stack
			if (lastKeyReadRsa)
			{
				// add to rsa stack
				sk_X509_push(*rsaCertChain_ptrptr, x509_ptr);
			}
			else
			{
				// add to dsa stack
				sk_X509_push(*dsaCertChain_ptrptr, x509_ptr);
			}

			waitingForCertificate = false;
		}

		// free the memory for the next loop
		if (data_ptr != NULL)
		{
			OPENSSL_free(data_ptr);
			data_ptr = NULL;
		}
		if (name_ptr != NULL) 
		{
			OPENSSL_free(name_ptr);
			name_ptr = NULL;
		}
		if (header_ptr != NULL) 
		{
			OPENSSL_free(header_ptr);
			header_ptr = NULL;
		}
	} // end of while PEM_read_bio loop
	// check the error
	err = ERR_peek_error();
	if ((ERR_GET_LIB(err) == ERR_LIB_PEM) && (ERR_GET_REASON(err) == PEM_R_NO_START_LINE))
	{
		// end of data - this is normal
		ERR_clear_error();
	}
	else
	{
		printError(logger_ptr, LOG_ERROR, "reading credentials file");
		goto end;
	}

	// check to make sure all is well
	if (waitingForCertificate)
	{
		// file didn't have required certificate
		if (logger_ptr)
		{
			logger_ptr->text(LOG_ERROR, 1, 
				"Private key does not have a corresponding certificate at end of credentials file");
		}
		goto end;
	}
	if (!rsaKeyRead && !dsaKeyRead)
	{
		// no private key read
		if (logger_ptr) 
		{
			logger_ptr->text(LOG_ERROR, 1, "Credentials file has no private keys");
		}
		goto end;
	}

	// everything is OK
	ret = MSG_OK;

end:
	if (data_ptr != NULL) OPENSSL_free(data_ptr);
	if (name_ptr != NULL) OPENSSL_free(name_ptr);
	if (header_ptr != NULL) OPENSSL_free(header_ptr);
	if (bio_ptr != NULL) BIO_free(bio_ptr);

	if (ret != MSG_OK)
	{
		// error, free the return structures
		if (*rsaPrivateKey_ptrptr != NULL)
		{
			EVP_PKEY_free(*rsaPrivateKey_ptrptr);
			*rsaPrivateKey_ptrptr = NULL;
		}
		if (*dsaPrivateKey_ptrptr != NULL)
		{
			EVP_PKEY_free(*dsaPrivateKey_ptrptr);
			*dsaPrivateKey_ptrptr = NULL;
		}
		if (*rsaCertChain_ptrptr != NULL)
		{
			sk_X509_pop_free(*rsaCertChain_ptrptr, X509_free);
			*rsaCertChain_ptrptr = NULL;
		}
		if (*dsaCertChain_ptrptr != NULL) 
		{
			sk_X509_pop_free(*dsaCertChain_ptrptr, X509_free);
			*dsaCertChain_ptrptr = NULL;
		}
	}

	return ret;
}


//>>===========================================================================

void OPENSSL_CLASS::threadLockingCallback(int mode, int n, const char* file, int line)

//  DESCRIPTION     : Callback function from OpenSSL to obtain or release a lock.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	file;line; // caller's file and line.  We don't need these.

	if (mode & CRYPTO_LOCK)
	{
		// obtain the lock
#if defined(_WINDOWS)
		WaitForSingleObject(*(mutexMapM_ptr + n), INFINITE);
#elif defined(_POSIX_THREADS)
		pthread_mutex_lock((mutexMapM_ptr + n));
#endif
	}
	else
	{
		// release the lock
#if defined(_WINDOWS)
		ReleaseMutex(*(mutexMapM_ptr + n));
#elif defined(_POSIX_THREADS)
		pthread_mutex_unlock((mutexMapM_ptr + n));
#endif
	}
}


//>>===========================================================================

unsigned long OPENSSL_CLASS::threadIdCallback()

//  DESCRIPTION     : Callback function from OpenSSL to get the current thread ID.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	return getThreadId();
}


//>>===========================================================================

struct CRYPTO_dynlock_value* OPENSSL_CLASS::dynamicLockCreateCallback(const char *file, int line)

//  DESCRIPTION     : Callback OpenSSL uses to create a dynamically allocated lock.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	struct CRYPTO_dynlock_value* value_ptr;

	file;line; // caller's file and line.  We don't need these.

	value_ptr = new CRYPTO_dynlock_value;

#if defined(_WINDOWS)
	value_ptr->mutex = CreateMutex(NULL, FALSE, NULL);
#elif defined(_POSIX_THREADS)
	pthread_mutex_init(&(value_ptr->mutex), NULL);
#endif

	return value_ptr;
}


//>>===========================================================================

void OPENSSL_CLASS::dynamicLockingCallback(int mode, CRYPTO_dynlock_value* lock_ptr, 
															const char* file, int line)

//  DESCRIPTION     : Callback function from OpenSSL to obtain or release a dynamic lock.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	file;line; // caller's file and line.  We don't need these.

	if (mode & CRYPTO_LOCK)
	{
		// obtain the lock
#if defined(_WINDOWS)
		WaitForSingleObject(lock_ptr->mutex, INFINITE);
#elif defined(_POSIX_THREADS)
		pthread_mutex_lock(&(lock_ptr->mutex));
#endif
	}
	else
	{
		// release the lock
#if defined(_WINDOWS)
		ReleaseMutex(lock_ptr->mutex);
#elif defined(_POSIX_THREADS)
		pthread_mutex_unlock(&(lock_ptr->mutex));
#endif
	}
}


//>>===========================================================================

void OPENSSL_CLASS::dynamicLockDestroyCallback(CRYPTO_dynlock_value* lock_ptr, 
																const char* file, int line)

//  DESCRIPTION     : Callback OpenSSL uses to remove a dynamically allocated lock.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	file;line; // caller's file and line.  We don't need these.

#if defined(_WINDOWS)
	CloseHandle(lock_ptr->mutex);
#elif defined(_POSIX_THREADS)
	pthread_mutex_destroy(&(lock_ptr->mutex));
#endif

	delete lock_ptr;
}


//>>===========================================================================

DH* OPENSSL_CLASS::tmpDhCallback(SSL*, int, int keylength)

//  DESCRIPTION     : Callback OpenSSL uses to obtain the needed DH parameters.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	DH* dh;

	if (keylength == 512)
	{
		// return the parameters for generating 512-bit keys
		// this code was generated by using the command "openssl dhparam -check -C -noout 512"
		static unsigned char dh512_p[]={
				0x9F,0x66,0xDC,0x9D,0x8D,0x11,0x42,0x42,0xB6,0xA8,0x0D,0x59,
				0x0C,0x4A,0x21,0x15,0xEA,0x1E,0xD4,0x8B,0x06,0x84,0xB6,0x94,
				0x9F,0x80,0x9E,0xCF,0xB9,0x88,0x95,0x62,0x97,0xA4,0x6F,0x0D,
				0xC1,0x7B,0x34,0x4E,0x9C,0x59,0xE6,0xA8,0xD1,0xC2,0xE6,0x35,
				0xF6,0x04,0x71,0xD1,0x4E,0xFA,0x6D,0x1E,0x6A,0x0B,0xE3,0xD1,
				0xDB,0xDD,0x43,0xCB,
				};
		static unsigned char dh512_g[]={
				0x02,
				};

		if ((dh=DH_new()) == NULL) return(NULL);
		dh->p=BN_bin2bn(dh512_p,sizeof(dh512_p),NULL);
		dh->g=BN_bin2bn(dh512_g,sizeof(dh512_g),NULL);
		if ((dh->p == NULL) || (dh->g == NULL))
				{ DH_free(dh); return(NULL); }
 	}
	else
	{
		// return the parameters for generating 1024-bit keys
		// this code was generated by using the command "openssl dhparam -check -C -noout 1024"
		static unsigned char dh1024_p[]={
				0xD3,0x7C,0xF2,0x9D,0x09,0x1D,0x3F,0x86,0x90,0xF4,0xDA,0xD7,
				0xCB,0x1E,0x43,0x85,0x06,0x6C,0xD5,0xB7,0x2B,0x2B,0x89,0xDE,
				0xBC,0x69,0x92,0x92,0x67,0xA8,0xA4,0x38,0xEF,0x61,0x30,0x51,
				0x80,0xB3,0x1E,0x79,0x2D,0x44,0xB3,0xD5,0xEF,0xC8,0x54,0x7C,
				0xA9,0x4E,0x97,0x37,0x11,0x99,0x24,0x8B,0x47,0x51,0xE5,0xCF,
				0xA4,0xF0,0xB2,0x9A,0xEE,0xE7,0x16,0x51,0x25,0xF8,0xB7,0x54,
				0x16,0x2F,0x3A,0xBF,0xDF,0xAA,0x05,0x4B,0x66,0xE3,0x31,0x3F,
				0x87,0x52,0xDB,0x5D,0x7A,0x4E,0xB8,0xAD,0x62,0xEA,0xA0,0xCD,
				0x11,0x46,0xF0,0x1B,0xFC,0x7C,0x15,0x48,0xC7,0x5B,0xC0,0x0D,
				0xB3,0xA5,0x9A,0xFA,0x93,0x51,0x2E,0x7A,0xB4,0xF1,0xE1,0x99,
				0x08,0x6E,0x81,0xF2,0x94,0xD2,0xA2,0x53,
				};
		static unsigned char dh1024_g[]={
				0x02,
				};

		if ((dh=DH_new()) == NULL) return(NULL);
		dh->p=BN_bin2bn(dh1024_p,sizeof(dh1024_p),NULL);
		dh->g=BN_bin2bn(dh1024_g,sizeof(dh1024_g),NULL);
		if ((dh->p == NULL) || (dh->g == NULL))
				{ DH_free(dh); return(NULL); }
	}

	return dh;
}

		
//>>===========================================================================

int OPENSSL_CLASS::openSslPasswordCallback(char* password, int buffersize, int, void* /*const char* */ pwd)

//  DESCRIPTION     : Static method that OpenSSL calls when it needs a password.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : The password is passed as a const char* as the last argument
//<<===========================================================================
{
	int length = strlen((const char *)pwd);
	if (length > buffersize)
	{
		*password = 0;
		return 0;
	}
	else
	{
		strcpy(password, (const char *)pwd);
		return length;
	}
}


//>>===========================================================================

bool OPENSSL_CLASS::loadSsleayLib()

//  DESCRIPTION     : Called to load the OpenSSL library DLLs.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	// load the library
	ssleayHandleM = LoadLibrary("ssleay32.dll");
	if (ssleayHandleM == NULL)
	{
		return false;
	}

	// load all of the function pointers
//	if ((BIO_f_ssl_FP = (BIO_f_ssl_TYPE)GetProcAddress(ssleayHandleM, "BIO_f_ssl")) == NULL) goto err;
//	if ((BIO_new_buffer_ssl_connect_FP = (BIO_new_buffer_ssl_connect_TYPE)GetProcAddress(ssleayHandleM, "BIO_new_buffer_ssl_connect")) == NULL) goto err;
//	if ((BIO_new_ssl_FP = (BIO_new_ssl_TYPE)GetProcAddress(ssleayHandleM, "BIO_new_ssl")) == NULL) goto err;
//	if ((BIO_new_ssl_connect_FP = (BIO_new_ssl_connect_TYPE)GetProcAddress(ssleayHandleM, "BIO_new_ssl_connect")) == NULL) goto err;
//	if ((BIO_ssl_copy_session_id_FP = (BIO_ssl_copy_session_id_TYPE)GetProcAddress(ssleayHandleM, "BIO_ssl_copy_session_id")) == NULL) goto err;
//	if ((BIO_ssl_shutdown_FP = (BIO_ssl_shutdown_TYPE)GetProcAddress(ssleayHandleM, "BIO_ssl_shutdown")) == NULL) goto err;
//	if ((ERR_load_SSL_strings_FP = (ERR_load_SSL_strings_TYPE)GetProcAddress(ssleayHandleM, "ERR_load_SSL_strings")) == NULL) goto err;
	if ((SSL_CIPHER_description_FP = (SSL_CIPHER_description_TYPE)GetProcAddress(ssleayHandleM, "SSL_CIPHER_description")) == NULL) goto err;
//	if ((SSL_CIPHER_get_bits_FP = (SSL_CIPHER_get_bits_TYPE)GetProcAddress(ssleayHandleM, "SSL_CIPHER_get_bits")) == NULL) goto err;
	if ((SSL_CIPHER_get_name_FP = (SSL_CIPHER_get_name_TYPE)GetProcAddress(ssleayHandleM, "SSL_CIPHER_get_name")) == NULL) goto err;
//	if ((SSL_CIPHER_get_version_FP = (SSL_CIPHER_get_version_TYPE)GetProcAddress(ssleayHandleM, "SSL_CIPHER_get_version")) == NULL) goto err;
//	if ((SSL_COMP_add_compression_method_FP = (SSL_COMP_add_compression_method_TYPE)GetProcAddress(ssleayHandleM, "SSL_COMP_add_compression_method")) == NULL) goto err;
//	if ((SSL_CTX_add_client_CA_FP = (SSL_CTX_add_client_CA_TYPE)GetProcAddress(ssleayHandleM, "SSL_CTX_add_client_CA")) == NULL) goto err;
//	if ((SSL_CTX_add_session_FP = (SSL_CTX_add_session_TYPE)GetProcAddress(ssleayHandleM, "SSL_CTX_add_session")) == NULL) goto err;
//	if ((SSL_CTX_callback_ctrl_FP = (SSL_CTX_callback_ctrl_TYPE)GetProcAddress(ssleayHandleM, "SSL_CTX_callback_ctrl")) == NULL) goto err;
//	if ((SSL_CTX_check_private_key_FP = (SSL_CTX_check_private_key_TYPE)GetProcAddress(ssleayHandleM, "SSL_CTX_check_private_key")) == NULL) goto err;
	if ((SSL_CTX_ctrl_FP = (SSL_CTX_ctrl_TYPE)GetProcAddress(ssleayHandleM, "SSL_CTX_ctrl")) == NULL) goto err;
//	if ((SSL_CTX_flush_sessions_FP = (SSL_CTX_flush_sessions_TYPE)GetProcAddress(ssleayHandleM, "SSL_CTX_flush_sessions")) == NULL) goto err;
	if ((SSL_CTX_free_FP = (SSL_CTX_free_TYPE)GetProcAddress(ssleayHandleM, "SSL_CTX_free")) == NULL) goto err;
//	if ((SSL_CTX_get_cert_store_FP = (SSL_CTX_get_cert_store_TYPE)GetProcAddress(ssleayHandleM, "SSL_CTX_get_cert_store")) == NULL) goto err;
//	if ((SSL_CTX_get_client_CA_list_FP = (SSL_CTX_get_client_CA_list_TYPE)GetProcAddress(ssleayHandleM, "SSL_CTX_get_client_CA_list")) == NULL) goto err;
//	if ((SSL_CTX_get_ex_data_FP = (SSL_CTX_get_ex_data_TYPE)GetProcAddress(ssleayHandleM, "SSL_CTX_get_ex_data")) == NULL) goto err;
//	if ((SSL_CTX_get_ex_new_index_FP = (SSL_CTX_get_ex_new_index_TYPE)GetProcAddress(ssleayHandleM, "SSL_CTX_get_ex_new_index")) == NULL) goto err;
//	if ((SSL_CTX_get_quiet_shutdown_FP = (SSL_CTX_get_quiet_shutdown_TYPE)GetProcAddress(ssleayHandleM, "SSL_CTX_get_quiet_shutdown")) == NULL) goto err;
//	if ((SSL_CTX_get_timeout_FP = (SSL_CTX_get_timeout_TYPE)GetProcAddress(ssleayHandleM, "SSL_CTX_get_timeout")) == NULL) goto err;
//	if ((SSL_CTX_get_verify_callback_FP = (SSL_CTX_get_verify_callback_TYPE)GetProcAddress(ssleayHandleM, "SSL_CTX_get_verify_callback")) == NULL) goto err;
//	if ((SSL_CTX_get_verify_depth_FP = (SSL_CTX_get_verify_depth_TYPE)GetProcAddress(ssleayHandleM, "SSL_CTX_get_verify_depth")) == NULL) goto err;
//	if ((SSL_CTX_get_verify_mode_FP = (SSL_CTX_get_verify_mode_TYPE)GetProcAddress(ssleayHandleM, "SSL_CTX_get_verify_mode")) == NULL) goto err;
	if ((SSL_CTX_load_verify_locations_FP = (SSL_CTX_load_verify_locations_TYPE)GetProcAddress(ssleayHandleM, "SSL_CTX_load_verify_locations")) == NULL) goto err;
	if ((SSL_CTX_new_FP = (SSL_CTX_new_TYPE)GetProcAddress(ssleayHandleM, "SSL_CTX_new")) == NULL) goto err;
//	if ((SSL_CTX_remove_session_FP = (SSL_CTX_remove_session_TYPE)GetProcAddress(ssleayHandleM, "SSL_CTX_remove_session")) == NULL) goto err;
//	if ((SSL_CTX_sessions_FP = (SSL_CTX_sessions_TYPE)GetProcAddress(ssleayHandleM, "SSL_CTX_sessions")) == NULL) goto err;
//	if ((SSL_CTX_set_cert_store_FP = (SSL_CTX_set_cert_store_TYPE)GetProcAddress(ssleayHandleM, "SSL_CTX_set_cert_store")) == NULL) goto err;
//	if ((SSL_CTX_set_cert_verify_callback_FP = (SSL_CTX_set_cert_verify_callback_TYPE)GetProcAddress(ssleayHandleM, "SSL_CTX_set_cert_verify_callback")) == NULL) goto err;
	if ((SSL_CTX_set_cipher_list_FP = (SSL_CTX_set_cipher_list_TYPE)GetProcAddress(ssleayHandleM, "SSL_CTX_set_cipher_list")) == NULL) goto err;
	if ((SSL_CTX_set_client_CA_list_FP = (SSL_CTX_set_client_CA_list_TYPE)GetProcAddress(ssleayHandleM, "SSL_CTX_set_client_CA_list")) == NULL) goto err;
	if ((SSL_CTX_set_default_passwd_cb_FP = (SSL_CTX_set_default_passwd_cb_TYPE)GetProcAddress(ssleayHandleM, "SSL_CTX_set_default_passwd_cb")) == NULL) goto err;
	if ((SSL_CTX_set_default_passwd_cb_userdata_FP = (SSL_CTX_set_default_passwd_cb_userdata_TYPE)GetProcAddress(ssleayHandleM, "SSL_CTX_set_default_passwd_cb_userdata")) == NULL) goto err;
//	if ((SSL_CTX_set_default_verify_paths_FP = (SSL_CTX_set_default_verify_paths_TYPE)GetProcAddress(ssleayHandleM, "SSL_CTX_set_default_verify_paths")) == NULL) goto err;
//	if ((SSL_CTX_set_ex_data_FP = (SSL_CTX_set_ex_data_TYPE)GetProcAddress(ssleayHandleM, "SSL_CTX_set_ex_data")) == NULL) goto err;
//	if ((SSL_CTX_set_generate_session_id_FP = (SSL_CTX_set_generate_session_id_TYPE)GetProcAddress(ssleayHandleM, "SSL_CTX_set_generate_session_id")) == NULL) goto err;
	if ((SSL_CTX_set_msg_callback_FP = (SSL_CTX_set_msg_callback_TYPE)GetProcAddress(ssleayHandleM, "SSL_CTX_set_msg_callback")) == NULL) goto err;
//	if ((SSL_CTX_set_purpose_FP = (SSL_CTX_set_purpose_TYPE)GetProcAddress(ssleayHandleM, "SSL_CTX_set_purpose")) == NULL) goto err;
//	if ((SSL_CTX_set_quiet_shutdown_FP = (SSL_CTX_set_quiet_shutdown_TYPE)GetProcAddress(ssleayHandleM, "SSL_CTX_set_quiet_shutdown")) == NULL) goto err;
	if ((SSL_CTX_set_session_id_context_FP = (SSL_CTX_set_session_id_context_TYPE)GetProcAddress(ssleayHandleM, "SSL_CTX_set_session_id_context")) == NULL) goto err;
//	if ((SSL_CTX_set_ssl_version_FP = (SSL_CTX_set_ssl_version_TYPE)GetProcAddress(ssleayHandleM, "SSL_CTX_set_ssl_version")) == NULL) goto err;
	if ((SSL_CTX_set_timeout_FP = (SSL_CTX_set_timeout_TYPE)GetProcAddress(ssleayHandleM, "SSL_CTX_set_timeout")) == NULL) goto err;
	if ((SSL_CTX_set_tmp_dh_callback_FP = (SSL_CTX_set_tmp_dh_callback_TYPE)GetProcAddress(ssleayHandleM, "SSL_CTX_set_tmp_dh_callback")) == NULL) goto err;
//	if ((SSL_CTX_set_tmp_rsa_callback_FP = (SSL_CTX_set_tmp_rsa_callback_TYPE)GetProcAddress(ssleayHandleM, "SSL_CTX_set_tmp_rsa_callback")) == NULL) goto err;
//	if ((SSL_CTX_set_trust_FP = (SSL_CTX_set_trust_TYPE)GetProcAddress(ssleayHandleM, "SSL_CTX_set_trust")) == NULL) goto err;
	if ((SSL_CTX_set_verify_FP = (SSL_CTX_set_verify_TYPE)GetProcAddress(ssleayHandleM, "SSL_CTX_set_verify")) == NULL) goto err;
//	if ((SSL_CTX_set_verify_depth_FP = (SSL_CTX_set_verify_depth_TYPE)GetProcAddress(ssleayHandleM, "SSL_CTX_set_verify_depth")) == NULL) goto err;
	if ((SSL_CTX_use_PrivateKey_FP = (SSL_CTX_use_PrivateKey_TYPE)GetProcAddress(ssleayHandleM, "SSL_CTX_use_PrivateKey")) == NULL) goto err;
//	if ((SSL_CTX_use_PrivateKey_ASN1_FP = (SSL_CTX_use_PrivateKey_ASN1_TYPE)GetProcAddress(ssleayHandleM, "SSL_CTX_use_PrivateKey_ASN1")) == NULL) goto err;
	if ((SSL_CTX_use_PrivateKey_file_FP = (SSL_CTX_use_PrivateKey_file_TYPE)GetProcAddress(ssleayHandleM, "SSL_CTX_use_PrivateKey_file")) == NULL) goto err;
//	if ((SSL_CTX_use_RSAPrivateKey_FP = (SSL_CTX_use_RSAPrivateKey_TYPE)GetProcAddress(ssleayHandleM, "SSL_CTX_use_RSAPrivateKey")) == NULL) goto err;
//	if ((SSL_CTX_use_RSAPrivateKey_ASN1_FP = (SSL_CTX_use_RSAPrivateKey_ASN1_TYPE)GetProcAddress(ssleayHandleM, "SSL_CTX_use_RSAPrivateKey_ASN1")) == NULL) goto err;
//	if ((SSL_CTX_use_RSAPrivateKey_file_FP = (SSL_CTX_use_RSAPrivateKey_file_TYPE)GetProcAddress(ssleayHandleM, "SSL_CTX_use_RSAPrivateKey_file")) == NULL) goto err;
	if ((SSL_CTX_use_certificate_FP = (SSL_CTX_use_certificate_TYPE)GetProcAddress(ssleayHandleM, "SSL_CTX_use_certificate")) == NULL) goto err;
//	if ((SSL_CTX_use_certificate_ASN1_FP = (SSL_CTX_use_certificate_ASN1_TYPE)GetProcAddress(ssleayHandleM, "SSL_CTX_use_certificate_ASN1")) == NULL) goto err;
	if ((SSL_CTX_use_certificate_chain_file_FP = (SSL_CTX_use_certificate_chain_file_TYPE)GetProcAddress(ssleayHandleM, "SSL_CTX_use_certificate_chain_file")) == NULL) goto err;
//	if ((SSL_CTX_use_certificate_file_FP = (SSL_CTX_use_certificate_file_TYPE)GetProcAddress(ssleayHandleM, "SSL_CTX_use_certificate_file")) == NULL) goto err;
//	if ((SSL_SESSION_cmp_FP = (SSL_SESSION_cmp_TYPE)GetProcAddress(ssleayHandleM, "SSL_SESSION_cmp")) == NULL) goto err;
	if ((SSL_SESSION_free_FP = (SSL_SESSION_free_TYPE)GetProcAddress(ssleayHandleM, "SSL_SESSION_free")) == NULL) goto err;
//	if ((SSL_SESSION_get_ex_data_FP = (SSL_SESSION_get_ex_data_TYPE)GetProcAddress(ssleayHandleM, "SSL_SESSION_get_ex_data")) == NULL) goto err;
//	if ((SSL_SESSION_get_ex_new_index_FP = (SSL_SESSION_get_ex_new_index_TYPE)GetProcAddress(ssleayHandleM, "SSL_SESSION_get_ex_new_index")) == NULL) goto err;
//	if ((SSL_SESSION_get_time_FP = (SSL_SESSION_get_time_TYPE)GetProcAddress(ssleayHandleM, "SSL_SESSION_get_time")) == NULL) goto err;
//	if ((SSL_SESSION_get_timeout_FP = (SSL_SESSION_get_timeout_TYPE)GetProcAddress(ssleayHandleM, "SSL_SESSION_get_timeout")) == NULL) goto err;
//	if ((SSL_SESSION_hash_FP = (SSL_SESSION_hash_TYPE)GetProcAddress(ssleayHandleM, "SSL_SESSION_hash")) == NULL) goto err;
//	if ((SSL_SESSION_new_FP = (SSL_SESSION_new_TYPE)GetProcAddress(ssleayHandleM, "SSL_SESSION_new")) == NULL) goto err;
//	if ((SSL_SESSION_print_FP = (SSL_SESSION_print_TYPE)GetProcAddress(ssleayHandleM, "SSL_SESSION_print")) == NULL) goto err;
//	if ((SSL_SESSION_print_fp_FP = (SSL_SESSION_print_fp_TYPE)GetProcAddress(ssleayHandleM, "SSL_SESSION_print_fp")) == NULL) goto err;
//	if ((SSL_SESSION_set_ex_data_FP = (SSL_SESSION_set_ex_data_TYPE)GetProcAddress(ssleayHandleM, "SSL_SESSION_set_ex_data")) == NULL) goto err;
//	if ((SSL_SESSION_set_time_FP = (SSL_SESSION_set_time_TYPE)GetProcAddress(ssleayHandleM, "SSL_SESSION_set_time")) == NULL) goto err;
//	if ((SSL_SESSION_set_timeout_FP = (SSL_SESSION_set_timeout_TYPE)GetProcAddress(ssleayHandleM, "SSL_SESSION_set_timeout")) == NULL) goto err;
	if ((SSL_accept_FP = (SSL_accept_TYPE)GetProcAddress(ssleayHandleM, "SSL_accept")) == NULL) goto err;
//	if ((SSL_add_client_CA_FP = (SSL_add_client_CA_TYPE)GetProcAddress(ssleayHandleM, "SSL_add_client_CA")) == NULL) goto err;
//	if ((SSL_add_dir_cert_subjects_to_stack_FP = (SSL_add_dir_cert_subjects_to_stack_TYPE)GetProcAddress(ssleayHandleM, "SSL_add_dir_cert_subjects_to_stack")) == NULL) goto err;
//	if ((SSL_add_file_cert_subjects_to_stack_FP = (SSL_add_file_cert_subjects_to_stack_TYPE)GetProcAddress(ssleayHandleM, "SSL_add_file_cert_subjects_to_stack")) == NULL) goto err;
//	if ((SSL_alert_desc_string_FP = (SSL_alert_desc_string_TYPE)GetProcAddress(ssleayHandleM, "SSL_alert_desc_string")) == NULL) goto err;
//	if ((SSL_alert_desc_string_long_FP = (SSL_alert_desc_string_long_TYPE)GetProcAddress(ssleayHandleM, "SSL_alert_desc_string_long")) == NULL) goto err;
//	if ((SSL_alert_type_string_FP = (SSL_alert_type_string_TYPE)GetProcAddress(ssleayHandleM, "SSL_alert_type_string")) == NULL) goto err;
//	if ((SSL_alert_type_string_long_FP = (SSL_alert_type_string_long_TYPE)GetProcAddress(ssleayHandleM, "SSL_alert_type_string_long")) == NULL) goto err;
//	if ((SSL_callback_ctrl_FP = (SSL_callback_ctrl_TYPE)GetProcAddress(ssleayHandleM, "SSL_callback_ctrl")) == NULL) goto err;
//	if ((SSL_check_private_key_FP = (SSL_check_private_key_TYPE)GetProcAddress(ssleayHandleM, "SSL_check_private_key")) == NULL) goto err;
//	if ((SSL_clear_FP = (SSL_clear_TYPE)GetProcAddress(ssleayHandleM, "SSL_clear")) == NULL) goto err;
	if ((SSL_connect_FP = (SSL_connect_TYPE)GetProcAddress(ssleayHandleM, "SSL_connect")) == NULL) goto err;
//	if ((SSL_copy_session_id_FP = (SSL_copy_session_id_TYPE)GetProcAddress(ssleayHandleM, "SSL_copy_session_id")) == NULL) goto err;
	if ((SSL_ctrl_FP = (SSL_ctrl_TYPE)GetProcAddress(ssleayHandleM, "SSL_ctrl")) == NULL) goto err;
//	if ((SSL_do_handshake_FP = (SSL_do_handshake_TYPE)GetProcAddress(ssleayHandleM, "SSL_do_handshake")) == NULL) goto err;
//	if ((SSL_dup_FP = (SSL_dup_TYPE)GetProcAddress(ssleayHandleM, "SSL_dup")) == NULL) goto err;
//	if ((SSL_dup_CA_list_FP = (SSL_dup_CA_list_TYPE)GetProcAddress(ssleayHandleM, "SSL_dup_CA_list")) == NULL) goto err;
	if ((SSL_free_FP = (SSL_free_TYPE)GetProcAddress(ssleayHandleM, "SSL_free")) == NULL) goto err;
	if ((SSL_get1_session_FP = (SSL_get1_session_TYPE)GetProcAddress(ssleayHandleM, "SSL_get1_session")) == NULL) goto err;
//	if ((SSL_get_SSL_CTX_FP = (SSL_get_SSL_CTX_TYPE)GetProcAddress(ssleayHandleM, "SSL_get_SSL_CTX")) == NULL) goto err;
//	if ((SSL_get_certificate_FP = (SSL_get_certificate_TYPE)GetProcAddress(ssleayHandleM, "SSL_get_certificate")) == NULL) goto err;
//	if ((SSL_get_cipher_list_FP = (SSL_get_cipher_list_TYPE)GetProcAddress(ssleayHandleM, "SSL_get_cipher_list")) == NULL) goto err;
//	if ((SSL_get_ciphers_FP = (SSL_get_ciphers_TYPE)GetProcAddress(ssleayHandleM, "SSL_get_ciphers")) == NULL) goto err;
//	if ((SSL_get_client_CA_list_FP = (SSL_get_client_CA_list_TYPE)GetProcAddress(ssleayHandleM, "SSL_get_client_CA_list")) == NULL) goto err;
	if ((SSL_get_current_cipher_FP = (SSL_get_current_cipher_TYPE)GetProcAddress(ssleayHandleM, "SSL_get_current_cipher")) == NULL) goto err;
//	if ((SSL_get_default_timeout_FP = (SSL_get_default_timeout_TYPE)GetProcAddress(ssleayHandleM, "SSL_get_default_timeout")) == NULL) goto err;
	if ((SSL_get_error_FP = (SSL_get_error_TYPE)GetProcAddress(ssleayHandleM, "SSL_get_error")) == NULL) goto err;
//	if ((SSL_get_ex_data_FP = (SSL_get_ex_data_TYPE)GetProcAddress(ssleayHandleM, "SSL_get_ex_data")) == NULL) goto err;
//	if ((SSL_get_ex_data_X509_STORE_CTX_idx_FP = (SSL_get_ex_data_X509_STORE_CTX_idx_TYPE)GetProcAddress(ssleayHandleM, "SSL_get_ex_data_X509_STORE_CTX_idx")) == NULL) goto err;
//	if ((SSL_get_ex_new_index_FP = (SSL_get_ex_new_index_TYPE)GetProcAddress(ssleayHandleM, "SSL_get_ex_new_index")) == NULL) goto err;
//	if ((SSL_get_fd_FP = (SSL_get_fd_TYPE)GetProcAddress(ssleayHandleM, "SSL_get_fd")) == NULL) goto err;
//	if ((SSL_get_finished_FP = (SSL_get_finished_TYPE)GetProcAddress(ssleayHandleM, "SSL_get_finished")) == NULL) goto err;
//	if ((SSL_get_info_callback_FP = (SSL_get_info_callback_TYPE)GetProcAddress(ssleayHandleM, "SSL_get_info_callback")) == NULL) goto err;
//	if ((SSL_get_peer_cert_chain_FP = (SSL_get_peer_cert_chain_TYPE)GetProcAddress(ssleayHandleM, "SSL_get_peer_cert_chain")) == NULL) goto err;
	if ((SSL_get_peer_certificate_FP = (SSL_get_peer_certificate_TYPE)GetProcAddress(ssleayHandleM, "SSL_get_peer_certificate")) == NULL) goto err;
//	if ((SSL_get_peer_finished_FP = (SSL_get_peer_finished_TYPE)GetProcAddress(ssleayHandleM, "SSL_get_peer_finished")) == NULL) goto err;
//	if ((SSL_get_privatekey_FP = (SSL_get_privatekey_TYPE)GetProcAddress(ssleayHandleM, "SSL_get_privatekey")) == NULL) goto err;
//	if ((SSL_get_quiet_shutdown_FP = (SSL_get_quiet_shutdown_TYPE)GetProcAddress(ssleayHandleM, "SSL_get_quiet_shutdown")) == NULL) goto err;
//	if ((SSL_get_rbio_FP = (SSL_get_rbio_TYPE)GetProcAddress(ssleayHandleM, "SSL_get_rbio")) == NULL) goto err;
//	if ((SSL_get_read_ahead_FP = (SSL_get_read_ahead_TYPE)GetProcAddress(ssleayHandleM, "SSL_get_read_ahead")) == NULL) goto err;
	if ((SSL_get_rfd_FP = (SSL_get_rfd_TYPE)GetProcAddress(ssleayHandleM, "SSL_get_rfd")) == NULL) goto err;
//	if ((SSL_get_session_FP = (SSL_get_session_TYPE)GetProcAddress(ssleayHandleM, "SSL_get_session")) == NULL) goto err;
//	if ((SSL_get_shared_ciphers_FP = (SSL_get_shared_ciphers_TYPE)GetProcAddress(ssleayHandleM, "SSL_get_shared_ciphers")) == NULL) goto err;
//	if ((SSL_get_shutdown_FP = (SSL_get_shutdown_TYPE)GetProcAddress(ssleayHandleM, "SSL_get_shutdown")) == NULL) goto err;
//	if ((SSL_get_ssl_method_FP = (SSL_get_ssl_method_TYPE)GetProcAddress(ssleayHandleM, "SSL_get_ssl_method")) == NULL) goto err;
//	if ((SSL_get_verify_callback_FP = (SSL_get_verify_callback_TYPE)GetProcAddress(ssleayHandleM, "SSL_get_verify_callback")) == NULL) goto err;
//	if ((SSL_get_verify_depth_FP = (SSL_get_verify_depth_TYPE)GetProcAddress(ssleayHandleM, "SSL_get_verify_depth")) == NULL) goto err;
//	if ((SSL_get_verify_mode_FP = (SSL_get_verify_mode_TYPE)GetProcAddress(ssleayHandleM, "SSL_get_verify_mode")) == NULL) goto err;
	if ((SSL_get_verify_result_FP = (SSL_get_verify_result_TYPE)GetProcAddress(ssleayHandleM, "SSL_get_verify_result")) == NULL) goto err;
	if ((SSL_get_version_FP = (SSL_get_version_TYPE)GetProcAddress(ssleayHandleM, "SSL_get_version")) == NULL) goto err;
//	if ((SSL_get_wbio_FP = (SSL_get_wbio_TYPE)GetProcAddress(ssleayHandleM, "SSL_get_wbio")) == NULL) goto err;
	if ((SSL_get_wfd_FP = (SSL_get_wfd_TYPE)GetProcAddress(ssleayHandleM, "SSL_get_wfd")) == NULL) goto err;
//	if ((SSL_has_matching_session_id_FP = (SSL_has_matching_session_id_TYPE)GetProcAddress(ssleayHandleM, "SSL_has_matching_session_id")) == NULL) goto err;
	if ((SSL_library_init_FP = (SSL_library_init_TYPE)GetProcAddress(ssleayHandleM, "SSL_library_init")) == NULL) goto err;
	if ((SSL_load_client_CA_file_FP = (SSL_load_client_CA_file_TYPE)GetProcAddress(ssleayHandleM, "SSL_load_client_CA_file")) == NULL) goto err;
	if ((SSL_load_error_strings_FP = (SSL_load_error_strings_TYPE)GetProcAddress(ssleayHandleM, "SSL_load_error_strings")) == NULL) goto err;
	if ((SSL_new_FP = (SSL_new_TYPE)GetProcAddress(ssleayHandleM, "SSL_new")) == NULL) goto err;
//	if ((SSL_peek_FP = (SSL_peek_TYPE)GetProcAddress(ssleayHandleM, "SSL_peek")) == NULL) goto err;
	if ((SSL_pending_FP = (SSL_pending_TYPE)GetProcAddress(ssleayHandleM, "SSL_pending")) == NULL) goto err;
	if ((SSL_read_FP = (SSL_read_TYPE)GetProcAddress(ssleayHandleM, "SSL_read")) == NULL) goto err;
//	if ((SSL_renegotiate_FP = (SSL_renegotiate_TYPE)GetProcAddress(ssleayHandleM, "SSL_renegotiate")) == NULL) goto err;
//	if ((SSL_renegotiate_pending_FP = (SSL_renegotiate_pending_TYPE)GetProcAddress(ssleayHandleM, "SSL_renegotiate_pending")) == NULL) goto err;
//	if ((SSL_rstate_string_FP = (SSL_rstate_string_TYPE)GetProcAddress(ssleayHandleM, "SSL_rstate_string")) == NULL) goto err;
//	if ((SSL_rstate_string_long_FP = (SSL_rstate_string_long_TYPE)GetProcAddress(ssleayHandleM, "SSL_rstate_string_long")) == NULL) goto err;
	if ((SSL_set_accept_state_FP = (SSL_set_accept_state_TYPE)GetProcAddress(ssleayHandleM, "SSL_set_accept_state")) == NULL) goto err;
	if ((SSL_set_bio_FP = (SSL_set_bio_TYPE)GetProcAddress(ssleayHandleM, "SSL_set_bio")) == NULL) goto err;
//	if ((SSL_set_cipher_list_FP = (SSL_set_cipher_list_TYPE)GetProcAddress(ssleayHandleM, "SSL_set_cipher_list")) == NULL) goto err;
//	if ((SSL_set_client_CA_list_FP = (SSL_set_client_CA_list_TYPE)GetProcAddress(ssleayHandleM, "SSL_set_client_CA_list")) == NULL) goto err;
//	if ((SSL_set_connect_state_FP = (SSL_set_connect_state_TYPE)GetProcAddress(ssleayHandleM, "SSL_set_connect_state")) == NULL) goto err;
//	if ((SSL_set_ex_data_FP = (SSL_set_ex_data_TYPE)GetProcAddress(ssleayHandleM, "SSL_set_ex_data")) == NULL) goto err;
//	if ((SSL_set_fd_FP = (SSL_set_fd_TYPE)GetProcAddress(ssleayHandleM, "SSL_set_fd")) == NULL) goto err;
//	if ((SSL_set_generate_session_id_FP = (SSL_set_generate_session_id_TYPE)GetProcAddress(ssleayHandleM, "SSL_set_generate_session_id")) == NULL) goto err;
//	if ((SSL_set_info_callback_FP = (SSL_set_info_callback_TYPE)GetProcAddress(ssleayHandleM, "SSL_set_info_callback")) == NULL) goto err;
//	if ((SSL_set_msg_callback_FP = (SSL_set_msg_callback_TYPE)GetProcAddress(ssleayHandleM, "SSL_set_msg_callback")) == NULL) goto err;
//	if ((SSL_set_purpose_FP = (SSL_set_purpose_TYPE)GetProcAddress(ssleayHandleM, "SSL_set_purpose")) == NULL) goto err;
//	if ((SSL_set_quiet_shutdown_FP = (SSL_set_quiet_shutdown_TYPE)GetProcAddress(ssleayHandleM, "SSL_set_quiet_shutdown")) == NULL) goto err;
//	if ((SSL_set_read_ahead_FP = (SSL_set_read_ahead_TYPE)GetProcAddress(ssleayHandleM, "SSL_set_read_ahead")) == NULL) goto err;
//	if ((SSL_set_rfd_FP = (SSL_set_rfd_TYPE)GetProcAddress(ssleayHandleM, "SSL_set_rfd")) == NULL) goto err;
	if ((SSL_set_session_FP = (SSL_set_session_TYPE)GetProcAddress(ssleayHandleM, "SSL_set_session")) == NULL) goto err;
//	if ((SSL_set_session_id_context_FP = (SSL_set_session_id_context_TYPE)GetProcAddress(ssleayHandleM, "SSL_set_session_id_context")) == NULL) goto err;
//	if ((SSL_set_shutdown_FP = (SSL_set_shutdown_TYPE)GetProcAddress(ssleayHandleM, "SSL_set_shutdown")) == NULL) goto err;
//	if ((SSL_set_ssl_method_FP = (SSL_set_ssl_method_TYPE)GetProcAddress(ssleayHandleM, "SSL_set_ssl_method")) == NULL) goto err;
//	if ((SSL_set_tmp_dh_callback_FP = (SSL_set_tmp_dh_callback_TYPE)GetProcAddress(ssleayHandleM, "SSL_set_tmp_dh_callback")) == NULL) goto err;
//	if ((SSL_set_tmp_rsa_callback_FP = (SSL_set_tmp_rsa_callback_TYPE)GetProcAddress(ssleayHandleM, "SSL_set_tmp_rsa_callback")) == NULL) goto err;
//	if ((SSL_set_trust_FP = (SSL_set_trust_TYPE)GetProcAddress(ssleayHandleM, "SSL_set_trust")) == NULL) goto err;
//	if ((SSL_set_verify_FP = (SSL_set_verify_TYPE)GetProcAddress(ssleayHandleM, "SSL_set_verify")) == NULL) goto err;
//	if ((SSL_set_verify_depth_FP = (SSL_set_verify_depth_TYPE)GetProcAddress(ssleayHandleM, "SSL_set_verify_depth")) == NULL) goto err;
//	if ((SSL_set_verify_result_FP = (SSL_set_verify_result_TYPE)GetProcAddress(ssleayHandleM, "SSL_set_verify_result")) == NULL) goto err;
//	if ((SSL_set_wfd_FP = (SSL_set_wfd_TYPE)GetProcAddress(ssleayHandleM, "SSL_set_wfd")) == NULL) goto err;
	if ((SSL_shutdown_FP = (SSL_shutdown_TYPE)GetProcAddress(ssleayHandleM, "SSL_shutdown")) == NULL) goto err;
//	if ((SSL_state_FP = (SSL_state_TYPE)GetProcAddress(ssleayHandleM, "SSL_state")) == NULL) goto err;
//	if ((SSL_state_string_FP = (SSL_state_string_TYPE)GetProcAddress(ssleayHandleM, "SSL_state_string")) == NULL) goto err;
//	if ((SSL_state_string_long_FP = (SSL_state_string_long_TYPE)GetProcAddress(ssleayHandleM, "SSL_state_string_long")) == NULL) goto err;
//	if ((SSL_use_PrivateKey_FP = (SSL_use_PrivateKey_TYPE)GetProcAddress(ssleayHandleM, "SSL_use_PrivateKey")) == NULL) goto err;
//	if ((SSL_use_PrivateKey_ASN1_FP = (SSL_use_PrivateKey_ASN1_TYPE)GetProcAddress(ssleayHandleM, "SSL_use_PrivateKey_ASN1")) == NULL) goto err;
//	if ((SSL_use_PrivateKey_file_FP = (SSL_use_PrivateKey_file_TYPE)GetProcAddress(ssleayHandleM, "SSL_use_PrivateKey_file")) == NULL) goto err;
//	if ((SSL_use_RSAPrivateKey_FP = (SSL_use_RSAPrivateKey_TYPE)GetProcAddress(ssleayHandleM, "SSL_use_RSAPrivateKey")) == NULL) goto err;
//	if ((SSL_use_RSAPrivateKey_ASN1_FP = (SSL_use_RSAPrivateKey_ASN1_TYPE)GetProcAddress(ssleayHandleM, "SSL_use_RSAPrivateKey_ASN1")) == NULL) goto err;
//	if ((SSL_use_RSAPrivateKey_file_FP = (SSL_use_RSAPrivateKey_file_TYPE)GetProcAddress(ssleayHandleM, "SSL_use_RSAPrivateKey_file")) == NULL) goto err;
//	if ((SSL_use_certificate_FP = (SSL_use_certificate_TYPE)GetProcAddress(ssleayHandleM, "SSL_use_certificate")) == NULL) goto err;
//	if ((SSL_use_certificate_ASN1_FP = (SSL_use_certificate_ASN1_TYPE)GetProcAddress(ssleayHandleM, "SSL_use_certificate_ASN1")) == NULL) goto err;
//	if ((SSL_use_certificate_file_FP = (SSL_use_certificate_file_TYPE)GetProcAddress(ssleayHandleM, "SSL_use_certificate_file")) == NULL) goto err;
//	if ((SSL_version_FP = (SSL_version_TYPE)GetProcAddress(ssleayHandleM, "SSL_version")) == NULL) goto err;
//	if ((SSL_want_FP = (SSL_want_TYPE)GetProcAddress(ssleayHandleM, "SSL_want")) == NULL) goto err;
	if ((SSL_write_FP = (SSL_write_TYPE)GetProcAddress(ssleayHandleM, "SSL_write")) == NULL) goto err;
//	if ((SSLv23_client_method_FP = (SSLv23_client_method_TYPE)GetProcAddress(ssleayHandleM, "SSLv23_client_method")) == NULL) goto err;
	if ((SSLv23_method_FP = (SSLv23_method_TYPE)GetProcAddress(ssleayHandleM, "SSLv23_method")) == NULL) goto err;
	if ((SSLv23_server_method_FP = (SSLv23_server_method_TYPE)GetProcAddress(ssleayHandleM, "SSLv23_server_method")) == NULL) goto err;
//	if ((SSLv2_client_method_FP = (SSLv2_client_method_TYPE)GetProcAddress(ssleayHandleM, "SSLv2_client_method")) == NULL) goto err;
//	if ((SSLv2_method_FP = (SSLv2_method_TYPE)GetProcAddress(ssleayHandleM, "SSLv2_method")) == NULL) goto err;
//	if ((SSLv2_server_method_FP = (SSLv2_server_method_TYPE)GetProcAddress(ssleayHandleM, "SSLv2_server_method")) == NULL) goto err;
//	if ((SSLv3_client_method_FP = (SSLv3_client_method_TYPE)GetProcAddress(ssleayHandleM, "SSLv3_client_method")) == NULL) goto err;
	if ((SSLv3_method_FP = (SSLv3_method_TYPE)GetProcAddress(ssleayHandleM, "SSLv3_method")) == NULL) goto err;
//	if ((SSLv3_server_method_FP = (SSLv3_server_method_TYPE)GetProcAddress(ssleayHandleM, "SSLv3_server_method")) == NULL) goto err;
//	if ((TLSv1_client_method_FP = (TLSv1_client_method_TYPE)GetProcAddress(ssleayHandleM, "TLSv1_client_method")) == NULL) goto err;
	if ((TLSv1_method_FP = (TLSv1_method_TYPE)GetProcAddress(ssleayHandleM, "TLSv1_method")) == NULL) goto err;
	if ((TLSv1_1_method_FP = (TLSv1_1_method_TYPE)GetProcAddress(ssleayHandleM, "TLSv1_1_method")) == NULL) goto err;
	if ((TLSv1_2_method_FP = (TLSv1_2_method_TYPE)GetProcAddress(ssleayHandleM, "TLSv1_2_method")) == NULL) goto err;
//	if ((TLSv1_server_method_FP = (TLSv1_server_method_TYPE)GetProcAddress(ssleayHandleM, "TLSv1_server_method")) == NULL) goto err;
//	if ((d2i_SSL_SESSION_FP = (d2i_SSL_SESSION_TYPE)GetProcAddress(ssleayHandleM, "d2i_SSL_SESSION")) == NULL) goto err;
//	if ((i2d_SSL_SESSION_FP = (i2d_SSL_SESSION_TYPE)GetProcAddress(ssleayHandleM, "i2d_SSL_SESSION")) == NULL) goto err;

	return true;

err:
	if (ssleayHandleM != NULL)
	{
		FreeLibrary(ssleayHandleM);
		ssleayHandleM = NULL;
	}

	return false;
}


//>>===========================================================================

bool OPENSSL_CLASS::loadLibeayLib()

//  DESCRIPTION     : Called to load the OpenSSL library DLLs.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	// load the library
	libeayHandleM = LoadLibrary("libeay32.dll");
	if (libeayHandleM == NULL)
	{
		return false;
	}

	// load all of the function pointers
//	if ((SSLeay_FP = (SSLeay_TYPE)GetProcAddress(libeayHandleM, "SSLeay")) == NULL) goto err;
//	if ((ACCESS_DESCRIPTION_free_FP = (ACCESS_DESCRIPTION_free_TYPE)GetProcAddress(libeayHandleM, "ACCESS_DESCRIPTION_free")) == NULL) goto err;
//	if ((ACCESS_DESCRIPTION_it_FP = (ACCESS_DESCRIPTION_it_TYPE)GetProcAddress(libeayHandleM, "ACCESS_DESCRIPTION_it")) == NULL) goto err;
//	if ((ACCESS_DESCRIPTION_new_FP = (ACCESS_DESCRIPTION_new_TYPE)GetProcAddress(libeayHandleM, "ACCESS_DESCRIPTION_new")) == NULL) goto err;
//	if ((AES_cbc_encrypt_FP = (AES_cbc_encrypt_TYPE)GetProcAddress(libeayHandleM, "AES_cbc_encrypt")) == NULL) goto err;
//	if ((AES_cfb128_encrypt_FP = (AES_cfb128_encrypt_TYPE)GetProcAddress(libeayHandleM, "AES_cfb128_encrypt")) == NULL) goto err;
//	if ((AES_ctr128_encrypt_FP = (AES_ctr128_encrypt_TYPE)GetProcAddress(libeayHandleM, "AES_ctr128_encrypt")) == NULL) goto err;
//	if ((AES_decrypt_FP = (AES_decrypt_TYPE)GetProcAddress(libeayHandleM, "AES_decrypt")) == NULL) goto err;
//	if ((AES_ecb_encrypt_FP = (AES_ecb_encrypt_TYPE)GetProcAddress(libeayHandleM, "AES_ecb_encrypt")) == NULL) goto err;
//	if ((AES_encrypt_FP = (AES_encrypt_TYPE)GetProcAddress(libeayHandleM, "AES_encrypt")) == NULL) goto err;
//	if ((AES_ofb128_encrypt_FP = (AES_ofb128_encrypt_TYPE)GetProcAddress(libeayHandleM, "AES_ofb128_encrypt")) == NULL) goto err;
//	if ((AES_options_FP = (AES_options_TYPE)GetProcAddress(libeayHandleM, "AES_options")) == NULL) goto err;
//	if ((AES_set_decrypt_key_FP = (AES_set_decrypt_key_TYPE)GetProcAddress(libeayHandleM, "AES_set_decrypt_key")) == NULL) goto err;
//	if ((AES_set_encrypt_key_FP = (AES_set_encrypt_key_TYPE)GetProcAddress(libeayHandleM, "AES_set_encrypt_key")) == NULL) goto err;
//	if ((ASN1_ANY_it_FP = (ASN1_ANY_it_TYPE)GetProcAddress(libeayHandleM, "ASN1_ANY_it")) == NULL) goto err;
//	if ((ASN1_BIT_STRING_asn1_meth_FP = (ASN1_BIT_STRING_asn1_meth_TYPE)GetProcAddress(libeayHandleM, "ASN1_BIT_STRING_asn1_meth")) == NULL) goto err;
//	if ((ASN1_BIT_STRING_free_FP = (ASN1_BIT_STRING_free_TYPE)GetProcAddress(libeayHandleM, "ASN1_BIT_STRING_free")) == NULL) goto err;
//	if ((ASN1_BIT_STRING_get_bit_FP = (ASN1_BIT_STRING_get_bit_TYPE)GetProcAddress(libeayHandleM, "ASN1_BIT_STRING_get_bit")) == NULL) goto err;
//	if ((ASN1_BIT_STRING_it_FP = (ASN1_BIT_STRING_it_TYPE)GetProcAddress(libeayHandleM, "ASN1_BIT_STRING_it")) == NULL) goto err;
//	if ((ASN1_BIT_STRING_name_print_FP = (ASN1_BIT_STRING_name_print_TYPE)GetProcAddress(libeayHandleM, "ASN1_BIT_STRING_name_print")) == NULL) goto err;
//	if ((ASN1_BIT_STRING_new_FP = (ASN1_BIT_STRING_new_TYPE)GetProcAddress(libeayHandleM, "ASN1_BIT_STRING_new")) == NULL) goto err;
//	if ((ASN1_BIT_STRING_num_asc_FP = (ASN1_BIT_STRING_num_asc_TYPE)GetProcAddress(libeayHandleM, "ASN1_BIT_STRING_num_asc")) == NULL) goto err;
//	if ((ASN1_BIT_STRING_set_FP = (ASN1_BIT_STRING_set_TYPE)GetProcAddress(libeayHandleM, "ASN1_BIT_STRING_set")) == NULL) goto err;
//	if ((ASN1_BIT_STRING_set_asc_FP = (ASN1_BIT_STRING_set_asc_TYPE)GetProcAddress(libeayHandleM, "ASN1_BIT_STRING_set_asc")) == NULL) goto err;
//	if ((ASN1_BIT_STRING_set_bit_FP = (ASN1_BIT_STRING_set_bit_TYPE)GetProcAddress(libeayHandleM, "ASN1_BIT_STRING_set_bit")) == NULL) goto err;
//	if ((ASN1_BMPSTRING_free_FP = (ASN1_BMPSTRING_free_TYPE)GetProcAddress(libeayHandleM, "ASN1_BMPSTRING_free")) == NULL) goto err;
//	if ((ASN1_BMPSTRING_it_FP = (ASN1_BMPSTRING_it_TYPE)GetProcAddress(libeayHandleM, "ASN1_BMPSTRING_it")) == NULL) goto err;
//	if ((ASN1_BMPSTRING_new_FP = (ASN1_BMPSTRING_new_TYPE)GetProcAddress(libeayHandleM, "ASN1_BMPSTRING_new")) == NULL) goto err;
//	if ((ASN1_BOOLEAN_it_FP = (ASN1_BOOLEAN_it_TYPE)GetProcAddress(libeayHandleM, "ASN1_BOOLEAN_it")) == NULL) goto err;
//	if ((ASN1_ENUMERATED_free_FP = (ASN1_ENUMERATED_free_TYPE)GetProcAddress(libeayHandleM, "ASN1_ENUMERATED_free")) == NULL) goto err;
//	if ((ASN1_ENUMERATED_get_FP = (ASN1_ENUMERATED_get_TYPE)GetProcAddress(libeayHandleM, "ASN1_ENUMERATED_get")) == NULL) goto err;
//	if ((ASN1_ENUMERATED_it_FP = (ASN1_ENUMERATED_it_TYPE)GetProcAddress(libeayHandleM, "ASN1_ENUMERATED_it")) == NULL) goto err;
//	if ((ASN1_ENUMERATED_new_FP = (ASN1_ENUMERATED_new_TYPE)GetProcAddress(libeayHandleM, "ASN1_ENUMERATED_new")) == NULL) goto err;
//	if ((ASN1_ENUMERATED_set_FP = (ASN1_ENUMERATED_set_TYPE)GetProcAddress(libeayHandleM, "ASN1_ENUMERATED_set")) == NULL) goto err;
//	if ((ASN1_ENUMERATED_to_BN_FP = (ASN1_ENUMERATED_to_BN_TYPE)GetProcAddress(libeayHandleM, "ASN1_ENUMERATED_to_BN")) == NULL) goto err;
//	if ((ASN1_FBOOLEAN_it_FP = (ASN1_FBOOLEAN_it_TYPE)GetProcAddress(libeayHandleM, "ASN1_FBOOLEAN_it")) == NULL) goto err;
//	if ((ASN1_GENERALIZEDTIME_check_FP = (ASN1_GENERALIZEDTIME_check_TYPE)GetProcAddress(libeayHandleM, "ASN1_GENERALIZEDTIME_check")) == NULL) goto err;
//	if ((ASN1_GENERALIZEDTIME_free_FP = (ASN1_GENERALIZEDTIME_free_TYPE)GetProcAddress(libeayHandleM, "ASN1_GENERALIZEDTIME_free")) == NULL) goto err;
//	if ((ASN1_GENERALIZEDTIME_it_FP = (ASN1_GENERALIZEDTIME_it_TYPE)GetProcAddress(libeayHandleM, "ASN1_GENERALIZEDTIME_it")) == NULL) goto err;
//	if ((ASN1_GENERALIZEDTIME_new_FP = (ASN1_GENERALIZEDTIME_new_TYPE)GetProcAddress(libeayHandleM, "ASN1_GENERALIZEDTIME_new")) == NULL) goto err;
//	if ((ASN1_GENERALIZEDTIME_print_FP = (ASN1_GENERALIZEDTIME_print_TYPE)GetProcAddress(libeayHandleM, "ASN1_GENERALIZEDTIME_print")) == NULL) goto err;
//	if ((ASN1_GENERALIZEDTIME_set_FP = (ASN1_GENERALIZEDTIME_set_TYPE)GetProcAddress(libeayHandleM, "ASN1_GENERALIZEDTIME_set")) == NULL) goto err;
//	if ((ASN1_GENERALIZEDTIME_set_string_FP = (ASN1_GENERALIZEDTIME_set_string_TYPE)GetProcAddress(libeayHandleM, "ASN1_GENERALIZEDTIME_set_string")) == NULL) goto err;
//	if ((ASN1_GENERALSTRING_free_FP = (ASN1_GENERALSTRING_free_TYPE)GetProcAddress(libeayHandleM, "ASN1_GENERALSTRING_free")) == NULL) goto err;
//	if ((ASN1_GENERALSTRING_it_FP = (ASN1_GENERALSTRING_it_TYPE)GetProcAddress(libeayHandleM, "ASN1_GENERALSTRING_it")) == NULL) goto err;
//	if ((ASN1_GENERALSTRING_new_FP = (ASN1_GENERALSTRING_new_TYPE)GetProcAddress(libeayHandleM, "ASN1_GENERALSTRING_new")) == NULL) goto err;
//	if ((ASN1_HEADER_free_FP = (ASN1_HEADER_free_TYPE)GetProcAddress(libeayHandleM, "ASN1_HEADER_free")) == NULL) goto err;
//	if ((ASN1_HEADER_new_FP = (ASN1_HEADER_new_TYPE)GetProcAddress(libeayHandleM, "ASN1_HEADER_new")) == NULL) goto err;
//	if ((ASN1_IA5STRING_asn1_meth_FP = (ASN1_IA5STRING_asn1_meth_TYPE)GetProcAddress(libeayHandleM, "ASN1_IA5STRING_asn1_meth")) == NULL) goto err;
//	if ((ASN1_IA5STRING_free_FP = (ASN1_IA5STRING_free_TYPE)GetProcAddress(libeayHandleM, "ASN1_IA5STRING_free")) == NULL) goto err;
//	if ((ASN1_IA5STRING_it_FP = (ASN1_IA5STRING_it_TYPE)GetProcAddress(libeayHandleM, "ASN1_IA5STRING_it")) == NULL) goto err;
//	if ((ASN1_IA5STRING_new_FP = (ASN1_IA5STRING_new_TYPE)GetProcAddress(libeayHandleM, "ASN1_IA5STRING_new")) == NULL) goto err;
//	if ((ASN1_INTEGER_cmp_FP = (ASN1_INTEGER_cmp_TYPE)GetProcAddress(libeayHandleM, "ASN1_INTEGER_cmp")) == NULL) goto err;
//	if ((ASN1_INTEGER_dup_FP = (ASN1_INTEGER_dup_TYPE)GetProcAddress(libeayHandleM, "ASN1_INTEGER_dup")) == NULL) goto err;
//	if ((ASN1_INTEGER_free_FP = (ASN1_INTEGER_free_TYPE)GetProcAddress(libeayHandleM, "ASN1_INTEGER_free")) == NULL) goto err;
	if ((ASN1_INTEGER_get_FP = (ASN1_INTEGER_get_TYPE)GetProcAddress(libeayHandleM, "ASN1_INTEGER_get")) == NULL) goto err;
//	if ((ASN1_INTEGER_it_FP = (ASN1_INTEGER_it_TYPE)GetProcAddress(libeayHandleM, "ASN1_INTEGER_it")) == NULL) goto err;
//	if ((ASN1_INTEGER_new_FP = (ASN1_INTEGER_new_TYPE)GetProcAddress(libeayHandleM, "ASN1_INTEGER_new")) == NULL) goto err;
	if ((ASN1_INTEGER_set_FP = (ASN1_INTEGER_set_TYPE)GetProcAddress(libeayHandleM, "ASN1_INTEGER_set")) == NULL) goto err;
//	if ((ASN1_INTEGER_to_BN_FP = (ASN1_INTEGER_to_BN_TYPE)GetProcAddress(libeayHandleM, "ASN1_INTEGER_to_BN")) == NULL) goto err;
//	if ((ASN1_NULL_free_FP = (ASN1_NULL_free_TYPE)GetProcAddress(libeayHandleM, "ASN1_NULL_free")) == NULL) goto err;
//	if ((ASN1_NULL_it_FP = (ASN1_NULL_it_TYPE)GetProcAddress(libeayHandleM, "ASN1_NULL_it")) == NULL) goto err;
//	if ((ASN1_NULL_new_FP = (ASN1_NULL_new_TYPE)GetProcAddress(libeayHandleM, "ASN1_NULL_new")) == NULL) goto err;
//	if ((ASN1_OBJECT_create_FP = (ASN1_OBJECT_create_TYPE)GetProcAddress(libeayHandleM, "ASN1_OBJECT_create")) == NULL) goto err;
//	if ((ASN1_OBJECT_free_FP = (ASN1_OBJECT_free_TYPE)GetProcAddress(libeayHandleM, "ASN1_OBJECT_free")) == NULL) goto err;
//	if ((ASN1_OBJECT_it_FP = (ASN1_OBJECT_it_TYPE)GetProcAddress(libeayHandleM, "ASN1_OBJECT_it")) == NULL) goto err;
//	if ((ASN1_OBJECT_new_FP = (ASN1_OBJECT_new_TYPE)GetProcAddress(libeayHandleM, "ASN1_OBJECT_new")) == NULL) goto err;
//	if ((ASN1_OCTET_STRING_cmp_FP = (ASN1_OCTET_STRING_cmp_TYPE)GetProcAddress(libeayHandleM, "ASN1_OCTET_STRING_cmp")) == NULL) goto err;
//	if ((ASN1_OCTET_STRING_dup_FP = (ASN1_OCTET_STRING_dup_TYPE)GetProcAddress(libeayHandleM, "ASN1_OCTET_STRING_dup")) == NULL) goto err;
//	if ((ASN1_OCTET_STRING_free_FP = (ASN1_OCTET_STRING_free_TYPE)GetProcAddress(libeayHandleM, "ASN1_OCTET_STRING_free")) == NULL) goto err;
//	if ((ASN1_OCTET_STRING_it_FP = (ASN1_OCTET_STRING_it_TYPE)GetProcAddress(libeayHandleM, "ASN1_OCTET_STRING_it")) == NULL) goto err;
//	if ((ASN1_OCTET_STRING_new_FP = (ASN1_OCTET_STRING_new_TYPE)GetProcAddress(libeayHandleM, "ASN1_OCTET_STRING_new")) == NULL) goto err;
//	if ((ASN1_OCTET_STRING_set_FP = (ASN1_OCTET_STRING_set_TYPE)GetProcAddress(libeayHandleM, "ASN1_OCTET_STRING_set")) == NULL) goto err;
//	if ((ASN1_PRINTABLESTRING_free_FP = (ASN1_PRINTABLESTRING_free_TYPE)GetProcAddress(libeayHandleM, "ASN1_PRINTABLESTRING_free")) == NULL) goto err;
//	if ((ASN1_PRINTABLESTRING_it_FP = (ASN1_PRINTABLESTRING_it_TYPE)GetProcAddress(libeayHandleM, "ASN1_PRINTABLESTRING_it")) == NULL) goto err;
//	if ((ASN1_PRINTABLESTRING_new_FP = (ASN1_PRINTABLESTRING_new_TYPE)GetProcAddress(libeayHandleM, "ASN1_PRINTABLESTRING_new")) == NULL) goto err;
//	if ((ASN1_PRINTABLE_free_FP = (ASN1_PRINTABLE_free_TYPE)GetProcAddress(libeayHandleM, "ASN1_PRINTABLE_free")) == NULL) goto err;
//	if ((ASN1_PRINTABLE_it_FP = (ASN1_PRINTABLE_it_TYPE)GetProcAddress(libeayHandleM, "ASN1_PRINTABLE_it")) == NULL) goto err;
//	if ((ASN1_PRINTABLE_new_FP = (ASN1_PRINTABLE_new_TYPE)GetProcAddress(libeayHandleM, "ASN1_PRINTABLE_new")) == NULL) goto err;
//	if ((ASN1_PRINTABLE_type_FP = (ASN1_PRINTABLE_type_TYPE)GetProcAddress(libeayHandleM, "ASN1_PRINTABLE_type")) == NULL) goto err;
//	if ((ASN1_SEQUENCE_it_FP = (ASN1_SEQUENCE_it_TYPE)GetProcAddress(libeayHandleM, "ASN1_SEQUENCE_it")) == NULL) goto err;
//	if ((ASN1_STRING_TABLE_add_FP = (ASN1_STRING_TABLE_add_TYPE)GetProcAddress(libeayHandleM, "ASN1_STRING_TABLE_add")) == NULL) goto err;
//	if ((ASN1_STRING_TABLE_cleanup_FP = (ASN1_STRING_TABLE_cleanup_TYPE)GetProcAddress(libeayHandleM, "ASN1_STRING_TABLE_cleanup")) == NULL) goto err;
//	if ((ASN1_STRING_TABLE_get_FP = (ASN1_STRING_TABLE_get_TYPE)GetProcAddress(libeayHandleM, "ASN1_STRING_TABLE_get")) == NULL) goto err;
//	if ((ASN1_STRING_cmp_FP = (ASN1_STRING_cmp_TYPE)GetProcAddress(libeayHandleM, "ASN1_STRING_cmp")) == NULL) goto err;
//	if ((ASN1_STRING_data_FP = (ASN1_STRING_data_TYPE)GetProcAddress(libeayHandleM, "ASN1_STRING_data")) == NULL) goto err;
//	if ((ASN1_STRING_dup_FP = (ASN1_STRING_dup_TYPE)GetProcAddress(libeayHandleM, "ASN1_STRING_dup")) == NULL) goto err;
//	if ((ASN1_STRING_encode_FP = (ASN1_STRING_encode_TYPE)GetProcAddress(libeayHandleM, "ASN1_STRING_encode")) == NULL) goto err;
//	if ((ASN1_STRING_free_FP = (ASN1_STRING_free_TYPE)GetProcAddress(libeayHandleM, "ASN1_STRING_free")) == NULL) goto err;
//	if ((ASN1_STRING_get_default_mask_FP = (ASN1_STRING_get_default_mask_TYPE)GetProcAddress(libeayHandleM, "ASN1_STRING_get_default_mask")) == NULL) goto err;
//	if ((ASN1_STRING_length_FP = (ASN1_STRING_length_TYPE)GetProcAddress(libeayHandleM, "ASN1_STRING_length")) == NULL) goto err;
//	if ((ASN1_STRING_length_set_FP = (ASN1_STRING_length_set_TYPE)GetProcAddress(libeayHandleM, "ASN1_STRING_length_set")) == NULL) goto err;
//	if ((ASN1_STRING_new_FP = (ASN1_STRING_new_TYPE)GetProcAddress(libeayHandleM, "ASN1_STRING_new")) == NULL) goto err;
//	if ((ASN1_STRING_print_FP = (ASN1_STRING_print_TYPE)GetProcAddress(libeayHandleM, "ASN1_STRING_print")) == NULL) goto err;
//	if ((ASN1_STRING_print_ex_FP = (ASN1_STRING_print_ex_TYPE)GetProcAddress(libeayHandleM, "ASN1_STRING_print_ex")) == NULL) goto err;
//	if ((ASN1_STRING_print_ex_fp_FP = (ASN1_STRING_print_ex_fp_TYPE)GetProcAddress(libeayHandleM, "ASN1_STRING_print_ex_fp")) == NULL) goto err;
//	if ((ASN1_STRING_set_FP = (ASN1_STRING_set_TYPE)GetProcAddress(libeayHandleM, "ASN1_STRING_set")) == NULL) goto err;
//	if ((ASN1_STRING_set_by_NID_FP = (ASN1_STRING_set_by_NID_TYPE)GetProcAddress(libeayHandleM, "ASN1_STRING_set_by_NID")) == NULL) goto err;
//	if ((ASN1_STRING_set_default_mask_FP = (ASN1_STRING_set_default_mask_TYPE)GetProcAddress(libeayHandleM, "ASN1_STRING_set_default_mask")) == NULL) goto err;
//	if ((ASN1_STRING_set_default_mask_asc_FP = (ASN1_STRING_set_default_mask_asc_TYPE)GetProcAddress(libeayHandleM, "ASN1_STRING_set_default_mask_asc")) == NULL) goto err;
//	if ((ASN1_STRING_to_UTF8_FP = (ASN1_STRING_to_UTF8_TYPE)GetProcAddress(libeayHandleM, "ASN1_STRING_to_UTF8")) == NULL) goto err;
//	if ((ASN1_STRING_type_FP = (ASN1_STRING_type_TYPE)GetProcAddress(libeayHandleM, "ASN1_STRING_type")) == NULL) goto err;
//	if ((ASN1_STRING_type_new_FP = (ASN1_STRING_type_new_TYPE)GetProcAddress(libeayHandleM, "ASN1_STRING_type_new")) == NULL) goto err;
//	if ((ASN1_T61STRING_free_FP = (ASN1_T61STRING_free_TYPE)GetProcAddress(libeayHandleM, "ASN1_T61STRING_free")) == NULL) goto err;
//	if ((ASN1_T61STRING_it_FP = (ASN1_T61STRING_it_TYPE)GetProcAddress(libeayHandleM, "ASN1_T61STRING_it")) == NULL) goto err;
//	if ((ASN1_T61STRING_new_FP = (ASN1_T61STRING_new_TYPE)GetProcAddress(libeayHandleM, "ASN1_T61STRING_new")) == NULL) goto err;
//	if ((ASN1_TBOOLEAN_it_FP = (ASN1_TBOOLEAN_it_TYPE)GetProcAddress(libeayHandleM, "ASN1_TBOOLEAN_it")) == NULL) goto err;
//	if ((ASN1_TIME_check_FP = (ASN1_TIME_check_TYPE)GetProcAddress(libeayHandleM, "ASN1_TIME_check")) == NULL) goto err;
//	if ((ASN1_TIME_free_FP = (ASN1_TIME_free_TYPE)GetProcAddress(libeayHandleM, "ASN1_TIME_free")) == NULL) goto err;
//	if ((ASN1_TIME_it_FP = (ASN1_TIME_it_TYPE)GetProcAddress(libeayHandleM, "ASN1_TIME_it")) == NULL) goto err;
//	if ((ASN1_TIME_new_FP = (ASN1_TIME_new_TYPE)GetProcAddress(libeayHandleM, "ASN1_TIME_new")) == NULL) goto err;
//	if ((ASN1_TIME_print_FP = (ASN1_TIME_print_TYPE)GetProcAddress(libeayHandleM, "ASN1_TIME_print")) == NULL) goto err;
//	if ((ASN1_TIME_set_FP = (ASN1_TIME_set_TYPE)GetProcAddress(libeayHandleM, "ASN1_TIME_set")) == NULL) goto err;
//	if ((ASN1_TIME_to_generalizedtime_FP = (ASN1_TIME_to_generalizedtime_TYPE)GetProcAddress(libeayHandleM, "ASN1_TIME_to_generalizedtime")) == NULL) goto err;
	if ((ASN1_TYPE_free_FP = (ASN1_TYPE_free_TYPE)GetProcAddress(libeayHandleM, "ASN1_TYPE_free")) == NULL) goto err;
//	if ((ASN1_TYPE_get_FP = (ASN1_TYPE_get_TYPE)GetProcAddress(libeayHandleM, "ASN1_TYPE_get")) == NULL) goto err;
//	if ((ASN1_TYPE_get_int_octetstring_FP = (ASN1_TYPE_get_int_octetstring_TYPE)GetProcAddress(libeayHandleM, "ASN1_TYPE_get_int_octetstring")) == NULL) goto err;
//	if ((ASN1_TYPE_get_octetstring_FP = (ASN1_TYPE_get_octetstring_TYPE)GetProcAddress(libeayHandleM, "ASN1_TYPE_get_octetstring")) == NULL) goto err;
//	if ((ASN1_TYPE_new_FP = (ASN1_TYPE_new_TYPE)GetProcAddress(libeayHandleM, "ASN1_TYPE_new")) == NULL) goto err;
//	if ((ASN1_TYPE_set_FP = (ASN1_TYPE_set_TYPE)GetProcAddress(libeayHandleM, "ASN1_TYPE_set")) == NULL) goto err;
//	if ((ASN1_TYPE_set_int_octetstring_FP = (ASN1_TYPE_set_int_octetstring_TYPE)GetProcAddress(libeayHandleM, "ASN1_TYPE_set_int_octetstring")) == NULL) goto err;
//	if ((ASN1_TYPE_set_octetstring_FP = (ASN1_TYPE_set_octetstring_TYPE)GetProcAddress(libeayHandleM, "ASN1_TYPE_set_octetstring")) == NULL) goto err;
//	if ((ASN1_UNIVERSALSTRING_free_FP = (ASN1_UNIVERSALSTRING_free_TYPE)GetProcAddress(libeayHandleM, "ASN1_UNIVERSALSTRING_free")) == NULL) goto err;
//	if ((ASN1_UNIVERSALSTRING_it_FP = (ASN1_UNIVERSALSTRING_it_TYPE)GetProcAddress(libeayHandleM, "ASN1_UNIVERSALSTRING_it")) == NULL) goto err;
//	if ((ASN1_UNIVERSALSTRING_new_FP = (ASN1_UNIVERSALSTRING_new_TYPE)GetProcAddress(libeayHandleM, "ASN1_UNIVERSALSTRING_new")) == NULL) goto err;
//	if ((ASN1_UNIVERSALSTRING_to_string_FP = (ASN1_UNIVERSALSTRING_to_string_TYPE)GetProcAddress(libeayHandleM, "ASN1_UNIVERSALSTRING_to_string")) == NULL) goto err;
//	if ((ASN1_UTCTIME_check_FP = (ASN1_UTCTIME_check_TYPE)GetProcAddress(libeayHandleM, "ASN1_UTCTIME_check")) == NULL) goto err;
//	if ((ASN1_UTCTIME_cmp_time_t_FP = (ASN1_UTCTIME_cmp_time_t_TYPE)GetProcAddress(libeayHandleM, "ASN1_UTCTIME_cmp_time_t")) == NULL) goto err;
//	if ((ASN1_UTCTIME_free_FP = (ASN1_UTCTIME_free_TYPE)GetProcAddress(libeayHandleM, "ASN1_UTCTIME_free")) == NULL) goto err;
//	if ((ASN1_UTCTIME_it_FP = (ASN1_UTCTIME_it_TYPE)GetProcAddress(libeayHandleM, "ASN1_UTCTIME_it")) == NULL) goto err;
//	if ((ASN1_UTCTIME_new_FP = (ASN1_UTCTIME_new_TYPE)GetProcAddress(libeayHandleM, "ASN1_UTCTIME_new")) == NULL) goto err;
//	if ((ASN1_UTCTIME_print_FP = (ASN1_UTCTIME_print_TYPE)GetProcAddress(libeayHandleM, "ASN1_UTCTIME_print")) == NULL) goto err;
//	if ((ASN1_UTCTIME_set_FP = (ASN1_UTCTIME_set_TYPE)GetProcAddress(libeayHandleM, "ASN1_UTCTIME_set")) == NULL) goto err;
//	if ((ASN1_UTCTIME_set_string_FP = (ASN1_UTCTIME_set_string_TYPE)GetProcAddress(libeayHandleM, "ASN1_UTCTIME_set_string")) == NULL) goto err;
//	if ((ASN1_UTF8STRING_free_FP = (ASN1_UTF8STRING_free_TYPE)GetProcAddress(libeayHandleM, "ASN1_UTF8STRING_free")) == NULL) goto err;
//	if ((ASN1_UTF8STRING_it_FP = (ASN1_UTF8STRING_it_TYPE)GetProcAddress(libeayHandleM, "ASN1_UTF8STRING_it")) == NULL) goto err;
//	if ((ASN1_UTF8STRING_new_FP = (ASN1_UTF8STRING_new_TYPE)GetProcAddress(libeayHandleM, "ASN1_UTF8STRING_new")) == NULL) goto err;
//	if ((ASN1_VISIBLESTRING_free_FP = (ASN1_VISIBLESTRING_free_TYPE)GetProcAddress(libeayHandleM, "ASN1_VISIBLESTRING_free")) == NULL) goto err;
//	if ((ASN1_VISIBLESTRING_it_FP = (ASN1_VISIBLESTRING_it_TYPE)GetProcAddress(libeayHandleM, "ASN1_VISIBLESTRING_it")) == NULL) goto err;
//	if ((ASN1_VISIBLESTRING_new_FP = (ASN1_VISIBLESTRING_new_TYPE)GetProcAddress(libeayHandleM, "ASN1_VISIBLESTRING_new")) == NULL) goto err;
//	if ((ASN1_add_oid_module_FP = (ASN1_add_oid_module_TYPE)GetProcAddress(libeayHandleM, "ASN1_add_oid_module")) == NULL) goto err;
//	if ((ASN1_check_infinite_end_FP = (ASN1_check_infinite_end_TYPE)GetProcAddress(libeayHandleM, "ASN1_check_infinite_end")) == NULL) goto err;
	if ((ASN1_d2i_bio_FP = (ASN1_d2i_bio_TYPE)GetProcAddress(libeayHandleM, "ASN1_d2i_bio")) == NULL) goto err;
//	if ((ASN1_d2i_fp_FP = (ASN1_d2i_fp_TYPE)GetProcAddress(libeayHandleM, "ASN1_d2i_fp")) == NULL) goto err;
//	if ((ASN1_digest_FP = (ASN1_digest_TYPE)GetProcAddress(libeayHandleM, "ASN1_digest")) == NULL) goto err;
//	if ((ASN1_dup_FP = (ASN1_dup_TYPE)GetProcAddress(libeayHandleM, "ASN1_dup")) == NULL) goto err;
//	if ((ASN1_get_object_FP = (ASN1_get_object_TYPE)GetProcAddress(libeayHandleM, "ASN1_get_object")) == NULL) goto err;
//	if ((ASN1_i2d_bio_FP = (ASN1_i2d_bio_TYPE)GetProcAddress(libeayHandleM, "ASN1_i2d_bio")) == NULL) goto err;
//	if ((ASN1_i2d_fp_FP = (ASN1_i2d_fp_TYPE)GetProcAddress(libeayHandleM, "ASN1_i2d_fp")) == NULL) goto err;
	if ((ASN1_item_d2i_FP = (ASN1_item_d2i_TYPE)GetProcAddress(libeayHandleM, "ASN1_item_d2i")) == NULL) goto err;
	if ((ASN1_item_d2i_bio_FP = (ASN1_item_d2i_bio_TYPE)GetProcAddress(libeayHandleM, "ASN1_item_d2i_bio")) == NULL) goto err;
//	if ((ASN1_item_d2i_fp_FP = (ASN1_item_d2i_fp_TYPE)GetProcAddress(libeayHandleM, "ASN1_item_d2i_fp")) == NULL) goto err;
//	if ((ASN1_item_digest_FP = (ASN1_item_digest_TYPE)GetProcAddress(libeayHandleM, "ASN1_item_digest")) == NULL) goto err;
//	if ((ASN1_item_dup_FP = (ASN1_item_dup_TYPE)GetProcAddress(libeayHandleM, "ASN1_item_dup")) == NULL) goto err;
//	if ((ASN1_item_ex_d2i_FP = (ASN1_item_ex_d2i_TYPE)GetProcAddress(libeayHandleM, "ASN1_item_ex_d2i")) == NULL) goto err;
//	if ((ASN1_item_ex_free_FP = (ASN1_item_ex_free_TYPE)GetProcAddress(libeayHandleM, "ASN1_item_ex_free")) == NULL) goto err;
//	if ((ASN1_item_ex_i2d_FP = (ASN1_item_ex_i2d_TYPE)GetProcAddress(libeayHandleM, "ASN1_item_ex_i2d")) == NULL) goto err;
//	if ((ASN1_item_ex_new_FP = (ASN1_item_ex_new_TYPE)GetProcAddress(libeayHandleM, "ASN1_item_ex_new")) == NULL) goto err;
//	if ((ASN1_item_free_FP = (ASN1_item_free_TYPE)GetProcAddress(libeayHandleM, "ASN1_item_free")) == NULL) goto err;
//	if ((ASN1_item_i2d_FP = (ASN1_item_i2d_TYPE)GetProcAddress(libeayHandleM, "ASN1_item_i2d")) == NULL) goto err;
//	if ((ASN1_item_i2d_bio_FP = (ASN1_item_i2d_bio_TYPE)GetProcAddress(libeayHandleM, "ASN1_item_i2d_bio")) == NULL) goto err;
//	if ((ASN1_item_i2d_fp_FP = (ASN1_item_i2d_fp_TYPE)GetProcAddress(libeayHandleM, "ASN1_item_i2d_fp")) == NULL) goto err;
//	if ((ASN1_item_new_FP = (ASN1_item_new_TYPE)GetProcAddress(libeayHandleM, "ASN1_item_new")) == NULL) goto err;
//	if ((ASN1_item_pack_FP = (ASN1_item_pack_TYPE)GetProcAddress(libeayHandleM, "ASN1_item_pack")) == NULL) goto err;
//	if ((ASN1_item_sign_FP = (ASN1_item_sign_TYPE)GetProcAddress(libeayHandleM, "ASN1_item_sign")) == NULL) goto err;
//	if ((ASN1_item_unpack_FP = (ASN1_item_unpack_TYPE)GetProcAddress(libeayHandleM, "ASN1_item_unpack")) == NULL) goto err;
//	if ((ASN1_item_verify_FP = (ASN1_item_verify_TYPE)GetProcAddress(libeayHandleM, "ASN1_item_verify")) == NULL) goto err;
//	if ((ASN1_mbstring_copy_FP = (ASN1_mbstring_copy_TYPE)GetProcAddress(libeayHandleM, "ASN1_mbstring_copy")) == NULL) goto err;
//	if ((ASN1_mbstring_ncopy_FP = (ASN1_mbstring_ncopy_TYPE)GetProcAddress(libeayHandleM, "ASN1_mbstring_ncopy")) == NULL) goto err;
//	if ((ASN1_object_size_FP = (ASN1_object_size_TYPE)GetProcAddress(libeayHandleM, "ASN1_object_size")) == NULL) goto err;
//	if ((ASN1_pack_string_FP = (ASN1_pack_string_TYPE)GetProcAddress(libeayHandleM, "ASN1_pack_string")) == NULL) goto err;
//	if ((ASN1_parse_FP = (ASN1_parse_TYPE)GetProcAddress(libeayHandleM, "ASN1_parse")) == NULL) goto err;
//	if ((ASN1_parse_dump_FP = (ASN1_parse_dump_TYPE)GetProcAddress(libeayHandleM, "ASN1_parse_dump")) == NULL) goto err;
//	if ((ASN1_primitive_free_FP = (ASN1_primitive_free_TYPE)GetProcAddress(libeayHandleM, "ASN1_primitive_free")) == NULL) goto err;
//	if ((ASN1_primitive_new_FP = (ASN1_primitive_new_TYPE)GetProcAddress(libeayHandleM, "ASN1_primitive_new")) == NULL) goto err;
//	if ((ASN1_put_object_FP = (ASN1_put_object_TYPE)GetProcAddress(libeayHandleM, "ASN1_put_object")) == NULL) goto err;
//	if ((ASN1_seq_pack_FP = (ASN1_seq_pack_TYPE)GetProcAddress(libeayHandleM, "ASN1_seq_pack")) == NULL) goto err;
//	if ((ASN1_seq_unpack_FP = (ASN1_seq_unpack_TYPE)GetProcAddress(libeayHandleM, "ASN1_seq_unpack")) == NULL) goto err;
//	if ((ASN1_sign_FP = (ASN1_sign_TYPE)GetProcAddress(libeayHandleM, "ASN1_sign")) == NULL) goto err;
//	if ((ASN1_tag2bit_FP = (ASN1_tag2bit_TYPE)GetProcAddress(libeayHandleM, "ASN1_tag2bit")) == NULL) goto err;
//	if ((ASN1_tag2str_FP = (ASN1_tag2str_TYPE)GetProcAddress(libeayHandleM, "ASN1_tag2str")) == NULL) goto err;
//	if ((ASN1_template_d2i_FP = (ASN1_template_d2i_TYPE)GetProcAddress(libeayHandleM, "ASN1_template_d2i")) == NULL) goto err;
//	if ((ASN1_template_free_FP = (ASN1_template_free_TYPE)GetProcAddress(libeayHandleM, "ASN1_template_free")) == NULL) goto err;
//	if ((ASN1_template_i2d_FP = (ASN1_template_i2d_TYPE)GetProcAddress(libeayHandleM, "ASN1_template_i2d")) == NULL) goto err;
//	if ((ASN1_template_new_FP = (ASN1_template_new_TYPE)GetProcAddress(libeayHandleM, "ASN1_template_new")) == NULL) goto err;
//	if ((ASN1_unpack_string_FP = (ASN1_unpack_string_TYPE)GetProcAddress(libeayHandleM, "ASN1_unpack_string")) == NULL) goto err;
//	if ((ASN1_verify_FP = (ASN1_verify_TYPE)GetProcAddress(libeayHandleM, "ASN1_verify")) == NULL) goto err;
//	if ((AUTHORITY_INFO_ACCESS_free_FP = (AUTHORITY_INFO_ACCESS_free_TYPE)GetProcAddress(libeayHandleM, "AUTHORITY_INFO_ACCESS_free")) == NULL) goto err;
//	if ((AUTHORITY_INFO_ACCESS_it_FP = (AUTHORITY_INFO_ACCESS_it_TYPE)GetProcAddress(libeayHandleM, "AUTHORITY_INFO_ACCESS_it")) == NULL) goto err;
//	if ((AUTHORITY_INFO_ACCESS_new_FP = (AUTHORITY_INFO_ACCESS_new_TYPE)GetProcAddress(libeayHandleM, "AUTHORITY_INFO_ACCESS_new")) == NULL) goto err;
//	if ((AUTHORITY_KEYID_free_FP = (AUTHORITY_KEYID_free_TYPE)GetProcAddress(libeayHandleM, "AUTHORITY_KEYID_free")) == NULL) goto err;
//	if ((AUTHORITY_KEYID_it_FP = (AUTHORITY_KEYID_it_TYPE)GetProcAddress(libeayHandleM, "AUTHORITY_KEYID_it")) == NULL) goto err;
//	if ((AUTHORITY_KEYID_new_FP = (AUTHORITY_KEYID_new_TYPE)GetProcAddress(libeayHandleM, "AUTHORITY_KEYID_new")) == NULL) goto err;
//	if ((BASIC_CONSTRAINTS_free_FP = (BASIC_CONSTRAINTS_free_TYPE)GetProcAddress(libeayHandleM, "BASIC_CONSTRAINTS_free")) == NULL) goto err;
//	if ((BASIC_CONSTRAINTS_it_FP = (BASIC_CONSTRAINTS_it_TYPE)GetProcAddress(libeayHandleM, "BASIC_CONSTRAINTS_it")) == NULL) goto err;
//	if ((BASIC_CONSTRAINTS_new_FP = (BASIC_CONSTRAINTS_new_TYPE)GetProcAddress(libeayHandleM, "BASIC_CONSTRAINTS_new")) == NULL) goto err;
//	if ((BF_cbc_encrypt_FP = (BF_cbc_encrypt_TYPE)GetProcAddress(libeayHandleM, "BF_cbc_encrypt")) == NULL) goto err;
//	if ((BF_cfb64_encrypt_FP = (BF_cfb64_encrypt_TYPE)GetProcAddress(libeayHandleM, "BF_cfb64_encrypt")) == NULL) goto err;
//	if ((BF_decrypt_FP = (BF_decrypt_TYPE)GetProcAddress(libeayHandleM, "BF_decrypt")) == NULL) goto err;
//	if ((BF_ecb_encrypt_FP = (BF_ecb_encrypt_TYPE)GetProcAddress(libeayHandleM, "BF_ecb_encrypt")) == NULL) goto err;
//	if ((BF_encrypt_FP = (BF_encrypt_TYPE)GetProcAddress(libeayHandleM, "BF_encrypt")) == NULL) goto err;
//	if ((BF_ofb64_encrypt_FP = (BF_ofb64_encrypt_TYPE)GetProcAddress(libeayHandleM, "BF_ofb64_encrypt")) == NULL) goto err;
//	if ((BF_options_FP = (BF_options_TYPE)GetProcAddress(libeayHandleM, "BF_options")) == NULL) goto err;
//	if ((BF_set_key_FP = (BF_set_key_TYPE)GetProcAddress(libeayHandleM, "BF_set_key")) == NULL) goto err;
//	if ((BIGNUM_it_FP = (BIGNUM_it_TYPE)GetProcAddress(libeayHandleM, "BIGNUM_it")) == NULL) goto err;
//	if ((BIO_accept_FP = (BIO_accept_TYPE)GetProcAddress(libeayHandleM, "BIO_accept")) == NULL) goto err;
//	if ((BIO_callback_ctrl_FP = (BIO_callback_ctrl_TYPE)GetProcAddress(libeayHandleM, "BIO_callback_ctrl")) == NULL) goto err;
//	if ((BIO_copy_next_retry_FP = (BIO_copy_next_retry_TYPE)GetProcAddress(libeayHandleM, "BIO_copy_next_retry")) == NULL) goto err;
	if ((BIO_ctrl_FP = (BIO_ctrl_TYPE)GetProcAddress(libeayHandleM, "BIO_ctrl")) == NULL) goto err;
//	if ((BIO_ctrl_get_read_request_FP = (BIO_ctrl_get_read_request_TYPE)GetProcAddress(libeayHandleM, "BIO_ctrl_get_read_request")) == NULL) goto err;
//	if ((BIO_ctrl_get_write_guarantee_FP = (BIO_ctrl_get_write_guarantee_TYPE)GetProcAddress(libeayHandleM, "BIO_ctrl_get_write_guarantee")) == NULL) goto err;
//	if ((BIO_ctrl_pending_FP = (BIO_ctrl_pending_TYPE)GetProcAddress(libeayHandleM, "BIO_ctrl_pending")) == NULL) goto err;
//	if ((BIO_ctrl_reset_read_request_FP = (BIO_ctrl_reset_read_request_TYPE)GetProcAddress(libeayHandleM, "BIO_ctrl_reset_read_request")) == NULL) goto err;
//	if ((BIO_ctrl_wpending_FP = (BIO_ctrl_wpending_TYPE)GetProcAddress(libeayHandleM, "BIO_ctrl_wpending")) == NULL) goto err;
//	if ((BIO_debug_callback_FP = (BIO_debug_callback_TYPE)GetProcAddress(libeayHandleM, "BIO_debug_callback")) == NULL) goto err;
//	if ((BIO_dump_FP = (BIO_dump_TYPE)GetProcAddress(libeayHandleM, "BIO_dump")) == NULL) goto err;
//	if ((BIO_dump_indent_FP = (BIO_dump_indent_TYPE)GetProcAddress(libeayHandleM, "BIO_dump_indent")) == NULL) goto err;
//	if ((BIO_dup_chain_FP = (BIO_dup_chain_TYPE)GetProcAddress(libeayHandleM, "BIO_dup_chain")) == NULL) goto err;
//	if ((BIO_f_base64_FP = (BIO_f_base64_TYPE)GetProcAddress(libeayHandleM, "BIO_f_base64")) == NULL) goto err;
//	if ((BIO_f_buffer_FP = (BIO_f_buffer_TYPE)GetProcAddress(libeayHandleM, "BIO_f_buffer")) == NULL) goto err;
//	if ((BIO_f_cipher_FP = (BIO_f_cipher_TYPE)GetProcAddress(libeayHandleM, "BIO_f_cipher")) == NULL) goto err;
//	if ((BIO_f_md_FP = (BIO_f_md_TYPE)GetProcAddress(libeayHandleM, "BIO_f_md")) == NULL) goto err;
//	if ((BIO_f_nbio_test_FP = (BIO_f_nbio_test_TYPE)GetProcAddress(libeayHandleM, "BIO_f_nbio_test")) == NULL) goto err;
//	if ((BIO_f_null_FP = (BIO_f_null_TYPE)GetProcAddress(libeayHandleM, "BIO_f_null")) == NULL) goto err;
//	if ((BIO_f_reliable_FP = (BIO_f_reliable_TYPE)GetProcAddress(libeayHandleM, "BIO_f_reliable")) == NULL) goto err;
//	if ((BIO_fd_non_fatal_error_FP = (BIO_fd_non_fatal_error_TYPE)GetProcAddress(libeayHandleM, "BIO_fd_non_fatal_error")) == NULL) goto err;
//	if ((BIO_fd_should_retry_FP = (BIO_fd_should_retry_TYPE)GetProcAddress(libeayHandleM, "BIO_fd_should_retry")) == NULL) goto err;
//	if ((BIO_find_type_FP = (BIO_find_type_TYPE)GetProcAddress(libeayHandleM, "BIO_find_type")) == NULL) goto err;
	if ((BIO_free_FP = (BIO_free_TYPE)GetProcAddress(libeayHandleM, "BIO_free")) == NULL) goto err;
//	if ((BIO_free_all_FP = (BIO_free_all_TYPE)GetProcAddress(libeayHandleM, "BIO_free_all")) == NULL) goto err;
//	if ((BIO_get_accept_socket_FP = (BIO_get_accept_socket_TYPE)GetProcAddress(libeayHandleM, "BIO_get_accept_socket")) == NULL) goto err;
//	if ((BIO_get_ex_data_FP = (BIO_get_ex_data_TYPE)GetProcAddress(libeayHandleM, "BIO_get_ex_data")) == NULL) goto err;
//	if ((BIO_get_ex_new_index_FP = (BIO_get_ex_new_index_TYPE)GetProcAddress(libeayHandleM, "BIO_get_ex_new_index")) == NULL) goto err;
//	if ((BIO_get_host_ip_FP = (BIO_get_host_ip_TYPE)GetProcAddress(libeayHandleM, "BIO_get_host_ip")) == NULL) goto err;
//	if ((BIO_get_port_FP = (BIO_get_port_TYPE)GetProcAddress(libeayHandleM, "BIO_get_port")) == NULL) goto err;
//	if ((BIO_get_retry_BIO_FP = (BIO_get_retry_BIO_TYPE)GetProcAddress(libeayHandleM, "BIO_get_retry_BIO")) == NULL) goto err;
//	if ((BIO_get_retry_reason_FP = (BIO_get_retry_reason_TYPE)GetProcAddress(libeayHandleM, "BIO_get_retry_reason")) == NULL) goto err;
//	if ((BIO_gethostbyname_FP = (BIO_gethostbyname_TYPE)GetProcAddress(libeayHandleM, "BIO_gethostbyname")) == NULL) goto err;
//	if ((BIO_gets_FP = (BIO_gets_TYPE)GetProcAddress(libeayHandleM, "BIO_gets")) == NULL) goto err;
//	if ((BIO_indent_FP = (BIO_indent_TYPE)GetProcAddress(libeayHandleM, "BIO_indent")) == NULL) goto err;
//	if ((BIO_int_ctrl_FP = (BIO_int_ctrl_TYPE)GetProcAddress(libeayHandleM, "BIO_int_ctrl")) == NULL) goto err;
	if ((BIO_new_FP = (BIO_new_TYPE)GetProcAddress(libeayHandleM, "BIO_new")) == NULL) goto err;
	if ((BIO_new_accept_FP = (BIO_new_accept_TYPE)GetProcAddress(libeayHandleM, "BIO_new_accept")) == NULL) goto err;
//	if ((BIO_new_bio_pair_FP = (BIO_new_bio_pair_TYPE)GetProcAddress(libeayHandleM, "BIO_new_bio_pair")) == NULL) goto err;
	if ((BIO_new_connect_FP = (BIO_new_connect_TYPE)GetProcAddress(libeayHandleM, "BIO_new_connect")) == NULL) goto err;
//	if ((BIO_new_fd_FP = (BIO_new_fd_TYPE)GetProcAddress(libeayHandleM, "BIO_new_fd")) == NULL) goto err;
//	if ((BIO_new_file_FP = (BIO_new_file_TYPE)GetProcAddress(libeayHandleM, "BIO_new_file")) == NULL) goto err;
//	if ((BIO_new_fp_FP = (BIO_new_fp_TYPE)GetProcAddress(libeayHandleM, "BIO_new_fp")) == NULL) goto err;
//	if ((BIO_new_mem_buf_FP = (BIO_new_mem_buf_TYPE)GetProcAddress(libeayHandleM, "BIO_new_mem_buf")) == NULL) goto err;
//	if ((BIO_new_socket_FP = (BIO_new_socket_TYPE)GetProcAddress(libeayHandleM, "BIO_new_socket")) == NULL) goto err;
//	if ((BIO_next_FP = (BIO_next_TYPE)GetProcAddress(libeayHandleM, "BIO_next")) == NULL) goto err;
//	if ((BIO_nread0_FP = (BIO_nread0_TYPE)GetProcAddress(libeayHandleM, "BIO_nread0")) == NULL) goto err;
//	if ((BIO_nread_FP = (BIO_nread_TYPE)GetProcAddress(libeayHandleM, "BIO_nread")) == NULL) goto err;
//	if ((BIO_number_read_FP = (BIO_number_read_TYPE)GetProcAddress(libeayHandleM, "BIO_number_read")) == NULL) goto err;
//	if ((BIO_number_written_FP = (BIO_number_written_TYPE)GetProcAddress(libeayHandleM, "BIO_number_written")) == NULL) goto err;
//	if ((BIO_nwrite0_FP = (BIO_nwrite0_TYPE)GetProcAddress(libeayHandleM, "BIO_nwrite0")) == NULL) goto err;
//	if ((BIO_nwrite_FP = (BIO_nwrite_TYPE)GetProcAddress(libeayHandleM, "BIO_nwrite")) == NULL) goto err;
	if ((BIO_pop_FP = (BIO_pop_TYPE)GetProcAddress(libeayHandleM, "BIO_pop")) == NULL) goto err;
//	if ((BIO_printf_FP = (BIO_printf_TYPE)GetProcAddress(libeayHandleM, "BIO_printf")) == NULL) goto err;
//	if ((BIO_ptr_ctrl_FP = (BIO_ptr_ctrl_TYPE)GetProcAddress(libeayHandleM, "BIO_ptr_ctrl")) == NULL) goto err;
//	if ((BIO_push_FP = (BIO_push_TYPE)GetProcAddress(libeayHandleM, "BIO_push")) == NULL) goto err;
//	if ((BIO_puts_FP = (BIO_puts_TYPE)GetProcAddress(libeayHandleM, "BIO_puts")) == NULL) goto err;
//	if ((BIO_read_FP = (BIO_read_TYPE)GetProcAddress(libeayHandleM, "BIO_read")) == NULL) goto err;
//	if ((BIO_s_accept_FP = (BIO_s_accept_TYPE)GetProcAddress(libeayHandleM, "BIO_s_accept")) == NULL) goto err;
//	if ((BIO_s_bio_FP = (BIO_s_bio_TYPE)GetProcAddress(libeayHandleM, "BIO_s_bio")) == NULL) goto err;
//	if ((BIO_s_connect_FP = (BIO_s_connect_TYPE)GetProcAddress(libeayHandleM, "BIO_s_connect")) == NULL) goto err;
//	if ((BIO_s_fd_FP = (BIO_s_fd_TYPE)GetProcAddress(libeayHandleM, "BIO_s_fd")) == NULL) goto err;
	if ((BIO_s_file_FP = (BIO_s_file_TYPE)GetProcAddress(libeayHandleM, "BIO_s_file")) == NULL) goto err;
//	if ((BIO_s_mem_FP = (BIO_s_mem_TYPE)GetProcAddress(libeayHandleM, "BIO_s_mem")) == NULL) goto err;
//	if ((BIO_s_null_FP = (BIO_s_null_TYPE)GetProcAddress(libeayHandleM, "BIO_s_null")) == NULL) goto err;
//	if ((BIO_s_socket_FP = (BIO_s_socket_TYPE)GetProcAddress(libeayHandleM, "BIO_s_socket")) == NULL) goto err;
//	if ((BIO_set_FP = (BIO_set_TYPE)GetProcAddress(libeayHandleM, "BIO_set")) == NULL) goto err;
//	if ((BIO_set_cipher_FP = (BIO_set_cipher_TYPE)GetProcAddress(libeayHandleM, "BIO_set_cipher")) == NULL) goto err;
//	if ((BIO_set_ex_data_FP = (BIO_set_ex_data_TYPE)GetProcAddress(libeayHandleM, "BIO_set_ex_data")) == NULL) goto err;
//	if ((BIO_set_tcp_ndelay_FP = (BIO_set_tcp_ndelay_TYPE)GetProcAddress(libeayHandleM, "BIO_set_tcp_ndelay")) == NULL) goto err;
//	if ((BIO_snprintf_FP = (BIO_snprintf_TYPE)GetProcAddress(libeayHandleM, "BIO_snprintf")) == NULL) goto err;
//	if ((BIO_sock_cleanup_FP = (BIO_sock_cleanup_TYPE)GetProcAddress(libeayHandleM, "BIO_sock_cleanup")) == NULL) goto err;
//	if ((BIO_sock_error_FP = (BIO_sock_error_TYPE)GetProcAddress(libeayHandleM, "BIO_sock_error")) == NULL) goto err;
//	if ((BIO_sock_init_FP = (BIO_sock_init_TYPE)GetProcAddress(libeayHandleM, "BIO_sock_init")) == NULL) goto err;
//	if ((BIO_sock_non_fatal_error_FP = (BIO_sock_non_fatal_error_TYPE)GetProcAddress(libeayHandleM, "BIO_sock_non_fatal_error")) == NULL) goto err;
//	if ((BIO_sock_should_retry_FP = (BIO_sock_should_retry_TYPE)GetProcAddress(libeayHandleM, "BIO_sock_should_retry")) == NULL) goto err;
//	if ((BIO_socket_ioctl_FP = (BIO_socket_ioctl_TYPE)GetProcAddress(libeayHandleM, "BIO_socket_ioctl")) == NULL) goto err;
//	if ((BIO_socket_nbio_FP = (BIO_socket_nbio_TYPE)GetProcAddress(libeayHandleM, "BIO_socket_nbio")) == NULL) goto err;
//	if ((BIO_vfree_FP = (BIO_vfree_TYPE)GetProcAddress(libeayHandleM, "BIO_vfree")) == NULL) goto err;
//	if ((BIO_vprintf_FP = (BIO_vprintf_TYPE)GetProcAddress(libeayHandleM, "BIO_vprintf")) == NULL) goto err;
//	if ((BIO_vsnprintf_FP = (BIO_vsnprintf_TYPE)GetProcAddress(libeayHandleM, "BIO_vsnprintf")) == NULL) goto err;
//	if ((BIO_write_FP = (BIO_write_TYPE)GetProcAddress(libeayHandleM, "BIO_write")) == NULL) goto err;
//	if ((BN_BLINDING_convert_FP = (BN_BLINDING_convert_TYPE)GetProcAddress(libeayHandleM, "BN_BLINDING_convert")) == NULL) goto err;
//	if ((BN_BLINDING_free_FP = (BN_BLINDING_free_TYPE)GetProcAddress(libeayHandleM, "BN_BLINDING_free")) == NULL) goto err;
//	if ((BN_BLINDING_invert_FP = (BN_BLINDING_invert_TYPE)GetProcAddress(libeayHandleM, "BN_BLINDING_invert")) == NULL) goto err;
//	if ((BN_BLINDING_new_FP = (BN_BLINDING_new_TYPE)GetProcAddress(libeayHandleM, "BN_BLINDING_new")) == NULL) goto err;
//	if ((BN_BLINDING_update_FP = (BN_BLINDING_update_TYPE)GetProcAddress(libeayHandleM, "BN_BLINDING_update")) == NULL) goto err;
//	if ((BN_CTX_end_FP = (BN_CTX_end_TYPE)GetProcAddress(libeayHandleM, "BN_CTX_end")) == NULL) goto err;
//	if ((BN_CTX_free_FP = (BN_CTX_free_TYPE)GetProcAddress(libeayHandleM, "BN_CTX_free")) == NULL) goto err;
//	if ((BN_CTX_get_FP = (BN_CTX_get_TYPE)GetProcAddress(libeayHandleM, "BN_CTX_get")) == NULL) goto err;
//	if ((BN_CTX_init_FP = (BN_CTX_init_TYPE)GetProcAddress(libeayHandleM, "BN_CTX_init")) == NULL) goto err;
//	if ((BN_CTX_new_FP = (BN_CTX_new_TYPE)GetProcAddress(libeayHandleM, "BN_CTX_new")) == NULL) goto err;
//	if ((BN_CTX_start_FP = (BN_CTX_start_TYPE)GetProcAddress(libeayHandleM, "BN_CTX_start")) == NULL) goto err;
//	if ((BN_MONT_CTX_copy_FP = (BN_MONT_CTX_copy_TYPE)GetProcAddress(libeayHandleM, "BN_MONT_CTX_copy")) == NULL) goto err;
//	if ((BN_MONT_CTX_free_FP = (BN_MONT_CTX_free_TYPE)GetProcAddress(libeayHandleM, "BN_MONT_CTX_free")) == NULL) goto err;
//	if ((BN_MONT_CTX_init_FP = (BN_MONT_CTX_init_TYPE)GetProcAddress(libeayHandleM, "BN_MONT_CTX_init")) == NULL) goto err;
//	if ((BN_MONT_CTX_new_FP = (BN_MONT_CTX_new_TYPE)GetProcAddress(libeayHandleM, "BN_MONT_CTX_new")) == NULL) goto err;
//	if ((BN_MONT_CTX_set_FP = (BN_MONT_CTX_set_TYPE)GetProcAddress(libeayHandleM, "BN_MONT_CTX_set")) == NULL) goto err;
//	if ((BN_RECP_CTX_free_FP = (BN_RECP_CTX_free_TYPE)GetProcAddress(libeayHandleM, "BN_RECP_CTX_free")) == NULL) goto err;
//	if ((BN_RECP_CTX_init_FP = (BN_RECP_CTX_init_TYPE)GetProcAddress(libeayHandleM, "BN_RECP_CTX_init")) == NULL) goto err;
//	if ((BN_RECP_CTX_new_FP = (BN_RECP_CTX_new_TYPE)GetProcAddress(libeayHandleM, "BN_RECP_CTX_new")) == NULL) goto err;
//	if ((BN_RECP_CTX_set_FP = (BN_RECP_CTX_set_TYPE)GetProcAddress(libeayHandleM, "BN_RECP_CTX_set")) == NULL) goto err;
//	if ((BN_add_FP = (BN_add_TYPE)GetProcAddress(libeayHandleM, "BN_add")) == NULL) goto err;
//	if ((BN_add_word_FP = (BN_add_word_TYPE)GetProcAddress(libeayHandleM, "BN_add_word")) == NULL) goto err;
	if ((BN_bin2bn_FP = (BN_bin2bn_TYPE)GetProcAddress(libeayHandleM, "BN_bin2bn")) == NULL) goto err;
//	if ((BN_bn2bin_FP = (BN_bn2bin_TYPE)GetProcAddress(libeayHandleM, "BN_bn2bin")) == NULL) goto err;
//	if ((BN_bn2dec_FP = (BN_bn2dec_TYPE)GetProcAddress(libeayHandleM, "BN_bn2dec")) == NULL) goto err;
//	if ((BN_bn2hex_FP = (BN_bn2hex_TYPE)GetProcAddress(libeayHandleM, "BN_bn2hex")) == NULL) goto err;
//	if ((BN_bn2mpi_FP = (BN_bn2mpi_TYPE)GetProcAddress(libeayHandleM, "BN_bn2mpi")) == NULL) goto err;
//	if ((BN_bntest_rand_FP = (BN_bntest_rand_TYPE)GetProcAddress(libeayHandleM, "BN_bntest_rand")) == NULL) goto err;
//	if ((BN_clear_FP = (BN_clear_TYPE)GetProcAddress(libeayHandleM, "BN_clear")) == NULL) goto err;
//	if ((BN_clear_bit_FP = (BN_clear_bit_TYPE)GetProcAddress(libeayHandleM, "BN_clear_bit")) == NULL) goto err;
//	if ((BN_clear_free_FP = (BN_clear_free_TYPE)GetProcAddress(libeayHandleM, "BN_clear_free")) == NULL) goto err;
//	if ((BN_cmp_FP = (BN_cmp_TYPE)GetProcAddress(libeayHandleM, "BN_cmp")) == NULL) goto err;
//	if ((BN_copy_FP = (BN_copy_TYPE)GetProcAddress(libeayHandleM, "BN_copy")) == NULL) goto err;
//	if ((BN_dec2bn_FP = (BN_dec2bn_TYPE)GetProcAddress(libeayHandleM, "BN_dec2bn")) == NULL) goto err;
//	if ((BN_div_FP = (BN_div_TYPE)GetProcAddress(libeayHandleM, "BN_div")) == NULL) goto err;
//	if ((BN_div_recp_FP = (BN_div_recp_TYPE)GetProcAddress(libeayHandleM, "BN_div_recp")) == NULL) goto err;
//	if ((BN_div_word_FP = (BN_div_word_TYPE)GetProcAddress(libeayHandleM, "BN_div_word")) == NULL) goto err;
//	if ((BN_dup_FP = (BN_dup_TYPE)GetProcAddress(libeayHandleM, "BN_dup")) == NULL) goto err;
//	if ((BN_exp_FP = (BN_exp_TYPE)GetProcAddress(libeayHandleM, "BN_exp")) == NULL) goto err;
//	if ((BN_free_FP = (BN_free_TYPE)GetProcAddress(libeayHandleM, "BN_free")) == NULL) goto err;
//	if ((BN_from_montgomery_FP = (BN_from_montgomery_TYPE)GetProcAddress(libeayHandleM, "BN_from_montgomery")) == NULL) goto err;
//	if ((BN_gcd_FP = (BN_gcd_TYPE)GetProcAddress(libeayHandleM, "BN_gcd")) == NULL) goto err;
//	if ((BN_generate_prime_FP = (BN_generate_prime_TYPE)GetProcAddress(libeayHandleM, "BN_generate_prime")) == NULL) goto err;
//	if ((BN_get_params_FP = (BN_get_params_TYPE)GetProcAddress(libeayHandleM, "BN_get_params")) == NULL) goto err;
//	if ((BN_get_word_FP = (BN_get_word_TYPE)GetProcAddress(libeayHandleM, "BN_get_word")) == NULL) goto err;
//	if ((BN_hex2bn_FP = (BN_hex2bn_TYPE)GetProcAddress(libeayHandleM, "BN_hex2bn")) == NULL) goto err;
//	if ((BN_init_FP = (BN_init_TYPE)GetProcAddress(libeayHandleM, "BN_init")) == NULL) goto err;
//	if ((BN_is_bit_set_FP = (BN_is_bit_set_TYPE)GetProcAddress(libeayHandleM, "BN_is_bit_set")) == NULL) goto err;
//	if ((BN_is_prime_FP = (BN_is_prime_TYPE)GetProcAddress(libeayHandleM, "BN_is_prime")) == NULL) goto err;
//	if ((BN_is_prime_fasttest_FP = (BN_is_prime_fasttest_TYPE)GetProcAddress(libeayHandleM, "BN_is_prime_fasttest")) == NULL) goto err;
//	if ((BN_kronecker_FP = (BN_kronecker_TYPE)GetProcAddress(libeayHandleM, "BN_kronecker")) == NULL) goto err;
//	if ((BN_lshift1_FP = (BN_lshift1_TYPE)GetProcAddress(libeayHandleM, "BN_lshift1")) == NULL) goto err;
//	if ((BN_lshift_FP = (BN_lshift_TYPE)GetProcAddress(libeayHandleM, "BN_lshift")) == NULL) goto err;
//	if ((BN_mask_bits_FP = (BN_mask_bits_TYPE)GetProcAddress(libeayHandleM, "BN_mask_bits")) == NULL) goto err;
//	if ((BN_mod_add_FP = (BN_mod_add_TYPE)GetProcAddress(libeayHandleM, "BN_mod_add")) == NULL) goto err;
//	if ((BN_mod_add_quick_FP = (BN_mod_add_quick_TYPE)GetProcAddress(libeayHandleM, "BN_mod_add_quick")) == NULL) goto err;
//	if ((BN_mod_exp2_mont_FP = (BN_mod_exp2_mont_TYPE)GetProcAddress(libeayHandleM, "BN_mod_exp2_mont")) == NULL) goto err;
//	if ((BN_mod_exp_FP = (BN_mod_exp_TYPE)GetProcAddress(libeayHandleM, "BN_mod_exp")) == NULL) goto err;
//	if ((BN_mod_exp_mont_FP = (BN_mod_exp_mont_TYPE)GetProcAddress(libeayHandleM, "BN_mod_exp_mont")) == NULL) goto err;
//	if ((BN_mod_exp_mont_word_FP = (BN_mod_exp_mont_word_TYPE)GetProcAddress(libeayHandleM, "BN_mod_exp_mont_word")) == NULL) goto err;
//	if ((BN_mod_exp_recp_FP = (BN_mod_exp_recp_TYPE)GetProcAddress(libeayHandleM, "BN_mod_exp_recp")) == NULL) goto err;
//	if ((BN_mod_exp_simple_FP = (BN_mod_exp_simple_TYPE)GetProcAddress(libeayHandleM, "BN_mod_exp_simple")) == NULL) goto err;
//	if ((BN_mod_inverse_FP = (BN_mod_inverse_TYPE)GetProcAddress(libeayHandleM, "BN_mod_inverse")) == NULL) goto err;
//	if ((BN_mod_lshift1_FP = (BN_mod_lshift1_TYPE)GetProcAddress(libeayHandleM, "BN_mod_lshift1")) == NULL) goto err;
//	if ((BN_mod_lshift1_quick_FP = (BN_mod_lshift1_quick_TYPE)GetProcAddress(libeayHandleM, "BN_mod_lshift1_quick")) == NULL) goto err;
//	if ((BN_mod_lshift_FP = (BN_mod_lshift_TYPE)GetProcAddress(libeayHandleM, "BN_mod_lshift")) == NULL) goto err;
//	if ((BN_mod_lshift_quick_FP = (BN_mod_lshift_quick_TYPE)GetProcAddress(libeayHandleM, "BN_mod_lshift_quick")) == NULL) goto err;
//	if ((BN_mod_mul_FP = (BN_mod_mul_TYPE)GetProcAddress(libeayHandleM, "BN_mod_mul")) == NULL) goto err;
//	if ((BN_mod_mul_montgomery_FP = (BN_mod_mul_montgomery_TYPE)GetProcAddress(libeayHandleM, "BN_mod_mul_montgomery")) == NULL) goto err;
//	if ((BN_mod_mul_reciprocal_FP = (BN_mod_mul_reciprocal_TYPE)GetProcAddress(libeayHandleM, "BN_mod_mul_reciprocal")) == NULL) goto err;
//	if ((BN_mod_sqr_FP = (BN_mod_sqr_TYPE)GetProcAddress(libeayHandleM, "BN_mod_sqr")) == NULL) goto err;
//	if ((BN_mod_sqrt_FP = (BN_mod_sqrt_TYPE)GetProcAddress(libeayHandleM, "BN_mod_sqrt")) == NULL) goto err;
//	if ((BN_mod_sub_FP = (BN_mod_sub_TYPE)GetProcAddress(libeayHandleM, "BN_mod_sub")) == NULL) goto err;
//	if ((BN_mod_sub_quick_FP = (BN_mod_sub_quick_TYPE)GetProcAddress(libeayHandleM, "BN_mod_sub_quick")) == NULL) goto err;
//	if ((BN_mod_word_FP = (BN_mod_word_TYPE)GetProcAddress(libeayHandleM, "BN_mod_word")) == NULL) goto err;
//	if ((BN_mpi2bn_FP = (BN_mpi2bn_TYPE)GetProcAddress(libeayHandleM, "BN_mpi2bn")) == NULL) goto err;
//	if ((BN_mul_FP = (BN_mul_TYPE)GetProcAddress(libeayHandleM, "BN_mul")) == NULL) goto err;
//	if ((BN_mul_word_FP = (BN_mul_word_TYPE)GetProcAddress(libeayHandleM, "BN_mul_word")) == NULL) goto err;
//	if ((BN_new_FP = (BN_new_TYPE)GetProcAddress(libeayHandleM, "BN_new")) == NULL) goto err;
//	if ((BN_nnmod_FP = (BN_nnmod_TYPE)GetProcAddress(libeayHandleM, "BN_nnmod")) == NULL) goto err;
	if ((BN_num_bits_FP = (BN_num_bits_TYPE)GetProcAddress(libeayHandleM, "BN_num_bits")) == NULL) goto err;
//	if ((BN_num_bits_word_FP = (BN_num_bits_word_TYPE)GetProcAddress(libeayHandleM, "BN_num_bits_word")) == NULL) goto err;
//	if ((BN_options_FP = (BN_options_TYPE)GetProcAddress(libeayHandleM, "BN_options")) == NULL) goto err;
//	if ((BN_print_FP = (BN_print_TYPE)GetProcAddress(libeayHandleM, "BN_print")) == NULL) goto err;
//	if ((BN_print_fp_FP = (BN_print_fp_TYPE)GetProcAddress(libeayHandleM, "BN_print_fp")) == NULL) goto err;
//	if ((BN_pseudo_rand_FP = (BN_pseudo_rand_TYPE)GetProcAddress(libeayHandleM, "BN_pseudo_rand")) == NULL) goto err;
//	if ((BN_pseudo_rand_range_FP = (BN_pseudo_rand_range_TYPE)GetProcAddress(libeayHandleM, "BN_pseudo_rand_range")) == NULL) goto err;
//	if ((BN_rand_FP = (BN_rand_TYPE)GetProcAddress(libeayHandleM, "BN_rand")) == NULL) goto err;
//	if ((BN_rand_range_FP = (BN_rand_range_TYPE)GetProcAddress(libeayHandleM, "BN_rand_range")) == NULL) goto err;
//	if ((BN_reciprocal_FP = (BN_reciprocal_TYPE)GetProcAddress(libeayHandleM, "BN_reciprocal")) == NULL) goto err;
//	if ((BN_rshift1_FP = (BN_rshift1_TYPE)GetProcAddress(libeayHandleM, "BN_rshift1")) == NULL) goto err;
//	if ((BN_rshift_FP = (BN_rshift_TYPE)GetProcAddress(libeayHandleM, "BN_rshift")) == NULL) goto err;
//	if ((BN_set_bit_FP = (BN_set_bit_TYPE)GetProcAddress(libeayHandleM, "BN_set_bit")) == NULL) goto err;
//	if ((BN_set_params_FP = (BN_set_params_TYPE)GetProcAddress(libeayHandleM, "BN_set_params")) == NULL) goto err;
//	if ((BN_set_word_FP = (BN_set_word_TYPE)GetProcAddress(libeayHandleM, "BN_set_word")) == NULL) goto err;
//	if ((BN_sqr_FP = (BN_sqr_TYPE)GetProcAddress(libeayHandleM, "BN_sqr")) == NULL) goto err;
//	if ((BN_sub_FP = (BN_sub_TYPE)GetProcAddress(libeayHandleM, "BN_sub")) == NULL) goto err;
//	if ((BN_sub_word_FP = (BN_sub_word_TYPE)GetProcAddress(libeayHandleM, "BN_sub_word")) == NULL) goto err;
//	if ((BN_swap_FP = (BN_swap_TYPE)GetProcAddress(libeayHandleM, "BN_swap")) == NULL) goto err;
//	if ((BN_to_ASN1_ENUMERATED_FP = (BN_to_ASN1_ENUMERATED_TYPE)GetProcAddress(libeayHandleM, "BN_to_ASN1_ENUMERATED")) == NULL) goto err;
//	if ((BN_to_ASN1_INTEGER_FP = (BN_to_ASN1_INTEGER_TYPE)GetProcAddress(libeayHandleM, "BN_to_ASN1_INTEGER")) == NULL) goto err;
//	if ((BN_uadd_FP = (BN_uadd_TYPE)GetProcAddress(libeayHandleM, "BN_uadd")) == NULL) goto err;
//	if ((BN_ucmp_FP = (BN_ucmp_TYPE)GetProcAddress(libeayHandleM, "BN_ucmp")) == NULL) goto err;
//	if ((BN_usub_FP = (BN_usub_TYPE)GetProcAddress(libeayHandleM, "BN_usub")) == NULL) goto err;
//	if ((BN_value_one_FP = (BN_value_one_TYPE)GetProcAddress(libeayHandleM, "BN_value_one")) == NULL) goto err;
//	if ((BUF_MEM_free_FP = (BUF_MEM_free_TYPE)GetProcAddress(libeayHandleM, "BUF_MEM_free")) == NULL) goto err;
//	if ((BUF_MEM_grow_FP = (BUF_MEM_grow_TYPE)GetProcAddress(libeayHandleM, "BUF_MEM_grow")) == NULL) goto err;
//	if ((BUF_MEM_grow_clean_FP = (BUF_MEM_grow_clean_TYPE)GetProcAddress(libeayHandleM, "BUF_MEM_grow_clean")) == NULL) goto err;
//	if ((BUF_MEM_new_FP = (BUF_MEM_new_TYPE)GetProcAddress(libeayHandleM, "BUF_MEM_new")) == NULL) goto err;
//	if ((BUF_strdup_FP = (BUF_strdup_TYPE)GetProcAddress(libeayHandleM, "BUF_strdup")) == NULL) goto err;
//	if ((BUF_strlcat_FP = (BUF_strlcat_TYPE)GetProcAddress(libeayHandleM, "BUF_strlcat")) == NULL) goto err;
//	if ((BUF_strlcpy_FP = (BUF_strlcpy_TYPE)GetProcAddress(libeayHandleM, "BUF_strlcpy")) == NULL) goto err;
//	if ((CAST_cbc_encrypt_FP = (CAST_cbc_encrypt_TYPE)GetProcAddress(libeayHandleM, "CAST_cbc_encrypt")) == NULL) goto err;
//	if ((CAST_cfb64_encrypt_FP = (CAST_cfb64_encrypt_TYPE)GetProcAddress(libeayHandleM, "CAST_cfb64_encrypt")) == NULL) goto err;
//	if ((CAST_decrypt_FP = (CAST_decrypt_TYPE)GetProcAddress(libeayHandleM, "CAST_decrypt")) == NULL) goto err;
//	if ((CAST_ecb_encrypt_FP = (CAST_ecb_encrypt_TYPE)GetProcAddress(libeayHandleM, "CAST_ecb_encrypt")) == NULL) goto err;
//	if ((CAST_encrypt_FP = (CAST_encrypt_TYPE)GetProcAddress(libeayHandleM, "CAST_encrypt")) == NULL) goto err;
//	if ((CAST_ofb64_encrypt_FP = (CAST_ofb64_encrypt_TYPE)GetProcAddress(libeayHandleM, "CAST_ofb64_encrypt")) == NULL) goto err;
//	if ((CAST_set_key_FP = (CAST_set_key_TYPE)GetProcAddress(libeayHandleM, "CAST_set_key")) == NULL) goto err;
//	if ((CBIGNUM_it_FP = (CBIGNUM_it_TYPE)GetProcAddress(libeayHandleM, "CBIGNUM_it")) == NULL) goto err;
//	if ((CERTIFICATEPOLICIES_free_FP = (CERTIFICATEPOLICIES_free_TYPE)GetProcAddress(libeayHandleM, "CERTIFICATEPOLICIES_free")) == NULL) goto err;
//	if ((CERTIFICATEPOLICIES_it_FP = (CERTIFICATEPOLICIES_it_TYPE)GetProcAddress(libeayHandleM, "CERTIFICATEPOLICIES_it")) == NULL) goto err;
//	if ((CERTIFICATEPOLICIES_new_FP = (CERTIFICATEPOLICIES_new_TYPE)GetProcAddress(libeayHandleM, "CERTIFICATEPOLICIES_new")) == NULL) goto err;
//	if ((COMP_CTX_free_FP = (COMP_CTX_free_TYPE)GetProcAddress(libeayHandleM, "COMP_CTX_free")) == NULL) goto err;
//	if ((COMP_CTX_new_FP = (COMP_CTX_new_TYPE)GetProcAddress(libeayHandleM, "COMP_CTX_new")) == NULL) goto err;
//	if ((COMP_compress_block_FP = (COMP_compress_block_TYPE)GetProcAddress(libeayHandleM, "COMP_compress_block")) == NULL) goto err;
//	if ((COMP_expand_block_FP = (COMP_expand_block_TYPE)GetProcAddress(libeayHandleM, "COMP_expand_block")) == NULL) goto err;
//	if ((COMP_rle_FP = (COMP_rle_TYPE)GetProcAddress(libeayHandleM, "COMP_rle")) == NULL) goto err;
//	if ((COMP_zlib_FP = (COMP_zlib_TYPE)GetProcAddress(libeayHandleM, "COMP_zlib")) == NULL) goto err;
//	if ((CONF_dump_bio_FP = (CONF_dump_bio_TYPE)GetProcAddress(libeayHandleM, "CONF_dump_bio")) == NULL) goto err;
//	if ((CONF_dump_fp_FP = (CONF_dump_fp_TYPE)GetProcAddress(libeayHandleM, "CONF_dump_fp")) == NULL) goto err;
//	if ((CONF_free_FP = (CONF_free_TYPE)GetProcAddress(libeayHandleM, "CONF_free")) == NULL) goto err;
//	if ((CONF_get1_default_config_file_FP = (CONF_get1_default_config_file_TYPE)GetProcAddress(libeayHandleM, "CONF_get1_default_config_file")) == NULL) goto err;
//	if ((CONF_get_number_FP = (CONF_get_number_TYPE)GetProcAddress(libeayHandleM, "CONF_get_number")) == NULL) goto err;
//	if ((CONF_get_section_FP = (CONF_get_section_TYPE)GetProcAddress(libeayHandleM, "CONF_get_section")) == NULL) goto err;
//	if ((CONF_get_string_FP = (CONF_get_string_TYPE)GetProcAddress(libeayHandleM, "CONF_get_string")) == NULL) goto err;
//	if ((CONF_imodule_get_flags_FP = (CONF_imodule_get_flags_TYPE)GetProcAddress(libeayHandleM, "CONF_imodule_get_flags")) == NULL) goto err;
//	if ((CONF_imodule_get_module_FP = (CONF_imodule_get_module_TYPE)GetProcAddress(libeayHandleM, "CONF_imodule_get_module")) == NULL) goto err;
//	if ((CONF_imodule_get_name_FP = (CONF_imodule_get_name_TYPE)GetProcAddress(libeayHandleM, "CONF_imodule_get_name")) == NULL) goto err;
//	if ((CONF_imodule_get_usr_data_FP = (CONF_imodule_get_usr_data_TYPE)GetProcAddress(libeayHandleM, "CONF_imodule_get_usr_data")) == NULL) goto err;
//	if ((CONF_imodule_get_value_FP = (CONF_imodule_get_value_TYPE)GetProcAddress(libeayHandleM, "CONF_imodule_get_value")) == NULL) goto err;
//	if ((CONF_imodule_set_flags_FP = (CONF_imodule_set_flags_TYPE)GetProcAddress(libeayHandleM, "CONF_imodule_set_flags")) == NULL) goto err;
//	if ((CONF_imodule_set_usr_data_FP = (CONF_imodule_set_usr_data_TYPE)GetProcAddress(libeayHandleM, "CONF_imodule_set_usr_data")) == NULL) goto err;
//	if ((CONF_load_FP = (CONF_load_TYPE)GetProcAddress(libeayHandleM, "CONF_load")) == NULL) goto err;
//	if ((CONF_load_bio_FP = (CONF_load_bio_TYPE)GetProcAddress(libeayHandleM, "CONF_load_bio")) == NULL) goto err;
//	if ((CONF_load_fp_FP = (CONF_load_fp_TYPE)GetProcAddress(libeayHandleM, "CONF_load_fp")) == NULL) goto err;
//	if ((CONF_module_add_FP = (CONF_module_add_TYPE)GetProcAddress(libeayHandleM, "CONF_module_add")) == NULL) goto err;
//	if ((CONF_module_get_usr_data_FP = (CONF_module_get_usr_data_TYPE)GetProcAddress(libeayHandleM, "CONF_module_get_usr_data")) == NULL) goto err;
//	if ((CONF_module_set_usr_data_FP = (CONF_module_set_usr_data_TYPE)GetProcAddress(libeayHandleM, "CONF_module_set_usr_data")) == NULL) goto err;
//	if ((CONF_modules_finish_FP = (CONF_modules_finish_TYPE)GetProcAddress(libeayHandleM, "CONF_modules_finish")) == NULL) goto err;
//	if ((CONF_modules_free_FP = (CONF_modules_free_TYPE)GetProcAddress(libeayHandleM, "CONF_modules_free")) == NULL) goto err;
//	if ((CONF_modules_load_FP = (CONF_modules_load_TYPE)GetProcAddress(libeayHandleM, "CONF_modules_load")) == NULL) goto err;
//	if ((CONF_modules_load_file_FP = (CONF_modules_load_file_TYPE)GetProcAddress(libeayHandleM, "CONF_modules_load_file")) == NULL) goto err;
//	if ((CONF_modules_unload_FP = (CONF_modules_unload_TYPE)GetProcAddress(libeayHandleM, "CONF_modules_unload")) == NULL) goto err;
//	if ((CONF_parse_list_FP = (CONF_parse_list_TYPE)GetProcAddress(libeayHandleM, "CONF_parse_list")) == NULL) goto err;
//	if ((CONF_set_default_method_FP = (CONF_set_default_method_TYPE)GetProcAddress(libeayHandleM, "CONF_set_default_method")) == NULL) goto err;
//	if ((CONF_set_nconf_FP = (CONF_set_nconf_TYPE)GetProcAddress(libeayHandleM, "CONF_set_nconf")) == NULL) goto err;
//	if ((CRL_DIST_POINTS_free_FP = (CRL_DIST_POINTS_free_TYPE)GetProcAddress(libeayHandleM, "CRL_DIST_POINTS_free")) == NULL) goto err;
//	if ((CRL_DIST_POINTS_it_FP = (CRL_DIST_POINTS_it_TYPE)GetProcAddress(libeayHandleM, "CRL_DIST_POINTS_it")) == NULL) goto err;
//	if ((CRL_DIST_POINTS_new_FP = (CRL_DIST_POINTS_new_TYPE)GetProcAddress(libeayHandleM, "CRL_DIST_POINTS_new")) == NULL) goto err;
	if ((CRYPTO_add_lock_FP = (CRYPTO_add_lock_TYPE)GetProcAddress(libeayHandleM, "CRYPTO_add_lock")) == NULL) goto err;
//	if ((CRYPTO_cleanup_all_ex_data_FP = (CRYPTO_cleanup_all_ex_data_TYPE)GetProcAddress(libeayHandleM, "CRYPTO_cleanup_all_ex_data")) == NULL) goto err;
//	if ((CRYPTO_dbg_free_FP = (CRYPTO_dbg_free_TYPE)GetProcAddress(libeayHandleM, "CRYPTO_dbg_free")) == NULL) goto err;
//	if ((CRYPTO_dbg_get_options_FP = (CRYPTO_dbg_get_options_TYPE)GetProcAddress(libeayHandleM, "CRYPTO_dbg_get_options")) == NULL) goto err;
//	if ((CRYPTO_dbg_malloc_FP = (CRYPTO_dbg_malloc_TYPE)GetProcAddress(libeayHandleM, "CRYPTO_dbg_malloc")) == NULL) goto err;
//	if ((CRYPTO_dbg_realloc_FP = (CRYPTO_dbg_realloc_TYPE)GetProcAddress(libeayHandleM, "CRYPTO_dbg_realloc")) == NULL) goto err;
//	if ((CRYPTO_dbg_set_options_FP = (CRYPTO_dbg_set_options_TYPE)GetProcAddress(libeayHandleM, "CRYPTO_dbg_set_options")) == NULL) goto err;
//	if ((CRYPTO_destroy_dynlockid_FP = (CRYPTO_destroy_dynlockid_TYPE)GetProcAddress(libeayHandleM, "CRYPTO_destroy_dynlockid")) == NULL) goto err;
//	if ((CRYPTO_dup_ex_data_FP = (CRYPTO_dup_ex_data_TYPE)GetProcAddress(libeayHandleM, "CRYPTO_dup_ex_data")) == NULL) goto err;
//	if ((CRYPTO_ex_data_new_class_FP = (CRYPTO_ex_data_new_class_TYPE)GetProcAddress(libeayHandleM, "CRYPTO_ex_data_new_class")) == NULL) goto err;
	if ((CRYPTO_free_FP = (CRYPTO_free_TYPE)GetProcAddress(libeayHandleM, "CRYPTO_free")) == NULL) goto err;
//	if ((CRYPTO_free_ex_data_FP = (CRYPTO_free_ex_data_TYPE)GetProcAddress(libeayHandleM, "CRYPTO_free_ex_data")) == NULL) goto err;
//	if ((CRYPTO_free_locked_FP = (CRYPTO_free_locked_TYPE)GetProcAddress(libeayHandleM, "CRYPTO_free_locked")) == NULL) goto err;
//	if ((CRYPTO_get_add_lock_callback_FP = (CRYPTO_get_add_lock_callback_TYPE)GetProcAddress(libeayHandleM, "CRYPTO_get_add_lock_callback")) == NULL) goto err;
//	if ((CRYPTO_get_dynlock_create_callback_FP = (CRYPTO_get_dynlock_create_callback_TYPE)GetProcAddress(libeayHandleM, "CRYPTO_get_dynlock_create_callback")) == NULL) goto err;
//	if ((CRYPTO_get_dynlock_destroy_callback_FP = (CRYPTO_get_dynlock_destroy_callback_TYPE)GetProcAddress(libeayHandleM, "CRYPTO_get_dynlock_destroy_callback")) == NULL) goto err;
//	if ((CRYPTO_get_dynlock_lock_callback_FP = (CRYPTO_get_dynlock_lock_callback_TYPE)GetProcAddress(libeayHandleM, "CRYPTO_get_dynlock_lock_callback")) == NULL) goto err;
//	if ((CRYPTO_get_dynlock_value_FP = (CRYPTO_get_dynlock_value_TYPE)GetProcAddress(libeayHandleM, "CRYPTO_get_dynlock_value")) == NULL) goto err;
//	if ((CRYPTO_get_ex_data_FP = (CRYPTO_get_ex_data_TYPE)GetProcAddress(libeayHandleM, "CRYPTO_get_ex_data")) == NULL) goto err;
//	if ((CRYPTO_get_ex_data_implementation_FP = (CRYPTO_get_ex_data_implementation_TYPE)GetProcAddress(libeayHandleM, "CRYPTO_get_ex_data_implementation")) == NULL) goto err;
//	if ((CRYPTO_get_ex_new_index_FP = (CRYPTO_get_ex_new_index_TYPE)GetProcAddress(libeayHandleM, "CRYPTO_get_ex_new_index")) == NULL) goto err;
//	if ((CRYPTO_get_id_callback_FP = (CRYPTO_get_id_callback_TYPE)GetProcAddress(libeayHandleM, "CRYPTO_get_id_callback")) == NULL) goto err;
//	if ((CRYPTO_get_lock_name_FP = (CRYPTO_get_lock_name_TYPE)GetProcAddress(libeayHandleM, "CRYPTO_get_lock_name")) == NULL) goto err;
//	if ((CRYPTO_get_locked_mem_ex_functions_FP = (CRYPTO_get_locked_mem_ex_functions_TYPE)GetProcAddress(libeayHandleM, "CRYPTO_get_locked_mem_ex_functions")) == NULL) goto err;
//	if ((CRYPTO_get_locked_mem_functions_FP = (CRYPTO_get_locked_mem_functions_TYPE)GetProcAddress(libeayHandleM, "CRYPTO_get_locked_mem_functions")) == NULL) goto err;
//	if ((CRYPTO_get_locking_callback_FP = (CRYPTO_get_locking_callback_TYPE)GetProcAddress(libeayHandleM, "CRYPTO_get_locking_callback")) == NULL) goto err;
//	if ((CRYPTO_get_mem_debug_functions_FP = (CRYPTO_get_mem_debug_functions_TYPE)GetProcAddress(libeayHandleM, "CRYPTO_get_mem_debug_functions")) == NULL) goto err;
//	if ((CRYPTO_get_mem_debug_options_FP = (CRYPTO_get_mem_debug_options_TYPE)GetProcAddress(libeayHandleM, "CRYPTO_get_mem_debug_options")) == NULL) goto err;
//	if ((CRYPTO_get_mem_ex_functions_FP = (CRYPTO_get_mem_ex_functions_TYPE)GetProcAddress(libeayHandleM, "CRYPTO_get_mem_ex_functions")) == NULL) goto err;
//	if ((CRYPTO_get_mem_functions_FP = (CRYPTO_get_mem_functions_TYPE)GetProcAddress(libeayHandleM, "CRYPTO_get_mem_functions")) == NULL) goto err;
//	if ((CRYPTO_get_new_dynlockid_FP = (CRYPTO_get_new_dynlockid_TYPE)GetProcAddress(libeayHandleM, "CRYPTO_get_new_dynlockid")) == NULL) goto err;
//	if ((CRYPTO_get_new_lockid_FP = (CRYPTO_get_new_lockid_TYPE)GetProcAddress(libeayHandleM, "CRYPTO_get_new_lockid")) == NULL) goto err;
//	if ((CRYPTO_is_mem_check_on_FP = (CRYPTO_is_mem_check_on_TYPE)GetProcAddress(libeayHandleM, "CRYPTO_is_mem_check_on")) == NULL) goto err;
//	if ((CRYPTO_lock_FP = (CRYPTO_lock_TYPE)GetProcAddress(libeayHandleM, "CRYPTO_lock")) == NULL) goto err;
//	if ((CRYPTO_malloc_FP = (CRYPTO_malloc_TYPE)GetProcAddress(libeayHandleM, "CRYPTO_malloc")) == NULL) goto err;
//	if ((CRYPTO_malloc_locked_FP = (CRYPTO_malloc_locked_TYPE)GetProcAddress(libeayHandleM, "CRYPTO_malloc_locked")) == NULL) goto err;
//	if ((CRYPTO_mem_ctrl_FP = (CRYPTO_mem_ctrl_TYPE)GetProcAddress(libeayHandleM, "CRYPTO_mem_ctrl")) == NULL) goto err;
//	if ((CRYPTO_mem_leaks_FP = (CRYPTO_mem_leaks_TYPE)GetProcAddress(libeayHandleM, "CRYPTO_mem_leaks")) == NULL) goto err;
//	if ((CRYPTO_mem_leaks_cb_FP = (CRYPTO_mem_leaks_cb_TYPE)GetProcAddress(libeayHandleM, "CRYPTO_mem_leaks_cb")) == NULL) goto err;
//	if ((CRYPTO_mem_leaks_fp_FP = (CRYPTO_mem_leaks_fp_TYPE)GetProcAddress(libeayHandleM, "CRYPTO_mem_leaks_fp")) == NULL) goto err;
//	if ((CRYPTO_new_ex_data_FP = (CRYPTO_new_ex_data_TYPE)GetProcAddress(libeayHandleM, "CRYPTO_new_ex_data")) == NULL) goto err;
	if ((CRYPTO_num_locks_FP = (CRYPTO_num_locks_TYPE)GetProcAddress(libeayHandleM, "CRYPTO_num_locks")) == NULL) goto err;
//	if ((CRYPTO_pop_info_FP = (CRYPTO_pop_info_TYPE)GetProcAddress(libeayHandleM, "CRYPTO_pop_info")) == NULL) goto err;
//	if ((CRYPTO_push_info__FP = (CRYPTO_push_info__TYPE)GetProcAddress(libeayHandleM, "CRYPTO_push_info_")) == NULL) goto err;
//	if ((CRYPTO_realloc_FP = (CRYPTO_realloc_TYPE)GetProcAddress(libeayHandleM, "CRYPTO_realloc")) == NULL) goto err;
//	if ((CRYPTO_realloc_clean_FP = (CRYPTO_realloc_clean_TYPE)GetProcAddress(libeayHandleM, "CRYPTO_realloc_clean")) == NULL) goto err;
//	if ((CRYPTO_remalloc_FP = (CRYPTO_remalloc_TYPE)GetProcAddress(libeayHandleM, "CRYPTO_remalloc")) == NULL) goto err;
//	if ((CRYPTO_remove_all_info_FP = (CRYPTO_remove_all_info_TYPE)GetProcAddress(libeayHandleM, "CRYPTO_remove_all_info")) == NULL) goto err;
//	if ((CRYPTO_set_add_lock_callback_FP = (CRYPTO_set_add_lock_callback_TYPE)GetProcAddress(libeayHandleM, "CRYPTO_set_add_lock_callback")) == NULL) goto err;
	if ((CRYPTO_set_dynlock_create_callback_FP = (CRYPTO_set_dynlock_create_callback_TYPE)GetProcAddress(libeayHandleM, "CRYPTO_set_dynlock_create_callback")) == NULL) goto err;
	if ((CRYPTO_set_dynlock_destroy_callback_FP = (CRYPTO_set_dynlock_destroy_callback_TYPE)GetProcAddress(libeayHandleM, "CRYPTO_set_dynlock_destroy_callback")) == NULL) goto err;
	if ((CRYPTO_set_dynlock_lock_callback_FP = (CRYPTO_set_dynlock_lock_callback_TYPE)GetProcAddress(libeayHandleM, "CRYPTO_set_dynlock_lock_callback")) == NULL) goto err;
//	if ((CRYPTO_set_ex_data_FP = (CRYPTO_set_ex_data_TYPE)GetProcAddress(libeayHandleM, "CRYPTO_set_ex_data")) == NULL) goto err;
//	if ((CRYPTO_set_ex_data_implementation_FP = (CRYPTO_set_ex_data_implementation_TYPE)GetProcAddress(libeayHandleM, "CRYPTO_set_ex_data_implementation")) == NULL) goto err;
	if ((CRYPTO_set_id_callback_FP = (CRYPTO_set_id_callback_TYPE)GetProcAddress(libeayHandleM, "CRYPTO_set_id_callback")) == NULL) goto err;
//	if ((CRYPTO_set_locked_mem_ex_functions_FP = (CRYPTO_set_locked_mem_ex_functions_TYPE)GetProcAddress(libeayHandleM, "CRYPTO_set_locked_mem_ex_functions")) == NULL) goto err;
//	if ((CRYPTO_set_locked_mem_functions_FP = (CRYPTO_set_locked_mem_functions_TYPE)GetProcAddress(libeayHandleM, "CRYPTO_set_locked_mem_functions")) == NULL) goto err;
	if ((CRYPTO_set_locking_callback_FP = (CRYPTO_set_locking_callback_TYPE)GetProcAddress(libeayHandleM, "CRYPTO_set_locking_callback")) == NULL) goto err;
//	if ((CRYPTO_set_mem_debug_functions_FP = (CRYPTO_set_mem_debug_functions_TYPE)GetProcAddress(libeayHandleM, "CRYPTO_set_mem_debug_functions")) == NULL) goto err;
//	if ((CRYPTO_set_mem_debug_options_FP = (CRYPTO_set_mem_debug_options_TYPE)GetProcAddress(libeayHandleM, "CRYPTO_set_mem_debug_options")) == NULL) goto err;
//	if ((CRYPTO_set_mem_ex_functions_FP = (CRYPTO_set_mem_ex_functions_TYPE)GetProcAddress(libeayHandleM, "CRYPTO_set_mem_ex_functions")) == NULL) goto err;
//	if ((CRYPTO_set_mem_functions_FP = (CRYPTO_set_mem_functions_TYPE)GetProcAddress(libeayHandleM, "CRYPTO_set_mem_functions")) == NULL) goto err;
//	if ((CRYPTO_thread_id_FP = (CRYPTO_thread_id_TYPE)GetProcAddress(libeayHandleM, "CRYPTO_thread_id")) == NULL) goto err;
//	if ((DES_cbc_cksum_FP = (DES_cbc_cksum_TYPE)GetProcAddress(libeayHandleM, "DES_cbc_cksum")) == NULL) goto err;
//	if ((DES_cbc_encrypt_FP = (DES_cbc_encrypt_TYPE)GetProcAddress(libeayHandleM, "DES_cbc_encrypt")) == NULL) goto err;
//	if ((DES_cfb64_encrypt_FP = (DES_cfb64_encrypt_TYPE)GetProcAddress(libeayHandleM, "DES_cfb64_encrypt")) == NULL) goto err;
//	if ((DES_cfb_encrypt_FP = (DES_cfb_encrypt_TYPE)GetProcAddress(libeayHandleM, "DES_cfb_encrypt")) == NULL) goto err;
//	if ((DES_check_key_parity_FP = (DES_check_key_parity_TYPE)GetProcAddress(libeayHandleM, "DES_check_key_parity")) == NULL) goto err;
//	if ((DES_crypt_FP = (DES_crypt_TYPE)GetProcAddress(libeayHandleM, "DES_crypt")) == NULL) goto err;
//	if ((DES_decrypt3_FP = (DES_decrypt3_TYPE)GetProcAddress(libeayHandleM, "DES_decrypt3")) == NULL) goto err;
//	if ((DES_ecb3_encrypt_FP = (DES_ecb3_encrypt_TYPE)GetProcAddress(libeayHandleM, "DES_ecb3_encrypt")) == NULL) goto err;
//	if ((DES_ecb_encrypt_FP = (DES_ecb_encrypt_TYPE)GetProcAddress(libeayHandleM, "DES_ecb_encrypt")) == NULL) goto err;
//	if ((DES_ede3_cbc_encrypt_FP = (DES_ede3_cbc_encrypt_TYPE)GetProcAddress(libeayHandleM, "DES_ede3_cbc_encrypt")) == NULL) goto err;
//	if ((DES_ede3_cbcm_encrypt_FP = (DES_ede3_cbcm_encrypt_TYPE)GetProcAddress(libeayHandleM, "DES_ede3_cbcm_encrypt")) == NULL) goto err;
//	if ((DES_ede3_cfb64_encrypt_FP = (DES_ede3_cfb64_encrypt_TYPE)GetProcAddress(libeayHandleM, "DES_ede3_cfb64_encrypt")) == NULL) goto err;
//	if ((DES_ede3_ofb64_encrypt_FP = (DES_ede3_ofb64_encrypt_TYPE)GetProcAddress(libeayHandleM, "DES_ede3_ofb64_encrypt")) == NULL) goto err;
//	if ((DES_enc_read_FP = (DES_enc_read_TYPE)GetProcAddress(libeayHandleM, "DES_enc_read")) == NULL) goto err;
//	if ((DES_enc_write_FP = (DES_enc_write_TYPE)GetProcAddress(libeayHandleM, "DES_enc_write")) == NULL) goto err;
//	if ((DES_encrypt1_FP = (DES_encrypt1_TYPE)GetProcAddress(libeayHandleM, "DES_encrypt1")) == NULL) goto err;
//	if ((DES_encrypt2_FP = (DES_encrypt2_TYPE)GetProcAddress(libeayHandleM, "DES_encrypt2")) == NULL) goto err;
//	if ((DES_encrypt3_FP = (DES_encrypt3_TYPE)GetProcAddress(libeayHandleM, "DES_encrypt3")) == NULL) goto err;
//	if ((DES_fcrypt_FP = (DES_fcrypt_TYPE)GetProcAddress(libeayHandleM, "DES_fcrypt")) == NULL) goto err;
//	if ((DES_is_weak_key_FP = (DES_is_weak_key_TYPE)GetProcAddress(libeayHandleM, "DES_is_weak_key")) == NULL) goto err;
//	if ((DES_key_sched_FP = (DES_key_sched_TYPE)GetProcAddress(libeayHandleM, "DES_key_sched")) == NULL) goto err;
//	if ((DES_ncbc_encrypt_FP = (DES_ncbc_encrypt_TYPE)GetProcAddress(libeayHandleM, "DES_ncbc_encrypt")) == NULL) goto err;
//	if ((DES_ofb64_encrypt_FP = (DES_ofb64_encrypt_TYPE)GetProcAddress(libeayHandleM, "DES_ofb64_encrypt")) == NULL) goto err;
//	if ((DES_ofb_encrypt_FP = (DES_ofb_encrypt_TYPE)GetProcAddress(libeayHandleM, "DES_ofb_encrypt")) == NULL) goto err;
//	if ((DES_options_FP = (DES_options_TYPE)GetProcAddress(libeayHandleM, "DES_options")) == NULL) goto err;
//	if ((DES_pcbc_encrypt_FP = (DES_pcbc_encrypt_TYPE)GetProcAddress(libeayHandleM, "DES_pcbc_encrypt")) == NULL) goto err;
//	if ((DES_quad_cksum_FP = (DES_quad_cksum_TYPE)GetProcAddress(libeayHandleM, "DES_quad_cksum")) == NULL) goto err;
//	if ((DES_random_key_FP = (DES_random_key_TYPE)GetProcAddress(libeayHandleM, "DES_random_key")) == NULL) goto err;
//	if ((DES_read_2passwords_FP = (DES_read_2passwords_TYPE)GetProcAddress(libeayHandleM, "DES_read_2passwords")) == NULL) goto err;
//	if ((DES_read_password_FP = (DES_read_password_TYPE)GetProcAddress(libeayHandleM, "DES_read_password")) == NULL) goto err;
//	if ((DES_set_key_FP = (DES_set_key_TYPE)GetProcAddress(libeayHandleM, "DES_set_key")) == NULL) goto err;
//	if ((DES_set_key_checked_FP = (DES_set_key_checked_TYPE)GetProcAddress(libeayHandleM, "DES_set_key_checked")) == NULL) goto err;
//	if ((DES_set_key_unchecked_FP = (DES_set_key_unchecked_TYPE)GetProcAddress(libeayHandleM, "DES_set_key_unchecked")) == NULL) goto err;
//	if ((DES_set_odd_parity_FP = (DES_set_odd_parity_TYPE)GetProcAddress(libeayHandleM, "DES_set_odd_parity")) == NULL) goto err;
//	if ((DES_string_to_2keys_FP = (DES_string_to_2keys_TYPE)GetProcAddress(libeayHandleM, "DES_string_to_2keys")) == NULL) goto err;
//	if ((DES_string_to_key_FP = (DES_string_to_key_TYPE)GetProcAddress(libeayHandleM, "DES_string_to_key")) == NULL) goto err;
//	if ((DES_xcbc_encrypt_FP = (DES_xcbc_encrypt_TYPE)GetProcAddress(libeayHandleM, "DES_xcbc_encrypt")) == NULL) goto err;
//	if ((DES_xwhite_in2out_FP = (DES_xwhite_in2out_TYPE)GetProcAddress(libeayHandleM, "DES_xwhite_in2out")) == NULL) goto err;
//	if ((DH_OpenSSL_FP = (DH_OpenSSL_TYPE)GetProcAddress(libeayHandleM, "DH_OpenSSL")) == NULL) goto err;
//	if ((DH_check_FP = (DH_check_TYPE)GetProcAddress(libeayHandleM, "DH_check")) == NULL) goto err;
//	if ((DH_compute_key_FP = (DH_compute_key_TYPE)GetProcAddress(libeayHandleM, "DH_compute_key")) == NULL) goto err;
	if ((DH_free_FP = (DH_free_TYPE)GetProcAddress(libeayHandleM, "DH_free")) == NULL) goto err;
//	if ((DH_generate_key_FP = (DH_generate_key_TYPE)GetProcAddress(libeayHandleM, "DH_generate_key")) == NULL) goto err;
//	if ((DH_generate_parameters_FP = (DH_generate_parameters_TYPE)GetProcAddress(libeayHandleM, "DH_generate_parameters")) == NULL) goto err;
//	if ((DH_get_default_method_FP = (DH_get_default_method_TYPE)GetProcAddress(libeayHandleM, "DH_get_default_method")) == NULL) goto err;
//	if ((DH_get_ex_data_FP = (DH_get_ex_data_TYPE)GetProcAddress(libeayHandleM, "DH_get_ex_data")) == NULL) goto err;
//	if ((DH_get_ex_new_index_FP = (DH_get_ex_new_index_TYPE)GetProcAddress(libeayHandleM, "DH_get_ex_new_index")) == NULL) goto err;
	if ((DH_new_FP = (DH_new_TYPE)GetProcAddress(libeayHandleM, "DH_new")) == NULL) goto err;
//	if ((DH_new_method_FP = (DH_new_method_TYPE)GetProcAddress(libeayHandleM, "DH_new_method")) == NULL) goto err;
//	if ((DH_set_default_method_FP = (DH_set_default_method_TYPE)GetProcAddress(libeayHandleM, "DH_set_default_method")) == NULL) goto err;
//	if ((DH_set_ex_data_FP = (DH_set_ex_data_TYPE)GetProcAddress(libeayHandleM, "DH_set_ex_data")) == NULL) goto err;
//	if ((DH_set_method_FP = (DH_set_method_TYPE)GetProcAddress(libeayHandleM, "DH_set_method")) == NULL) goto err;
//	if ((DH_size_FP = (DH_size_TYPE)GetProcAddress(libeayHandleM, "DH_size")) == NULL) goto err;
//	if ((DH_up_ref_FP = (DH_up_ref_TYPE)GetProcAddress(libeayHandleM, "DH_up_ref")) == NULL) goto err;
//	if ((DHparams_print_FP = (DHparams_print_TYPE)GetProcAddress(libeayHandleM, "DHparams_print")) == NULL) goto err;
//	if ((DHparams_print_fp_FP = (DHparams_print_fp_TYPE)GetProcAddress(libeayHandleM, "DHparams_print_fp")) == NULL) goto err;
//	if ((DIRECTORYSTRING_free_FP = (DIRECTORYSTRING_free_TYPE)GetProcAddress(libeayHandleM, "DIRECTORYSTRING_free")) == NULL) goto err;
//	if ((DIRECTORYSTRING_it_FP = (DIRECTORYSTRING_it_TYPE)GetProcAddress(libeayHandleM, "DIRECTORYSTRING_it")) == NULL) goto err;
//	if ((DIRECTORYSTRING_new_FP = (DIRECTORYSTRING_new_TYPE)GetProcAddress(libeayHandleM, "DIRECTORYSTRING_new")) == NULL) goto err;
//	if ((DISPLAYTEXT_free_FP = (DISPLAYTEXT_free_TYPE)GetProcAddress(libeayHandleM, "DISPLAYTEXT_free")) == NULL) goto err;
//	if ((DISPLAYTEXT_it_FP = (DISPLAYTEXT_it_TYPE)GetProcAddress(libeayHandleM, "DISPLAYTEXT_it")) == NULL) goto err;
//	if ((DISPLAYTEXT_new_FP = (DISPLAYTEXT_new_TYPE)GetProcAddress(libeayHandleM, "DISPLAYTEXT_new")) == NULL) goto err;
//	if ((DIST_POINT_NAME_free_FP = (DIST_POINT_NAME_free_TYPE)GetProcAddress(libeayHandleM, "DIST_POINT_NAME_free")) == NULL) goto err;
//	if ((DIST_POINT_NAME_it_FP = (DIST_POINT_NAME_it_TYPE)GetProcAddress(libeayHandleM, "DIST_POINT_NAME_it")) == NULL) goto err;
//	if ((DIST_POINT_NAME_new_FP = (DIST_POINT_NAME_new_TYPE)GetProcAddress(libeayHandleM, "DIST_POINT_NAME_new")) == NULL) goto err;
//	if ((DIST_POINT_free_FP = (DIST_POINT_free_TYPE)GetProcAddress(libeayHandleM, "DIST_POINT_free")) == NULL) goto err;
//	if ((DIST_POINT_it_FP = (DIST_POINT_it_TYPE)GetProcAddress(libeayHandleM, "DIST_POINT_it")) == NULL) goto err;
//	if ((DIST_POINT_new_FP = (DIST_POINT_new_TYPE)GetProcAddress(libeayHandleM, "DIST_POINT_new")) == NULL) goto err;
//	if ((DSA_OpenSSL_FP = (DSA_OpenSSL_TYPE)GetProcAddress(libeayHandleM, "DSA_OpenSSL")) == NULL) goto err;
//	if ((DSA_SIG_free_FP = (DSA_SIG_free_TYPE)GetProcAddress(libeayHandleM, "DSA_SIG_free")) == NULL) goto err;
//	if ((DSA_SIG_new_FP = (DSA_SIG_new_TYPE)GetProcAddress(libeayHandleM, "DSA_SIG_new")) == NULL) goto err;
//	if ((DSA_do_sign_FP = (DSA_do_sign_TYPE)GetProcAddress(libeayHandleM, "DSA_do_sign")) == NULL) goto err;
//	if ((DSA_do_verify_FP = (DSA_do_verify_TYPE)GetProcAddress(libeayHandleM, "DSA_do_verify")) == NULL) goto err;
//	if ((DSA_dup_DH_FP = (DSA_dup_DH_TYPE)GetProcAddress(libeayHandleM, "DSA_dup_DH")) == NULL) goto err;
	if ((DSA_free_FP = (DSA_free_TYPE)GetProcAddress(libeayHandleM, "DSA_free")) == NULL) goto err;
	if ((DSA_generate_key_FP = (DSA_generate_key_TYPE)GetProcAddress(libeayHandleM, "DSA_generate_key")) == NULL) goto err;
	if ((DSA_generate_parameters_FP = (DSA_generate_parameters_TYPE)GetProcAddress(libeayHandleM, "DSA_generate_parameters")) == NULL) goto err;
//	if ((DSA_get_default_method_FP = (DSA_get_default_method_TYPE)GetProcAddress(libeayHandleM, "DSA_get_default_method")) == NULL) goto err;
//	if ((DSA_get_ex_data_FP = (DSA_get_ex_data_TYPE)GetProcAddress(libeayHandleM, "DSA_get_ex_data")) == NULL) goto err;
//	if ((DSA_get_ex_new_index_FP = (DSA_get_ex_new_index_TYPE)GetProcAddress(libeayHandleM, "DSA_get_ex_new_index")) == NULL) goto err;
//	if ((DSA_new_FP = (DSA_new_TYPE)GetProcAddress(libeayHandleM, "DSA_new")) == NULL) goto err;
//	if ((DSA_new_method_FP = (DSA_new_method_TYPE)GetProcAddress(libeayHandleM, "DSA_new_method")) == NULL) goto err;
//	if ((DSA_print_FP = (DSA_print_TYPE)GetProcAddress(libeayHandleM, "DSA_print")) == NULL) goto err;
//	if ((DSA_print_fp_FP = (DSA_print_fp_TYPE)GetProcAddress(libeayHandleM, "DSA_print_fp")) == NULL) goto err;
//	if ((DSA_set_default_method_FP = (DSA_set_default_method_TYPE)GetProcAddress(libeayHandleM, "DSA_set_default_method")) == NULL) goto err;
//	if ((DSA_set_ex_data_FP = (DSA_set_ex_data_TYPE)GetProcAddress(libeayHandleM, "DSA_set_ex_data")) == NULL) goto err;
//	if ((DSA_set_method_FP = (DSA_set_method_TYPE)GetProcAddress(libeayHandleM, "DSA_set_method")) == NULL) goto err;
//	if ((DSA_sign_FP = (DSA_sign_TYPE)GetProcAddress(libeayHandleM, "DSA_sign")) == NULL) goto err;
//	if ((DSA_sign_setup_FP = (DSA_sign_setup_TYPE)GetProcAddress(libeayHandleM, "DSA_sign_setup")) == NULL) goto err;
//	if ((DSA_size_FP = (DSA_size_TYPE)GetProcAddress(libeayHandleM, "DSA_size")) == NULL) goto err;
//	if ((DSA_up_ref_FP = (DSA_up_ref_TYPE)GetProcAddress(libeayHandleM, "DSA_up_ref")) == NULL) goto err;
//	if ((DSA_verify_FP = (DSA_verify_TYPE)GetProcAddress(libeayHandleM, "DSA_verify")) == NULL) goto err;
//	if ((DSAparams_print_FP = (DSAparams_print_TYPE)GetProcAddress(libeayHandleM, "DSAparams_print")) == NULL) goto err;
//	if ((DSAparams_print_fp_FP = (DSAparams_print_fp_TYPE)GetProcAddress(libeayHandleM, "DSAparams_print_fp")) == NULL) goto err;
//	if ((DSO_METHOD_dl_FP = (DSO_METHOD_dl_TYPE)GetProcAddress(libeayHandleM, "DSO_METHOD_dl")) == NULL) goto err;
//	if ((DSO_METHOD_dlfcn_FP = (DSO_METHOD_dlfcn_TYPE)GetProcAddress(libeayHandleM, "DSO_METHOD_dlfcn")) == NULL) goto err;
//	if ((DSO_METHOD_null_FP = (DSO_METHOD_null_TYPE)GetProcAddress(libeayHandleM, "DSO_METHOD_null")) == NULL) goto err;
//	if ((DSO_METHOD_openssl_FP = (DSO_METHOD_openssl_TYPE)GetProcAddress(libeayHandleM, "DSO_METHOD_openssl")) == NULL) goto err;
//	if ((DSO_METHOD_vms_FP = (DSO_METHOD_vms_TYPE)GetProcAddress(libeayHandleM, "DSO_METHOD_vms")) == NULL) goto err;
//	if ((DSO_METHOD_win32_FP = (DSO_METHOD_win32_TYPE)GetProcAddress(libeayHandleM, "DSO_METHOD_win32")) == NULL) goto err;
//	if ((DSO_bind_func_FP = (DSO_bind_func_TYPE)GetProcAddress(libeayHandleM, "DSO_bind_func")) == NULL) goto err;
//	if ((DSO_bind_var_FP = (DSO_bind_var_TYPE)GetProcAddress(libeayHandleM, "DSO_bind_var")) == NULL) goto err;
//	if ((DSO_convert_filename_FP = (DSO_convert_filename_TYPE)GetProcAddress(libeayHandleM, "DSO_convert_filename")) == NULL) goto err;
//	if ((DSO_ctrl_FP = (DSO_ctrl_TYPE)GetProcAddress(libeayHandleM, "DSO_ctrl")) == NULL) goto err;
//	if ((DSO_flags_FP = (DSO_flags_TYPE)GetProcAddress(libeayHandleM, "DSO_flags")) == NULL) goto err;
//	if ((DSO_free_FP = (DSO_free_TYPE)GetProcAddress(libeayHandleM, "DSO_free")) == NULL) goto err;
//	if ((DSO_get_default_method_FP = (DSO_get_default_method_TYPE)GetProcAddress(libeayHandleM, "DSO_get_default_method")) == NULL) goto err;
//	if ((DSO_get_filename_FP = (DSO_get_filename_TYPE)GetProcAddress(libeayHandleM, "DSO_get_filename")) == NULL) goto err;
//	if ((DSO_get_loaded_filename_FP = (DSO_get_loaded_filename_TYPE)GetProcAddress(libeayHandleM, "DSO_get_loaded_filename")) == NULL) goto err;
//	if ((DSO_get_method_FP = (DSO_get_method_TYPE)GetProcAddress(libeayHandleM, "DSO_get_method")) == NULL) goto err;
//	if ((DSO_load_FP = (DSO_load_TYPE)GetProcAddress(libeayHandleM, "DSO_load")) == NULL) goto err;
//	if ((DSO_new_FP = (DSO_new_TYPE)GetProcAddress(libeayHandleM, "DSO_new")) == NULL) goto err;
//	if ((DSO_new_method_FP = (DSO_new_method_TYPE)GetProcAddress(libeayHandleM, "DSO_new_method")) == NULL) goto err;
//	if ((DSO_set_default_method_FP = (DSO_set_default_method_TYPE)GetProcAddress(libeayHandleM, "DSO_set_default_method")) == NULL) goto err;
//	if ((DSO_set_filename_FP = (DSO_set_filename_TYPE)GetProcAddress(libeayHandleM, "DSO_set_filename")) == NULL) goto err;
//	if ((DSO_set_method_FP = (DSO_set_method_TYPE)GetProcAddress(libeayHandleM, "DSO_set_method")) == NULL) goto err;
//	if ((DSO_set_name_converter_FP = (DSO_set_name_converter_TYPE)GetProcAddress(libeayHandleM, "DSO_set_name_converter")) == NULL) goto err;
//	if ((DSO_up_ref_FP = (DSO_up_ref_TYPE)GetProcAddress(libeayHandleM, "DSO_up_ref")) == NULL) goto err;
//	if ((EC_GFp_mont_method_FP = (EC_GFp_mont_method_TYPE)GetProcAddress(libeayHandleM, "EC_GFp_mont_method")) == NULL) goto err;
//	if ((EC_GFp_simple_method_FP = (EC_GFp_simple_method_TYPE)GetProcAddress(libeayHandleM, "EC_GFp_simple_method")) == NULL) goto err;
//	if ((EC_GROUP_clear_free_FP = (EC_GROUP_clear_free_TYPE)GetProcAddress(libeayHandleM, "EC_GROUP_clear_free")) == NULL) goto err;
//	if ((EC_GROUP_copy_FP = (EC_GROUP_copy_TYPE)GetProcAddress(libeayHandleM, "EC_GROUP_copy")) == NULL) goto err;
//	if ((EC_GROUP_free_FP = (EC_GROUP_free_TYPE)GetProcAddress(libeayHandleM, "EC_GROUP_free")) == NULL) goto err;
//	if ((EC_GROUP_get0_generator_FP = (EC_GROUP_get0_generator_TYPE)GetProcAddress(libeayHandleM, "EC_GROUP_get0_generator")) == NULL) goto err;
//	if ((EC_GROUP_get_cofactor_FP = (EC_GROUP_get_cofactor_TYPE)GetProcAddress(libeayHandleM, "EC_GROUP_get_cofactor")) == NULL) goto err;
//	if ((EC_GROUP_get_curve_GFp_FP = (EC_GROUP_get_curve_GFp_TYPE)GetProcAddress(libeayHandleM, "EC_GROUP_get_curve_GFp")) == NULL) goto err;
//	if ((EC_GROUP_get_order_FP = (EC_GROUP_get_order_TYPE)GetProcAddress(libeayHandleM, "EC_GROUP_get_order")) == NULL) goto err;
//	if ((EC_GROUP_method_of_FP = (EC_GROUP_method_of_TYPE)GetProcAddress(libeayHandleM, "EC_GROUP_method_of")) == NULL) goto err;
//	if ((EC_GROUP_new_FP = (EC_GROUP_new_TYPE)GetProcAddress(libeayHandleM, "EC_GROUP_new")) == NULL) goto err;
//	if ((EC_GROUP_new_curve_GFp_FP = (EC_GROUP_new_curve_GFp_TYPE)GetProcAddress(libeayHandleM, "EC_GROUP_new_curve_GFp")) == NULL) goto err;
//	if ((EC_GROUP_precompute_mult_FP = (EC_GROUP_precompute_mult_TYPE)GetProcAddress(libeayHandleM, "EC_GROUP_precompute_mult")) == NULL) goto err;
//	if ((EC_GROUP_set_curve_GFp_FP = (EC_GROUP_set_curve_GFp_TYPE)GetProcAddress(libeayHandleM, "EC_GROUP_set_curve_GFp")) == NULL) goto err;
//	if ((EC_GROUP_set_generator_FP = (EC_GROUP_set_generator_TYPE)GetProcAddress(libeayHandleM, "EC_GROUP_set_generator")) == NULL) goto err;
//	if ((EC_POINT_add_FP = (EC_POINT_add_TYPE)GetProcAddress(libeayHandleM, "EC_POINT_add")) == NULL) goto err;
//	if ((EC_POINT_clear_free_FP = (EC_POINT_clear_free_TYPE)GetProcAddress(libeayHandleM, "EC_POINT_clear_free")) == NULL) goto err;
//	if ((EC_POINT_cmp_FP = (EC_POINT_cmp_TYPE)GetProcAddress(libeayHandleM, "EC_POINT_cmp")) == NULL) goto err;
//	if ((EC_POINT_copy_FP = (EC_POINT_copy_TYPE)GetProcAddress(libeayHandleM, "EC_POINT_copy")) == NULL) goto err;
//	if ((EC_POINT_dbl_FP = (EC_POINT_dbl_TYPE)GetProcAddress(libeayHandleM, "EC_POINT_dbl")) == NULL) goto err;
//	if ((EC_POINT_free_FP = (EC_POINT_free_TYPE)GetProcAddress(libeayHandleM, "EC_POINT_free")) == NULL) goto err;
//	if ((EC_POINT_get_Jprojective_coordinates_GFp_FP = (EC_POINT_get_Jprojective_coordinates_GFp_TYPE)GetProcAddress(libeayHandleM, "EC_POINT_get_Jprojective_coordinates_GFp")) == NULL) goto err;
//	if ((EC_POINT_get_affine_coordinates_GFp_FP = (EC_POINT_get_affine_coordinates_GFp_TYPE)GetProcAddress(libeayHandleM, "EC_POINT_get_affine_coordinates_GFp")) == NULL) goto err;
//	if ((EC_POINT_invert_FP = (EC_POINT_invert_TYPE)GetProcAddress(libeayHandleM, "EC_POINT_invert")) == NULL) goto err;
//	if ((EC_POINT_is_at_infinity_FP = (EC_POINT_is_at_infinity_TYPE)GetProcAddress(libeayHandleM, "EC_POINT_is_at_infinity")) == NULL) goto err;
//	if ((EC_POINT_is_on_curve_FP = (EC_POINT_is_on_curve_TYPE)GetProcAddress(libeayHandleM, "EC_POINT_is_on_curve")) == NULL) goto err;
//	if ((EC_POINT_make_affine_FP = (EC_POINT_make_affine_TYPE)GetProcAddress(libeayHandleM, "EC_POINT_make_affine")) == NULL) goto err;
//	if ((EC_POINT_method_of_FP = (EC_POINT_method_of_TYPE)GetProcAddress(libeayHandleM, "EC_POINT_method_of")) == NULL) goto err;
//	if ((EC_POINT_mul_FP = (EC_POINT_mul_TYPE)GetProcAddress(libeayHandleM, "EC_POINT_mul")) == NULL) goto err;
//	if ((EC_POINT_new_FP = (EC_POINT_new_TYPE)GetProcAddress(libeayHandleM, "EC_POINT_new")) == NULL) goto err;
//	if ((EC_POINT_oct2point_FP = (EC_POINT_oct2point_TYPE)GetProcAddress(libeayHandleM, "EC_POINT_oct2point")) == NULL) goto err;
//	if ((EC_POINT_point2oct_FP = (EC_POINT_point2oct_TYPE)GetProcAddress(libeayHandleM, "EC_POINT_point2oct")) == NULL) goto err;
//	if ((EC_POINT_set_Jprojective_coordinates_GFp_FP = (EC_POINT_set_Jprojective_coordinates_GFp_TYPE)GetProcAddress(libeayHandleM, "EC_POINT_set_Jprojective_coordinates_GFp")) == NULL) goto err;
//	if ((EC_POINT_set_affine_coordinates_GFp_FP = (EC_POINT_set_affine_coordinates_GFp_TYPE)GetProcAddress(libeayHandleM, "EC_POINT_set_affine_coordinates_GFp")) == NULL) goto err;
//	if ((EC_POINT_set_compressed_coordinates_GFp_FP = (EC_POINT_set_compressed_coordinates_GFp_TYPE)GetProcAddress(libeayHandleM, "EC_POINT_set_compressed_coordinates_GFp")) == NULL) goto err;
//	if ((EC_POINT_set_to_infinity_FP = (EC_POINT_set_to_infinity_TYPE)GetProcAddress(libeayHandleM, "EC_POINT_set_to_infinity")) == NULL) goto err;
//	if ((EC_POINTs_make_affine_FP = (EC_POINTs_make_affine_TYPE)GetProcAddress(libeayHandleM, "EC_POINTs_make_affine")) == NULL) goto err;
//	if ((EC_POINTs_mul_FP = (EC_POINTs_mul_TYPE)GetProcAddress(libeayHandleM, "EC_POINTs_mul")) == NULL) goto err;
//	if ((EDIPARTYNAME_free_FP = (EDIPARTYNAME_free_TYPE)GetProcAddress(libeayHandleM, "EDIPARTYNAME_free")) == NULL) goto err;
//	if ((EDIPARTYNAME_it_FP = (EDIPARTYNAME_it_TYPE)GetProcAddress(libeayHandleM, "EDIPARTYNAME_it")) == NULL) goto err;
//	if ((EDIPARTYNAME_new_FP = (EDIPARTYNAME_new_TYPE)GetProcAddress(libeayHandleM, "EDIPARTYNAME_new")) == NULL) goto err;
//	if ((ERR_add_error_data_FP = (ERR_add_error_data_TYPE)GetProcAddress(libeayHandleM, "ERR_add_error_data")) == NULL) goto err;
	if ((ERR_clear_error_FP = (ERR_clear_error_TYPE)GetProcAddress(libeayHandleM, "ERR_clear_error")) == NULL) goto err;
//	if ((ERR_error_string_FP = (ERR_error_string_TYPE)GetProcAddress(libeayHandleM, "ERR_error_string")) == NULL) goto err;
	if ((ERR_error_string_n_FP = (ERR_error_string_n_TYPE)GetProcAddress(libeayHandleM, "ERR_error_string_n")) == NULL) goto err;
//	if ((ERR_free_strings_FP = (ERR_free_strings_TYPE)GetProcAddress(libeayHandleM, "ERR_free_strings")) == NULL) goto err;
//	if ((ERR_func_error_string_FP = (ERR_func_error_string_TYPE)GetProcAddress(libeayHandleM, "ERR_func_error_string")) == NULL) goto err;
//	if ((ERR_get_err_state_table_FP = (ERR_get_err_state_table_TYPE)GetProcAddress(libeayHandleM, "ERR_get_err_state_table")) == NULL) goto err;
	if ((ERR_get_error_FP = (ERR_get_error_TYPE)GetProcAddress(libeayHandleM, "ERR_get_error")) == NULL) goto err;
//	if ((ERR_get_error_line_FP = (ERR_get_error_line_TYPE)GetProcAddress(libeayHandleM, "ERR_get_error_line")) == NULL) goto err;
	if ((ERR_get_error_line_data_FP = (ERR_get_error_line_data_TYPE)GetProcAddress(libeayHandleM, "ERR_get_error_line_data")) == NULL) goto err;
//	if ((ERR_get_implementation_FP = (ERR_get_implementation_TYPE)GetProcAddress(libeayHandleM, "ERR_get_implementation")) == NULL) goto err;
//	if ((ERR_get_next_error_library_FP = (ERR_get_next_error_library_TYPE)GetProcAddress(libeayHandleM, "ERR_get_next_error_library")) == NULL) goto err;
//	if ((ERR_get_state_FP = (ERR_get_state_TYPE)GetProcAddress(libeayHandleM, "ERR_get_state")) == NULL) goto err;
//	if ((ERR_get_string_table_FP = (ERR_get_string_table_TYPE)GetProcAddress(libeayHandleM, "ERR_get_string_table")) == NULL) goto err;
//	if ((ERR_lib_error_string_FP = (ERR_lib_error_string_TYPE)GetProcAddress(libeayHandleM, "ERR_lib_error_string")) == NULL) goto err;
//	if ((ERR_load_ASN1_strings_FP = (ERR_load_ASN1_strings_TYPE)GetProcAddress(libeayHandleM, "ERR_load_ASN1_strings")) == NULL) goto err;
//	if ((ERR_load_BIO_strings_FP = (ERR_load_BIO_strings_TYPE)GetProcAddress(libeayHandleM, "ERR_load_BIO_strings")) == NULL) goto err;
//	if ((ERR_load_BN_strings_FP = (ERR_load_BN_strings_TYPE)GetProcAddress(libeayHandleM, "ERR_load_BN_strings")) == NULL) goto err;
//	if ((ERR_load_BUF_strings_FP = (ERR_load_BUF_strings_TYPE)GetProcAddress(libeayHandleM, "ERR_load_BUF_strings")) == NULL) goto err;
//	if ((ERR_load_COMP_strings_FP = (ERR_load_COMP_strings_TYPE)GetProcAddress(libeayHandleM, "ERR_load_COMP_strings")) == NULL) goto err;
//	if ((ERR_load_CONF_strings_FP = (ERR_load_CONF_strings_TYPE)GetProcAddress(libeayHandleM, "ERR_load_CONF_strings")) == NULL) goto err;
//	if ((ERR_load_CRYPTO_strings_FP = (ERR_load_CRYPTO_strings_TYPE)GetProcAddress(libeayHandleM, "ERR_load_CRYPTO_strings")) == NULL) goto err;
//	if ((ERR_load_DH_strings_FP = (ERR_load_DH_strings_TYPE)GetProcAddress(libeayHandleM, "ERR_load_DH_strings")) == NULL) goto err;
//	if ((ERR_load_DSA_strings_FP = (ERR_load_DSA_strings_TYPE)GetProcAddress(libeayHandleM, "ERR_load_DSA_strings")) == NULL) goto err;
//	if ((ERR_load_DSO_strings_FP = (ERR_load_DSO_strings_TYPE)GetProcAddress(libeayHandleM, "ERR_load_DSO_strings")) == NULL) goto err;
//	if ((ERR_load_EC_strings_FP = (ERR_load_EC_strings_TYPE)GetProcAddress(libeayHandleM, "ERR_load_EC_strings")) == NULL) goto err;
//	if ((ERR_load_ERR_strings_FP = (ERR_load_ERR_strings_TYPE)GetProcAddress(libeayHandleM, "ERR_load_ERR_strings")) == NULL) goto err;
//	if ((ERR_load_EVP_strings_FP = (ERR_load_EVP_strings_TYPE)GetProcAddress(libeayHandleM, "ERR_load_EVP_strings")) == NULL) goto err;
//	if ((ERR_load_OBJ_strings_FP = (ERR_load_OBJ_strings_TYPE)GetProcAddress(libeayHandleM, "ERR_load_OBJ_strings")) == NULL) goto err;
//	if ((ERR_load_OCSP_strings_FP = (ERR_load_OCSP_strings_TYPE)GetProcAddress(libeayHandleM, "ERR_load_OCSP_strings")) == NULL) goto err;
//	if ((ERR_load_PEM_strings_FP = (ERR_load_PEM_strings_TYPE)GetProcAddress(libeayHandleM, "ERR_load_PEM_strings")) == NULL) goto err;
//	if ((ERR_load_PKCS12_strings_FP = (ERR_load_PKCS12_strings_TYPE)GetProcAddress(libeayHandleM, "ERR_load_PKCS12_strings")) == NULL) goto err;
//	if ((ERR_load_PKCS7_strings_FP = (ERR_load_PKCS7_strings_TYPE)GetProcAddress(libeayHandleM, "ERR_load_PKCS7_strings")) == NULL) goto err;
//	if ((ERR_load_RAND_strings_FP = (ERR_load_RAND_strings_TYPE)GetProcAddress(libeayHandleM, "ERR_load_RAND_strings")) == NULL) goto err;
//	if ((ERR_load_RSA_strings_FP = (ERR_load_RSA_strings_TYPE)GetProcAddress(libeayHandleM, "ERR_load_RSA_strings")) == NULL) goto err;
//	if ((ERR_load_UI_strings_FP = (ERR_load_UI_strings_TYPE)GetProcAddress(libeayHandleM, "ERR_load_UI_strings")) == NULL) goto err;
//	if ((ERR_load_X509V3_strings_FP = (ERR_load_X509V3_strings_TYPE)GetProcAddress(libeayHandleM, "ERR_load_X509V3_strings")) == NULL) goto err;
//	if ((ERR_load_X509_strings_FP = (ERR_load_X509_strings_TYPE)GetProcAddress(libeayHandleM, "ERR_load_X509_strings")) == NULL) goto err;
//	if ((ERR_load_crypto_strings_FP = (ERR_load_crypto_strings_TYPE)GetProcAddress(libeayHandleM, "ERR_load_crypto_strings")) == NULL) goto err;
//	if ((ERR_load_strings_FP = (ERR_load_strings_TYPE)GetProcAddress(libeayHandleM, "ERR_load_strings")) == NULL) goto err;
	if ((ERR_peek_error_FP = (ERR_peek_error_TYPE)GetProcAddress(libeayHandleM, "ERR_peek_error")) == NULL) goto err;
//	if ((ERR_peek_error_line_FP = (ERR_peek_error_line_TYPE)GetProcAddress(libeayHandleM, "ERR_peek_error_line")) == NULL) goto err;
//	if ((ERR_peek_error_line_data_FP = (ERR_peek_error_line_data_TYPE)GetProcAddress(libeayHandleM, "ERR_peek_error_line_data")) == NULL) goto err;
	if ((ERR_peek_last_error_FP = (ERR_peek_last_error_TYPE)GetProcAddress(libeayHandleM, "ERR_peek_last_error")) == NULL) goto err;
//	if ((ERR_peek_last_error_line_FP = (ERR_peek_last_error_line_TYPE)GetProcAddress(libeayHandleM, "ERR_peek_last_error_line")) == NULL) goto err;
//	if ((ERR_peek_last_error_line_data_FP = (ERR_peek_last_error_line_data_TYPE)GetProcAddress(libeayHandleM, "ERR_peek_last_error_line_data")) == NULL) goto err;
//	if ((ERR_print_errors_FP = (ERR_print_errors_TYPE)GetProcAddress(libeayHandleM, "ERR_print_errors")) == NULL) goto err;
//	if ((ERR_print_errors_cb_FP = (ERR_print_errors_cb_TYPE)GetProcAddress(libeayHandleM, "ERR_print_errors_cb")) == NULL) goto err;
//	if ((ERR_print_errors_fp_FP = (ERR_print_errors_fp_TYPE)GetProcAddress(libeayHandleM, "ERR_print_errors_fp")) == NULL) goto err;
	if ((ERR_put_error_FP = (ERR_put_error_TYPE)GetProcAddress(libeayHandleM, "ERR_put_error")) == NULL) goto err;
//	if ((ERR_reason_error_string_FP = (ERR_reason_error_string_TYPE)GetProcAddress(libeayHandleM, "ERR_reason_error_string")) == NULL) goto err;
	if ((ERR_remove_state_FP = (ERR_remove_state_TYPE)GetProcAddress(libeayHandleM, "ERR_remove_state")) == NULL) goto err;
//	if ((ERR_set_error_data_FP = (ERR_set_error_data_TYPE)GetProcAddress(libeayHandleM, "ERR_set_error_data")) == NULL) goto err;
//	if ((ERR_set_implementation_FP = (ERR_set_implementation_TYPE)GetProcAddress(libeayHandleM, "ERR_set_implementation")) == NULL) goto err;
//	if ((ERR_unload_strings_FP = (ERR_unload_strings_TYPE)GetProcAddress(libeayHandleM, "ERR_unload_strings")) == NULL) goto err;
//	if ((EVP_BytesToKey_FP = (EVP_BytesToKey_TYPE)GetProcAddress(libeayHandleM, "EVP_BytesToKey")) == NULL) goto err;
//	if ((EVP_CIPHER_CTX_cleanup_FP = (EVP_CIPHER_CTX_cleanup_TYPE)GetProcAddress(libeayHandleM, "EVP_CIPHER_CTX_cleanup")) == NULL) goto err;
//	if ((EVP_CIPHER_CTX_ctrl_FP = (EVP_CIPHER_CTX_ctrl_TYPE)GetProcAddress(libeayHandleM, "EVP_CIPHER_CTX_ctrl")) == NULL) goto err;
//	if ((EVP_CIPHER_CTX_init_FP = (EVP_CIPHER_CTX_init_TYPE)GetProcAddress(libeayHandleM, "EVP_CIPHER_CTX_init")) == NULL) goto err;
//	if ((EVP_CIPHER_CTX_set_key_length_FP = (EVP_CIPHER_CTX_set_key_length_TYPE)GetProcAddress(libeayHandleM, "EVP_CIPHER_CTX_set_key_length")) == NULL) goto err;
//	if ((EVP_CIPHER_CTX_set_padding_FP = (EVP_CIPHER_CTX_set_padding_TYPE)GetProcAddress(libeayHandleM, "EVP_CIPHER_CTX_set_padding")) == NULL) goto err;
//	if ((EVP_CIPHER_asn1_to_param_FP = (EVP_CIPHER_asn1_to_param_TYPE)GetProcAddress(libeayHandleM, "EVP_CIPHER_asn1_to_param")) == NULL) goto err;
//	if ((EVP_CIPHER_get_asn1_iv_FP = (EVP_CIPHER_get_asn1_iv_TYPE)GetProcAddress(libeayHandleM, "EVP_CIPHER_get_asn1_iv")) == NULL) goto err;
//	if ((EVP_CIPHER_param_to_asn1_FP = (EVP_CIPHER_param_to_asn1_TYPE)GetProcAddress(libeayHandleM, "EVP_CIPHER_param_to_asn1")) == NULL) goto err;
//	if ((EVP_CIPHER_set_asn1_iv_FP = (EVP_CIPHER_set_asn1_iv_TYPE)GetProcAddress(libeayHandleM, "EVP_CIPHER_set_asn1_iv")) == NULL) goto err;
//	if ((EVP_CIPHER_type_FP = (EVP_CIPHER_type_TYPE)GetProcAddress(libeayHandleM, "EVP_CIPHER_type")) == NULL) goto err;
//	if ((EVP_CipherFinal_FP = (EVP_CipherFinal_TYPE)GetProcAddress(libeayHandleM, "EVP_CipherFinal")) == NULL) goto err;
//	if ((EVP_CipherFinal_ex_FP = (EVP_CipherFinal_ex_TYPE)GetProcAddress(libeayHandleM, "EVP_CipherFinal_ex")) == NULL) goto err;
//	if ((EVP_CipherInit_FP = (EVP_CipherInit_TYPE)GetProcAddress(libeayHandleM, "EVP_CipherInit")) == NULL) goto err;
//	if ((EVP_CipherInit_ex_FP = (EVP_CipherInit_ex_TYPE)GetProcAddress(libeayHandleM, "EVP_CipherInit_ex")) == NULL) goto err;
//	if ((EVP_CipherUpdate_FP = (EVP_CipherUpdate_TYPE)GetProcAddress(libeayHandleM, "EVP_CipherUpdate")) == NULL) goto err;
//	if ((EVP_DecodeBlock_FP = (EVP_DecodeBlock_TYPE)GetProcAddress(libeayHandleM, "EVP_DecodeBlock")) == NULL) goto err;
//	if ((EVP_DecodeFinal_FP = (EVP_DecodeFinal_TYPE)GetProcAddress(libeayHandleM, "EVP_DecodeFinal")) == NULL) goto err;
//	if ((EVP_DecodeInit_FP = (EVP_DecodeInit_TYPE)GetProcAddress(libeayHandleM, "EVP_DecodeInit")) == NULL) goto err;
//	if ((EVP_DecodeUpdate_FP = (EVP_DecodeUpdate_TYPE)GetProcAddress(libeayHandleM, "EVP_DecodeUpdate")) == NULL) goto err;
//	if ((EVP_DecryptFinal_FP = (EVP_DecryptFinal_TYPE)GetProcAddress(libeayHandleM, "EVP_DecryptFinal")) == NULL) goto err;
//	if ((EVP_DecryptFinal_ex_FP = (EVP_DecryptFinal_ex_TYPE)GetProcAddress(libeayHandleM, "EVP_DecryptFinal_ex")) == NULL) goto err;
//	if ((EVP_DecryptInit_FP = (EVP_DecryptInit_TYPE)GetProcAddress(libeayHandleM, "EVP_DecryptInit")) == NULL) goto err;
//	if ((EVP_DecryptInit_ex_FP = (EVP_DecryptInit_ex_TYPE)GetProcAddress(libeayHandleM, "EVP_DecryptInit_ex")) == NULL) goto err;
//	if ((EVP_DecryptUpdate_FP = (EVP_DecryptUpdate_TYPE)GetProcAddress(libeayHandleM, "EVP_DecryptUpdate")) == NULL) goto err;
//	if ((EVP_DigestFinal_FP = (EVP_DigestFinal_TYPE)GetProcAddress(libeayHandleM, "EVP_DigestFinal")) == NULL) goto err;
//	if ((EVP_DigestFinal_ex_FP = (EVP_DigestFinal_ex_TYPE)GetProcAddress(libeayHandleM, "EVP_DigestFinal_ex")) == NULL) goto err;
//	if ((EVP_DigestInit_FP = (EVP_DigestInit_TYPE)GetProcAddress(libeayHandleM, "EVP_DigestInit")) == NULL) goto err;
//	if ((EVP_DigestInit_ex_FP = (EVP_DigestInit_ex_TYPE)GetProcAddress(libeayHandleM, "EVP_DigestInit_ex")) == NULL) goto err;
//	if ((EVP_DigestUpdate_FP = (EVP_DigestUpdate_TYPE)GetProcAddress(libeayHandleM, "EVP_DigestUpdate")) == NULL) goto err;
//	if ((EVP_Digest_FP = (EVP_Digest_TYPE)GetProcAddress(libeayHandleM, "EVP_Digest")) == NULL) goto err;
//	if ((EVP_EncodeBlock_FP = (EVP_EncodeBlock_TYPE)GetProcAddress(libeayHandleM, "EVP_EncodeBlock")) == NULL) goto err;
//	if ((EVP_EncodeFinal_FP = (EVP_EncodeFinal_TYPE)GetProcAddress(libeayHandleM, "EVP_EncodeFinal")) == NULL) goto err;
//	if ((EVP_EncodeInit_FP = (EVP_EncodeInit_TYPE)GetProcAddress(libeayHandleM, "EVP_EncodeInit")) == NULL) goto err;
//	if ((EVP_EncodeUpdate_FP = (EVP_EncodeUpdate_TYPE)GetProcAddress(libeayHandleM, "EVP_EncodeUpdate")) == NULL) goto err;
//	if ((EVP_EncryptFinal_FP = (EVP_EncryptFinal_TYPE)GetProcAddress(libeayHandleM, "EVP_EncryptFinal")) == NULL) goto err;
//	if ((EVP_EncryptFinal_ex_FP = (EVP_EncryptFinal_ex_TYPE)GetProcAddress(libeayHandleM, "EVP_EncryptFinal_ex")) == NULL) goto err;
//	if ((EVP_EncryptInit_FP = (EVP_EncryptInit_TYPE)GetProcAddress(libeayHandleM, "EVP_EncryptInit")) == NULL) goto err;
//	if ((EVP_EncryptInit_ex_FP = (EVP_EncryptInit_ex_TYPE)GetProcAddress(libeayHandleM, "EVP_EncryptInit_ex")) == NULL) goto err;
//	if ((EVP_EncryptUpdate_FP = (EVP_EncryptUpdate_TYPE)GetProcAddress(libeayHandleM, "EVP_EncryptUpdate")) == NULL) goto err;
//	if ((EVP_MD_CTX_cleanup_FP = (EVP_MD_CTX_cleanup_TYPE)GetProcAddress(libeayHandleM, "EVP_MD_CTX_cleanup")) == NULL) goto err;
//	if ((EVP_MD_CTX_copy_FP = (EVP_MD_CTX_copy_TYPE)GetProcAddress(libeayHandleM, "EVP_MD_CTX_copy")) == NULL) goto err;
//	if ((EVP_MD_CTX_copy_ex_FP = (EVP_MD_CTX_copy_ex_TYPE)GetProcAddress(libeayHandleM, "EVP_MD_CTX_copy_ex")) == NULL) goto err;
//	if ((EVP_MD_CTX_create_FP = (EVP_MD_CTX_create_TYPE)GetProcAddress(libeayHandleM, "EVP_MD_CTX_create")) == NULL) goto err;
//	if ((EVP_MD_CTX_destroy_FP = (EVP_MD_CTX_destroy_TYPE)GetProcAddress(libeayHandleM, "EVP_MD_CTX_destroy")) == NULL) goto err;
//	if ((EVP_MD_CTX_init_FP = (EVP_MD_CTX_init_TYPE)GetProcAddress(libeayHandleM, "EVP_MD_CTX_init")) == NULL) goto err;
//	if ((EVP_OpenFinal_FP = (EVP_OpenFinal_TYPE)GetProcAddress(libeayHandleM, "EVP_OpenFinal")) == NULL) goto err;
//	if ((EVP_OpenInit_FP = (EVP_OpenInit_TYPE)GetProcAddress(libeayHandleM, "EVP_OpenInit")) == NULL) goto err;
//	if ((EVP_PBE_CipherInit_FP = (EVP_PBE_CipherInit_TYPE)GetProcAddress(libeayHandleM, "EVP_PBE_CipherInit")) == NULL) goto err;
//	if ((EVP_PBE_alg_add_FP = (EVP_PBE_alg_add_TYPE)GetProcAddress(libeayHandleM, "EVP_PBE_alg_add")) == NULL) goto err;
//	if ((EVP_PBE_cleanup_FP = (EVP_PBE_cleanup_TYPE)GetProcAddress(libeayHandleM, "EVP_PBE_cleanup")) == NULL) goto err;
	if ((EVP_PKCS82PKEY_FP = (EVP_PKCS82PKEY_TYPE)GetProcAddress(libeayHandleM, "EVP_PKCS82PKEY")) == NULL) goto err;
//	if ((EVP_PKEY2PKCS8_FP = (EVP_PKEY2PKCS8_TYPE)GetProcAddress(libeayHandleM, "EVP_PKEY2PKCS8")) == NULL) goto err;
//	if ((EVP_PKEY2PKCS8_broken_FP = (EVP_PKEY2PKCS8_broken_TYPE)GetProcAddress(libeayHandleM, "EVP_PKEY2PKCS8_broken")) == NULL) goto err;
	if ((EVP_PKEY_assign_FP = (EVP_PKEY_assign_TYPE)GetProcAddress(libeayHandleM, "EVP_PKEY_assign")) == NULL) goto err;
//	if ((EVP_PKEY_bits_FP = (EVP_PKEY_bits_TYPE)GetProcAddress(libeayHandleM, "EVP_PKEY_bits")) == NULL) goto err;
//	if ((EVP_PKEY_cmp_parameters_FP = (EVP_PKEY_cmp_parameters_TYPE)GetProcAddress(libeayHandleM, "EVP_PKEY_cmp_parameters")) == NULL) goto err;
//	if ((EVP_PKEY_copy_parameters_FP = (EVP_PKEY_copy_parameters_TYPE)GetProcAddress(libeayHandleM, "EVP_PKEY_copy_parameters")) == NULL) goto err;
//	if ((EVP_PKEY_decrypt_FP = (EVP_PKEY_decrypt_TYPE)GetProcAddress(libeayHandleM, "EVP_PKEY_decrypt")) == NULL) goto err;
//	if ((EVP_PKEY_encrypt_FP = (EVP_PKEY_encrypt_TYPE)GetProcAddress(libeayHandleM, "EVP_PKEY_encrypt")) == NULL) goto err;
	if ((EVP_PKEY_free_FP = (EVP_PKEY_free_TYPE)GetProcAddress(libeayHandleM, "EVP_PKEY_free")) == NULL) goto err;
//	if ((EVP_PKEY_get1_DH_FP = (EVP_PKEY_get1_DH_TYPE)GetProcAddress(libeayHandleM, "EVP_PKEY_get1_DH")) == NULL) goto err;
//	if ((EVP_PKEY_get1_DSA_FP = (EVP_PKEY_get1_DSA_TYPE)GetProcAddress(libeayHandleM, "EVP_PKEY_get1_DSA")) == NULL) goto err;
//	if ((EVP_PKEY_get1_RSA_FP = (EVP_PKEY_get1_RSA_TYPE)GetProcAddress(libeayHandleM, "EVP_PKEY_get1_RSA")) == NULL) goto err;
//	if ((EVP_PKEY_missing_parameters_FP = (EVP_PKEY_missing_parameters_TYPE)GetProcAddress(libeayHandleM, "EVP_PKEY_missing_parameters")) == NULL) goto err;
	if ((EVP_PKEY_new_FP = (EVP_PKEY_new_TYPE)GetProcAddress(libeayHandleM, "EVP_PKEY_new")) == NULL) goto err;
//	if ((EVP_PKEY_save_parameters_FP = (EVP_PKEY_save_parameters_TYPE)GetProcAddress(libeayHandleM, "EVP_PKEY_save_parameters")) == NULL) goto err;
//	if ((EVP_PKEY_set1_DH_FP = (EVP_PKEY_set1_DH_TYPE)GetProcAddress(libeayHandleM, "EVP_PKEY_set1_DH")) == NULL) goto err;
//	if ((EVP_PKEY_set1_DSA_FP = (EVP_PKEY_set1_DSA_TYPE)GetProcAddress(libeayHandleM, "EVP_PKEY_set1_DSA")) == NULL) goto err;
//	if ((EVP_PKEY_set1_RSA_FP = (EVP_PKEY_set1_RSA_TYPE)GetProcAddress(libeayHandleM, "EVP_PKEY_set1_RSA")) == NULL) goto err;
//	if ((EVP_PKEY_size_FP = (EVP_PKEY_size_TYPE)GetProcAddress(libeayHandleM, "EVP_PKEY_size")) == NULL) goto err;
	if ((EVP_PKEY_type_FP = (EVP_PKEY_type_TYPE)GetProcAddress(libeayHandleM, "EVP_PKEY_type")) == NULL) goto err;
//	if ((EVP_SealFinal_FP = (EVP_SealFinal_TYPE)GetProcAddress(libeayHandleM, "EVP_SealFinal")) == NULL) goto err;
//	if ((EVP_SealInit_FP = (EVP_SealInit_TYPE)GetProcAddress(libeayHandleM, "EVP_SealInit")) == NULL) goto err;
//	if ((EVP_SignFinal_FP = (EVP_SignFinal_TYPE)GetProcAddress(libeayHandleM, "EVP_SignFinal")) == NULL) goto err;
//	if ((EVP_VerifyFinal_FP = (EVP_VerifyFinal_TYPE)GetProcAddress(libeayHandleM, "EVP_VerifyFinal")) == NULL) goto err;
	if ((EVP_add_cipher_FP = (EVP_add_cipher_TYPE)GetProcAddress(libeayHandleM, "EVP_add_cipher")) == NULL) goto err;
//	if ((EVP_add_digest_FP = (EVP_add_digest_TYPE)GetProcAddress(libeayHandleM, "EVP_add_digest")) == NULL) goto err;
//	if ((EVP_aes_128_cbc_FP = (EVP_aes_128_cbc_TYPE)GetProcAddress(libeayHandleM, "EVP_aes_128_cbc")) == NULL) goto err;
//	if ((EVP_aes_128_cfb_FP = (EVP_aes_128_cfb_TYPE)GetProcAddress(libeayHandleM, "EVP_aes_128_cfb")) == NULL) goto err;
//	if ((EVP_aes_128_ecb_FP = (EVP_aes_128_ecb_TYPE)GetProcAddress(libeayHandleM, "EVP_aes_128_ecb")) == NULL) goto err;
//	if ((EVP_aes_128_ofb_FP = (EVP_aes_128_ofb_TYPE)GetProcAddress(libeayHandleM, "EVP_aes_128_ofb")) == NULL) goto err;
//	if ((EVP_aes_192_cbc_FP = (EVP_aes_192_cbc_TYPE)GetProcAddress(libeayHandleM, "EVP_aes_192_cbc")) == NULL) goto err;
//	if ((EVP_aes_192_cfb_FP = (EVP_aes_192_cfb_TYPE)GetProcAddress(libeayHandleM, "EVP_aes_192_cfb")) == NULL) goto err;
//	if ((EVP_aes_192_ecb_FP = (EVP_aes_192_ecb_TYPE)GetProcAddress(libeayHandleM, "EVP_aes_192_ecb")) == NULL) goto err;
//	if ((EVP_aes_192_ofb_FP = (EVP_aes_192_ofb_TYPE)GetProcAddress(libeayHandleM, "EVP_aes_192_ofb")) == NULL) goto err;
//	if ((EVP_aes_256_cbc_FP = (EVP_aes_256_cbc_TYPE)GetProcAddress(libeayHandleM, "EVP_aes_256_cbc")) == NULL) goto err;
//	if ((EVP_aes_256_cfb_FP = (EVP_aes_256_cfb_TYPE)GetProcAddress(libeayHandleM, "EVP_aes_256_cfb")) == NULL) goto err;
//	if ((EVP_aes_256_ecb_FP = (EVP_aes_256_ecb_TYPE)GetProcAddress(libeayHandleM, "EVP_aes_256_ecb")) == NULL) goto err;
//	if ((EVP_aes_256_ofb_FP = (EVP_aes_256_ofb_TYPE)GetProcAddress(libeayHandleM, "EVP_aes_256_ofb")) == NULL) goto err;
//	if ((EVP_bf_cbc_FP = (EVP_bf_cbc_TYPE)GetProcAddress(libeayHandleM, "EVP_bf_cbc")) == NULL) goto err;
//	if ((EVP_bf_cfb_FP = (EVP_bf_cfb_TYPE)GetProcAddress(libeayHandleM, "EVP_bf_cfb")) == NULL) goto err;
//	if ((EVP_bf_ecb_FP = (EVP_bf_ecb_TYPE)GetProcAddress(libeayHandleM, "EVP_bf_ecb")) == NULL) goto err;
//	if ((EVP_bf_ofb_FP = (EVP_bf_ofb_TYPE)GetProcAddress(libeayHandleM, "EVP_bf_ofb")) == NULL) goto err;
//	if ((EVP_cast5_cbc_FP = (EVP_cast5_cbc_TYPE)GetProcAddress(libeayHandleM, "EVP_cast5_cbc")) == NULL) goto err;
//	if ((EVP_cast5_cfb_FP = (EVP_cast5_cfb_TYPE)GetProcAddress(libeayHandleM, "EVP_cast5_cfb")) == NULL) goto err;
//	if ((EVP_cast5_ecb_FP = (EVP_cast5_ecb_TYPE)GetProcAddress(libeayHandleM, "EVP_cast5_ecb")) == NULL) goto err;
//	if ((EVP_cast5_ofb_FP = (EVP_cast5_ofb_TYPE)GetProcAddress(libeayHandleM, "EVP_cast5_ofb")) == NULL) goto err;
	if ((EVP_cleanup_FP = (EVP_cleanup_TYPE)GetProcAddress(libeayHandleM, "EVP_cleanup")) == NULL) goto err;
//	if ((EVP_des_cbc_FP = (EVP_des_cbc_TYPE)GetProcAddress(libeayHandleM, "EVP_des_cbc")) == NULL) goto err;
//	if ((EVP_des_cfb_FP = (EVP_des_cfb_TYPE)GetProcAddress(libeayHandleM, "EVP_des_cfb")) == NULL) goto err;
//	if ((EVP_des_ecb_FP = (EVP_des_ecb_TYPE)GetProcAddress(libeayHandleM, "EVP_des_ecb")) == NULL) goto err;
//	if ((EVP_des_ede3_FP = (EVP_des_ede3_TYPE)GetProcAddress(libeayHandleM, "EVP_des_ede3")) == NULL) goto err;
	if ((EVP_des_ede3_cbc_FP = (EVP_des_ede3_cbc_TYPE)GetProcAddress(libeayHandleM, "EVP_des_ede3_cbc")) == NULL) goto err;
//	if ((EVP_des_ede3_cfb_FP = (EVP_des_ede3_cfb_TYPE)GetProcAddress(libeayHandleM, "EVP_des_ede3_cfb")) == NULL) goto err;
//	if ((EVP_des_ede3_ecb_FP = (EVP_des_ede3_ecb_TYPE)GetProcAddress(libeayHandleM, "EVP_des_ede3_ecb")) == NULL) goto err;
//	if ((EVP_des_ede3_ofb_FP = (EVP_des_ede3_ofb_TYPE)GetProcAddress(libeayHandleM, "EVP_des_ede3_ofb")) == NULL) goto err;
//	if ((EVP_des_ede_FP = (EVP_des_ede_TYPE)GetProcAddress(libeayHandleM, "EVP_des_ede")) == NULL) goto err;
//	if ((EVP_des_ede_cbc_FP = (EVP_des_ede_cbc_TYPE)GetProcAddress(libeayHandleM, "EVP_des_ede_cbc")) == NULL) goto err;
//	if ((EVP_des_ede_cfb_FP = (EVP_des_ede_cfb_TYPE)GetProcAddress(libeayHandleM, "EVP_des_ede_cfb")) == NULL) goto err;
//	if ((EVP_des_ede_ecb_FP = (EVP_des_ede_ecb_TYPE)GetProcAddress(libeayHandleM, "EVP_des_ede_ecb")) == NULL) goto err;
//	if ((EVP_des_ede_ofb_FP = (EVP_des_ede_ofb_TYPE)GetProcAddress(libeayHandleM, "EVP_des_ede_ofb")) == NULL) goto err;
//	if ((EVP_des_ofb_FP = (EVP_des_ofb_TYPE)GetProcAddress(libeayHandleM, "EVP_des_ofb")) == NULL) goto err;
//	if ((EVP_desx_cbc_FP = (EVP_desx_cbc_TYPE)GetProcAddress(libeayHandleM, "EVP_desx_cbc")) == NULL) goto err;
	if ((EVP_dss1_FP = (EVP_dss1_TYPE)GetProcAddress(libeayHandleM, "EVP_dss1")) == NULL) goto err;
//	if ((EVP_dss_FP = (EVP_dss_TYPE)GetProcAddress(libeayHandleM, "EVP_dss")) == NULL) goto err;
//	if ((EVP_enc_null_FP = (EVP_enc_null_TYPE)GetProcAddress(libeayHandleM, "EVP_enc_null")) == NULL) goto err;
//	if ((EVP_get_cipherbyname_FP = (EVP_get_cipherbyname_TYPE)GetProcAddress(libeayHandleM, "EVP_get_cipherbyname")) == NULL) goto err;
//	if ((EVP_get_digestbyname_FP = (EVP_get_digestbyname_TYPE)GetProcAddress(libeayHandleM, "EVP_get_digestbyname")) == NULL) goto err;
//	if ((EVP_get_pw_prompt_FP = (EVP_get_pw_prompt_TYPE)GetProcAddress(libeayHandleM, "EVP_get_pw_prompt")) == NULL) goto err;
//	if ((EVP_md2_FP = (EVP_md2_TYPE)GetProcAddress(libeayHandleM, "EVP_md2")) == NULL) goto err;
//	if ((EVP_md4_FP = (EVP_md4_TYPE)GetProcAddress(libeayHandleM, "EVP_md4")) == NULL) goto err;
//	if ((EVP_md5_FP = (EVP_md5_TYPE)GetProcAddress(libeayHandleM, "EVP_md5")) == NULL) goto err;
//	if ((EVP_md_null_FP = (EVP_md_null_TYPE)GetProcAddress(libeayHandleM, "EVP_md_null")) == NULL) goto err;
//	if ((EVP_mdc2_FP = (EVP_mdc2_TYPE)GetProcAddress(libeayHandleM, "EVP_mdc2")) == NULL) goto err;
//	if ((EVP_rc2_40_cbc_FP = (EVP_rc2_40_cbc_TYPE)GetProcAddress(libeayHandleM, "EVP_rc2_40_cbc")) == NULL) goto err;
//	if ((EVP_rc2_64_cbc_FP = (EVP_rc2_64_cbc_TYPE)GetProcAddress(libeayHandleM, "EVP_rc2_64_cbc")) == NULL) goto err;
//	if ((EVP_rc2_cbc_FP = (EVP_rc2_cbc_TYPE)GetProcAddress(libeayHandleM, "EVP_rc2_cbc")) == NULL) goto err;
//	if ((EVP_rc2_cfb_FP = (EVP_rc2_cfb_TYPE)GetProcAddress(libeayHandleM, "EVP_rc2_cfb")) == NULL) goto err;
//	if ((EVP_rc2_ecb_FP = (EVP_rc2_ecb_TYPE)GetProcAddress(libeayHandleM, "EVP_rc2_ecb")) == NULL) goto err;
//	if ((EVP_rc2_ofb_FP = (EVP_rc2_ofb_TYPE)GetProcAddress(libeayHandleM, "EVP_rc2_ofb")) == NULL) goto err;
//	if ((EVP_rc4_FP = (EVP_rc4_TYPE)GetProcAddress(libeayHandleM, "EVP_rc4")) == NULL) goto err;
//	if ((EVP_rc4_40_FP = (EVP_rc4_40_TYPE)GetProcAddress(libeayHandleM, "EVP_rc4_40")) == NULL) goto err;
//	if ((EVP_read_pw_string_FP = (EVP_read_pw_string_TYPE)GetProcAddress(libeayHandleM, "EVP_read_pw_string")) == NULL) goto err;
//	if ((EVP_ripemd160_FP = (EVP_ripemd160_TYPE)GetProcAddress(libeayHandleM, "EVP_ripemd160")) == NULL) goto err;
//	if ((EVP_set_pw_prompt_FP = (EVP_set_pw_prompt_TYPE)GetProcAddress(libeayHandleM, "EVP_set_pw_prompt")) == NULL) goto err;
	if ((EVP_sha1_FP = (EVP_sha1_TYPE)GetProcAddress(libeayHandleM, "EVP_sha1")) == NULL) goto err;
//	if ((EVP_sha_FP = (EVP_sha_TYPE)GetProcAddress(libeayHandleM, "EVP_sha")) == NULL) goto err;
//	if ((EXTENDED_KEY_USAGE_free_FP = (EXTENDED_KEY_USAGE_free_TYPE)GetProcAddress(libeayHandleM, "EXTENDED_KEY_USAGE_free")) == NULL) goto err;
//	if ((EXTENDED_KEY_USAGE_it_FP = (EXTENDED_KEY_USAGE_it_TYPE)GetProcAddress(libeayHandleM, "EXTENDED_KEY_USAGE_it")) == NULL) goto err;
//	if ((EXTENDED_KEY_USAGE_new_FP = (EXTENDED_KEY_USAGE_new_TYPE)GetProcAddress(libeayHandleM, "EXTENDED_KEY_USAGE_new")) == NULL) goto err;
//	if ((GENERAL_NAMES_free_FP = (GENERAL_NAMES_free_TYPE)GetProcAddress(libeayHandleM, "GENERAL_NAMES_free")) == NULL) goto err;
//	if ((GENERAL_NAMES_it_FP = (GENERAL_NAMES_it_TYPE)GetProcAddress(libeayHandleM, "GENERAL_NAMES_it")) == NULL) goto err;
//	if ((GENERAL_NAMES_new_FP = (GENERAL_NAMES_new_TYPE)GetProcAddress(libeayHandleM, "GENERAL_NAMES_new")) == NULL) goto err;
//	if ((GENERAL_NAME_free_FP = (GENERAL_NAME_free_TYPE)GetProcAddress(libeayHandleM, "GENERAL_NAME_free")) == NULL) goto err;
//	if ((GENERAL_NAME_it_FP = (GENERAL_NAME_it_TYPE)GetProcAddress(libeayHandleM, "GENERAL_NAME_it")) == NULL) goto err;
//	if ((GENERAL_NAME_new_FP = (GENERAL_NAME_new_TYPE)GetProcAddress(libeayHandleM, "GENERAL_NAME_new")) == NULL) goto err;
//	if ((GENERAL_NAME_print_FP = (GENERAL_NAME_print_TYPE)GetProcAddress(libeayHandleM, "GENERAL_NAME_print")) == NULL) goto err;
//	if ((HMAC_FP = (HMAC_TYPE)GetProcAddress(libeayHandleM, "HMAC")) == NULL) goto err;
//	if ((HMAC_CTX_cleanup_FP = (HMAC_CTX_cleanup_TYPE)GetProcAddress(libeayHandleM, "HMAC_CTX_cleanup")) == NULL) goto err;
//	if ((HMAC_CTX_init_FP = (HMAC_CTX_init_TYPE)GetProcAddress(libeayHandleM, "HMAC_CTX_init")) == NULL) goto err;
//	if ((HMAC_Final_FP = (HMAC_Final_TYPE)GetProcAddress(libeayHandleM, "HMAC_Final")) == NULL) goto err;
//	if ((HMAC_Init_FP = (HMAC_Init_TYPE)GetProcAddress(libeayHandleM, "HMAC_Init")) == NULL) goto err;
//	if ((HMAC_Init_ex_FP = (HMAC_Init_ex_TYPE)GetProcAddress(libeayHandleM, "HMAC_Init_ex")) == NULL) goto err;
//	if ((HMAC_Update_FP = (HMAC_Update_TYPE)GetProcAddress(libeayHandleM, "HMAC_Update")) == NULL) goto err;
//	if ((KRB5_APREQBODY_free_FP = (KRB5_APREQBODY_free_TYPE)GetProcAddress(libeayHandleM, "KRB5_APREQBODY_free")) == NULL) goto err;
//	if ((KRB5_APREQBODY_it_FP = (KRB5_APREQBODY_it_TYPE)GetProcAddress(libeayHandleM, "KRB5_APREQBODY_it")) == NULL) goto err;
//	if ((KRB5_APREQBODY_new_FP = (KRB5_APREQBODY_new_TYPE)GetProcAddress(libeayHandleM, "KRB5_APREQBODY_new")) == NULL) goto err;
//	if ((KRB5_APREQ_free_FP = (KRB5_APREQ_free_TYPE)GetProcAddress(libeayHandleM, "KRB5_APREQ_free")) == NULL) goto err;
//	if ((KRB5_APREQ_it_FP = (KRB5_APREQ_it_TYPE)GetProcAddress(libeayHandleM, "KRB5_APREQ_it")) == NULL) goto err;
//	if ((KRB5_APREQ_new_FP = (KRB5_APREQ_new_TYPE)GetProcAddress(libeayHandleM, "KRB5_APREQ_new")) == NULL) goto err;
//	if ((KRB5_AUTHDATA_free_FP = (KRB5_AUTHDATA_free_TYPE)GetProcAddress(libeayHandleM, "KRB5_AUTHDATA_free")) == NULL) goto err;
//	if ((KRB5_AUTHDATA_it_FP = (KRB5_AUTHDATA_it_TYPE)GetProcAddress(libeayHandleM, "KRB5_AUTHDATA_it")) == NULL) goto err;
//	if ((KRB5_AUTHDATA_new_FP = (KRB5_AUTHDATA_new_TYPE)GetProcAddress(libeayHandleM, "KRB5_AUTHDATA_new")) == NULL) goto err;
//	if ((KRB5_AUTHENTBODY_free_FP = (KRB5_AUTHENTBODY_free_TYPE)GetProcAddress(libeayHandleM, "KRB5_AUTHENTBODY_free")) == NULL) goto err;
//	if ((KRB5_AUTHENTBODY_it_FP = (KRB5_AUTHENTBODY_it_TYPE)GetProcAddress(libeayHandleM, "KRB5_AUTHENTBODY_it")) == NULL) goto err;
//	if ((KRB5_AUTHENTBODY_new_FP = (KRB5_AUTHENTBODY_new_TYPE)GetProcAddress(libeayHandleM, "KRB5_AUTHENTBODY_new")) == NULL) goto err;
//	if ((KRB5_AUTHENT_free_FP = (KRB5_AUTHENT_free_TYPE)GetProcAddress(libeayHandleM, "KRB5_AUTHENT_free")) == NULL) goto err;
//	if ((KRB5_AUTHENT_it_FP = (KRB5_AUTHENT_it_TYPE)GetProcAddress(libeayHandleM, "KRB5_AUTHENT_it")) == NULL) goto err;
//	if ((KRB5_AUTHENT_new_FP = (KRB5_AUTHENT_new_TYPE)GetProcAddress(libeayHandleM, "KRB5_AUTHENT_new")) == NULL) goto err;
//	if ((KRB5_CHECKSUM_free_FP = (KRB5_CHECKSUM_free_TYPE)GetProcAddress(libeayHandleM, "KRB5_CHECKSUM_free")) == NULL) goto err;
//	if ((KRB5_CHECKSUM_it_FP = (KRB5_CHECKSUM_it_TYPE)GetProcAddress(libeayHandleM, "KRB5_CHECKSUM_it")) == NULL) goto err;
//	if ((KRB5_CHECKSUM_new_FP = (KRB5_CHECKSUM_new_TYPE)GetProcAddress(libeayHandleM, "KRB5_CHECKSUM_new")) == NULL) goto err;
//	if ((KRB5_ENCDATA_free_FP = (KRB5_ENCDATA_free_TYPE)GetProcAddress(libeayHandleM, "KRB5_ENCDATA_free")) == NULL) goto err;
//	if ((KRB5_ENCDATA_it_FP = (KRB5_ENCDATA_it_TYPE)GetProcAddress(libeayHandleM, "KRB5_ENCDATA_it")) == NULL) goto err;
//	if ((KRB5_ENCDATA_new_FP = (KRB5_ENCDATA_new_TYPE)GetProcAddress(libeayHandleM, "KRB5_ENCDATA_new")) == NULL) goto err;
//	if ((KRB5_ENCKEY_free_FP = (KRB5_ENCKEY_free_TYPE)GetProcAddress(libeayHandleM, "KRB5_ENCKEY_free")) == NULL) goto err;
//	if ((KRB5_ENCKEY_it_FP = (KRB5_ENCKEY_it_TYPE)GetProcAddress(libeayHandleM, "KRB5_ENCKEY_it")) == NULL) goto err;
//	if ((KRB5_ENCKEY_new_FP = (KRB5_ENCKEY_new_TYPE)GetProcAddress(libeayHandleM, "KRB5_ENCKEY_new")) == NULL) goto err;
//	if ((KRB5_PRINCNAME_free_FP = (KRB5_PRINCNAME_free_TYPE)GetProcAddress(libeayHandleM, "KRB5_PRINCNAME_free")) == NULL) goto err;
//	if ((KRB5_PRINCNAME_it_FP = (KRB5_PRINCNAME_it_TYPE)GetProcAddress(libeayHandleM, "KRB5_PRINCNAME_it")) == NULL) goto err;
//	if ((KRB5_PRINCNAME_new_FP = (KRB5_PRINCNAME_new_TYPE)GetProcAddress(libeayHandleM, "KRB5_PRINCNAME_new")) == NULL) goto err;
//	if ((KRB5_TICKET_free_FP = (KRB5_TICKET_free_TYPE)GetProcAddress(libeayHandleM, "KRB5_TICKET_free")) == NULL) goto err;
//	if ((KRB5_TICKET_it_FP = (KRB5_TICKET_it_TYPE)GetProcAddress(libeayHandleM, "KRB5_TICKET_it")) == NULL) goto err;
//	if ((KRB5_TICKET_new_FP = (KRB5_TICKET_new_TYPE)GetProcAddress(libeayHandleM, "KRB5_TICKET_new")) == NULL) goto err;
//	if ((KRB5_TKTBODY_free_FP = (KRB5_TKTBODY_free_TYPE)GetProcAddress(libeayHandleM, "KRB5_TKTBODY_free")) == NULL) goto err;
//	if ((KRB5_TKTBODY_it_FP = (KRB5_TKTBODY_it_TYPE)GetProcAddress(libeayHandleM, "KRB5_TKTBODY_it")) == NULL) goto err;
//	if ((KRB5_TKTBODY_new_FP = (KRB5_TKTBODY_new_TYPE)GetProcAddress(libeayHandleM, "KRB5_TKTBODY_new")) == NULL) goto err;
//	if ((LONG_it_FP = (LONG_it_TYPE)GetProcAddress(libeayHandleM, "LONG_it")) == NULL) goto err;
//	if ((MD2_FP = (MD2_TYPE)GetProcAddress(libeayHandleM, "MD2")) == NULL) goto err;
//	if ((MD2_Final_FP = (MD2_Final_TYPE)GetProcAddress(libeayHandleM, "MD2_Final")) == NULL) goto err;
//	if ((MD2_Init_FP = (MD2_Init_TYPE)GetProcAddress(libeayHandleM, "MD2_Init")) == NULL) goto err;
//	if ((MD2_Update_FP = (MD2_Update_TYPE)GetProcAddress(libeayHandleM, "MD2_Update")) == NULL) goto err;
//	if ((MD2_options_FP = (MD2_options_TYPE)GetProcAddress(libeayHandleM, "MD2_options")) == NULL) goto err;
//	if ((MD4_FP = (MD4_TYPE)GetProcAddress(libeayHandleM, "MD4")) == NULL) goto err;
//	if ((MD4_Final_FP = (MD4_Final_TYPE)GetProcAddress(libeayHandleM, "MD4_Final")) == NULL) goto err;
//	if ((MD4_Init_FP = (MD4_Init_TYPE)GetProcAddress(libeayHandleM, "MD4_Init")) == NULL) goto err;
//	if ((MD4_Transform_FP = (MD4_Transform_TYPE)GetProcAddress(libeayHandleM, "MD4_Transform")) == NULL) goto err;
//	if ((MD4_Update_FP = (MD4_Update_TYPE)GetProcAddress(libeayHandleM, "MD4_Update")) == NULL) goto err;
//	if ((MD5_FP = (MD5_TYPE)GetProcAddress(libeayHandleM, "MD5")) == NULL) goto err;
//	if ((MD5_Final_FP = (MD5_Final_TYPE)GetProcAddress(libeayHandleM, "MD5_Final")) == NULL) goto err;
//	if ((MD5_Init_FP = (MD5_Init_TYPE)GetProcAddress(libeayHandleM, "MD5_Init")) == NULL) goto err;
//	if ((MD5_Transform_FP = (MD5_Transform_TYPE)GetProcAddress(libeayHandleM, "MD5_Transform")) == NULL) goto err;
//	if ((MD5_Update_FP = (MD5_Update_TYPE)GetProcAddress(libeayHandleM, "MD5_Update")) == NULL) goto err;
//	if ((MDC2_FP = (MDC2_TYPE)GetProcAddress(libeayHandleM, "MDC2")) == NULL) goto err;
//	if ((MDC2_Final_FP = (MDC2_Final_TYPE)GetProcAddress(libeayHandleM, "MDC2_Final")) == NULL) goto err;
//	if ((MDC2_Init_FP = (MDC2_Init_TYPE)GetProcAddress(libeayHandleM, "MDC2_Init")) == NULL) goto err;
//	if ((MDC2_Update_FP = (MDC2_Update_TYPE)GetProcAddress(libeayHandleM, "MDC2_Update")) == NULL) goto err;
//	if ((NCONF_WIN32_FP = (NCONF_WIN32_TYPE)GetProcAddress(libeayHandleM, "NCONF_WIN32")) == NULL) goto err;
//	if ((NCONF_default_FP = (NCONF_default_TYPE)GetProcAddress(libeayHandleM, "NCONF_default")) == NULL) goto err;
//	if ((NCONF_dump_bio_FP = (NCONF_dump_bio_TYPE)GetProcAddress(libeayHandleM, "NCONF_dump_bio")) == NULL) goto err;
//	if ((NCONF_dump_fp_FP = (NCONF_dump_fp_TYPE)GetProcAddress(libeayHandleM, "NCONF_dump_fp")) == NULL) goto err;
//	if ((NCONF_free_FP = (NCONF_free_TYPE)GetProcAddress(libeayHandleM, "NCONF_free")) == NULL) goto err;
//	if ((NCONF_free_data_FP = (NCONF_free_data_TYPE)GetProcAddress(libeayHandleM, "NCONF_free_data")) == NULL) goto err;
//	if ((NCONF_get_number_e_FP = (NCONF_get_number_e_TYPE)GetProcAddress(libeayHandleM, "NCONF_get_number_e")) == NULL) goto err;
//	if ((NCONF_get_section_FP = (NCONF_get_section_TYPE)GetProcAddress(libeayHandleM, "NCONF_get_section")) == NULL) goto err;
//	if ((NCONF_get_string_FP = (NCONF_get_string_TYPE)GetProcAddress(libeayHandleM, "NCONF_get_string")) == NULL) goto err;
//	if ((NCONF_load_FP = (NCONF_load_TYPE)GetProcAddress(libeayHandleM, "NCONF_load")) == NULL) goto err;
//	if ((NCONF_load_bio_FP = (NCONF_load_bio_TYPE)GetProcAddress(libeayHandleM, "NCONF_load_bio")) == NULL) goto err;
//	if ((NCONF_load_fp_FP = (NCONF_load_fp_TYPE)GetProcAddress(libeayHandleM, "NCONF_load_fp")) == NULL) goto err;
//	if ((NCONF_new_FP = (NCONF_new_TYPE)GetProcAddress(libeayHandleM, "NCONF_new")) == NULL) goto err;
//	if ((NETSCAPE_CERT_SEQUENCE_free_FP = (NETSCAPE_CERT_SEQUENCE_free_TYPE)GetProcAddress(libeayHandleM, "NETSCAPE_CERT_SEQUENCE_free")) == NULL) goto err;
//	if ((NETSCAPE_CERT_SEQUENCE_it_FP = (NETSCAPE_CERT_SEQUENCE_it_TYPE)GetProcAddress(libeayHandleM, "NETSCAPE_CERT_SEQUENCE_it")) == NULL) goto err;
//	if ((NETSCAPE_CERT_SEQUENCE_new_FP = (NETSCAPE_CERT_SEQUENCE_new_TYPE)GetProcAddress(libeayHandleM, "NETSCAPE_CERT_SEQUENCE_new")) == NULL) goto err;
//	if ((NETSCAPE_SPKAC_free_FP = (NETSCAPE_SPKAC_free_TYPE)GetProcAddress(libeayHandleM, "NETSCAPE_SPKAC_free")) == NULL) goto err;
//	if ((NETSCAPE_SPKAC_it_FP = (NETSCAPE_SPKAC_it_TYPE)GetProcAddress(libeayHandleM, "NETSCAPE_SPKAC_it")) == NULL) goto err;
//	if ((NETSCAPE_SPKAC_new_FP = (NETSCAPE_SPKAC_new_TYPE)GetProcAddress(libeayHandleM, "NETSCAPE_SPKAC_new")) == NULL) goto err;
//	if ((NETSCAPE_SPKI_b64_decode_FP = (NETSCAPE_SPKI_b64_decode_TYPE)GetProcAddress(libeayHandleM, "NETSCAPE_SPKI_b64_decode")) == NULL) goto err;
//	if ((NETSCAPE_SPKI_b64_encode_FP = (NETSCAPE_SPKI_b64_encode_TYPE)GetProcAddress(libeayHandleM, "NETSCAPE_SPKI_b64_encode")) == NULL) goto err;
//	if ((NETSCAPE_SPKI_free_FP = (NETSCAPE_SPKI_free_TYPE)GetProcAddress(libeayHandleM, "NETSCAPE_SPKI_free")) == NULL) goto err;
//	if ((NETSCAPE_SPKI_get_pubkey_FP = (NETSCAPE_SPKI_get_pubkey_TYPE)GetProcAddress(libeayHandleM, "NETSCAPE_SPKI_get_pubkey")) == NULL) goto err;
//	if ((NETSCAPE_SPKI_it_FP = (NETSCAPE_SPKI_it_TYPE)GetProcAddress(libeayHandleM, "NETSCAPE_SPKI_it")) == NULL) goto err;
//	if ((NETSCAPE_SPKI_new_FP = (NETSCAPE_SPKI_new_TYPE)GetProcAddress(libeayHandleM, "NETSCAPE_SPKI_new")) == NULL) goto err;
//	if ((NETSCAPE_SPKI_print_FP = (NETSCAPE_SPKI_print_TYPE)GetProcAddress(libeayHandleM, "NETSCAPE_SPKI_print")) == NULL) goto err;
//	if ((NETSCAPE_SPKI_set_pubkey_FP = (NETSCAPE_SPKI_set_pubkey_TYPE)GetProcAddress(libeayHandleM, "NETSCAPE_SPKI_set_pubkey")) == NULL) goto err;
//	if ((NETSCAPE_SPKI_sign_FP = (NETSCAPE_SPKI_sign_TYPE)GetProcAddress(libeayHandleM, "NETSCAPE_SPKI_sign")) == NULL) goto err;
//	if ((NETSCAPE_SPKI_verify_FP = (NETSCAPE_SPKI_verify_TYPE)GetProcAddress(libeayHandleM, "NETSCAPE_SPKI_verify")) == NULL) goto err;
//	if ((NOTICEREF_free_FP = (NOTICEREF_free_TYPE)GetProcAddress(libeayHandleM, "NOTICEREF_free")) == NULL) goto err;
//	if ((NOTICEREF_it_FP = (NOTICEREF_it_TYPE)GetProcAddress(libeayHandleM, "NOTICEREF_it")) == NULL) goto err;
//	if ((NOTICEREF_new_FP = (NOTICEREF_new_TYPE)GetProcAddress(libeayHandleM, "NOTICEREF_new")) == NULL) goto err;
//	if ((OBJ_NAME_add_FP = (OBJ_NAME_add_TYPE)GetProcAddress(libeayHandleM, "OBJ_NAME_add")) == NULL) goto err;
//	if ((OBJ_NAME_cleanup_FP = (OBJ_NAME_cleanup_TYPE)GetProcAddress(libeayHandleM, "OBJ_NAME_cleanup")) == NULL) goto err;
//	if ((OBJ_NAME_do_all_FP = (OBJ_NAME_do_all_TYPE)GetProcAddress(libeayHandleM, "OBJ_NAME_do_all")) == NULL) goto err;
//	if ((OBJ_NAME_do_all_sorted_FP = (OBJ_NAME_do_all_sorted_TYPE)GetProcAddress(libeayHandleM, "OBJ_NAME_do_all_sorted")) == NULL) goto err;
//	if ((OBJ_NAME_get_FP = (OBJ_NAME_get_TYPE)GetProcAddress(libeayHandleM, "OBJ_NAME_get")) == NULL) goto err;
//	if ((OBJ_NAME_init_FP = (OBJ_NAME_init_TYPE)GetProcAddress(libeayHandleM, "OBJ_NAME_init")) == NULL) goto err;
//	if ((OBJ_NAME_new_index_FP = (OBJ_NAME_new_index_TYPE)GetProcAddress(libeayHandleM, "OBJ_NAME_new_index")) == NULL) goto err;
//	if ((OBJ_NAME_remove_FP = (OBJ_NAME_remove_TYPE)GetProcAddress(libeayHandleM, "OBJ_NAME_remove")) == NULL) goto err;
//	if ((OBJ_add_object_FP = (OBJ_add_object_TYPE)GetProcAddress(libeayHandleM, "OBJ_add_object")) == NULL) goto err;
//	if ((OBJ_bsearch_FP = (OBJ_bsearch_TYPE)GetProcAddress(libeayHandleM, "OBJ_bsearch")) == NULL) goto err;
//	if ((OBJ_cleanup_FP = (OBJ_cleanup_TYPE)GetProcAddress(libeayHandleM, "OBJ_cleanup")) == NULL) goto err;
//	if ((OBJ_cmp_FP = (OBJ_cmp_TYPE)GetProcAddress(libeayHandleM, "OBJ_cmp")) == NULL) goto err;
//	if ((OBJ_create_FP = (OBJ_create_TYPE)GetProcAddress(libeayHandleM, "OBJ_create")) == NULL) goto err;
//	if ((OBJ_create_objects_FP = (OBJ_create_objects_TYPE)GetProcAddress(libeayHandleM, "OBJ_create_objects")) == NULL) goto err;
//	if ((OBJ_dup_FP = (OBJ_dup_TYPE)GetProcAddress(libeayHandleM, "OBJ_dup")) == NULL) goto err;
//	if ((OBJ_ln2nid_FP = (OBJ_ln2nid_TYPE)GetProcAddress(libeayHandleM, "OBJ_ln2nid")) == NULL) goto err;
//	if ((OBJ_new_nid_FP = (OBJ_new_nid_TYPE)GetProcAddress(libeayHandleM, "OBJ_new_nid")) == NULL) goto err;
//	if ((OBJ_nid2ln_FP = (OBJ_nid2ln_TYPE)GetProcAddress(libeayHandleM, "OBJ_nid2ln")) == NULL) goto err;
//	if ((OBJ_nid2obj_FP = (OBJ_nid2obj_TYPE)GetProcAddress(libeayHandleM, "OBJ_nid2obj")) == NULL) goto err;
//	if ((OBJ_nid2sn_FP = (OBJ_nid2sn_TYPE)GetProcAddress(libeayHandleM, "OBJ_nid2sn")) == NULL) goto err;
	if ((OBJ_obj2nid_FP = (OBJ_obj2nid_TYPE)GetProcAddress(libeayHandleM, "OBJ_obj2nid")) == NULL) goto err;
//	if ((OBJ_obj2txt_FP = (OBJ_obj2txt_TYPE)GetProcAddress(libeayHandleM, "OBJ_obj2txt")) == NULL) goto err;
	if ((OBJ_sn2nid_FP = (OBJ_sn2nid_TYPE)GetProcAddress(libeayHandleM, "OBJ_sn2nid")) == NULL) goto err;
//	if ((OBJ_txt2nid_FP = (OBJ_txt2nid_TYPE)GetProcAddress(libeayHandleM, "OBJ_txt2nid")) == NULL) goto err;
//	if ((OBJ_txt2obj_FP = (OBJ_txt2obj_TYPE)GetProcAddress(libeayHandleM, "OBJ_txt2obj")) == NULL) goto err;
//	if ((OCSP_BASICRESP_add1_ext_i2d_FP = (OCSP_BASICRESP_add1_ext_i2d_TYPE)GetProcAddress(libeayHandleM, "OCSP_BASICRESP_add1_ext_i2d")) == NULL) goto err;
//	if ((OCSP_BASICRESP_add_ext_FP = (OCSP_BASICRESP_add_ext_TYPE)GetProcAddress(libeayHandleM, "OCSP_BASICRESP_add_ext")) == NULL) goto err;
//	if ((OCSP_BASICRESP_delete_ext_FP = (OCSP_BASICRESP_delete_ext_TYPE)GetProcAddress(libeayHandleM, "OCSP_BASICRESP_delete_ext")) == NULL) goto err;
//	if ((OCSP_BASICRESP_free_FP = (OCSP_BASICRESP_free_TYPE)GetProcAddress(libeayHandleM, "OCSP_BASICRESP_free")) == NULL) goto err;
//	if ((OCSP_BASICRESP_get1_ext_d2i_FP = (OCSP_BASICRESP_get1_ext_d2i_TYPE)GetProcAddress(libeayHandleM, "OCSP_BASICRESP_get1_ext_d2i")) == NULL) goto err;
//	if ((OCSP_BASICRESP_get_ext_FP = (OCSP_BASICRESP_get_ext_TYPE)GetProcAddress(libeayHandleM, "OCSP_BASICRESP_get_ext")) == NULL) goto err;
//	if ((OCSP_BASICRESP_get_ext_by_NID_FP = (OCSP_BASICRESP_get_ext_by_NID_TYPE)GetProcAddress(libeayHandleM, "OCSP_BASICRESP_get_ext_by_NID")) == NULL) goto err;
//	if ((OCSP_BASICRESP_get_ext_by_OBJ_FP = (OCSP_BASICRESP_get_ext_by_OBJ_TYPE)GetProcAddress(libeayHandleM, "OCSP_BASICRESP_get_ext_by_OBJ")) == NULL) goto err;
//	if ((OCSP_BASICRESP_get_ext_by_critical_FP = (OCSP_BASICRESP_get_ext_by_critical_TYPE)GetProcAddress(libeayHandleM, "OCSP_BASICRESP_get_ext_by_critical")) == NULL) goto err;
//	if ((OCSP_BASICRESP_get_ext_count_FP = (OCSP_BASICRESP_get_ext_count_TYPE)GetProcAddress(libeayHandleM, "OCSP_BASICRESP_get_ext_count")) == NULL) goto err;
//	if ((OCSP_BASICRESP_it_FP = (OCSP_BASICRESP_it_TYPE)GetProcAddress(libeayHandleM, "OCSP_BASICRESP_it")) == NULL) goto err;
//	if ((OCSP_BASICRESP_new_FP = (OCSP_BASICRESP_new_TYPE)GetProcAddress(libeayHandleM, "OCSP_BASICRESP_new")) == NULL) goto err;
//	if ((OCSP_CERTID_free_FP = (OCSP_CERTID_free_TYPE)GetProcAddress(libeayHandleM, "OCSP_CERTID_free")) == NULL) goto err;
//	if ((OCSP_CERTID_it_FP = (OCSP_CERTID_it_TYPE)GetProcAddress(libeayHandleM, "OCSP_CERTID_it")) == NULL) goto err;
//	if ((OCSP_CERTID_new_FP = (OCSP_CERTID_new_TYPE)GetProcAddress(libeayHandleM, "OCSP_CERTID_new")) == NULL) goto err;
//	if ((OCSP_CERTSTATUS_free_FP = (OCSP_CERTSTATUS_free_TYPE)GetProcAddress(libeayHandleM, "OCSP_CERTSTATUS_free")) == NULL) goto err;
//	if ((OCSP_CERTSTATUS_it_FP = (OCSP_CERTSTATUS_it_TYPE)GetProcAddress(libeayHandleM, "OCSP_CERTSTATUS_it")) == NULL) goto err;
//	if ((OCSP_CERTSTATUS_new_FP = (OCSP_CERTSTATUS_new_TYPE)GetProcAddress(libeayHandleM, "OCSP_CERTSTATUS_new")) == NULL) goto err;
//	if ((OCSP_CRLID_free_FP = (OCSP_CRLID_free_TYPE)GetProcAddress(libeayHandleM, "OCSP_CRLID_free")) == NULL) goto err;
//	if ((OCSP_CRLID_it_FP = (OCSP_CRLID_it_TYPE)GetProcAddress(libeayHandleM, "OCSP_CRLID_it")) == NULL) goto err;
//	if ((OCSP_CRLID_new_FP = (OCSP_CRLID_new_TYPE)GetProcAddress(libeayHandleM, "OCSP_CRLID_new")) == NULL) goto err;
//	if ((OCSP_ONEREQ_add1_ext_i2d_FP = (OCSP_ONEREQ_add1_ext_i2d_TYPE)GetProcAddress(libeayHandleM, "OCSP_ONEREQ_add1_ext_i2d")) == NULL) goto err;
//	if ((OCSP_ONEREQ_add_ext_FP = (OCSP_ONEREQ_add_ext_TYPE)GetProcAddress(libeayHandleM, "OCSP_ONEREQ_add_ext")) == NULL) goto err;
//	if ((OCSP_ONEREQ_delete_ext_FP = (OCSP_ONEREQ_delete_ext_TYPE)GetProcAddress(libeayHandleM, "OCSP_ONEREQ_delete_ext")) == NULL) goto err;
//	if ((OCSP_ONEREQ_free_FP = (OCSP_ONEREQ_free_TYPE)GetProcAddress(libeayHandleM, "OCSP_ONEREQ_free")) == NULL) goto err;
//	if ((OCSP_ONEREQ_get1_ext_d2i_FP = (OCSP_ONEREQ_get1_ext_d2i_TYPE)GetProcAddress(libeayHandleM, "OCSP_ONEREQ_get1_ext_d2i")) == NULL) goto err;
//	if ((OCSP_ONEREQ_get_ext_FP = (OCSP_ONEREQ_get_ext_TYPE)GetProcAddress(libeayHandleM, "OCSP_ONEREQ_get_ext")) == NULL) goto err;
//	if ((OCSP_ONEREQ_get_ext_by_NID_FP = (OCSP_ONEREQ_get_ext_by_NID_TYPE)GetProcAddress(libeayHandleM, "OCSP_ONEREQ_get_ext_by_NID")) == NULL) goto err;
//	if ((OCSP_ONEREQ_get_ext_by_OBJ_FP = (OCSP_ONEREQ_get_ext_by_OBJ_TYPE)GetProcAddress(libeayHandleM, "OCSP_ONEREQ_get_ext_by_OBJ")) == NULL) goto err;
//	if ((OCSP_ONEREQ_get_ext_by_critical_FP = (OCSP_ONEREQ_get_ext_by_critical_TYPE)GetProcAddress(libeayHandleM, "OCSP_ONEREQ_get_ext_by_critical")) == NULL) goto err;
//	if ((OCSP_ONEREQ_get_ext_count_FP = (OCSP_ONEREQ_get_ext_count_TYPE)GetProcAddress(libeayHandleM, "OCSP_ONEREQ_get_ext_count")) == NULL) goto err;
//	if ((OCSP_ONEREQ_it_FP = (OCSP_ONEREQ_it_TYPE)GetProcAddress(libeayHandleM, "OCSP_ONEREQ_it")) == NULL) goto err;
//	if ((OCSP_ONEREQ_new_FP = (OCSP_ONEREQ_new_TYPE)GetProcAddress(libeayHandleM, "OCSP_ONEREQ_new")) == NULL) goto err;
//	if ((OCSP_REQINFO_free_FP = (OCSP_REQINFO_free_TYPE)GetProcAddress(libeayHandleM, "OCSP_REQINFO_free")) == NULL) goto err;
//	if ((OCSP_REQINFO_it_FP = (OCSP_REQINFO_it_TYPE)GetProcAddress(libeayHandleM, "OCSP_REQINFO_it")) == NULL) goto err;
//	if ((OCSP_REQINFO_new_FP = (OCSP_REQINFO_new_TYPE)GetProcAddress(libeayHandleM, "OCSP_REQINFO_new")) == NULL) goto err;
//	if ((OCSP_REQUEST_add1_ext_i2d_FP = (OCSP_REQUEST_add1_ext_i2d_TYPE)GetProcAddress(libeayHandleM, "OCSP_REQUEST_add1_ext_i2d")) == NULL) goto err;
//	if ((OCSP_REQUEST_add_ext_FP = (OCSP_REQUEST_add_ext_TYPE)GetProcAddress(libeayHandleM, "OCSP_REQUEST_add_ext")) == NULL) goto err;
//	if ((OCSP_REQUEST_delete_ext_FP = (OCSP_REQUEST_delete_ext_TYPE)GetProcAddress(libeayHandleM, "OCSP_REQUEST_delete_ext")) == NULL) goto err;
//	if ((OCSP_REQUEST_free_FP = (OCSP_REQUEST_free_TYPE)GetProcAddress(libeayHandleM, "OCSP_REQUEST_free")) == NULL) goto err;
//	if ((OCSP_REQUEST_get1_ext_d2i_FP = (OCSP_REQUEST_get1_ext_d2i_TYPE)GetProcAddress(libeayHandleM, "OCSP_REQUEST_get1_ext_d2i")) == NULL) goto err;
//	if ((OCSP_REQUEST_get_ext_FP = (OCSP_REQUEST_get_ext_TYPE)GetProcAddress(libeayHandleM, "OCSP_REQUEST_get_ext")) == NULL) goto err;
//	if ((OCSP_REQUEST_get_ext_by_NID_FP = (OCSP_REQUEST_get_ext_by_NID_TYPE)GetProcAddress(libeayHandleM, "OCSP_REQUEST_get_ext_by_NID")) == NULL) goto err;
//	if ((OCSP_REQUEST_get_ext_by_OBJ_FP = (OCSP_REQUEST_get_ext_by_OBJ_TYPE)GetProcAddress(libeayHandleM, "OCSP_REQUEST_get_ext_by_OBJ")) == NULL) goto err;
//	if ((OCSP_REQUEST_get_ext_by_critical_FP = (OCSP_REQUEST_get_ext_by_critical_TYPE)GetProcAddress(libeayHandleM, "OCSP_REQUEST_get_ext_by_critical")) == NULL) goto err;
//	if ((OCSP_REQUEST_get_ext_count_FP = (OCSP_REQUEST_get_ext_count_TYPE)GetProcAddress(libeayHandleM, "OCSP_REQUEST_get_ext_count")) == NULL) goto err;
//	if ((OCSP_REQUEST_it_FP = (OCSP_REQUEST_it_TYPE)GetProcAddress(libeayHandleM, "OCSP_REQUEST_it")) == NULL) goto err;
//	if ((OCSP_REQUEST_new_FP = (OCSP_REQUEST_new_TYPE)GetProcAddress(libeayHandleM, "OCSP_REQUEST_new")) == NULL) goto err;
//	if ((OCSP_REQUEST_print_FP = (OCSP_REQUEST_print_TYPE)GetProcAddress(libeayHandleM, "OCSP_REQUEST_print")) == NULL) goto err;
//	if ((OCSP_RESPBYTES_free_FP = (OCSP_RESPBYTES_free_TYPE)GetProcAddress(libeayHandleM, "OCSP_RESPBYTES_free")) == NULL) goto err;
//	if ((OCSP_RESPBYTES_it_FP = (OCSP_RESPBYTES_it_TYPE)GetProcAddress(libeayHandleM, "OCSP_RESPBYTES_it")) == NULL) goto err;
//	if ((OCSP_RESPBYTES_new_FP = (OCSP_RESPBYTES_new_TYPE)GetProcAddress(libeayHandleM, "OCSP_RESPBYTES_new")) == NULL) goto err;
//	if ((OCSP_RESPDATA_free_FP = (OCSP_RESPDATA_free_TYPE)GetProcAddress(libeayHandleM, "OCSP_RESPDATA_free")) == NULL) goto err;
//	if ((OCSP_RESPDATA_it_FP = (OCSP_RESPDATA_it_TYPE)GetProcAddress(libeayHandleM, "OCSP_RESPDATA_it")) == NULL) goto err;
//	if ((OCSP_RESPDATA_new_FP = (OCSP_RESPDATA_new_TYPE)GetProcAddress(libeayHandleM, "OCSP_RESPDATA_new")) == NULL) goto err;
//	if ((OCSP_RESPID_free_FP = (OCSP_RESPID_free_TYPE)GetProcAddress(libeayHandleM, "OCSP_RESPID_free")) == NULL) goto err;
//	if ((OCSP_RESPID_it_FP = (OCSP_RESPID_it_TYPE)GetProcAddress(libeayHandleM, "OCSP_RESPID_it")) == NULL) goto err;
//	if ((OCSP_RESPID_new_FP = (OCSP_RESPID_new_TYPE)GetProcAddress(libeayHandleM, "OCSP_RESPID_new")) == NULL) goto err;
//	if ((OCSP_RESPONSE_free_FP = (OCSP_RESPONSE_free_TYPE)GetProcAddress(libeayHandleM, "OCSP_RESPONSE_free")) == NULL) goto err;
//	if ((OCSP_RESPONSE_it_FP = (OCSP_RESPONSE_it_TYPE)GetProcAddress(libeayHandleM, "OCSP_RESPONSE_it")) == NULL) goto err;
//	if ((OCSP_RESPONSE_new_FP = (OCSP_RESPONSE_new_TYPE)GetProcAddress(libeayHandleM, "OCSP_RESPONSE_new")) == NULL) goto err;
//	if ((OCSP_RESPONSE_print_FP = (OCSP_RESPONSE_print_TYPE)GetProcAddress(libeayHandleM, "OCSP_RESPONSE_print")) == NULL) goto err;
//	if ((OCSP_REVOKEDINFO_free_FP = (OCSP_REVOKEDINFO_free_TYPE)GetProcAddress(libeayHandleM, "OCSP_REVOKEDINFO_free")) == NULL) goto err;
//	if ((OCSP_REVOKEDINFO_it_FP = (OCSP_REVOKEDINFO_it_TYPE)GetProcAddress(libeayHandleM, "OCSP_REVOKEDINFO_it")) == NULL) goto err;
//	if ((OCSP_REVOKEDINFO_new_FP = (OCSP_REVOKEDINFO_new_TYPE)GetProcAddress(libeayHandleM, "OCSP_REVOKEDINFO_new")) == NULL) goto err;
//	if ((OCSP_SERVICELOC_free_FP = (OCSP_SERVICELOC_free_TYPE)GetProcAddress(libeayHandleM, "OCSP_SERVICELOC_free")) == NULL) goto err;
//	if ((OCSP_SERVICELOC_it_FP = (OCSP_SERVICELOC_it_TYPE)GetProcAddress(libeayHandleM, "OCSP_SERVICELOC_it")) == NULL) goto err;
//	if ((OCSP_SERVICELOC_new_FP = (OCSP_SERVICELOC_new_TYPE)GetProcAddress(libeayHandleM, "OCSP_SERVICELOC_new")) == NULL) goto err;
//	if ((OCSP_SIGNATURE_free_FP = (OCSP_SIGNATURE_free_TYPE)GetProcAddress(libeayHandleM, "OCSP_SIGNATURE_free")) == NULL) goto err;
//	if ((OCSP_SIGNATURE_it_FP = (OCSP_SIGNATURE_it_TYPE)GetProcAddress(libeayHandleM, "OCSP_SIGNATURE_it")) == NULL) goto err;
//	if ((OCSP_SIGNATURE_new_FP = (OCSP_SIGNATURE_new_TYPE)GetProcAddress(libeayHandleM, "OCSP_SIGNATURE_new")) == NULL) goto err;
//	if ((OCSP_SINGLERESP_add1_ext_i2d_FP = (OCSP_SINGLERESP_add1_ext_i2d_TYPE)GetProcAddress(libeayHandleM, "OCSP_SINGLERESP_add1_ext_i2d")) == NULL) goto err;
//	if ((OCSP_SINGLERESP_add_ext_FP = (OCSP_SINGLERESP_add_ext_TYPE)GetProcAddress(libeayHandleM, "OCSP_SINGLERESP_add_ext")) == NULL) goto err;
//	if ((OCSP_SINGLERESP_delete_ext_FP = (OCSP_SINGLERESP_delete_ext_TYPE)GetProcAddress(libeayHandleM, "OCSP_SINGLERESP_delete_ext")) == NULL) goto err;
//	if ((OCSP_SINGLERESP_free_FP = (OCSP_SINGLERESP_free_TYPE)GetProcAddress(libeayHandleM, "OCSP_SINGLERESP_free")) == NULL) goto err;
//	if ((OCSP_SINGLERESP_get1_ext_d2i_FP = (OCSP_SINGLERESP_get1_ext_d2i_TYPE)GetProcAddress(libeayHandleM, "OCSP_SINGLERESP_get1_ext_d2i")) == NULL) goto err;
//	if ((OCSP_SINGLERESP_get_ext_FP = (OCSP_SINGLERESP_get_ext_TYPE)GetProcAddress(libeayHandleM, "OCSP_SINGLERESP_get_ext")) == NULL) goto err;
//	if ((OCSP_SINGLERESP_get_ext_by_NID_FP = (OCSP_SINGLERESP_get_ext_by_NID_TYPE)GetProcAddress(libeayHandleM, "OCSP_SINGLERESP_get_ext_by_NID")) == NULL) goto err;
//	if ((OCSP_SINGLERESP_get_ext_by_OBJ_FP = (OCSP_SINGLERESP_get_ext_by_OBJ_TYPE)GetProcAddress(libeayHandleM, "OCSP_SINGLERESP_get_ext_by_OBJ")) == NULL) goto err;
//	if ((OCSP_SINGLERESP_get_ext_by_critical_FP = (OCSP_SINGLERESP_get_ext_by_critical_TYPE)GetProcAddress(libeayHandleM, "OCSP_SINGLERESP_get_ext_by_critical")) == NULL) goto err;
//	if ((OCSP_SINGLERESP_get_ext_count_FP = (OCSP_SINGLERESP_get_ext_count_TYPE)GetProcAddress(libeayHandleM, "OCSP_SINGLERESP_get_ext_count")) == NULL) goto err;
//	if ((OCSP_SINGLERESP_it_FP = (OCSP_SINGLERESP_it_TYPE)GetProcAddress(libeayHandleM, "OCSP_SINGLERESP_it")) == NULL) goto err;
//	if ((OCSP_SINGLERESP_new_FP = (OCSP_SINGLERESP_new_TYPE)GetProcAddress(libeayHandleM, "OCSP_SINGLERESP_new")) == NULL) goto err;
//	if ((OCSP_accept_responses_new_FP = (OCSP_accept_responses_new_TYPE)GetProcAddress(libeayHandleM, "OCSP_accept_responses_new")) == NULL) goto err;
//	if ((OCSP_archive_cutoff_new_FP = (OCSP_archive_cutoff_new_TYPE)GetProcAddress(libeayHandleM, "OCSP_archive_cutoff_new")) == NULL) goto err;
//	if ((OCSP_basic_add1_cert_FP = (OCSP_basic_add1_cert_TYPE)GetProcAddress(libeayHandleM, "OCSP_basic_add1_cert")) == NULL) goto err;
//	if ((OCSP_basic_add1_nonce_FP = (OCSP_basic_add1_nonce_TYPE)GetProcAddress(libeayHandleM, "OCSP_basic_add1_nonce")) == NULL) goto err;
//	if ((OCSP_basic_add1_status_FP = (OCSP_basic_add1_status_TYPE)GetProcAddress(libeayHandleM, "OCSP_basic_add1_status")) == NULL) goto err;
//	if ((OCSP_basic_sign_FP = (OCSP_basic_sign_TYPE)GetProcAddress(libeayHandleM, "OCSP_basic_sign")) == NULL) goto err;
//	if ((OCSP_basic_verify_FP = (OCSP_basic_verify_TYPE)GetProcAddress(libeayHandleM, "OCSP_basic_verify")) == NULL) goto err;
//	if ((OCSP_cert_id_new_FP = (OCSP_cert_id_new_TYPE)GetProcAddress(libeayHandleM, "OCSP_cert_id_new")) == NULL) goto err;
//	if ((OCSP_cert_status_str_FP = (OCSP_cert_status_str_TYPE)GetProcAddress(libeayHandleM, "OCSP_cert_status_str")) == NULL) goto err;
//	if ((OCSP_cert_to_id_FP = (OCSP_cert_to_id_TYPE)GetProcAddress(libeayHandleM, "OCSP_cert_to_id")) == NULL) goto err;
//	if ((OCSP_check_nonce_FP = (OCSP_check_nonce_TYPE)GetProcAddress(libeayHandleM, "OCSP_check_nonce")) == NULL) goto err;
//	if ((OCSP_check_validity_FP = (OCSP_check_validity_TYPE)GetProcAddress(libeayHandleM, "OCSP_check_validity")) == NULL) goto err;
//	if ((OCSP_copy_nonce_FP = (OCSP_copy_nonce_TYPE)GetProcAddress(libeayHandleM, "OCSP_copy_nonce")) == NULL) goto err;
//	if ((OCSP_crlID_new_FP = (OCSP_crlID_new_TYPE)GetProcAddress(libeayHandleM, "OCSP_crlID_new")) == NULL) goto err;
//	if ((OCSP_crl_reason_str_FP = (OCSP_crl_reason_str_TYPE)GetProcAddress(libeayHandleM, "OCSP_crl_reason_str")) == NULL) goto err;
//	if ((OCSP_id_cmp_FP = (OCSP_id_cmp_TYPE)GetProcAddress(libeayHandleM, "OCSP_id_cmp")) == NULL) goto err;
//	if ((OCSP_id_get0_info_FP = (OCSP_id_get0_info_TYPE)GetProcAddress(libeayHandleM, "OCSP_id_get0_info")) == NULL) goto err;
//	if ((OCSP_id_issuer_cmp_FP = (OCSP_id_issuer_cmp_TYPE)GetProcAddress(libeayHandleM, "OCSP_id_issuer_cmp")) == NULL) goto err;
//	if ((OCSP_onereq_get0_id_FP = (OCSP_onereq_get0_id_TYPE)GetProcAddress(libeayHandleM, "OCSP_onereq_get0_id")) == NULL) goto err;
//	if ((OCSP_parse_url_FP = (OCSP_parse_url_TYPE)GetProcAddress(libeayHandleM, "OCSP_parse_url")) == NULL) goto err;
//	if ((OCSP_request_add0_id_FP = (OCSP_request_add0_id_TYPE)GetProcAddress(libeayHandleM, "OCSP_request_add0_id")) == NULL) goto err;
//	if ((OCSP_request_add1_cert_FP = (OCSP_request_add1_cert_TYPE)GetProcAddress(libeayHandleM, "OCSP_request_add1_cert")) == NULL) goto err;
//	if ((OCSP_request_add1_nonce_FP = (OCSP_request_add1_nonce_TYPE)GetProcAddress(libeayHandleM, "OCSP_request_add1_nonce")) == NULL) goto err;
//	if ((OCSP_request_is_signed_FP = (OCSP_request_is_signed_TYPE)GetProcAddress(libeayHandleM, "OCSP_request_is_signed")) == NULL) goto err;
//	if ((OCSP_request_onereq_count_FP = (OCSP_request_onereq_count_TYPE)GetProcAddress(libeayHandleM, "OCSP_request_onereq_count")) == NULL) goto err;
//	if ((OCSP_request_onereq_get0_FP = (OCSP_request_onereq_get0_TYPE)GetProcAddress(libeayHandleM, "OCSP_request_onereq_get0")) == NULL) goto err;
//	if ((OCSP_request_set1_name_FP = (OCSP_request_set1_name_TYPE)GetProcAddress(libeayHandleM, "OCSP_request_set1_name")) == NULL) goto err;
//	if ((OCSP_request_sign_FP = (OCSP_request_sign_TYPE)GetProcAddress(libeayHandleM, "OCSP_request_sign")) == NULL) goto err;
//	if ((OCSP_request_verify_FP = (OCSP_request_verify_TYPE)GetProcAddress(libeayHandleM, "OCSP_request_verify")) == NULL) goto err;
//	if ((OCSP_resp_count_FP = (OCSP_resp_count_TYPE)GetProcAddress(libeayHandleM, "OCSP_resp_count")) == NULL) goto err;
//	if ((OCSP_resp_find_FP = (OCSP_resp_find_TYPE)GetProcAddress(libeayHandleM, "OCSP_resp_find")) == NULL) goto err;
//	if ((OCSP_resp_find_status_FP = (OCSP_resp_find_status_TYPE)GetProcAddress(libeayHandleM, "OCSP_resp_find_status")) == NULL) goto err;
//	if ((OCSP_resp_get0_FP = (OCSP_resp_get0_TYPE)GetProcAddress(libeayHandleM, "OCSP_resp_get0")) == NULL) goto err;
//	if ((OCSP_response_create_FP = (OCSP_response_create_TYPE)GetProcAddress(libeayHandleM, "OCSP_response_create")) == NULL) goto err;
//	if ((OCSP_response_get1_basic_FP = (OCSP_response_get1_basic_TYPE)GetProcAddress(libeayHandleM, "OCSP_response_get1_basic")) == NULL) goto err;
//	if ((OCSP_response_status_FP = (OCSP_response_status_TYPE)GetProcAddress(libeayHandleM, "OCSP_response_status")) == NULL) goto err;
//	if ((OCSP_response_status_str_FP = (OCSP_response_status_str_TYPE)GetProcAddress(libeayHandleM, "OCSP_response_status_str")) == NULL) goto err;
//	if ((OCSP_sendreq_bio_FP = (OCSP_sendreq_bio_TYPE)GetProcAddress(libeayHandleM, "OCSP_sendreq_bio")) == NULL) goto err;
//	if ((OCSP_single_get0_status_FP = (OCSP_single_get0_status_TYPE)GetProcAddress(libeayHandleM, "OCSP_single_get0_status")) == NULL) goto err;
//	if ((OCSP_url_svcloc_new_FP = (OCSP_url_svcloc_new_TYPE)GetProcAddress(libeayHandleM, "OCSP_url_svcloc_new")) == NULL) goto err;
//	if ((OPENSSL_add_all_algorithms_conf_FP = (OPENSSL_add_all_algorithms_conf_TYPE)GetProcAddress(libeayHandleM, "OPENSSL_add_all_algorithms_conf")) == NULL) goto err;
	if ((OPENSSL_add_all_algorithms_noconf_FP = (OPENSSL_add_all_algorithms_noconf_TYPE)GetProcAddress(libeayHandleM, "OPENSSL_add_all_algorithms_noconf")) == NULL) goto err;
//	if ((OPENSSL_cleanse_FP = (OPENSSL_cleanse_TYPE)GetProcAddress(libeayHandleM, "OPENSSL_cleanse")) == NULL) goto err;
//	if ((OPENSSL_config_FP = (OPENSSL_config_TYPE)GetProcAddress(libeayHandleM, "OPENSSL_config")) == NULL) goto err;
//	if ((OPENSSL_issetugid_FP = (OPENSSL_issetugid_TYPE)GetProcAddress(libeayHandleM, "OPENSSL_issetugid")) == NULL) goto err;
//	if ((OPENSSL_load_builtin_modules_FP = (OPENSSL_load_builtin_modules_TYPE)GetProcAddress(libeayHandleM, "OPENSSL_load_builtin_modules")) == NULL) goto err;
//	if ((OPENSSL_no_config_FP = (OPENSSL_no_config_TYPE)GetProcAddress(libeayHandleM, "OPENSSL_no_config")) == NULL) goto err;
//	if ((OTHERNAME_free_FP = (OTHERNAME_free_TYPE)GetProcAddress(libeayHandleM, "OTHERNAME_free")) == NULL) goto err;
//	if ((OTHERNAME_it_FP = (OTHERNAME_it_TYPE)GetProcAddress(libeayHandleM, "OTHERNAME_it")) == NULL) goto err;
//	if ((OTHERNAME_new_FP = (OTHERNAME_new_TYPE)GetProcAddress(libeayHandleM, "OTHERNAME_new")) == NULL) goto err;
//	if ((OpenSSLDie_FP = (OpenSSLDie_TYPE)GetProcAddress(libeayHandleM, "OpenSSLDie")) == NULL) goto err;
//	if ((OpenSSL_add_all_ciphers_FP = (OpenSSL_add_all_ciphers_TYPE)GetProcAddress(libeayHandleM, "OpenSSL_add_all_ciphers")) == NULL) goto err;
//	if ((OpenSSL_add_all_digests_FP = (OpenSSL_add_all_digests_TYPE)GetProcAddress(libeayHandleM, "OpenSSL_add_all_digests")) == NULL) goto err;
//	if ((PBE2PARAM_free_FP = (PBE2PARAM_free_TYPE)GetProcAddress(libeayHandleM, "PBE2PARAM_free")) == NULL) goto err;
//	if ((PBE2PARAM_it_FP = (PBE2PARAM_it_TYPE)GetProcAddress(libeayHandleM, "PBE2PARAM_it")) == NULL) goto err;
//	if ((PBE2PARAM_new_FP = (PBE2PARAM_new_TYPE)GetProcAddress(libeayHandleM, "PBE2PARAM_new")) == NULL) goto err;
//	if ((PBEPARAM_free_FP = (PBEPARAM_free_TYPE)GetProcAddress(libeayHandleM, "PBEPARAM_free")) == NULL) goto err;
//	if ((PBEPARAM_it_FP = (PBEPARAM_it_TYPE)GetProcAddress(libeayHandleM, "PBEPARAM_it")) == NULL) goto err;
//	if ((PBEPARAM_new_FP = (PBEPARAM_new_TYPE)GetProcAddress(libeayHandleM, "PBEPARAM_new")) == NULL) goto err;
//	if ((PBKDF2PARAM_free_FP = (PBKDF2PARAM_free_TYPE)GetProcAddress(libeayHandleM, "PBKDF2PARAM_free")) == NULL) goto err;
//	if ((PBKDF2PARAM_it_FP = (PBKDF2PARAM_it_TYPE)GetProcAddress(libeayHandleM, "PBKDF2PARAM_it")) == NULL) goto err;
//	if ((PBKDF2PARAM_new_FP = (PBKDF2PARAM_new_TYPE)GetProcAddress(libeayHandleM, "PBKDF2PARAM_new")) == NULL) goto err;
//	if ((PEM_ASN1_read_FP = (PEM_ASN1_read_TYPE)GetProcAddress(libeayHandleM, "PEM_ASN1_read")) == NULL) goto err;
//	if ((PEM_ASN1_read_bio_FP = (PEM_ASN1_read_bio_TYPE)GetProcAddress(libeayHandleM, "PEM_ASN1_read_bio")) == NULL) goto err;
//	if ((PEM_ASN1_write_FP = (PEM_ASN1_write_TYPE)GetProcAddress(libeayHandleM, "PEM_ASN1_write")) == NULL) goto err;
//	if ((PEM_ASN1_write_bio_FP = (PEM_ASN1_write_bio_TYPE)GetProcAddress(libeayHandleM, "PEM_ASN1_write_bio")) == NULL) goto err;
//	if ((PEM_SealFinal_FP = (PEM_SealFinal_TYPE)GetProcAddress(libeayHandleM, "PEM_SealFinal")) == NULL) goto err;
//	if ((PEM_SealInit_FP = (PEM_SealInit_TYPE)GetProcAddress(libeayHandleM, "PEM_SealInit")) == NULL) goto err;
//	if ((PEM_SealUpdate_FP = (PEM_SealUpdate_TYPE)GetProcAddress(libeayHandleM, "PEM_SealUpdate")) == NULL) goto err;
//	if ((PEM_SignFinal_FP = (PEM_SignFinal_TYPE)GetProcAddress(libeayHandleM, "PEM_SignFinal")) == NULL) goto err;
//	if ((PEM_SignInit_FP = (PEM_SignInit_TYPE)GetProcAddress(libeayHandleM, "PEM_SignInit")) == NULL) goto err;
//	if ((PEM_SignUpdate_FP = (PEM_SignUpdate_TYPE)GetProcAddress(libeayHandleM, "PEM_SignUpdate")) == NULL) goto err;
//	if ((PEM_X509_INFO_read_FP = (PEM_X509_INFO_read_TYPE)GetProcAddress(libeayHandleM, "PEM_X509_INFO_read")) == NULL) goto err;
	if ((PEM_X509_INFO_read_bio_FP = (PEM_X509_INFO_read_bio_TYPE)GetProcAddress(libeayHandleM, "PEM_X509_INFO_read_bio")) == NULL) goto err;
//	if ((PEM_X509_INFO_write_bio_FP = (PEM_X509_INFO_write_bio_TYPE)GetProcAddress(libeayHandleM, "PEM_X509_INFO_write_bio")) == NULL) goto err;
//	if ((PEM_bytes_read_bio_FP = (PEM_bytes_read_bio_TYPE)GetProcAddress(libeayHandleM, "PEM_bytes_read_bio")) == NULL) goto err;
//	if ((PEM_def_callback_FP = (PEM_def_callback_TYPE)GetProcAddress(libeayHandleM, "PEM_def_callback")) == NULL) goto err;
//	if ((PEM_dek_info_FP = (PEM_dek_info_TYPE)GetProcAddress(libeayHandleM, "PEM_dek_info")) == NULL) goto err;
	if ((PEM_do_header_FP = (PEM_do_header_TYPE)GetProcAddress(libeayHandleM, "PEM_do_header")) == NULL) goto err;
	if ((PEM_get_EVP_CIPHER_INFO_FP = (PEM_get_EVP_CIPHER_INFO_TYPE)GetProcAddress(libeayHandleM, "PEM_get_EVP_CIPHER_INFO")) == NULL) goto err;
//	if ((PEM_proc_type_FP = (PEM_proc_type_TYPE)GetProcAddress(libeayHandleM, "PEM_proc_type")) == NULL) goto err;
//	if ((PEM_read_FP = (PEM_read_TYPE)GetProcAddress(libeayHandleM, "PEM_read")) == NULL) goto err;
//	if ((PEM_read_DHparams_FP = (PEM_read_DHparams_TYPE)GetProcAddress(libeayHandleM, "PEM_read_DHparams")) == NULL) goto err;
//	if ((PEM_read_DSAPrivateKey_FP = (PEM_read_DSAPrivateKey_TYPE)GetProcAddress(libeayHandleM, "PEM_read_DSAPrivateKey")) == NULL) goto err;
//	if ((PEM_read_DSA_PUBKEY_FP = (PEM_read_DSA_PUBKEY_TYPE)GetProcAddress(libeayHandleM, "PEM_read_DSA_PUBKEY")) == NULL) goto err;
//	if ((PEM_read_DSAparams_FP = (PEM_read_DSAparams_TYPE)GetProcAddress(libeayHandleM, "PEM_read_DSAparams")) == NULL) goto err;
//	if ((PEM_read_NETSCAPE_CERT_SEQUENCE_FP = (PEM_read_NETSCAPE_CERT_SEQUENCE_TYPE)GetProcAddress(libeayHandleM, "PEM_read_NETSCAPE_CERT_SEQUENCE")) == NULL) goto err;
//	if ((PEM_read_PKCS7_FP = (PEM_read_PKCS7_TYPE)GetProcAddress(libeayHandleM, "PEM_read_PKCS7")) == NULL) goto err;
//	if ((PEM_read_PKCS8_FP = (PEM_read_PKCS8_TYPE)GetProcAddress(libeayHandleM, "PEM_read_PKCS8")) == NULL) goto err;
//	if ((PEM_read_PKCS8_PRIV_KEY_INFO_FP = (PEM_read_PKCS8_PRIV_KEY_INFO_TYPE)GetProcAddress(libeayHandleM, "PEM_read_PKCS8_PRIV_KEY_INFO")) == NULL) goto err;
//	if ((PEM_read_PUBKEY_FP = (PEM_read_PUBKEY_TYPE)GetProcAddress(libeayHandleM, "PEM_read_PUBKEY")) == NULL) goto err;
//	if ((PEM_read_PrivateKey_FP = (PEM_read_PrivateKey_TYPE)GetProcAddress(libeayHandleM, "PEM_read_PrivateKey")) == NULL) goto err;
//	if ((PEM_read_RSAPrivateKey_FP = (PEM_read_RSAPrivateKey_TYPE)GetProcAddress(libeayHandleM, "PEM_read_RSAPrivateKey")) == NULL) goto err;
//	if ((PEM_read_RSAPublicKey_FP = (PEM_read_RSAPublicKey_TYPE)GetProcAddress(libeayHandleM, "PEM_read_RSAPublicKey")) == NULL) goto err;
//	if ((PEM_read_RSA_PUBKEY_FP = (PEM_read_RSA_PUBKEY_TYPE)GetProcAddress(libeayHandleM, "PEM_read_RSA_PUBKEY")) == NULL) goto err;
//	if ((PEM_read_X509_FP = (PEM_read_X509_TYPE)GetProcAddress(libeayHandleM, "PEM_read_X509")) == NULL) goto err;
//	if ((PEM_read_X509_AUX_FP = (PEM_read_X509_AUX_TYPE)GetProcAddress(libeayHandleM, "PEM_read_X509_AUX")) == NULL) goto err;
//	if ((PEM_read_X509_CRL_FP = (PEM_read_X509_CRL_TYPE)GetProcAddress(libeayHandleM, "PEM_read_X509_CRL")) == NULL) goto err;
//	if ((PEM_read_X509_REQ_FP = (PEM_read_X509_REQ_TYPE)GetProcAddress(libeayHandleM, "PEM_read_X509_REQ")) == NULL) goto err;
	if ((PEM_read_bio_FP = (PEM_read_bio_TYPE)GetProcAddress(libeayHandleM, "PEM_read_bio")) == NULL) goto err;
//	if ((PEM_read_bio_DHparams_FP = (PEM_read_bio_DHparams_TYPE)GetProcAddress(libeayHandleM, "PEM_read_bio_DHparams")) == NULL) goto err;
//	if ((PEM_read_bio_DSAPrivateKey_FP = (PEM_read_bio_DSAPrivateKey_TYPE)GetProcAddress(libeayHandleM, "PEM_read_bio_DSAPrivateKey")) == NULL) goto err;
//	if ((PEM_read_bio_DSA_PUBKEY_FP = (PEM_read_bio_DSA_PUBKEY_TYPE)GetProcAddress(libeayHandleM, "PEM_read_bio_DSA_PUBKEY")) == NULL) goto err;
//	if ((PEM_read_bio_DSAparams_FP = (PEM_read_bio_DSAparams_TYPE)GetProcAddress(libeayHandleM, "PEM_read_bio_DSAparams")) == NULL) goto err;
//	if ((PEM_read_bio_NETSCAPE_CERT_SEQUENCE_FP = (PEM_read_bio_NETSCAPE_CERT_SEQUENCE_TYPE)GetProcAddress(libeayHandleM, "PEM_read_bio_NETSCAPE_CERT_SEQUENCE")) == NULL) goto err;
	if ((PEM_read_bio_PKCS7_FP = (PEM_read_bio_PKCS7_TYPE)GetProcAddress(libeayHandleM, "PEM_read_bio_PKCS7")) == NULL) goto err;
//	if ((PEM_read_bio_PKCS8_FP = (PEM_read_bio_PKCS8_TYPE)GetProcAddress(libeayHandleM, "PEM_read_bio_PKCS8")) == NULL) goto err;
//	if ((PEM_read_bio_PKCS8_PRIV_KEY_INFO_FP = (PEM_read_bio_PKCS8_PRIV_KEY_INFO_TYPE)GetProcAddress(libeayHandleM, "PEM_read_bio_PKCS8_PRIV_KEY_INFO")) == NULL) goto err;
//	if ((PEM_read_bio_PUBKEY_FP = (PEM_read_bio_PUBKEY_TYPE)GetProcAddress(libeayHandleM, "PEM_read_bio_PUBKEY")) == NULL) goto err;
	if ((PEM_read_bio_PrivateKey_FP = (PEM_read_bio_PrivateKey_TYPE)GetProcAddress(libeayHandleM, "PEM_read_bio_PrivateKey")) == NULL) goto err;
//	if ((PEM_read_bio_RSAPrivateKey_FP = (PEM_read_bio_RSAPrivateKey_TYPE)GetProcAddress(libeayHandleM, "PEM_read_bio_RSAPrivateKey")) == NULL) goto err;
//	if ((PEM_read_bio_RSAPublicKey_FP = (PEM_read_bio_RSAPublicKey_TYPE)GetProcAddress(libeayHandleM, "PEM_read_bio_RSAPublicKey")) == NULL) goto err;
//	if ((PEM_read_bio_RSA_PUBKEY_FP = (PEM_read_bio_RSA_PUBKEY_TYPE)GetProcAddress(libeayHandleM, "PEM_read_bio_RSA_PUBKEY")) == NULL) goto err;
	if ((PEM_read_bio_X509_FP = (PEM_read_bio_X509_TYPE)GetProcAddress(libeayHandleM, "PEM_read_bio_X509")) == NULL) goto err;
//	if ((PEM_read_bio_X509_AUX_FP = (PEM_read_bio_X509_AUX_TYPE)GetProcAddress(libeayHandleM, "PEM_read_bio_X509_AUX")) == NULL) goto err;
//	if ((PEM_read_bio_X509_CRL_FP = (PEM_read_bio_X509_CRL_TYPE)GetProcAddress(libeayHandleM, "PEM_read_bio_X509_CRL")) == NULL) goto err;
//	if ((PEM_read_bio_X509_REQ_FP = (PEM_read_bio_X509_REQ_TYPE)GetProcAddress(libeayHandleM, "PEM_read_bio_X509_REQ")) == NULL) goto err;
//	if ((PEM_write_FP = (PEM_write_TYPE)GetProcAddress(libeayHandleM, "PEM_write")) == NULL) goto err;
//	if ((PEM_write_DHparams_FP = (PEM_write_DHparams_TYPE)GetProcAddress(libeayHandleM, "PEM_write_DHparams")) == NULL) goto err;
//	if ((PEM_write_DSAPrivateKey_FP = (PEM_write_DSAPrivateKey_TYPE)GetProcAddress(libeayHandleM, "PEM_write_DSAPrivateKey")) == NULL) goto err;
//	if ((PEM_write_DSA_PUBKEY_FP = (PEM_write_DSA_PUBKEY_TYPE)GetProcAddress(libeayHandleM, "PEM_write_DSA_PUBKEY")) == NULL) goto err;
//	if ((PEM_write_DSAparams_FP = (PEM_write_DSAparams_TYPE)GetProcAddress(libeayHandleM, "PEM_write_DSAparams")) == NULL) goto err;
//	if ((PEM_write_NETSCAPE_CERT_SEQUENCE_FP = (PEM_write_NETSCAPE_CERT_SEQUENCE_TYPE)GetProcAddress(libeayHandleM, "PEM_write_NETSCAPE_CERT_SEQUENCE")) == NULL) goto err;
//	if ((PEM_write_PKCS7_FP = (PEM_write_PKCS7_TYPE)GetProcAddress(libeayHandleM, "PEM_write_PKCS7")) == NULL) goto err;
//	if ((PEM_write_PKCS8PrivateKey_FP = (PEM_write_PKCS8PrivateKey_TYPE)GetProcAddress(libeayHandleM, "PEM_write_PKCS8PrivateKey")) == NULL) goto err;
//	if ((PEM_write_PKCS8PrivateKey_nid_FP = (PEM_write_PKCS8PrivateKey_nid_TYPE)GetProcAddress(libeayHandleM, "PEM_write_PKCS8PrivateKey_nid")) == NULL) goto err;
//	if ((PEM_write_PKCS8_FP = (PEM_write_PKCS8_TYPE)GetProcAddress(libeayHandleM, "PEM_write_PKCS8")) == NULL) goto err;
//	if ((PEM_write_PKCS8_PRIV_KEY_INFO_FP = (PEM_write_PKCS8_PRIV_KEY_INFO_TYPE)GetProcAddress(libeayHandleM, "PEM_write_PKCS8_PRIV_KEY_INFO")) == NULL) goto err;
//	if ((PEM_write_PUBKEY_FP = (PEM_write_PUBKEY_TYPE)GetProcAddress(libeayHandleM, "PEM_write_PUBKEY")) == NULL) goto err;
//	if ((PEM_write_PrivateKey_FP = (PEM_write_PrivateKey_TYPE)GetProcAddress(libeayHandleM, "PEM_write_PrivateKey")) == NULL) goto err;
//	if ((PEM_write_RSAPrivateKey_FP = (PEM_write_RSAPrivateKey_TYPE)GetProcAddress(libeayHandleM, "PEM_write_RSAPrivateKey")) == NULL) goto err;
//	if ((PEM_write_RSAPublicKey_FP = (PEM_write_RSAPublicKey_TYPE)GetProcAddress(libeayHandleM, "PEM_write_RSAPublicKey")) == NULL) goto err;
//	if ((PEM_write_RSA_PUBKEY_FP = (PEM_write_RSA_PUBKEY_TYPE)GetProcAddress(libeayHandleM, "PEM_write_RSA_PUBKEY")) == NULL) goto err;
//	if ((PEM_write_X509_FP = (PEM_write_X509_TYPE)GetProcAddress(libeayHandleM, "PEM_write_X509")) == NULL) goto err;
//	if ((PEM_write_X509_AUX_FP = (PEM_write_X509_AUX_TYPE)GetProcAddress(libeayHandleM, "PEM_write_X509_AUX")) == NULL) goto err;
//	if ((PEM_write_X509_CRL_FP = (PEM_write_X509_CRL_TYPE)GetProcAddress(libeayHandleM, "PEM_write_X509_CRL")) == NULL) goto err;
//	if ((PEM_write_X509_REQ_FP = (PEM_write_X509_REQ_TYPE)GetProcAddress(libeayHandleM, "PEM_write_X509_REQ")) == NULL) goto err;
//	if ((PEM_write_X509_REQ_NEW_FP = (PEM_write_X509_REQ_NEW_TYPE)GetProcAddress(libeayHandleM, "PEM_write_X509_REQ_NEW")) == NULL) goto err;
//	if ((PEM_write_bio_FP = (PEM_write_bio_TYPE)GetProcAddress(libeayHandleM, "PEM_write_bio")) == NULL) goto err;
//	if ((PEM_write_bio_DHparams_FP = (PEM_write_bio_DHparams_TYPE)GetProcAddress(libeayHandleM, "PEM_write_bio_DHparams")) == NULL) goto err;
//	if ((PEM_write_bio_DSAPrivateKey_FP = (PEM_write_bio_DSAPrivateKey_TYPE)GetProcAddress(libeayHandleM, "PEM_write_bio_DSAPrivateKey")) == NULL) goto err;
//	if ((PEM_write_bio_DSA_PUBKEY_FP = (PEM_write_bio_DSA_PUBKEY_TYPE)GetProcAddress(libeayHandleM, "PEM_write_bio_DSA_PUBKEY")) == NULL) goto err;
//	if ((PEM_write_bio_DSAparams_FP = (PEM_write_bio_DSAparams_TYPE)GetProcAddress(libeayHandleM, "PEM_write_bio_DSAparams")) == NULL) goto err;
//	if ((PEM_write_bio_NETSCAPE_CERT_SEQUENCE_FP = (PEM_write_bio_NETSCAPE_CERT_SEQUENCE_TYPE)GetProcAddress(libeayHandleM, "PEM_write_bio_NETSCAPE_CERT_SEQUENCE")) == NULL) goto err;
//	if ((PEM_write_bio_PKCS7_FP = (PEM_write_bio_PKCS7_TYPE)GetProcAddress(libeayHandleM, "PEM_write_bio_PKCS7")) == NULL) goto err;
	if ((PEM_write_bio_PKCS8PrivateKey_FP = (PEM_write_bio_PKCS8PrivateKey_TYPE)GetProcAddress(libeayHandleM, "PEM_write_bio_PKCS8PrivateKey")) == NULL) goto err;
//	if ((PEM_write_bio_PKCS8PrivateKey_nid_FP = (PEM_write_bio_PKCS8PrivateKey_nid_TYPE)GetProcAddress(libeayHandleM, "PEM_write_bio_PKCS8PrivateKey_nid")) == NULL) goto err;
//	if ((PEM_write_bio_PKCS8_FP = (PEM_write_bio_PKCS8_TYPE)GetProcAddress(libeayHandleM, "PEM_write_bio_PKCS8")) == NULL) goto err;
//	if ((PEM_write_bio_PKCS8_PRIV_KEY_INFO_FP = (PEM_write_bio_PKCS8_PRIV_KEY_INFO_TYPE)GetProcAddress(libeayHandleM, "PEM_write_bio_PKCS8_PRIV_KEY_INFO")) == NULL) goto err;
//	if ((PEM_write_bio_PUBKEY_FP = (PEM_write_bio_PUBKEY_TYPE)GetProcAddress(libeayHandleM, "PEM_write_bio_PUBKEY")) == NULL) goto err;
//	if ((PEM_write_bio_PrivateKey_FP = (PEM_write_bio_PrivateKey_TYPE)GetProcAddress(libeayHandleM, "PEM_write_bio_PrivateKey")) == NULL) goto err;
//	if ((PEM_write_bio_RSAPrivateKey_FP = (PEM_write_bio_RSAPrivateKey_TYPE)GetProcAddress(libeayHandleM, "PEM_write_bio_RSAPrivateKey")) == NULL) goto err;
//	if ((PEM_write_bio_RSAPublicKey_FP = (PEM_write_bio_RSAPublicKey_TYPE)GetProcAddress(libeayHandleM, "PEM_write_bio_RSAPublicKey")) == NULL) goto err;
//	if ((PEM_write_bio_RSA_PUBKEY_FP = (PEM_write_bio_RSA_PUBKEY_TYPE)GetProcAddress(libeayHandleM, "PEM_write_bio_RSA_PUBKEY")) == NULL) goto err;
	if ((PEM_write_bio_X509_FP = (PEM_write_bio_X509_TYPE)GetProcAddress(libeayHandleM, "PEM_write_bio_X509")) == NULL) goto err;
//	if ((PEM_write_bio_X509_AUX_FP = (PEM_write_bio_X509_AUX_TYPE)GetProcAddress(libeayHandleM, "PEM_write_bio_X509_AUX")) == NULL) goto err;
//	if ((PEM_write_bio_X509_CRL_FP = (PEM_write_bio_X509_CRL_TYPE)GetProcAddress(libeayHandleM, "PEM_write_bio_X509_CRL")) == NULL) goto err;
//	if ((PEM_write_bio_X509_REQ_FP = (PEM_write_bio_X509_REQ_TYPE)GetProcAddress(libeayHandleM, "PEM_write_bio_X509_REQ")) == NULL) goto err;
//	if ((PEM_write_bio_X509_REQ_NEW_FP = (PEM_write_bio_X509_REQ_NEW_TYPE)GetProcAddress(libeayHandleM, "PEM_write_bio_X509_REQ_NEW")) == NULL) goto err;
//	if ((PKCS12_AUTHSAFES_it_FP = (PKCS12_AUTHSAFES_it_TYPE)GetProcAddress(libeayHandleM, "PKCS12_AUTHSAFES_it")) == NULL) goto err;
//	if ((PKCS12_BAGS_free_FP = (PKCS12_BAGS_free_TYPE)GetProcAddress(libeayHandleM, "PKCS12_BAGS_free")) == NULL) goto err;
//	if ((PKCS12_BAGS_it_FP = (PKCS12_BAGS_it_TYPE)GetProcAddress(libeayHandleM, "PKCS12_BAGS_it")) == NULL) goto err;
//	if ((PKCS12_BAGS_new_FP = (PKCS12_BAGS_new_TYPE)GetProcAddress(libeayHandleM, "PKCS12_BAGS_new")) == NULL) goto err;
//	if ((PKCS12_MAC_DATA_free_FP = (PKCS12_MAC_DATA_free_TYPE)GetProcAddress(libeayHandleM, "PKCS12_MAC_DATA_free")) == NULL) goto err;
//	if ((PKCS12_MAC_DATA_it_FP = (PKCS12_MAC_DATA_it_TYPE)GetProcAddress(libeayHandleM, "PKCS12_MAC_DATA_it")) == NULL) goto err;
//	if ((PKCS12_MAC_DATA_new_FP = (PKCS12_MAC_DATA_new_TYPE)GetProcAddress(libeayHandleM, "PKCS12_MAC_DATA_new")) == NULL) goto err;
//	if ((PKCS12_MAKE_KEYBAG_FP = (PKCS12_MAKE_KEYBAG_TYPE)GetProcAddress(libeayHandleM, "PKCS12_MAKE_KEYBAG")) == NULL) goto err;
//	if ((PKCS12_MAKE_SHKEYBAG_FP = (PKCS12_MAKE_SHKEYBAG_TYPE)GetProcAddress(libeayHandleM, "PKCS12_MAKE_SHKEYBAG")) == NULL) goto err;
//	if ((PKCS12_PBE_add_FP = (PKCS12_PBE_add_TYPE)GetProcAddress(libeayHandleM, "PKCS12_PBE_add")) == NULL) goto err;
//	if ((PKCS12_PBE_keyivgen_FP = (PKCS12_PBE_keyivgen_TYPE)GetProcAddress(libeayHandleM, "PKCS12_PBE_keyivgen")) == NULL) goto err;
//	if ((PKCS12_SAFEBAGS_it_FP = (PKCS12_SAFEBAGS_it_TYPE)GetProcAddress(libeayHandleM, "PKCS12_SAFEBAGS_it")) == NULL) goto err;
//	if ((PKCS12_SAFEBAG_free_FP = (PKCS12_SAFEBAG_free_TYPE)GetProcAddress(libeayHandleM, "PKCS12_SAFEBAG_free")) == NULL) goto err;
//	if ((PKCS12_SAFEBAG_it_FP = (PKCS12_SAFEBAG_it_TYPE)GetProcAddress(libeayHandleM, "PKCS12_SAFEBAG_it")) == NULL) goto err;
//	if ((PKCS12_SAFEBAG_new_FP = (PKCS12_SAFEBAG_new_TYPE)GetProcAddress(libeayHandleM, "PKCS12_SAFEBAG_new")) == NULL) goto err;
//	if ((PKCS12_add_CSPName_asc_FP = (PKCS12_add_CSPName_asc_TYPE)GetProcAddress(libeayHandleM, "PKCS12_add_CSPName_asc")) == NULL) goto err;
//	if ((PKCS12_add_friendlyname_asc_FP = (PKCS12_add_friendlyname_asc_TYPE)GetProcAddress(libeayHandleM, "PKCS12_add_friendlyname_asc")) == NULL) goto err;
//	if ((PKCS12_add_friendlyname_uni_FP = (PKCS12_add_friendlyname_uni_TYPE)GetProcAddress(libeayHandleM, "PKCS12_add_friendlyname_uni")) == NULL) goto err;
//	if ((PKCS12_add_localkeyid_FP = (PKCS12_add_localkeyid_TYPE)GetProcAddress(libeayHandleM, "PKCS12_add_localkeyid")) == NULL) goto err;
//	if ((PKCS12_certbag2x509_FP = (PKCS12_certbag2x509_TYPE)GetProcAddress(libeayHandleM, "PKCS12_certbag2x509")) == NULL) goto err;
//	if ((PKCS12_certbag2x509crl_FP = (PKCS12_certbag2x509crl_TYPE)GetProcAddress(libeayHandleM, "PKCS12_certbag2x509crl")) == NULL) goto err;
//	if ((PKCS12_create_FP = (PKCS12_create_TYPE)GetProcAddress(libeayHandleM, "PKCS12_create")) == NULL) goto err;
//	if ((PKCS12_decrypt_skey_FP = (PKCS12_decrypt_skey_TYPE)GetProcAddress(libeayHandleM, "PKCS12_decrypt_skey")) == NULL) goto err;
	if ((PKCS12_free_FP = (PKCS12_free_TYPE)GetProcAddress(libeayHandleM, "PKCS12_free")) == NULL) goto err;
//	if ((PKCS12_gen_mac_FP = (PKCS12_gen_mac_TYPE)GetProcAddress(libeayHandleM, "PKCS12_gen_mac")) == NULL) goto err;
//	if ((PKCS12_get_attr_gen_FP = (PKCS12_get_attr_gen_TYPE)GetProcAddress(libeayHandleM, "PKCS12_get_attr_gen")) == NULL) goto err;
//	if ((PKCS12_get_friendlyname_FP = (PKCS12_get_friendlyname_TYPE)GetProcAddress(libeayHandleM, "PKCS12_get_friendlyname")) == NULL) goto err;
//	if ((PKCS12_init_FP = (PKCS12_init_TYPE)GetProcAddress(libeayHandleM, "PKCS12_init")) == NULL) goto err;
//	if ((PKCS12_item_decrypt_d2i_FP = (PKCS12_item_decrypt_d2i_TYPE)GetProcAddress(libeayHandleM, "PKCS12_item_decrypt_d2i")) == NULL) goto err;
//	if ((PKCS12_item_i2d_encrypt_FP = (PKCS12_item_i2d_encrypt_TYPE)GetProcAddress(libeayHandleM, "PKCS12_item_i2d_encrypt")) == NULL) goto err;
//	if ((PKCS12_item_pack_safebag_FP = (PKCS12_item_pack_safebag_TYPE)GetProcAddress(libeayHandleM, "PKCS12_item_pack_safebag")) == NULL) goto err;
//	if ((PKCS12_it_FP = (PKCS12_it_TYPE)GetProcAddress(libeayHandleM, "PKCS12_it")) == NULL) goto err;
//	if ((PKCS12_key_gen_asc_FP = (PKCS12_key_gen_asc_TYPE)GetProcAddress(libeayHandleM, "PKCS12_key_gen_asc")) == NULL) goto err;
//	if ((PKCS12_key_gen_uni_FP = (PKCS12_key_gen_uni_TYPE)GetProcAddress(libeayHandleM, "PKCS12_key_gen_uni")) == NULL) goto err;
//	if ((PKCS12_new_FP = (PKCS12_new_TYPE)GetProcAddress(libeayHandleM, "PKCS12_new")) == NULL) goto err;
//	if ((PKCS12_newpass_FP = (PKCS12_newpass_TYPE)GetProcAddress(libeayHandleM, "PKCS12_newpass")) == NULL) goto err;
//	if ((PKCS12_pack_authsafes_FP = (PKCS12_pack_authsafes_TYPE)GetProcAddress(libeayHandleM, "PKCS12_pack_authsafes")) == NULL) goto err;
//	if ((PKCS12_pack_p7data_FP = (PKCS12_pack_p7data_TYPE)GetProcAddress(libeayHandleM, "PKCS12_pack_p7data")) == NULL) goto err;
//	if ((PKCS12_pack_p7encdata_FP = (PKCS12_pack_p7encdata_TYPE)GetProcAddress(libeayHandleM, "PKCS12_pack_p7encdata")) == NULL) goto err;
	if ((PKCS12_parse_FP = (PKCS12_parse_TYPE)GetProcAddress(libeayHandleM, "PKCS12_parse")) == NULL) goto err;
//	if ((PKCS12_pbe_crypt_FP = (PKCS12_pbe_crypt_TYPE)GetProcAddress(libeayHandleM, "PKCS12_pbe_crypt")) == NULL) goto err;
//	if ((PKCS12_set_mac_FP = (PKCS12_set_mac_TYPE)GetProcAddress(libeayHandleM, "PKCS12_set_mac")) == NULL) goto err;
//	if ((PKCS12_setup_mac_FP = (PKCS12_setup_mac_TYPE)GetProcAddress(libeayHandleM, "PKCS12_setup_mac")) == NULL) goto err;
//	if ((PKCS12_unpack_authsafes_FP = (PKCS12_unpack_authsafes_TYPE)GetProcAddress(libeayHandleM, "PKCS12_unpack_authsafes")) == NULL) goto err;
//	if ((PKCS12_unpack_p7data_FP = (PKCS12_unpack_p7data_TYPE)GetProcAddress(libeayHandleM, "PKCS12_unpack_p7data")) == NULL) goto err;
//	if ((PKCS12_unpack_p7encdata_FP = (PKCS12_unpack_p7encdata_TYPE)GetProcAddress(libeayHandleM, "PKCS12_unpack_p7encdata")) == NULL) goto err;
	if ((PKCS12_verify_mac_FP = (PKCS12_verify_mac_TYPE)GetProcAddress(libeayHandleM, "PKCS12_verify_mac")) == NULL) goto err;
//	if ((PKCS12_x5092certbag_FP = (PKCS12_x5092certbag_TYPE)GetProcAddress(libeayHandleM, "PKCS12_x5092certbag")) == NULL) goto err;
//	if ((PKCS12_x509crl2certbag_FP = (PKCS12_x509crl2certbag_TYPE)GetProcAddress(libeayHandleM, "PKCS12_x509crl2certbag")) == NULL) goto err;
//	if ((PKCS5_PBE_add_FP = (PKCS5_PBE_add_TYPE)GetProcAddress(libeayHandleM, "PKCS5_PBE_add")) == NULL) goto err;
//	if ((PKCS5_PBE_keyivgen_FP = (PKCS5_PBE_keyivgen_TYPE)GetProcAddress(libeayHandleM, "PKCS5_PBE_keyivgen")) == NULL) goto err;
//	if ((PKCS5_PBKDF2_HMAC_SHA1_FP = (PKCS5_PBKDF2_HMAC_SHA1_TYPE)GetProcAddress(libeayHandleM, "PKCS5_PBKDF2_HMAC_SHA1")) == NULL) goto err;
//	if ((PKCS5_pbe2_set_FP = (PKCS5_pbe2_set_TYPE)GetProcAddress(libeayHandleM, "PKCS5_pbe2_set")) == NULL) goto err;
//	if ((PKCS5_pbe_set_FP = (PKCS5_pbe_set_TYPE)GetProcAddress(libeayHandleM, "PKCS5_pbe_set")) == NULL) goto err;
//	if ((PKCS5_v2_PBE_keyivgen_FP = (PKCS5_v2_PBE_keyivgen_TYPE)GetProcAddress(libeayHandleM, "PKCS5_v2_PBE_keyivgen")) == NULL) goto err;
//	if ((PKCS7_ATTR_SIGN_it_FP = (PKCS7_ATTR_SIGN_it_TYPE)GetProcAddress(libeayHandleM, "PKCS7_ATTR_SIGN_it")) == NULL) goto err;
//	if ((PKCS7_ATTR_VERIFY_it_FP = (PKCS7_ATTR_VERIFY_it_TYPE)GetProcAddress(libeayHandleM, "PKCS7_ATTR_VERIFY_it")) == NULL) goto err;
//	if ((PKCS7_DIGEST_free_FP = (PKCS7_DIGEST_free_TYPE)GetProcAddress(libeayHandleM, "PKCS7_DIGEST_free")) == NULL) goto err;
//	if ((PKCS7_DIGEST_it_FP = (PKCS7_DIGEST_it_TYPE)GetProcAddress(libeayHandleM, "PKCS7_DIGEST_it")) == NULL) goto err;
//	if ((PKCS7_DIGEST_new_FP = (PKCS7_DIGEST_new_TYPE)GetProcAddress(libeayHandleM, "PKCS7_DIGEST_new")) == NULL) goto err;
//	if ((PKCS7_ENCRYPT_free_FP = (PKCS7_ENCRYPT_free_TYPE)GetProcAddress(libeayHandleM, "PKCS7_ENCRYPT_free")) == NULL) goto err;
//	if ((PKCS7_ENCRYPT_it_FP = (PKCS7_ENCRYPT_it_TYPE)GetProcAddress(libeayHandleM, "PKCS7_ENCRYPT_it")) == NULL) goto err;
//	if ((PKCS7_ENCRYPT_new_FP = (PKCS7_ENCRYPT_new_TYPE)GetProcAddress(libeayHandleM, "PKCS7_ENCRYPT_new")) == NULL) goto err;
//	if ((PKCS7_ENC_CONTENT_free_FP = (PKCS7_ENC_CONTENT_free_TYPE)GetProcAddress(libeayHandleM, "PKCS7_ENC_CONTENT_free")) == NULL) goto err;
//	if ((PKCS7_ENC_CONTENT_it_FP = (PKCS7_ENC_CONTENT_it_TYPE)GetProcAddress(libeayHandleM, "PKCS7_ENC_CONTENT_it")) == NULL) goto err;
//	if ((PKCS7_ENC_CONTENT_new_FP = (PKCS7_ENC_CONTENT_new_TYPE)GetProcAddress(libeayHandleM, "PKCS7_ENC_CONTENT_new")) == NULL) goto err;
//	if ((PKCS7_ENVELOPE_free_FP = (PKCS7_ENVELOPE_free_TYPE)GetProcAddress(libeayHandleM, "PKCS7_ENVELOPE_free")) == NULL) goto err;
//	if ((PKCS7_ENVELOPE_it_FP = (PKCS7_ENVELOPE_it_TYPE)GetProcAddress(libeayHandleM, "PKCS7_ENVELOPE_it")) == NULL) goto err;
//	if ((PKCS7_ENVELOPE_new_FP = (PKCS7_ENVELOPE_new_TYPE)GetProcAddress(libeayHandleM, "PKCS7_ENVELOPE_new")) == NULL) goto err;
//	if ((PKCS7_ISSUER_AND_SERIAL_digest_FP = (PKCS7_ISSUER_AND_SERIAL_digest_TYPE)GetProcAddress(libeayHandleM, "PKCS7_ISSUER_AND_SERIAL_digest")) == NULL) goto err;
//	if ((PKCS7_ISSUER_AND_SERIAL_free_FP = (PKCS7_ISSUER_AND_SERIAL_free_TYPE)GetProcAddress(libeayHandleM, "PKCS7_ISSUER_AND_SERIAL_free")) == NULL) goto err;
//	if ((PKCS7_ISSUER_AND_SERIAL_it_FP = (PKCS7_ISSUER_AND_SERIAL_it_TYPE)GetProcAddress(libeayHandleM, "PKCS7_ISSUER_AND_SERIAL_it")) == NULL) goto err;
//	if ((PKCS7_ISSUER_AND_SERIAL_new_FP = (PKCS7_ISSUER_AND_SERIAL_new_TYPE)GetProcAddress(libeayHandleM, "PKCS7_ISSUER_AND_SERIAL_new")) == NULL) goto err;
//	if ((PKCS7_RECIP_INFO_free_FP = (PKCS7_RECIP_INFO_free_TYPE)GetProcAddress(libeayHandleM, "PKCS7_RECIP_INFO_free")) == NULL) goto err;
//	if ((PKCS7_RECIP_INFO_it_FP = (PKCS7_RECIP_INFO_it_TYPE)GetProcAddress(libeayHandleM, "PKCS7_RECIP_INFO_it")) == NULL) goto err;
//	if ((PKCS7_RECIP_INFO_new_FP = (PKCS7_RECIP_INFO_new_TYPE)GetProcAddress(libeayHandleM, "PKCS7_RECIP_INFO_new")) == NULL) goto err;
//	if ((PKCS7_RECIP_INFO_set_FP = (PKCS7_RECIP_INFO_set_TYPE)GetProcAddress(libeayHandleM, "PKCS7_RECIP_INFO_set")) == NULL) goto err;
//	if ((PKCS7_SIGNED_free_FP = (PKCS7_SIGNED_free_TYPE)GetProcAddress(libeayHandleM, "PKCS7_SIGNED_free")) == NULL) goto err;
//	if ((PKCS7_SIGNED_it_FP = (PKCS7_SIGNED_it_TYPE)GetProcAddress(libeayHandleM, "PKCS7_SIGNED_it")) == NULL) goto err;
//	if ((PKCS7_SIGNED_new_FP = (PKCS7_SIGNED_new_TYPE)GetProcAddress(libeayHandleM, "PKCS7_SIGNED_new")) == NULL) goto err;
//	if ((PKCS7_SIGNER_INFO_free_FP = (PKCS7_SIGNER_INFO_free_TYPE)GetProcAddress(libeayHandleM, "PKCS7_SIGNER_INFO_free")) == NULL) goto err;
//	if ((PKCS7_SIGNER_INFO_it_FP = (PKCS7_SIGNER_INFO_it_TYPE)GetProcAddress(libeayHandleM, "PKCS7_SIGNER_INFO_it")) == NULL) goto err;
//	if ((PKCS7_SIGNER_INFO_new_FP = (PKCS7_SIGNER_INFO_new_TYPE)GetProcAddress(libeayHandleM, "PKCS7_SIGNER_INFO_new")) == NULL) goto err;
//	if ((PKCS7_SIGNER_INFO_set_FP = (PKCS7_SIGNER_INFO_set_TYPE)GetProcAddress(libeayHandleM, "PKCS7_SIGNER_INFO_set")) == NULL) goto err;
//	if ((PKCS7_SIGN_ENVELOPE_free_FP = (PKCS7_SIGN_ENVELOPE_free_TYPE)GetProcAddress(libeayHandleM, "PKCS7_SIGN_ENVELOPE_free")) == NULL) goto err;
//	if ((PKCS7_SIGN_ENVELOPE_it_FP = (PKCS7_SIGN_ENVELOPE_it_TYPE)GetProcAddress(libeayHandleM, "PKCS7_SIGN_ENVELOPE_it")) == NULL) goto err;
//	if ((PKCS7_SIGN_ENVELOPE_new_FP = (PKCS7_SIGN_ENVELOPE_new_TYPE)GetProcAddress(libeayHandleM, "PKCS7_SIGN_ENVELOPE_new")) == NULL) goto err;
//	if ((PKCS7_add_attrib_smimecap_FP = (PKCS7_add_attrib_smimecap_TYPE)GetProcAddress(libeayHandleM, "PKCS7_add_attrib_smimecap")) == NULL) goto err;
//	if ((PKCS7_add_attribute_FP = (PKCS7_add_attribute_TYPE)GetProcAddress(libeayHandleM, "PKCS7_add_attribute")) == NULL) goto err;
//	if ((PKCS7_add_certificate_FP = (PKCS7_add_certificate_TYPE)GetProcAddress(libeayHandleM, "PKCS7_add_certificate")) == NULL) goto err;
//	if ((PKCS7_add_crl_FP = (PKCS7_add_crl_TYPE)GetProcAddress(libeayHandleM, "PKCS7_add_crl")) == NULL) goto err;
//	if ((PKCS7_add_recipient_FP = (PKCS7_add_recipient_TYPE)GetProcAddress(libeayHandleM, "PKCS7_add_recipient")) == NULL) goto err;
//	if ((PKCS7_add_recipient_info_FP = (PKCS7_add_recipient_info_TYPE)GetProcAddress(libeayHandleM, "PKCS7_add_recipient_info")) == NULL) goto err;
//	if ((PKCS7_add_signature_FP = (PKCS7_add_signature_TYPE)GetProcAddress(libeayHandleM, "PKCS7_add_signature")) == NULL) goto err;
//	if ((PKCS7_add_signed_attribute_FP = (PKCS7_add_signed_attribute_TYPE)GetProcAddress(libeayHandleM, "PKCS7_add_signed_attribute")) == NULL) goto err;
//	if ((PKCS7_add_signer_FP = (PKCS7_add_signer_TYPE)GetProcAddress(libeayHandleM, "PKCS7_add_signer")) == NULL) goto err;
//	if ((PKCS7_cert_from_signer_info_FP = (PKCS7_cert_from_signer_info_TYPE)GetProcAddress(libeayHandleM, "PKCS7_cert_from_signer_info")) == NULL) goto err;
//	if ((PKCS7_content_new_FP = (PKCS7_content_new_TYPE)GetProcAddress(libeayHandleM, "PKCS7_content_new")) == NULL) goto err;
//	if ((PKCS7_ctrl_FP = (PKCS7_ctrl_TYPE)GetProcAddress(libeayHandleM, "PKCS7_ctrl")) == NULL) goto err;
//	if ((PKCS7_dataDecode_FP = (PKCS7_dataDecode_TYPE)GetProcAddress(libeayHandleM, "PKCS7_dataDecode")) == NULL) goto err;
//	if ((PKCS7_dataFinal_FP = (PKCS7_dataFinal_TYPE)GetProcAddress(libeayHandleM, "PKCS7_dataFinal")) == NULL) goto err;
//	if ((PKCS7_dataInit_FP = (PKCS7_dataInit_TYPE)GetProcAddress(libeayHandleM, "PKCS7_dataInit")) == NULL) goto err;
//	if ((PKCS7_dataVerify_FP = (PKCS7_dataVerify_TYPE)GetProcAddress(libeayHandleM, "PKCS7_dataVerify")) == NULL) goto err;
//	if ((PKCS7_decrypt_FP = (PKCS7_decrypt_TYPE)GetProcAddress(libeayHandleM, "PKCS7_decrypt")) == NULL) goto err;
//	if ((PKCS7_digest_from_attributes_FP = (PKCS7_digest_from_attributes_TYPE)GetProcAddress(libeayHandleM, "PKCS7_digest_from_attributes")) == NULL) goto err;
//	if ((PKCS7_dup_FP = (PKCS7_dup_TYPE)GetProcAddress(libeayHandleM, "PKCS7_dup")) == NULL) goto err;
//	if ((PKCS7_encrypt_FP = (PKCS7_encrypt_TYPE)GetProcAddress(libeayHandleM, "PKCS7_encrypt")) == NULL) goto err;
	if ((PKCS7_free_FP = (PKCS7_free_TYPE)GetProcAddress(libeayHandleM, "PKCS7_free")) == NULL) goto err;
//	if ((PKCS7_get0_signers_FP = (PKCS7_get0_signers_TYPE)GetProcAddress(libeayHandleM, "PKCS7_get0_signers")) == NULL) goto err;
//	if ((PKCS7_get_attribute_FP = (PKCS7_get_attribute_TYPE)GetProcAddress(libeayHandleM, "PKCS7_get_attribute")) == NULL) goto err;
//	if ((PKCS7_get_issuer_and_serial_FP = (PKCS7_get_issuer_and_serial_TYPE)GetProcAddress(libeayHandleM, "PKCS7_get_issuer_and_serial")) == NULL) goto err;
//	if ((PKCS7_get_signed_attribute_FP = (PKCS7_get_signed_attribute_TYPE)GetProcAddress(libeayHandleM, "PKCS7_get_signed_attribute")) == NULL) goto err;
//	if ((PKCS7_get_signer_info_FP = (PKCS7_get_signer_info_TYPE)GetProcAddress(libeayHandleM, "PKCS7_get_signer_info")) == NULL) goto err;
//	if ((PKCS7_get_smimecap_FP = (PKCS7_get_smimecap_TYPE)GetProcAddress(libeayHandleM, "PKCS7_get_smimecap")) == NULL) goto err;
//	if ((PKCS7_it_FP = (PKCS7_it_TYPE)GetProcAddress(libeayHandleM, "PKCS7_it")) == NULL) goto err;
//	if ((PKCS7_new_FP = (PKCS7_new_TYPE)GetProcAddress(libeayHandleM, "PKCS7_new")) == NULL) goto err;
//	if ((PKCS7_set_attributes_FP = (PKCS7_set_attributes_TYPE)GetProcAddress(libeayHandleM, "PKCS7_set_attributes")) == NULL) goto err;
//	if ((PKCS7_set_cipher_FP = (PKCS7_set_cipher_TYPE)GetProcAddress(libeayHandleM, "PKCS7_set_cipher")) == NULL) goto err;
//	if ((PKCS7_set_content_FP = (PKCS7_set_content_TYPE)GetProcAddress(libeayHandleM, "PKCS7_set_content")) == NULL) goto err;
//	if ((PKCS7_set_signed_attributes_FP = (PKCS7_set_signed_attributes_TYPE)GetProcAddress(libeayHandleM, "PKCS7_set_signed_attributes")) == NULL) goto err;
//	if ((PKCS7_set_type_FP = (PKCS7_set_type_TYPE)GetProcAddress(libeayHandleM, "PKCS7_set_type")) == NULL) goto err;
//	if ((PKCS7_sign_FP = (PKCS7_sign_TYPE)GetProcAddress(libeayHandleM, "PKCS7_sign")) == NULL) goto err;
//	if ((PKCS7_signatureVerify_FP = (PKCS7_signatureVerify_TYPE)GetProcAddress(libeayHandleM, "PKCS7_signatureVerify")) == NULL) goto err;
//	if ((PKCS7_simple_smimecap_FP = (PKCS7_simple_smimecap_TYPE)GetProcAddress(libeayHandleM, "PKCS7_simple_smimecap")) == NULL) goto err;
//	if ((PKCS7_verify_FP = (PKCS7_verify_TYPE)GetProcAddress(libeayHandleM, "PKCS7_verify")) == NULL) goto err;
	if ((PKCS8_PRIV_KEY_INFO_free_FP = (PKCS8_PRIV_KEY_INFO_free_TYPE)GetProcAddress(libeayHandleM, "PKCS8_PRIV_KEY_INFO_free")) == NULL) goto err;
//	if ((PKCS8_PRIV_KEY_INFO_it_FP = (PKCS8_PRIV_KEY_INFO_it_TYPE)GetProcAddress(libeayHandleM, "PKCS8_PRIV_KEY_INFO_it")) == NULL) goto err;
//	if ((PKCS8_PRIV_KEY_INFO_new_FP = (PKCS8_PRIV_KEY_INFO_new_TYPE)GetProcAddress(libeayHandleM, "PKCS8_PRIV_KEY_INFO_new")) == NULL) goto err;
//	if ((PKCS8_add_keyusage_FP = (PKCS8_add_keyusage_TYPE)GetProcAddress(libeayHandleM, "PKCS8_add_keyusage")) == NULL) goto err;
	if ((PKCS8_decrypt_FP = (PKCS8_decrypt_TYPE)GetProcAddress(libeayHandleM, "PKCS8_decrypt")) == NULL) goto err;
//	if ((PKCS8_encrypt_FP = (PKCS8_encrypt_TYPE)GetProcAddress(libeayHandleM, "PKCS8_encrypt")) == NULL) goto err;
//	if ((PKCS8_set_broken_FP = (PKCS8_set_broken_TYPE)GetProcAddress(libeayHandleM, "PKCS8_set_broken")) == NULL) goto err;
//	if ((PKEY_USAGE_PERIOD_free_FP = (PKEY_USAGE_PERIOD_free_TYPE)GetProcAddress(libeayHandleM, "PKEY_USAGE_PERIOD_free")) == NULL) goto err;
//	if ((PKEY_USAGE_PERIOD_it_FP = (PKEY_USAGE_PERIOD_it_TYPE)GetProcAddress(libeayHandleM, "PKEY_USAGE_PERIOD_it")) == NULL) goto err;
//	if ((PKEY_USAGE_PERIOD_new_FP = (PKEY_USAGE_PERIOD_new_TYPE)GetProcAddress(libeayHandleM, "PKEY_USAGE_PERIOD_new")) == NULL) goto err;
//	if ((POLICYINFO_free_FP = (POLICYINFO_free_TYPE)GetProcAddress(libeayHandleM, "POLICYINFO_free")) == NULL) goto err;
//	if ((POLICYINFO_it_FP = (POLICYINFO_it_TYPE)GetProcAddress(libeayHandleM, "POLICYINFO_it")) == NULL) goto err;
//	if ((POLICYINFO_new_FP = (POLICYINFO_new_TYPE)GetProcAddress(libeayHandleM, "POLICYINFO_new")) == NULL) goto err;
//	if ((POLICYQUALINFO_free_FP = (POLICYQUALINFO_free_TYPE)GetProcAddress(libeayHandleM, "POLICYQUALINFO_free")) == NULL) goto err;
//	if ((POLICYQUALINFO_it_FP = (POLICYQUALINFO_it_TYPE)GetProcAddress(libeayHandleM, "POLICYQUALINFO_it")) == NULL) goto err;
//	if ((POLICYQUALINFO_new_FP = (POLICYQUALINFO_new_TYPE)GetProcAddress(libeayHandleM, "POLICYQUALINFO_new")) == NULL) goto err;
//	if ((RAND_SSLeay_FP = (RAND_SSLeay_TYPE)GetProcAddress(libeayHandleM, "RAND_SSLeay")) == NULL) goto err;
	if ((RAND_add_FP = (RAND_add_TYPE)GetProcAddress(libeayHandleM, "RAND_add")) == NULL) goto err;
//	if ((RAND_bytes_FP = (RAND_bytes_TYPE)GetProcAddress(libeayHandleM, "RAND_bytes")) == NULL) goto err;
//	if ((RAND_cleanup_FP = (RAND_cleanup_TYPE)GetProcAddress(libeayHandleM, "RAND_cleanup")) == NULL) goto err;
//	if ((RAND_egd_FP = (RAND_egd_TYPE)GetProcAddress(libeayHandleM, "RAND_egd")) == NULL) goto err;
//	if ((RAND_egd_bytes_FP = (RAND_egd_bytes_TYPE)GetProcAddress(libeayHandleM, "RAND_egd_bytes")) == NULL) goto err;
//	if ((RAND_event_FP = (RAND_event_TYPE)GetProcAddress(libeayHandleM, "RAND_event")) == NULL) goto err;
	if ((RAND_file_name_FP = (RAND_file_name_TYPE)GetProcAddress(libeayHandleM, "RAND_file_name")) == NULL) goto err;
//	if ((RAND_get_rand_method_FP = (RAND_get_rand_method_TYPE)GetProcAddress(libeayHandleM, "RAND_get_rand_method")) == NULL) goto err;
	if ((RAND_load_file_FP = (RAND_load_file_TYPE)GetProcAddress(libeayHandleM, "RAND_load_file")) == NULL) goto err;
//	if ((RAND_poll_FP = (RAND_poll_TYPE)GetProcAddress(libeayHandleM, "RAND_poll")) == NULL) goto err;
//	if ((RAND_pseudo_bytes_FP = (RAND_pseudo_bytes_TYPE)GetProcAddress(libeayHandleM, "RAND_pseudo_bytes")) == NULL) goto err;
//	if ((RAND_query_egd_bytes_FP = (RAND_query_egd_bytes_TYPE)GetProcAddress(libeayHandleM, "RAND_query_egd_bytes")) == NULL) goto err;
//	if ((RAND_screen_FP = (RAND_screen_TYPE)GetProcAddress(libeayHandleM, "RAND_screen")) == NULL) goto err;
	if ((RAND_seed_FP = (RAND_seed_TYPE)GetProcAddress(libeayHandleM, "RAND_seed")) == NULL) goto err;
//	if ((RAND_set_rand_method_FP = (RAND_set_rand_method_TYPE)GetProcAddress(libeayHandleM, "RAND_set_rand_method")) == NULL) goto err;
	if ((RAND_status_FP = (RAND_status_TYPE)GetProcAddress(libeayHandleM, "RAND_status")) == NULL) goto err;
	if ((RAND_write_file_FP = (RAND_write_file_TYPE)GetProcAddress(libeayHandleM, "RAND_write_file")) == NULL) goto err;
//	if ((RC2_cbc_encrypt_FP = (RC2_cbc_encrypt_TYPE)GetProcAddress(libeayHandleM, "RC2_cbc_encrypt")) == NULL) goto err;
//	if ((RC2_cfb64_encrypt_FP = (RC2_cfb64_encrypt_TYPE)GetProcAddress(libeayHandleM, "RC2_cfb64_encrypt")) == NULL) goto err;
//	if ((RC2_decrypt_FP = (RC2_decrypt_TYPE)GetProcAddress(libeayHandleM, "RC2_decrypt")) == NULL) goto err;
//	if ((RC2_ecb_encrypt_FP = (RC2_ecb_encrypt_TYPE)GetProcAddress(libeayHandleM, "RC2_ecb_encrypt")) == NULL) goto err;
//	if ((RC2_encrypt_FP = (RC2_encrypt_TYPE)GetProcAddress(libeayHandleM, "RC2_encrypt")) == NULL) goto err;
//	if ((RC2_ofb64_encrypt_FP = (RC2_ofb64_encrypt_TYPE)GetProcAddress(libeayHandleM, "RC2_ofb64_encrypt")) == NULL) goto err;
//	if ((RC2_set_key_FP = (RC2_set_key_TYPE)GetProcAddress(libeayHandleM, "RC2_set_key")) == NULL) goto err;
//	if ((RC4_FP = (RC4_TYPE)GetProcAddress(libeayHandleM, "RC4")) == NULL) goto err;
//	if ((RC4_options_FP = (RC4_options_TYPE)GetProcAddress(libeayHandleM, "RC4_options")) == NULL) goto err;
//	if ((RC4_set_key_FP = (RC4_set_key_TYPE)GetProcAddress(libeayHandleM, "RC4_set_key")) == NULL) goto err;
//	if ((RIPEMD160_FP = (RIPEMD160_TYPE)GetProcAddress(libeayHandleM, "RIPEMD160")) == NULL) goto err;
//	if ((RIPEMD160_Final_FP = (RIPEMD160_Final_TYPE)GetProcAddress(libeayHandleM, "RIPEMD160_Final")) == NULL) goto err;
//	if ((RIPEMD160_Init_FP = (RIPEMD160_Init_TYPE)GetProcAddress(libeayHandleM, "RIPEMD160_Init")) == NULL) goto err;
//	if ((RIPEMD160_Transform_FP = (RIPEMD160_Transform_TYPE)GetProcAddress(libeayHandleM, "RIPEMD160_Transform")) == NULL) goto err;
//	if ((RIPEMD160_Update_FP = (RIPEMD160_Update_TYPE)GetProcAddress(libeayHandleM, "RIPEMD160_Update")) == NULL) goto err;
//	if ((RSAPrivateKey_asn1_meth_FP = (RSAPrivateKey_asn1_meth_TYPE)GetProcAddress(libeayHandleM, "RSAPrivateKey_asn1_meth")) == NULL) goto err;
//	if ((RSAPrivateKey_dup_FP = (RSAPrivateKey_dup_TYPE)GetProcAddress(libeayHandleM, "RSAPrivateKey_dup")) == NULL) goto err;
//	if ((RSAPrivateKey_it_FP = (RSAPrivateKey_it_TYPE)GetProcAddress(libeayHandleM, "RSAPrivateKey_it")) == NULL) goto err;
//	if ((RSAPublicKey_dup_FP = (RSAPublicKey_dup_TYPE)GetProcAddress(libeayHandleM, "RSAPublicKey_dup")) == NULL) goto err;
//	if ((RSAPublicKey_it_FP = (RSAPublicKey_it_TYPE)GetProcAddress(libeayHandleM, "RSAPublicKey_it")) == NULL) goto err;
//	if ((RSA_PKCS1_SSLeay_FP = (RSA_PKCS1_SSLeay_TYPE)GetProcAddress(libeayHandleM, "RSA_PKCS1_SSLeay")) == NULL) goto err;
//	if ((RSA_blinding_off_FP = (RSA_blinding_off_TYPE)GetProcAddress(libeayHandleM, "RSA_blinding_off")) == NULL) goto err;
//	if ((RSA_blinding_on_FP = (RSA_blinding_on_TYPE)GetProcAddress(libeayHandleM, "RSA_blinding_on")) == NULL) goto err;
//	if ((RSA_check_key_FP = (RSA_check_key_TYPE)GetProcAddress(libeayHandleM, "RSA_check_key")) == NULL) goto err;
//	if ((RSA_flags_FP = (RSA_flags_TYPE)GetProcAddress(libeayHandleM, "RSA_flags")) == NULL) goto err;
	if ((RSA_free_FP = (RSA_free_TYPE)GetProcAddress(libeayHandleM, "RSA_free")) == NULL) goto err;
	if ((RSA_generate_key_FP = (RSA_generate_key_TYPE)GetProcAddress(libeayHandleM, "RSA_generate_key")) == NULL) goto err;
//	if ((RSA_get_default_method_FP = (RSA_get_default_method_TYPE)GetProcAddress(libeayHandleM, "RSA_get_default_method")) == NULL) goto err;
//	if ((RSA_get_ex_data_FP = (RSA_get_ex_data_TYPE)GetProcAddress(libeayHandleM, "RSA_get_ex_data")) == NULL) goto err;
//	if ((RSA_get_ex_new_index_FP = (RSA_get_ex_new_index_TYPE)GetProcAddress(libeayHandleM, "RSA_get_ex_new_index")) == NULL) goto err;
//	if ((RSA_get_method_FP = (RSA_get_method_TYPE)GetProcAddress(libeayHandleM, "RSA_get_method")) == NULL) goto err;
//	if ((RSA_memory_lock_FP = (RSA_memory_lock_TYPE)GetProcAddress(libeayHandleM, "RSA_memory_lock")) == NULL) goto err;
//	if ((RSA_new_FP = (RSA_new_TYPE)GetProcAddress(libeayHandleM, "RSA_new")) == NULL) goto err;
//	if ((RSA_new_method_FP = (RSA_new_method_TYPE)GetProcAddress(libeayHandleM, "RSA_new_method")) == NULL) goto err;
//	if ((RSA_null_method_FP = (RSA_null_method_TYPE)GetProcAddress(libeayHandleM, "RSA_null_method")) == NULL) goto err;
//	if ((RSA_padding_add_PKCS1_OAEP_FP = (RSA_padding_add_PKCS1_OAEP_TYPE)GetProcAddress(libeayHandleM, "RSA_padding_add_PKCS1_OAEP")) == NULL) goto err;
//	if ((RSA_padding_add_PKCS1_type_1_FP = (RSA_padding_add_PKCS1_type_1_TYPE)GetProcAddress(libeayHandleM, "RSA_padding_add_PKCS1_type_1")) == NULL) goto err;
//	if ((RSA_padding_add_PKCS1_type_2_FP = (RSA_padding_add_PKCS1_type_2_TYPE)GetProcAddress(libeayHandleM, "RSA_padding_add_PKCS1_type_2")) == NULL) goto err;
//	if ((RSA_padding_add_SSLv23_FP = (RSA_padding_add_SSLv23_TYPE)GetProcAddress(libeayHandleM, "RSA_padding_add_SSLv23")) == NULL) goto err;
//	if ((RSA_padding_add_none_FP = (RSA_padding_add_none_TYPE)GetProcAddress(libeayHandleM, "RSA_padding_add_none")) == NULL) goto err;
//	if ((RSA_padding_check_PKCS1_OAEP_FP = (RSA_padding_check_PKCS1_OAEP_TYPE)GetProcAddress(libeayHandleM, "RSA_padding_check_PKCS1_OAEP")) == NULL) goto err;
//	if ((RSA_padding_check_PKCS1_type_1_FP = (RSA_padding_check_PKCS1_type_1_TYPE)GetProcAddress(libeayHandleM, "RSA_padding_check_PKCS1_type_1")) == NULL) goto err;
//	if ((RSA_padding_check_PKCS1_type_2_FP = (RSA_padding_check_PKCS1_type_2_TYPE)GetProcAddress(libeayHandleM, "RSA_padding_check_PKCS1_type_2")) == NULL) goto err;
//	if ((RSA_padding_check_SSLv23_FP = (RSA_padding_check_SSLv23_TYPE)GetProcAddress(libeayHandleM, "RSA_padding_check_SSLv23")) == NULL) goto err;
//	if ((RSA_padding_check_none_FP = (RSA_padding_check_none_TYPE)GetProcAddress(libeayHandleM, "RSA_padding_check_none")) == NULL) goto err;
//	if ((RSA_print_FP = (RSA_print_TYPE)GetProcAddress(libeayHandleM, "RSA_print")) == NULL) goto err;
//	if ((RSA_print_fp_FP = (RSA_print_fp_TYPE)GetProcAddress(libeayHandleM, "RSA_print_fp")) == NULL) goto err;
//	if ((RSA_private_decrypt_FP = (RSA_private_decrypt_TYPE)GetProcAddress(libeayHandleM, "RSA_private_decrypt")) == NULL) goto err;
//	if ((RSA_private_encrypt_FP = (RSA_private_encrypt_TYPE)GetProcAddress(libeayHandleM, "RSA_private_encrypt")) == NULL) goto err;
//	if ((RSA_public_decrypt_FP = (RSA_public_decrypt_TYPE)GetProcAddress(libeayHandleM, "RSA_public_decrypt")) == NULL) goto err;
//	if ((RSA_public_encrypt_FP = (RSA_public_encrypt_TYPE)GetProcAddress(libeayHandleM, "RSA_public_encrypt")) == NULL) goto err;
//	if ((RSA_set_default_method_FP = (RSA_set_default_method_TYPE)GetProcAddress(libeayHandleM, "RSA_set_default_method")) == NULL) goto err;
//	if ((RSA_set_ex_data_FP = (RSA_set_ex_data_TYPE)GetProcAddress(libeayHandleM, "RSA_set_ex_data")) == NULL) goto err;
//	if ((RSA_set_method_FP = (RSA_set_method_TYPE)GetProcAddress(libeayHandleM, "RSA_set_method")) == NULL) goto err;
//	if ((RSA_sign_FP = (RSA_sign_TYPE)GetProcAddress(libeayHandleM, "RSA_sign")) == NULL) goto err;
//	if ((RSA_sign_ASN1_OCTET_STRING_FP = (RSA_sign_ASN1_OCTET_STRING_TYPE)GetProcAddress(libeayHandleM, "RSA_sign_ASN1_OCTET_STRING")) == NULL) goto err;
//	if ((RSA_size_FP = (RSA_size_TYPE)GetProcAddress(libeayHandleM, "RSA_size")) == NULL) goto err;
//	if ((RSA_up_ref_FP = (RSA_up_ref_TYPE)GetProcAddress(libeayHandleM, "RSA_up_ref")) == NULL) goto err;
//	if ((RSA_verify_FP = (RSA_verify_TYPE)GetProcAddress(libeayHandleM, "RSA_verify")) == NULL) goto err;
//	if ((RSA_verify_ASN1_OCTET_STRING_FP = (RSA_verify_ASN1_OCTET_STRING_TYPE)GetProcAddress(libeayHandleM, "RSA_verify_ASN1_OCTET_STRING")) == NULL) goto err;
//	if ((SHA1_FP = (SHA1_TYPE)GetProcAddress(libeayHandleM, "SHA1")) == NULL) goto err;
//	if ((SHA1_Final_FP = (SHA1_Final_TYPE)GetProcAddress(libeayHandleM, "SHA1_Final")) == NULL) goto err;
//	if ((SHA1_Init_FP = (SHA1_Init_TYPE)GetProcAddress(libeayHandleM, "SHA1_Init")) == NULL) goto err;
//	if ((SHA1_Transform_FP = (SHA1_Transform_TYPE)GetProcAddress(libeayHandleM, "SHA1_Transform")) == NULL) goto err;
//	if ((SHA1_Update_FP = (SHA1_Update_TYPE)GetProcAddress(libeayHandleM, "SHA1_Update")) == NULL) goto err;
//	if ((SHA_FP = (SHA_TYPE)GetProcAddress(libeayHandleM, "SHA")) == NULL) goto err;
//	if ((SHA_Final_FP = (SHA_Final_TYPE)GetProcAddress(libeayHandleM, "SHA_Final")) == NULL) goto err;
//	if ((SHA_Init_FP = (SHA_Init_TYPE)GetProcAddress(libeayHandleM, "SHA_Init")) == NULL) goto err;
//	if ((SHA_Transform_FP = (SHA_Transform_TYPE)GetProcAddress(libeayHandleM, "SHA_Transform")) == NULL) goto err;
//	if ((SHA_Update_FP = (SHA_Update_TYPE)GetProcAddress(libeayHandleM, "SHA_Update")) == NULL) goto err;
//	if ((SMIME_crlf_copy_FP = (SMIME_crlf_copy_TYPE)GetProcAddress(libeayHandleM, "SMIME_crlf_copy")) == NULL) goto err;
//	if ((SMIME_read_PKCS7_FP = (SMIME_read_PKCS7_TYPE)GetProcAddress(libeayHandleM, "SMIME_read_PKCS7")) == NULL) goto err;
//	if ((SMIME_text_FP = (SMIME_text_TYPE)GetProcAddress(libeayHandleM, "SMIME_text")) == NULL) goto err;
//	if ((SMIME_write_PKCS7_FP = (SMIME_write_PKCS7_TYPE)GetProcAddress(libeayHandleM, "SMIME_write_PKCS7")) == NULL) goto err;
//	if ((SSLeay_version_FP = (SSLeay_version_TYPE)GetProcAddress(libeayHandleM, "SSLeay_version")) == NULL) goto err;
//	if ((SXNETID_free_FP = (SXNETID_free_TYPE)GetProcAddress(libeayHandleM, "SXNETID_free")) == NULL) goto err;
//	if ((SXNETID_it_FP = (SXNETID_it_TYPE)GetProcAddress(libeayHandleM, "SXNETID_it")) == NULL) goto err;
//	if ((SXNETID_new_FP = (SXNETID_new_TYPE)GetProcAddress(libeayHandleM, "SXNETID_new")) == NULL) goto err;
//	if ((SXNET_add_id_INTEGER_FP = (SXNET_add_id_INTEGER_TYPE)GetProcAddress(libeayHandleM, "SXNET_add_id_INTEGER")) == NULL) goto err;
//	if ((SXNET_add_id_asc_FP = (SXNET_add_id_asc_TYPE)GetProcAddress(libeayHandleM, "SXNET_add_id_asc")) == NULL) goto err;
//	if ((SXNET_add_id_ulong_FP = (SXNET_add_id_ulong_TYPE)GetProcAddress(libeayHandleM, "SXNET_add_id_ulong")) == NULL) goto err;
//	if ((SXNET_free_FP = (SXNET_free_TYPE)GetProcAddress(libeayHandleM, "SXNET_free")) == NULL) goto err;
//	if ((SXNET_get_id_INTEGER_FP = (SXNET_get_id_INTEGER_TYPE)GetProcAddress(libeayHandleM, "SXNET_get_id_INTEGER")) == NULL) goto err;
//	if ((SXNET_get_id_asc_FP = (SXNET_get_id_asc_TYPE)GetProcAddress(libeayHandleM, "SXNET_get_id_asc")) == NULL) goto err;
//	if ((SXNET_get_id_ulong_FP = (SXNET_get_id_ulong_TYPE)GetProcAddress(libeayHandleM, "SXNET_get_id_ulong")) == NULL) goto err;
//	if ((SXNET_it_FP = (SXNET_it_TYPE)GetProcAddress(libeayHandleM, "SXNET_it")) == NULL) goto err;
//	if ((SXNET_new_FP = (SXNET_new_TYPE)GetProcAddress(libeayHandleM, "SXNET_new")) == NULL) goto err;
//	if ((TXT_DB_create_index_FP = (TXT_DB_create_index_TYPE)GetProcAddress(libeayHandleM, "TXT_DB_create_index")) == NULL) goto err;
//	if ((TXT_DB_free_FP = (TXT_DB_free_TYPE)GetProcAddress(libeayHandleM, "TXT_DB_free")) == NULL) goto err;
//	if ((TXT_DB_get_by_index_FP = (TXT_DB_get_by_index_TYPE)GetProcAddress(libeayHandleM, "TXT_DB_get_by_index")) == NULL) goto err;
//	if ((TXT_DB_insert_FP = (TXT_DB_insert_TYPE)GetProcAddress(libeayHandleM, "TXT_DB_insert")) == NULL) goto err;
//	if ((TXT_DB_read_FP = (TXT_DB_read_TYPE)GetProcAddress(libeayHandleM, "TXT_DB_read")) == NULL) goto err;
//	if ((TXT_DB_write_FP = (TXT_DB_write_TYPE)GetProcAddress(libeayHandleM, "TXT_DB_write")) == NULL) goto err;
//	if ((UI_OpenSSL_FP = (UI_OpenSSL_TYPE)GetProcAddress(libeayHandleM, "UI_OpenSSL")) == NULL) goto err;
//	if ((UI_UTIL_read_pw_FP = (UI_UTIL_read_pw_TYPE)GetProcAddress(libeayHandleM, "UI_UTIL_read_pw")) == NULL) goto err;
//	if ((UI_UTIL_read_pw_string_FP = (UI_UTIL_read_pw_string_TYPE)GetProcAddress(libeayHandleM, "UI_UTIL_read_pw_string")) == NULL) goto err;
//	if ((UI_add_error_string_FP = (UI_add_error_string_TYPE)GetProcAddress(libeayHandleM, "UI_add_error_string")) == NULL) goto err;
//	if ((UI_add_info_string_FP = (UI_add_info_string_TYPE)GetProcAddress(libeayHandleM, "UI_add_info_string")) == NULL) goto err;
//	if ((UI_add_input_boolean_FP = (UI_add_input_boolean_TYPE)GetProcAddress(libeayHandleM, "UI_add_input_boolean")) == NULL) goto err;
//	if ((UI_add_input_string_FP = (UI_add_input_string_TYPE)GetProcAddress(libeayHandleM, "UI_add_input_string")) == NULL) goto err;
//	if ((UI_add_user_data_FP = (UI_add_user_data_TYPE)GetProcAddress(libeayHandleM, "UI_add_user_data")) == NULL) goto err;
//	if ((UI_add_verify_string_FP = (UI_add_verify_string_TYPE)GetProcAddress(libeayHandleM, "UI_add_verify_string")) == NULL) goto err;
//	if ((UI_construct_prompt_FP = (UI_construct_prompt_TYPE)GetProcAddress(libeayHandleM, "UI_construct_prompt")) == NULL) goto err;
//	if ((UI_create_method_FP = (UI_create_method_TYPE)GetProcAddress(libeayHandleM, "UI_create_method")) == NULL) goto err;
//	if ((UI_ctrl_FP = (UI_ctrl_TYPE)GetProcAddress(libeayHandleM, "UI_ctrl")) == NULL) goto err;
//	if ((UI_destroy_method_FP = (UI_destroy_method_TYPE)GetProcAddress(libeayHandleM, "UI_destroy_method")) == NULL) goto err;
//	if ((UI_dup_error_string_FP = (UI_dup_error_string_TYPE)GetProcAddress(libeayHandleM, "UI_dup_error_string")) == NULL) goto err;
//	if ((UI_dup_info_string_FP = (UI_dup_info_string_TYPE)GetProcAddress(libeayHandleM, "UI_dup_info_string")) == NULL) goto err;
//	if ((UI_dup_input_boolean_FP = (UI_dup_input_boolean_TYPE)GetProcAddress(libeayHandleM, "UI_dup_input_boolean")) == NULL) goto err;
//	if ((UI_dup_input_string_FP = (UI_dup_input_string_TYPE)GetProcAddress(libeayHandleM, "UI_dup_input_string")) == NULL) goto err;
//	if ((UI_dup_verify_string_FP = (UI_dup_verify_string_TYPE)GetProcAddress(libeayHandleM, "UI_dup_verify_string")) == NULL) goto err;
//	if ((UI_free_FP = (UI_free_TYPE)GetProcAddress(libeayHandleM, "UI_free")) == NULL) goto err;
//	if ((UI_get0_action_string_FP = (UI_get0_action_string_TYPE)GetProcAddress(libeayHandleM, "UI_get0_action_string")) == NULL) goto err;
//	if ((UI_get0_output_string_FP = (UI_get0_output_string_TYPE)GetProcAddress(libeayHandleM, "UI_get0_output_string")) == NULL) goto err;
//	if ((UI_get0_result_FP = (UI_get0_result_TYPE)GetProcAddress(libeayHandleM, "UI_get0_result")) == NULL) goto err;
//	if ((UI_get0_result_string_FP = (UI_get0_result_string_TYPE)GetProcAddress(libeayHandleM, "UI_get0_result_string")) == NULL) goto err;
//	if ((UI_get0_test_string_FP = (UI_get0_test_string_TYPE)GetProcAddress(libeayHandleM, "UI_get0_test_string")) == NULL) goto err;
//	if ((UI_get0_user_data_FP = (UI_get0_user_data_TYPE)GetProcAddress(libeayHandleM, "UI_get0_user_data")) == NULL) goto err;
//	if ((UI_get_default_method_FP = (UI_get_default_method_TYPE)GetProcAddress(libeayHandleM, "UI_get_default_method")) == NULL) goto err;
//	if ((UI_get_ex_data_FP = (UI_get_ex_data_TYPE)GetProcAddress(libeayHandleM, "UI_get_ex_data")) == NULL) goto err;
//	if ((UI_get_ex_new_index_FP = (UI_get_ex_new_index_TYPE)GetProcAddress(libeayHandleM, "UI_get_ex_new_index")) == NULL) goto err;
//	if ((UI_get_input_flags_FP = (UI_get_input_flags_TYPE)GetProcAddress(libeayHandleM, "UI_get_input_flags")) == NULL) goto err;
//	if ((UI_get_method_FP = (UI_get_method_TYPE)GetProcAddress(libeayHandleM, "UI_get_method")) == NULL) goto err;
//	if ((UI_get_result_maxsize_FP = (UI_get_result_maxsize_TYPE)GetProcAddress(libeayHandleM, "UI_get_result_maxsize")) == NULL) goto err;
//	if ((UI_get_result_minsize_FP = (UI_get_result_minsize_TYPE)GetProcAddress(libeayHandleM, "UI_get_result_minsize")) == NULL) goto err;
//	if ((UI_get_string_type_FP = (UI_get_string_type_TYPE)GetProcAddress(libeayHandleM, "UI_get_string_type")) == NULL) goto err;
//	if ((UI_method_get_closer_FP = (UI_method_get_closer_TYPE)GetProcAddress(libeayHandleM, "UI_method_get_closer")) == NULL) goto err;
//	if ((UI_method_get_flusher_FP = (UI_method_get_flusher_TYPE)GetProcAddress(libeayHandleM, "UI_method_get_flusher")) == NULL) goto err;
//	if ((UI_method_get_opener_FP = (UI_method_get_opener_TYPE)GetProcAddress(libeayHandleM, "UI_method_get_opener")) == NULL) goto err;
//	if ((UI_method_get_reader_FP = (UI_method_get_reader_TYPE)GetProcAddress(libeayHandleM, "UI_method_get_reader")) == NULL) goto err;
//	if ((UI_method_get_writer_FP = (UI_method_get_writer_TYPE)GetProcAddress(libeayHandleM, "UI_method_get_writer")) == NULL) goto err;
//	if ((UI_method_set_closer_FP = (UI_method_set_closer_TYPE)GetProcAddress(libeayHandleM, "UI_method_set_closer")) == NULL) goto err;
//	if ((UI_method_set_flusher_FP = (UI_method_set_flusher_TYPE)GetProcAddress(libeayHandleM, "UI_method_set_flusher")) == NULL) goto err;
//	if ((UI_method_set_opener_FP = (UI_method_set_opener_TYPE)GetProcAddress(libeayHandleM, "UI_method_set_opener")) == NULL) goto err;
//	if ((UI_method_set_reader_FP = (UI_method_set_reader_TYPE)GetProcAddress(libeayHandleM, "UI_method_set_reader")) == NULL) goto err;
//	if ((UI_method_set_writer_FP = (UI_method_set_writer_TYPE)GetProcAddress(libeayHandleM, "UI_method_set_writer")) == NULL) goto err;
//	if ((UI_new_FP = (UI_new_TYPE)GetProcAddress(libeayHandleM, "UI_new")) == NULL) goto err;
//	if ((UI_new_method_FP = (UI_new_method_TYPE)GetProcAddress(libeayHandleM, "UI_new_method")) == NULL) goto err;
//	if ((UI_process_FP = (UI_process_TYPE)GetProcAddress(libeayHandleM, "UI_process")) == NULL) goto err;
//	if ((UI_set_default_method_FP = (UI_set_default_method_TYPE)GetProcAddress(libeayHandleM, "UI_set_default_method")) == NULL) goto err;
//	if ((UI_set_ex_data_FP = (UI_set_ex_data_TYPE)GetProcAddress(libeayHandleM, "UI_set_ex_data")) == NULL) goto err;
//	if ((UI_set_method_FP = (UI_set_method_TYPE)GetProcAddress(libeayHandleM, "UI_set_method")) == NULL) goto err;
//	if ((UI_set_result_FP = (UI_set_result_TYPE)GetProcAddress(libeayHandleM, "UI_set_result")) == NULL) goto err;
//	if ((USERNOTICE_free_FP = (USERNOTICE_free_TYPE)GetProcAddress(libeayHandleM, "USERNOTICE_free")) == NULL) goto err;
//	if ((USERNOTICE_it_FP = (USERNOTICE_it_TYPE)GetProcAddress(libeayHandleM, "USERNOTICE_it")) == NULL) goto err;
//	if ((USERNOTICE_new_FP = (USERNOTICE_new_TYPE)GetProcAddress(libeayHandleM, "USERNOTICE_new")) == NULL) goto err;
//	if ((UTF8_getc_FP = (UTF8_getc_TYPE)GetProcAddress(libeayHandleM, "UTF8_getc")) == NULL) goto err;
//	if ((UTF8_putc_FP = (UTF8_putc_TYPE)GetProcAddress(libeayHandleM, "UTF8_putc")) == NULL) goto err;
//	if ((X509V3_EXT_CRL_add_conf_FP = (X509V3_EXT_CRL_add_conf_TYPE)GetProcAddress(libeayHandleM, "X509V3_EXT_CRL_add_conf")) == NULL) goto err;
//	if ((X509V3_EXT_CRL_add_nconf_FP = (X509V3_EXT_CRL_add_nconf_TYPE)GetProcAddress(libeayHandleM, "X509V3_EXT_CRL_add_nconf")) == NULL) goto err;
//	if ((X509V3_EXT_REQ_add_conf_FP = (X509V3_EXT_REQ_add_conf_TYPE)GetProcAddress(libeayHandleM, "X509V3_EXT_REQ_add_conf")) == NULL) goto err;
//	if ((X509V3_EXT_REQ_add_nconf_FP = (X509V3_EXT_REQ_add_nconf_TYPE)GetProcAddress(libeayHandleM, "X509V3_EXT_REQ_add_nconf")) == NULL) goto err;
//	if ((X509V3_EXT_add_FP = (X509V3_EXT_add_TYPE)GetProcAddress(libeayHandleM, "X509V3_EXT_add")) == NULL) goto err;
//	if ((X509V3_EXT_add_alias_FP = (X509V3_EXT_add_alias_TYPE)GetProcAddress(libeayHandleM, "X509V3_EXT_add_alias")) == NULL) goto err;
//	if ((X509V3_EXT_add_conf_FP = (X509V3_EXT_add_conf_TYPE)GetProcAddress(libeayHandleM, "X509V3_EXT_add_conf")) == NULL) goto err;
//	if ((X509V3_EXT_add_list_FP = (X509V3_EXT_add_list_TYPE)GetProcAddress(libeayHandleM, "X509V3_EXT_add_list")) == NULL) goto err;
//	if ((X509V3_EXT_add_nconf_FP = (X509V3_EXT_add_nconf_TYPE)GetProcAddress(libeayHandleM, "X509V3_EXT_add_nconf")) == NULL) goto err;
//	if ((X509V3_EXT_add_nconf_sk_FP = (X509V3_EXT_add_nconf_sk_TYPE)GetProcAddress(libeayHandleM, "X509V3_EXT_add_nconf_sk")) == NULL) goto err;
//	if ((X509V3_EXT_cleanup_FP = (X509V3_EXT_cleanup_TYPE)GetProcAddress(libeayHandleM, "X509V3_EXT_cleanup")) == NULL) goto err;
//	if ((X509V3_EXT_conf_FP = (X509V3_EXT_conf_TYPE)GetProcAddress(libeayHandleM, "X509V3_EXT_conf")) == NULL) goto err;
//	if ((X509V3_EXT_conf_nid_FP = (X509V3_EXT_conf_nid_TYPE)GetProcAddress(libeayHandleM, "X509V3_EXT_conf_nid")) == NULL) goto err;
//	if ((X509V3_EXT_d2i_FP = (X509V3_EXT_d2i_TYPE)GetProcAddress(libeayHandleM, "X509V3_EXT_d2i")) == NULL) goto err;
//	if ((X509V3_EXT_get_FP = (X509V3_EXT_get_TYPE)GetProcAddress(libeayHandleM, "X509V3_EXT_get")) == NULL) goto err;
//	if ((X509V3_EXT_get_nid_FP = (X509V3_EXT_get_nid_TYPE)GetProcAddress(libeayHandleM, "X509V3_EXT_get_nid")) == NULL) goto err;
//	if ((X509V3_EXT_i2d_FP = (X509V3_EXT_i2d_TYPE)GetProcAddress(libeayHandleM, "X509V3_EXT_i2d")) == NULL) goto err;
//	if ((X509V3_EXT_nconf_FP = (X509V3_EXT_nconf_TYPE)GetProcAddress(libeayHandleM, "X509V3_EXT_nconf")) == NULL) goto err;
//	if ((X509V3_EXT_nconf_nid_FP = (X509V3_EXT_nconf_nid_TYPE)GetProcAddress(libeayHandleM, "X509V3_EXT_nconf_nid")) == NULL) goto err;
//	if ((X509V3_EXT_print_FP = (X509V3_EXT_print_TYPE)GetProcAddress(libeayHandleM, "X509V3_EXT_print")) == NULL) goto err;
//	if ((X509V3_EXT_print_fp_FP = (X509V3_EXT_print_fp_TYPE)GetProcAddress(libeayHandleM, "X509V3_EXT_print_fp")) == NULL) goto err;
//	if ((X509V3_EXT_val_prn_FP = (X509V3_EXT_val_prn_TYPE)GetProcAddress(libeayHandleM, "X509V3_EXT_val_prn")) == NULL) goto err;
//	if ((X509V3_add1_i2d_FP = (X509V3_add1_i2d_TYPE)GetProcAddress(libeayHandleM, "X509V3_add1_i2d")) == NULL) goto err;
//	if ((X509V3_add_standard_extensions_FP = (X509V3_add_standard_extensions_TYPE)GetProcAddress(libeayHandleM, "X509V3_add_standard_extensions")) == NULL) goto err;
//	if ((X509V3_add_value_FP = (X509V3_add_value_TYPE)GetProcAddress(libeayHandleM, "X509V3_add_value")) == NULL) goto err;
//	if ((X509V3_add_value_bool_FP = (X509V3_add_value_bool_TYPE)GetProcAddress(libeayHandleM, "X509V3_add_value_bool")) == NULL) goto err;
//	if ((X509V3_add_value_bool_nf_FP = (X509V3_add_value_bool_nf_TYPE)GetProcAddress(libeayHandleM, "X509V3_add_value_bool_nf")) == NULL) goto err;
//	if ((X509V3_add_value_int_FP = (X509V3_add_value_int_TYPE)GetProcAddress(libeayHandleM, "X509V3_add_value_int")) == NULL) goto err;
//	if ((X509V3_add_value_uchar_FP = (X509V3_add_value_uchar_TYPE)GetProcAddress(libeayHandleM, "X509V3_add_value_uchar")) == NULL) goto err;
//	if ((X509V3_conf_free_FP = (X509V3_conf_free_TYPE)GetProcAddress(libeayHandleM, "X509V3_conf_free")) == NULL) goto err;
//	if ((X509V3_extensions_print_FP = (X509V3_extensions_print_TYPE)GetProcAddress(libeayHandleM, "X509V3_extensions_print")) == NULL) goto err;
//	if ((X509V3_get_d2i_FP = (X509V3_get_d2i_TYPE)GetProcAddress(libeayHandleM, "X509V3_get_d2i")) == NULL) goto err;
//	if ((X509V3_get_section_FP = (X509V3_get_section_TYPE)GetProcAddress(libeayHandleM, "X509V3_get_section")) == NULL) goto err;
//	if ((X509V3_get_string_FP = (X509V3_get_string_TYPE)GetProcAddress(libeayHandleM, "X509V3_get_string")) == NULL) goto err;
//	if ((X509V3_get_value_bool_FP = (X509V3_get_value_bool_TYPE)GetProcAddress(libeayHandleM, "X509V3_get_value_bool")) == NULL) goto err;
//	if ((X509V3_get_value_int_FP = (X509V3_get_value_int_TYPE)GetProcAddress(libeayHandleM, "X509V3_get_value_int")) == NULL) goto err;
//	if ((X509V3_parse_list_FP = (X509V3_parse_list_TYPE)GetProcAddress(libeayHandleM, "X509V3_parse_list")) == NULL) goto err;
//	if ((X509V3_section_free_FP = (X509V3_section_free_TYPE)GetProcAddress(libeayHandleM, "X509V3_section_free")) == NULL) goto err;
//	if ((X509V3_set_conf_lhash_FP = (X509V3_set_conf_lhash_TYPE)GetProcAddress(libeayHandleM, "X509V3_set_conf_lhash")) == NULL) goto err;
//	if ((X509V3_set_ctx_FP = (X509V3_set_ctx_TYPE)GetProcAddress(libeayHandleM, "X509V3_set_ctx")) == NULL) goto err;
//	if ((X509V3_set_nconf_FP = (X509V3_set_nconf_TYPE)GetProcAddress(libeayHandleM, "X509V3_set_nconf")) == NULL) goto err;
//	if ((X509V3_string_free_FP = (X509V3_string_free_TYPE)GetProcAddress(libeayHandleM, "X509V3_string_free")) == NULL) goto err;
//	if ((X509_ALGOR_dup_FP = (X509_ALGOR_dup_TYPE)GetProcAddress(libeayHandleM, "X509_ALGOR_dup")) == NULL) goto err;
//	if ((X509_ALGOR_free_FP = (X509_ALGOR_free_TYPE)GetProcAddress(libeayHandleM, "X509_ALGOR_free")) == NULL) goto err;
//	if ((X509_ALGOR_it_FP = (X509_ALGOR_it_TYPE)GetProcAddress(libeayHandleM, "X509_ALGOR_it")) == NULL) goto err;
//	if ((X509_ALGOR_new_FP = (X509_ALGOR_new_TYPE)GetProcAddress(libeayHandleM, "X509_ALGOR_new")) == NULL) goto err;
//	if ((X509_ATTRIBUTE_count_FP = (X509_ATTRIBUTE_count_TYPE)GetProcAddress(libeayHandleM, "X509_ATTRIBUTE_count")) == NULL) goto err;
//	if ((X509_ATTRIBUTE_create_FP = (X509_ATTRIBUTE_create_TYPE)GetProcAddress(libeayHandleM, "X509_ATTRIBUTE_create")) == NULL) goto err;
//	if ((X509_ATTRIBUTE_create_by_NID_FP = (X509_ATTRIBUTE_create_by_NID_TYPE)GetProcAddress(libeayHandleM, "X509_ATTRIBUTE_create_by_NID")) == NULL) goto err;
//	if ((X509_ATTRIBUTE_create_by_OBJ_FP = (X509_ATTRIBUTE_create_by_OBJ_TYPE)GetProcAddress(libeayHandleM, "X509_ATTRIBUTE_create_by_OBJ")) == NULL) goto err;
//	if ((X509_ATTRIBUTE_create_by_txt_FP = (X509_ATTRIBUTE_create_by_txt_TYPE)GetProcAddress(libeayHandleM, "X509_ATTRIBUTE_create_by_txt")) == NULL) goto err;
//	if ((X509_ATTRIBUTE_dup_FP = (X509_ATTRIBUTE_dup_TYPE)GetProcAddress(libeayHandleM, "X509_ATTRIBUTE_dup")) == NULL) goto err;
//	if ((X509_ATTRIBUTE_free_FP = (X509_ATTRIBUTE_free_TYPE)GetProcAddress(libeayHandleM, "X509_ATTRIBUTE_free")) == NULL) goto err;
//	if ((X509_ATTRIBUTE_get0_data_FP = (X509_ATTRIBUTE_get0_data_TYPE)GetProcAddress(libeayHandleM, "X509_ATTRIBUTE_get0_data")) == NULL) goto err;
//	if ((X509_ATTRIBUTE_get0_object_FP = (X509_ATTRIBUTE_get0_object_TYPE)GetProcAddress(libeayHandleM, "X509_ATTRIBUTE_get0_object")) == NULL) goto err;
//	if ((X509_ATTRIBUTE_get0_type_FP = (X509_ATTRIBUTE_get0_type_TYPE)GetProcAddress(libeayHandleM, "X509_ATTRIBUTE_get0_type")) == NULL) goto err;
//	if ((X509_ATTRIBUTE_it_FP = (X509_ATTRIBUTE_it_TYPE)GetProcAddress(libeayHandleM, "X509_ATTRIBUTE_it")) == NULL) goto err;
//	if ((X509_ATTRIBUTE_new_FP = (X509_ATTRIBUTE_new_TYPE)GetProcAddress(libeayHandleM, "X509_ATTRIBUTE_new")) == NULL) goto err;
//	if ((X509_ATTRIBUTE_set1_data_FP = (X509_ATTRIBUTE_set1_data_TYPE)GetProcAddress(libeayHandleM, "X509_ATTRIBUTE_set1_data")) == NULL) goto err;
//	if ((X509_ATTRIBUTE_set1_object_FP = (X509_ATTRIBUTE_set1_object_TYPE)GetProcAddress(libeayHandleM, "X509_ATTRIBUTE_set1_object")) == NULL) goto err;
//	if ((X509_CERT_AUX_free_FP = (X509_CERT_AUX_free_TYPE)GetProcAddress(libeayHandleM, "X509_CERT_AUX_free")) == NULL) goto err;
//	if ((X509_CERT_AUX_it_FP = (X509_CERT_AUX_it_TYPE)GetProcAddress(libeayHandleM, "X509_CERT_AUX_it")) == NULL) goto err;
//	if ((X509_CERT_AUX_new_FP = (X509_CERT_AUX_new_TYPE)GetProcAddress(libeayHandleM, "X509_CERT_AUX_new")) == NULL) goto err;
//	if ((X509_CERT_AUX_print_FP = (X509_CERT_AUX_print_TYPE)GetProcAddress(libeayHandleM, "X509_CERT_AUX_print")) == NULL) goto err;
//	if ((X509_CINF_free_FP = (X509_CINF_free_TYPE)GetProcAddress(libeayHandleM, "X509_CINF_free")) == NULL) goto err;
//	if ((X509_CINF_it_FP = (X509_CINF_it_TYPE)GetProcAddress(libeayHandleM, "X509_CINF_it")) == NULL) goto err;
//	if ((X509_CINF_new_FP = (X509_CINF_new_TYPE)GetProcAddress(libeayHandleM, "X509_CINF_new")) == NULL) goto err;
//	if ((X509_CRL_INFO_free_FP = (X509_CRL_INFO_free_TYPE)GetProcAddress(libeayHandleM, "X509_CRL_INFO_free")) == NULL) goto err;
//	if ((X509_CRL_INFO_it_FP = (X509_CRL_INFO_it_TYPE)GetProcAddress(libeayHandleM, "X509_CRL_INFO_it")) == NULL) goto err;
//	if ((X509_CRL_INFO_new_FP = (X509_CRL_INFO_new_TYPE)GetProcAddress(libeayHandleM, "X509_CRL_INFO_new")) == NULL) goto err;
//	if ((X509_CRL_add0_revoked_FP = (X509_CRL_add0_revoked_TYPE)GetProcAddress(libeayHandleM, "X509_CRL_add0_revoked")) == NULL) goto err;
//	if ((X509_CRL_add1_ext_i2d_FP = (X509_CRL_add1_ext_i2d_TYPE)GetProcAddress(libeayHandleM, "X509_CRL_add1_ext_i2d")) == NULL) goto err;
//	if ((X509_CRL_add_ext_FP = (X509_CRL_add_ext_TYPE)GetProcAddress(libeayHandleM, "X509_CRL_add_ext")) == NULL) goto err;
//	if ((X509_CRL_cmp_FP = (X509_CRL_cmp_TYPE)GetProcAddress(libeayHandleM, "X509_CRL_cmp")) == NULL) goto err;
//	if ((X509_CRL_delete_ext_FP = (X509_CRL_delete_ext_TYPE)GetProcAddress(libeayHandleM, "X509_CRL_delete_ext")) == NULL) goto err;
//	if ((X509_CRL_digest_FP = (X509_CRL_digest_TYPE)GetProcAddress(libeayHandleM, "X509_CRL_digest")) == NULL) goto err;
//	if ((X509_CRL_dup_FP = (X509_CRL_dup_TYPE)GetProcAddress(libeayHandleM, "X509_CRL_dup")) == NULL) goto err;
//	if ((X509_CRL_free_FP = (X509_CRL_free_TYPE)GetProcAddress(libeayHandleM, "X509_CRL_free")) == NULL) goto err;
//	if ((X509_CRL_get_ext_FP = (X509_CRL_get_ext_TYPE)GetProcAddress(libeayHandleM, "X509_CRL_get_ext")) == NULL) goto err;
//	if ((X509_CRL_get_ext_by_NID_FP = (X509_CRL_get_ext_by_NID_TYPE)GetProcAddress(libeayHandleM, "X509_CRL_get_ext_by_NID")) == NULL) goto err;
//	if ((X509_CRL_get_ext_by_OBJ_FP = (X509_CRL_get_ext_by_OBJ_TYPE)GetProcAddress(libeayHandleM, "X509_CRL_get_ext_by_OBJ")) == NULL) goto err;
//	if ((X509_CRL_get_ext_by_critical_FP = (X509_CRL_get_ext_by_critical_TYPE)GetProcAddress(libeayHandleM, "X509_CRL_get_ext_by_critical")) == NULL) goto err;
//	if ((X509_CRL_get_ext_count_FP = (X509_CRL_get_ext_count_TYPE)GetProcAddress(libeayHandleM, "X509_CRL_get_ext_count")) == NULL) goto err;
//	if ((X509_CRL_get_ext_d2i_FP = (X509_CRL_get_ext_d2i_TYPE)GetProcAddress(libeayHandleM, "X509_CRL_get_ext_d2i")) == NULL) goto err;
//	if ((X509_CRL_it_FP = (X509_CRL_it_TYPE)GetProcAddress(libeayHandleM, "X509_CRL_it")) == NULL) goto err;
//	if ((X509_CRL_new_FP = (X509_CRL_new_TYPE)GetProcAddress(libeayHandleM, "X509_CRL_new")) == NULL) goto err;
//	if ((X509_CRL_print_FP = (X509_CRL_print_TYPE)GetProcAddress(libeayHandleM, "X509_CRL_print")) == NULL) goto err;
//	if ((X509_CRL_print_fp_FP = (X509_CRL_print_fp_TYPE)GetProcAddress(libeayHandleM, "X509_CRL_print_fp")) == NULL) goto err;
//	if ((X509_CRL_set_issuer_name_FP = (X509_CRL_set_issuer_name_TYPE)GetProcAddress(libeayHandleM, "X509_CRL_set_issuer_name")) == NULL) goto err;
//	if ((X509_CRL_set_lastUpdate_FP = (X509_CRL_set_lastUpdate_TYPE)GetProcAddress(libeayHandleM, "X509_CRL_set_lastUpdate")) == NULL) goto err;
//	if ((X509_CRL_set_nextUpdate_FP = (X509_CRL_set_nextUpdate_TYPE)GetProcAddress(libeayHandleM, "X509_CRL_set_nextUpdate")) == NULL) goto err;
//	if ((X509_CRL_set_version_FP = (X509_CRL_set_version_TYPE)GetProcAddress(libeayHandleM, "X509_CRL_set_version")) == NULL) goto err;
//	if ((X509_CRL_sign_FP = (X509_CRL_sign_TYPE)GetProcAddress(libeayHandleM, "X509_CRL_sign")) == NULL) goto err;
//	if ((X509_CRL_sort_FP = (X509_CRL_sort_TYPE)GetProcAddress(libeayHandleM, "X509_CRL_sort")) == NULL) goto err;
//	if ((X509_CRL_verify_FP = (X509_CRL_verify_TYPE)GetProcAddress(libeayHandleM, "X509_CRL_verify")) == NULL) goto err;
//	if ((X509_EXTENSION_create_by_NID_FP = (X509_EXTENSION_create_by_NID_TYPE)GetProcAddress(libeayHandleM, "X509_EXTENSION_create_by_NID")) == NULL) goto err;
//	if ((X509_EXTENSION_create_by_OBJ_FP = (X509_EXTENSION_create_by_OBJ_TYPE)GetProcAddress(libeayHandleM, "X509_EXTENSION_create_by_OBJ")) == NULL) goto err;
//	if ((X509_EXTENSION_dup_FP = (X509_EXTENSION_dup_TYPE)GetProcAddress(libeayHandleM, "X509_EXTENSION_dup")) == NULL) goto err;
//	if ((X509_EXTENSION_free_FP = (X509_EXTENSION_free_TYPE)GetProcAddress(libeayHandleM, "X509_EXTENSION_free")) == NULL) goto err;
//	if ((X509_EXTENSION_get_critical_FP = (X509_EXTENSION_get_critical_TYPE)GetProcAddress(libeayHandleM, "X509_EXTENSION_get_critical")) == NULL) goto err;
//	if ((X509_EXTENSION_get_data_FP = (X509_EXTENSION_get_data_TYPE)GetProcAddress(libeayHandleM, "X509_EXTENSION_get_data")) == NULL) goto err;
//	if ((X509_EXTENSION_get_object_FP = (X509_EXTENSION_get_object_TYPE)GetProcAddress(libeayHandleM, "X509_EXTENSION_get_object")) == NULL) goto err;
//	if ((X509_EXTENSION_it_FP = (X509_EXTENSION_it_TYPE)GetProcAddress(libeayHandleM, "X509_EXTENSION_it")) == NULL) goto err;
//	if ((X509_EXTENSION_new_FP = (X509_EXTENSION_new_TYPE)GetProcAddress(libeayHandleM, "X509_EXTENSION_new")) == NULL) goto err;
//	if ((X509_EXTENSION_set_critical_FP = (X509_EXTENSION_set_critical_TYPE)GetProcAddress(libeayHandleM, "X509_EXTENSION_set_critical")) == NULL) goto err;
//	if ((X509_EXTENSION_set_data_FP = (X509_EXTENSION_set_data_TYPE)GetProcAddress(libeayHandleM, "X509_EXTENSION_set_data")) == NULL) goto err;
//	if ((X509_EXTENSION_set_object_FP = (X509_EXTENSION_set_object_TYPE)GetProcAddress(libeayHandleM, "X509_EXTENSION_set_object")) == NULL) goto err;
	if ((X509_INFO_free_FP = (X509_INFO_free_TYPE)GetProcAddress(libeayHandleM, "X509_INFO_free")) == NULL) goto err;
	if ((X509_INFO_new_FP = (X509_INFO_new_TYPE)GetProcAddress(libeayHandleM, "X509_INFO_new")) == NULL) goto err;
//	if ((X509_LOOKUP_by_alias_FP = (X509_LOOKUP_by_alias_TYPE)GetProcAddress(libeayHandleM, "X509_LOOKUP_by_alias")) == NULL) goto err;
//	if ((X509_LOOKUP_by_fingerprint_FP = (X509_LOOKUP_by_fingerprint_TYPE)GetProcAddress(libeayHandleM, "X509_LOOKUP_by_fingerprint")) == NULL) goto err;
//	if ((X509_LOOKUP_by_issuer_serial_FP = (X509_LOOKUP_by_issuer_serial_TYPE)GetProcAddress(libeayHandleM, "X509_LOOKUP_by_issuer_serial")) == NULL) goto err;
//	if ((X509_LOOKUP_by_subject_FP = (X509_LOOKUP_by_subject_TYPE)GetProcAddress(libeayHandleM, "X509_LOOKUP_by_subject")) == NULL) goto err;
//	if ((X509_LOOKUP_ctrl_FP = (X509_LOOKUP_ctrl_TYPE)GetProcAddress(libeayHandleM, "X509_LOOKUP_ctrl")) == NULL) goto err;
//	if ((X509_LOOKUP_file_FP = (X509_LOOKUP_file_TYPE)GetProcAddress(libeayHandleM, "X509_LOOKUP_file")) == NULL) goto err;
//	if ((X509_LOOKUP_free_FP = (X509_LOOKUP_free_TYPE)GetProcAddress(libeayHandleM, "X509_LOOKUP_free")) == NULL) goto err;
//	if ((X509_LOOKUP_hash_dir_FP = (X509_LOOKUP_hash_dir_TYPE)GetProcAddress(libeayHandleM, "X509_LOOKUP_hash_dir")) == NULL) goto err;
//	if ((X509_LOOKUP_init_FP = (X509_LOOKUP_init_TYPE)GetProcAddress(libeayHandleM, "X509_LOOKUP_init")) == NULL) goto err;
//	if ((X509_LOOKUP_new_FP = (X509_LOOKUP_new_TYPE)GetProcAddress(libeayHandleM, "X509_LOOKUP_new")) == NULL) goto err;
//	if ((X509_LOOKUP_shutdown_FP = (X509_LOOKUP_shutdown_TYPE)GetProcAddress(libeayHandleM, "X509_LOOKUP_shutdown")) == NULL) goto err;
	if ((X509_NAME_ENTRY_create_by_NID_FP = (X509_NAME_ENTRY_create_by_NID_TYPE)GetProcAddress(libeayHandleM, "X509_NAME_ENTRY_create_by_NID")) == NULL) goto err;
//	if ((X509_NAME_ENTRY_create_by_OBJ_FP = (X509_NAME_ENTRY_create_by_OBJ_TYPE)GetProcAddress(libeayHandleM, "X509_NAME_ENTRY_create_by_OBJ")) == NULL) goto err;
//	if ((X509_NAME_ENTRY_create_by_txt_FP = (X509_NAME_ENTRY_create_by_txt_TYPE)GetProcAddress(libeayHandleM, "X509_NAME_ENTRY_create_by_txt")) == NULL) goto err;
//	if ((X509_NAME_ENTRY_dup_FP = (X509_NAME_ENTRY_dup_TYPE)GetProcAddress(libeayHandleM, "X509_NAME_ENTRY_dup")) == NULL) goto err;
//	if ((X509_NAME_ENTRY_free_FP = (X509_NAME_ENTRY_free_TYPE)GetProcAddress(libeayHandleM, "X509_NAME_ENTRY_free")) == NULL) goto err;
//	if ((X509_NAME_ENTRY_get_data_FP = (X509_NAME_ENTRY_get_data_TYPE)GetProcAddress(libeayHandleM, "X509_NAME_ENTRY_get_data")) == NULL) goto err;
//	if ((X509_NAME_ENTRY_get_object_FP = (X509_NAME_ENTRY_get_object_TYPE)GetProcAddress(libeayHandleM, "X509_NAME_ENTRY_get_object")) == NULL) goto err;
//	if ((X509_NAME_ENTRY_it_FP = (X509_NAME_ENTRY_it_TYPE)GetProcAddress(libeayHandleM, "X509_NAME_ENTRY_it")) == NULL) goto err;
//	if ((X509_NAME_ENTRY_new_FP = (X509_NAME_ENTRY_new_TYPE)GetProcAddress(libeayHandleM, "X509_NAME_ENTRY_new")) == NULL) goto err;
//	if ((X509_NAME_ENTRY_set_data_FP = (X509_NAME_ENTRY_set_data_TYPE)GetProcAddress(libeayHandleM, "X509_NAME_ENTRY_set_data")) == NULL) goto err;
//	if ((X509_NAME_ENTRY_set_object_FP = (X509_NAME_ENTRY_set_object_TYPE)GetProcAddress(libeayHandleM, "X509_NAME_ENTRY_set_object")) == NULL) goto err;
	if ((X509_NAME_add_entry_FP = (X509_NAME_add_entry_TYPE)GetProcAddress(libeayHandleM, "X509_NAME_add_entry")) == NULL) goto err;
//	if ((X509_NAME_add_entry_by_NID_FP = (X509_NAME_add_entry_by_NID_TYPE)GetProcAddress(libeayHandleM, "X509_NAME_add_entry_by_NID")) == NULL) goto err;
//	if ((X509_NAME_add_entry_by_OBJ_FP = (X509_NAME_add_entry_by_OBJ_TYPE)GetProcAddress(libeayHandleM, "X509_NAME_add_entry_by_OBJ")) == NULL) goto err;
//	if ((X509_NAME_add_entry_by_txt_FP = (X509_NAME_add_entry_by_txt_TYPE)GetProcAddress(libeayHandleM, "X509_NAME_add_entry_by_txt")) == NULL) goto err;
//	if ((X509_NAME_cmp_FP = (X509_NAME_cmp_TYPE)GetProcAddress(libeayHandleM, "X509_NAME_cmp")) == NULL) goto err;
//	if ((X509_NAME_delete_entry_FP = (X509_NAME_delete_entry_TYPE)GetProcAddress(libeayHandleM, "X509_NAME_delete_entry")) == NULL) goto err;
//	if ((X509_NAME_digest_FP = (X509_NAME_digest_TYPE)GetProcAddress(libeayHandleM, "X509_NAME_digest")) == NULL) goto err;
	if ((X509_NAME_dup_FP = (X509_NAME_dup_TYPE)GetProcAddress(libeayHandleM, "X509_NAME_dup")) == NULL) goto err;
//	if ((X509_NAME_entry_count_FP = (X509_NAME_entry_count_TYPE)GetProcAddress(libeayHandleM, "X509_NAME_entry_count")) == NULL) goto err;
	if ((X509_NAME_free_FP = (X509_NAME_free_TYPE)GetProcAddress(libeayHandleM, "X509_NAME_free")) == NULL) goto err;
//	if ((X509_NAME_get_entry_FP = (X509_NAME_get_entry_TYPE)GetProcAddress(libeayHandleM, "X509_NAME_get_entry")) == NULL) goto err;
//	if ((X509_NAME_get_index_by_NID_FP = (X509_NAME_get_index_by_NID_TYPE)GetProcAddress(libeayHandleM, "X509_NAME_get_index_by_NID")) == NULL) goto err;
//	if ((X509_NAME_get_index_by_OBJ_FP = (X509_NAME_get_index_by_OBJ_TYPE)GetProcAddress(libeayHandleM, "X509_NAME_get_index_by_OBJ")) == NULL) goto err;
//	if ((X509_NAME_get_text_by_NID_FP = (X509_NAME_get_text_by_NID_TYPE)GetProcAddress(libeayHandleM, "X509_NAME_get_text_by_NID")) == NULL) goto err;
//	if ((X509_NAME_get_text_by_OBJ_FP = (X509_NAME_get_text_by_OBJ_TYPE)GetProcAddress(libeayHandleM, "X509_NAME_get_text_by_OBJ")) == NULL) goto err;
//	if ((X509_NAME_hash_FP = (X509_NAME_hash_TYPE)GetProcAddress(libeayHandleM, "X509_NAME_hash")) == NULL) goto err;
//	if ((X509_NAME_it_FP = (X509_NAME_it_TYPE)GetProcAddress(libeayHandleM, "X509_NAME_it")) == NULL) goto err;
	if ((X509_NAME_new_FP = (X509_NAME_new_TYPE)GetProcAddress(libeayHandleM, "X509_NAME_new")) == NULL) goto err;
	if ((X509_NAME_oneline_FP = (X509_NAME_oneline_TYPE)GetProcAddress(libeayHandleM, "X509_NAME_oneline")) == NULL) goto err;
//	if ((X509_NAME_print_FP = (X509_NAME_print_TYPE)GetProcAddress(libeayHandleM, "X509_NAME_print")) == NULL) goto err;
//	if ((X509_NAME_print_ex_FP = (X509_NAME_print_ex_TYPE)GetProcAddress(libeayHandleM, "X509_NAME_print_ex")) == NULL) goto err;
//	if ((X509_NAME_print_ex_fp_FP = (X509_NAME_print_ex_fp_TYPE)GetProcAddress(libeayHandleM, "X509_NAME_print_ex_fp")) == NULL) goto err;
//	if ((X509_NAME_set_FP = (X509_NAME_set_TYPE)GetProcAddress(libeayHandleM, "X509_NAME_set")) == NULL) goto err;
//	if ((X509_OBJECT_free_contents_FP = (X509_OBJECT_free_contents_TYPE)GetProcAddress(libeayHandleM, "X509_OBJECT_free_contents")) == NULL) goto err;
//	if ((X509_OBJECT_idx_by_subject_FP = (X509_OBJECT_idx_by_subject_TYPE)GetProcAddress(libeayHandleM, "X509_OBJECT_idx_by_subject")) == NULL) goto err;
//	if ((X509_OBJECT_retrieve_by_subject_FP = (X509_OBJECT_retrieve_by_subject_TYPE)GetProcAddress(libeayHandleM, "X509_OBJECT_retrieve_by_subject")) == NULL) goto err;
//	if ((X509_OBJECT_retrieve_match_FP = (X509_OBJECT_retrieve_match_TYPE)GetProcAddress(libeayHandleM, "X509_OBJECT_retrieve_match")) == NULL) goto err;
//	if ((X509_OBJECT_up_ref_count_FP = (X509_OBJECT_up_ref_count_TYPE)GetProcAddress(libeayHandleM, "X509_OBJECT_up_ref_count")) == NULL) goto err;
//	if ((X509_PKEY_free_FP = (X509_PKEY_free_TYPE)GetProcAddress(libeayHandleM, "X509_PKEY_free")) == NULL) goto err;
	if ((X509_PKEY_new_FP = (X509_PKEY_new_TYPE)GetProcAddress(libeayHandleM, "X509_PKEY_new")) == NULL) goto err;
//	if ((X509_PUBKEY_free_FP = (X509_PUBKEY_free_TYPE)GetProcAddress(libeayHandleM, "X509_PUBKEY_free")) == NULL) goto err;
//	if ((X509_PUBKEY_get_FP = (X509_PUBKEY_get_TYPE)GetProcAddress(libeayHandleM, "X509_PUBKEY_get")) == NULL) goto err;
//	if ((X509_PUBKEY_it_FP = (X509_PUBKEY_it_TYPE)GetProcAddress(libeayHandleM, "X509_PUBKEY_it")) == NULL) goto err;
//	if ((X509_PUBKEY_new_FP = (X509_PUBKEY_new_TYPE)GetProcAddress(libeayHandleM, "X509_PUBKEY_new")) == NULL) goto err;
//	if ((X509_PUBKEY_set_FP = (X509_PUBKEY_set_TYPE)GetProcAddress(libeayHandleM, "X509_PUBKEY_set")) == NULL) goto err;
//	if ((X509_PURPOSE_add_FP = (X509_PURPOSE_add_TYPE)GetProcAddress(libeayHandleM, "X509_PURPOSE_add")) == NULL) goto err;
//	if ((X509_PURPOSE_cleanup_FP = (X509_PURPOSE_cleanup_TYPE)GetProcAddress(libeayHandleM, "X509_PURPOSE_cleanup")) == NULL) goto err;
//	if ((X509_PURPOSE_get0_FP = (X509_PURPOSE_get0_TYPE)GetProcAddress(libeayHandleM, "X509_PURPOSE_get0")) == NULL) goto err;
//	if ((X509_PURPOSE_get0_name_FP = (X509_PURPOSE_get0_name_TYPE)GetProcAddress(libeayHandleM, "X509_PURPOSE_get0_name")) == NULL) goto err;
//	if ((X509_PURPOSE_get0_sname_FP = (X509_PURPOSE_get0_sname_TYPE)GetProcAddress(libeayHandleM, "X509_PURPOSE_get0_sname")) == NULL) goto err;
//	if ((X509_PURPOSE_get_by_id_FP = (X509_PURPOSE_get_by_id_TYPE)GetProcAddress(libeayHandleM, "X509_PURPOSE_get_by_id")) == NULL) goto err;
//	if ((X509_PURPOSE_get_by_sname_FP = (X509_PURPOSE_get_by_sname_TYPE)GetProcAddress(libeayHandleM, "X509_PURPOSE_get_by_sname")) == NULL) goto err;
//	if ((X509_PURPOSE_get_count_FP = (X509_PURPOSE_get_count_TYPE)GetProcAddress(libeayHandleM, "X509_PURPOSE_get_count")) == NULL) goto err;
//	if ((X509_PURPOSE_get_id_FP = (X509_PURPOSE_get_id_TYPE)GetProcAddress(libeayHandleM, "X509_PURPOSE_get_id")) == NULL) goto err;
//	if ((X509_PURPOSE_get_trust_FP = (X509_PURPOSE_get_trust_TYPE)GetProcAddress(libeayHandleM, "X509_PURPOSE_get_trust")) == NULL) goto err;
//	if ((X509_PURPOSE_set_FP = (X509_PURPOSE_set_TYPE)GetProcAddress(libeayHandleM, "X509_PURPOSE_set")) == NULL) goto err;
//	if ((X509_REQ_INFO_free_FP = (X509_REQ_INFO_free_TYPE)GetProcAddress(libeayHandleM, "X509_REQ_INFO_free")) == NULL) goto err;
//	if ((X509_REQ_INFO_it_FP = (X509_REQ_INFO_it_TYPE)GetProcAddress(libeayHandleM, "X509_REQ_INFO_it")) == NULL) goto err;
//	if ((X509_REQ_INFO_new_FP = (X509_REQ_INFO_new_TYPE)GetProcAddress(libeayHandleM, "X509_REQ_INFO_new")) == NULL) goto err;
//	if ((X509_REQ_add1_attr_FP = (X509_REQ_add1_attr_TYPE)GetProcAddress(libeayHandleM, "X509_REQ_add1_attr")) == NULL) goto err;
//	if ((X509_REQ_add1_attr_by_NID_FP = (X509_REQ_add1_attr_by_NID_TYPE)GetProcAddress(libeayHandleM, "X509_REQ_add1_attr_by_NID")) == NULL) goto err;
//	if ((X509_REQ_add1_attr_by_OBJ_FP = (X509_REQ_add1_attr_by_OBJ_TYPE)GetProcAddress(libeayHandleM, "X509_REQ_add1_attr_by_OBJ")) == NULL) goto err;
//	if ((X509_REQ_add1_attr_by_txt_FP = (X509_REQ_add1_attr_by_txt_TYPE)GetProcAddress(libeayHandleM, "X509_REQ_add1_attr_by_txt")) == NULL) goto err;
//	if ((X509_REQ_add_extensions_FP = (X509_REQ_add_extensions_TYPE)GetProcAddress(libeayHandleM, "X509_REQ_add_extensions")) == NULL) goto err;
//	if ((X509_REQ_add_extensions_nid_FP = (X509_REQ_add_extensions_nid_TYPE)GetProcAddress(libeayHandleM, "X509_REQ_add_extensions_nid")) == NULL) goto err;
//	if ((X509_REQ_delete_attr_FP = (X509_REQ_delete_attr_TYPE)GetProcAddress(libeayHandleM, "X509_REQ_delete_attr")) == NULL) goto err;
//	if ((X509_REQ_digest_FP = (X509_REQ_digest_TYPE)GetProcAddress(libeayHandleM, "X509_REQ_digest")) == NULL) goto err;
//	if ((X509_REQ_dup_FP = (X509_REQ_dup_TYPE)GetProcAddress(libeayHandleM, "X509_REQ_dup")) == NULL) goto err;
//	if ((X509_REQ_extension_nid_FP = (X509_REQ_extension_nid_TYPE)GetProcAddress(libeayHandleM, "X509_REQ_extension_nid")) == NULL) goto err;
//	if ((X509_REQ_free_FP = (X509_REQ_free_TYPE)GetProcAddress(libeayHandleM, "X509_REQ_free")) == NULL) goto err;
//	if ((X509_REQ_get1_email_FP = (X509_REQ_get1_email_TYPE)GetProcAddress(libeayHandleM, "X509_REQ_get1_email")) == NULL) goto err;
//	if ((X509_REQ_get_attr_FP = (X509_REQ_get_attr_TYPE)GetProcAddress(libeayHandleM, "X509_REQ_get_attr")) == NULL) goto err;
//	if ((X509_REQ_get_attr_by_NID_FP = (X509_REQ_get_attr_by_NID_TYPE)GetProcAddress(libeayHandleM, "X509_REQ_get_attr_by_NID")) == NULL) goto err;
//	if ((X509_REQ_get_attr_by_OBJ_FP = (X509_REQ_get_attr_by_OBJ_TYPE)GetProcAddress(libeayHandleM, "X509_REQ_get_attr_by_OBJ")) == NULL) goto err;
//	if ((X509_REQ_get_attr_count_FP = (X509_REQ_get_attr_count_TYPE)GetProcAddress(libeayHandleM, "X509_REQ_get_attr_count")) == NULL) goto err;
//	if ((X509_REQ_get_extension_nids_FP = (X509_REQ_get_extension_nids_TYPE)GetProcAddress(libeayHandleM, "X509_REQ_get_extension_nids")) == NULL) goto err;
//	if ((X509_REQ_get_extensions_FP = (X509_REQ_get_extensions_TYPE)GetProcAddress(libeayHandleM, "X509_REQ_get_extensions")) == NULL) goto err;
//	if ((X509_REQ_get_pubkey_FP = (X509_REQ_get_pubkey_TYPE)GetProcAddress(libeayHandleM, "X509_REQ_get_pubkey")) == NULL) goto err;
//	if ((X509_REQ_it_FP = (X509_REQ_it_TYPE)GetProcAddress(libeayHandleM, "X509_REQ_it")) == NULL) goto err;
//	if ((X509_REQ_new_FP = (X509_REQ_new_TYPE)GetProcAddress(libeayHandleM, "X509_REQ_new")) == NULL) goto err;
//	if ((X509_REQ_print_FP = (X509_REQ_print_TYPE)GetProcAddress(libeayHandleM, "X509_REQ_print")) == NULL) goto err;
//	if ((X509_REQ_print_ex_FP = (X509_REQ_print_ex_TYPE)GetProcAddress(libeayHandleM, "X509_REQ_print_ex")) == NULL) goto err;
//	if ((X509_REQ_print_fp_FP = (X509_REQ_print_fp_TYPE)GetProcAddress(libeayHandleM, "X509_REQ_print_fp")) == NULL) goto err;
//	if ((X509_REQ_set_extension_nids_FP = (X509_REQ_set_extension_nids_TYPE)GetProcAddress(libeayHandleM, "X509_REQ_set_extension_nids")) == NULL) goto err;
//	if ((X509_REQ_set_pubkey_FP = (X509_REQ_set_pubkey_TYPE)GetProcAddress(libeayHandleM, "X509_REQ_set_pubkey")) == NULL) goto err;
//	if ((X509_REQ_set_subject_name_FP = (X509_REQ_set_subject_name_TYPE)GetProcAddress(libeayHandleM, "X509_REQ_set_subject_name")) == NULL) goto err;
//	if ((X509_REQ_set_version_FP = (X509_REQ_set_version_TYPE)GetProcAddress(libeayHandleM, "X509_REQ_set_version")) == NULL) goto err;
//	if ((X509_REQ_sign_FP = (X509_REQ_sign_TYPE)GetProcAddress(libeayHandleM, "X509_REQ_sign")) == NULL) goto err;
//	if ((X509_REQ_to_X509_FP = (X509_REQ_to_X509_TYPE)GetProcAddress(libeayHandleM, "X509_REQ_to_X509")) == NULL) goto err;
//	if ((X509_REQ_verify_FP = (X509_REQ_verify_TYPE)GetProcAddress(libeayHandleM, "X509_REQ_verify")) == NULL) goto err;
//	if ((X509_REVOKED_add1_ext_i2d_FP = (X509_REVOKED_add1_ext_i2d_TYPE)GetProcAddress(libeayHandleM, "X509_REVOKED_add1_ext_i2d")) == NULL) goto err;
//	if ((X509_REVOKED_add_ext_FP = (X509_REVOKED_add_ext_TYPE)GetProcAddress(libeayHandleM, "X509_REVOKED_add_ext")) == NULL) goto err;
//	if ((X509_REVOKED_delete_ext_FP = (X509_REVOKED_delete_ext_TYPE)GetProcAddress(libeayHandleM, "X509_REVOKED_delete_ext")) == NULL) goto err;
//	if ((X509_REVOKED_free_FP = (X509_REVOKED_free_TYPE)GetProcAddress(libeayHandleM, "X509_REVOKED_free")) == NULL) goto err;
//	if ((X509_REVOKED_get_ext_FP = (X509_REVOKED_get_ext_TYPE)GetProcAddress(libeayHandleM, "X509_REVOKED_get_ext")) == NULL) goto err;
//	if ((X509_REVOKED_get_ext_by_NID_FP = (X509_REVOKED_get_ext_by_NID_TYPE)GetProcAddress(libeayHandleM, "X509_REVOKED_get_ext_by_NID")) == NULL) goto err;
//	if ((X509_REVOKED_get_ext_by_OBJ_FP = (X509_REVOKED_get_ext_by_OBJ_TYPE)GetProcAddress(libeayHandleM, "X509_REVOKED_get_ext_by_OBJ")) == NULL) goto err;
//	if ((X509_REVOKED_get_ext_by_critical_FP = (X509_REVOKED_get_ext_by_critical_TYPE)GetProcAddress(libeayHandleM, "X509_REVOKED_get_ext_by_critical")) == NULL) goto err;
//	if ((X509_REVOKED_get_ext_count_FP = (X509_REVOKED_get_ext_count_TYPE)GetProcAddress(libeayHandleM, "X509_REVOKED_get_ext_count")) == NULL) goto err;
//	if ((X509_REVOKED_get_ext_d2i_FP = (X509_REVOKED_get_ext_d2i_TYPE)GetProcAddress(libeayHandleM, "X509_REVOKED_get_ext_d2i")) == NULL) goto err;
//	if ((X509_REVOKED_it_FP = (X509_REVOKED_it_TYPE)GetProcAddress(libeayHandleM, "X509_REVOKED_it")) == NULL) goto err;
//	if ((X509_REVOKED_new_FP = (X509_REVOKED_new_TYPE)GetProcAddress(libeayHandleM, "X509_REVOKED_new")) == NULL) goto err;
//	if ((X509_REVOKED_set_revocationDate_FP = (X509_REVOKED_set_revocationDate_TYPE)GetProcAddress(libeayHandleM, "X509_REVOKED_set_revocationDate")) == NULL) goto err;
//	if ((X509_REVOKED_set_serialNumber_FP = (X509_REVOKED_set_serialNumber_TYPE)GetProcAddress(libeayHandleM, "X509_REVOKED_set_serialNumber")) == NULL) goto err;
	if ((X509_SIG_free_FP = (X509_SIG_free_TYPE)GetProcAddress(libeayHandleM, "X509_SIG_free")) == NULL) goto err;
//	if ((X509_SIG_it_FP = (X509_SIG_it_TYPE)GetProcAddress(libeayHandleM, "X509_SIG_it")) == NULL) goto err;
//	if ((X509_SIG_new_FP = (X509_SIG_new_TYPE)GetProcAddress(libeayHandleM, "X509_SIG_new")) == NULL) goto err;
//	if ((X509_STORE_CTX_cleanup_FP = (X509_STORE_CTX_cleanup_TYPE)GetProcAddress(libeayHandleM, "X509_STORE_CTX_cleanup")) == NULL) goto err;
//	if ((X509_STORE_CTX_free_FP = (X509_STORE_CTX_free_TYPE)GetProcAddress(libeayHandleM, "X509_STORE_CTX_free")) == NULL) goto err;
//	if ((X509_STORE_CTX_get1_chain_FP = (X509_STORE_CTX_get1_chain_TYPE)GetProcAddress(libeayHandleM, "X509_STORE_CTX_get1_chain")) == NULL) goto err;
//	if ((X509_STORE_CTX_get1_issuer_FP = (X509_STORE_CTX_get1_issuer_TYPE)GetProcAddress(libeayHandleM, "X509_STORE_CTX_get1_issuer")) == NULL) goto err;
//	if ((X509_STORE_CTX_get_chain_FP = (X509_STORE_CTX_get_chain_TYPE)GetProcAddress(libeayHandleM, "X509_STORE_CTX_get_chain")) == NULL) goto err;
	if ((X509_STORE_CTX_get_current_cert_FP = (X509_STORE_CTX_get_current_cert_TYPE)GetProcAddress(libeayHandleM, "X509_STORE_CTX_get_current_cert")) == NULL) goto err;
	if ((X509_STORE_CTX_get_error_FP = (X509_STORE_CTX_get_error_TYPE)GetProcAddress(libeayHandleM, "X509_STORE_CTX_get_error")) == NULL) goto err;
	if ((X509_STORE_CTX_get_error_depth_FP = (X509_STORE_CTX_get_error_depth_TYPE)GetProcAddress(libeayHandleM, "X509_STORE_CTX_get_error_depth")) == NULL) goto err;
//	if ((X509_STORE_CTX_get_ex_data_FP = (X509_STORE_CTX_get_ex_data_TYPE)GetProcAddress(libeayHandleM, "X509_STORE_CTX_get_ex_data")) == NULL) goto err;
//	if ((X509_STORE_CTX_get_ex_new_index_FP = (X509_STORE_CTX_get_ex_new_index_TYPE)GetProcAddress(libeayHandleM, "X509_STORE_CTX_get_ex_new_index")) == NULL) goto err;
//	if ((X509_STORE_CTX_init_FP = (X509_STORE_CTX_init_TYPE)GetProcAddress(libeayHandleM, "X509_STORE_CTX_init")) == NULL) goto err;
//	if ((X509_STORE_CTX_new_FP = (X509_STORE_CTX_new_TYPE)GetProcAddress(libeayHandleM, "X509_STORE_CTX_new")) == NULL) goto err;
//	if ((X509_STORE_CTX_purpose_inherit_FP = (X509_STORE_CTX_purpose_inherit_TYPE)GetProcAddress(libeayHandleM, "X509_STORE_CTX_purpose_inherit")) == NULL) goto err;
//	if ((X509_STORE_CTX_set_cert_FP = (X509_STORE_CTX_set_cert_TYPE)GetProcAddress(libeayHandleM, "X509_STORE_CTX_set_cert")) == NULL) goto err;
//	if ((X509_STORE_CTX_set_chain_FP = (X509_STORE_CTX_set_chain_TYPE)GetProcAddress(libeayHandleM, "X509_STORE_CTX_set_chain")) == NULL) goto err;
//	if ((X509_STORE_CTX_set_error_FP = (X509_STORE_CTX_set_error_TYPE)GetProcAddress(libeayHandleM, "X509_STORE_CTX_set_error")) == NULL) goto err;
//	if ((X509_STORE_CTX_set_ex_data_FP = (X509_STORE_CTX_set_ex_data_TYPE)GetProcAddress(libeayHandleM, "X509_STORE_CTX_set_ex_data")) == NULL) goto err;
//	if ((X509_STORE_CTX_set_flags_FP = (X509_STORE_CTX_set_flags_TYPE)GetProcAddress(libeayHandleM, "X509_STORE_CTX_set_flags")) == NULL) goto err;
//	if ((X509_STORE_CTX_set_purpose_FP = (X509_STORE_CTX_set_purpose_TYPE)GetProcAddress(libeayHandleM, "X509_STORE_CTX_set_purpose")) == NULL) goto err;
//	if ((X509_STORE_CTX_set_time_FP = (X509_STORE_CTX_set_time_TYPE)GetProcAddress(libeayHandleM, "X509_STORE_CTX_set_time")) == NULL) goto err;
//	if ((X509_STORE_CTX_set_trust_FP = (X509_STORE_CTX_set_trust_TYPE)GetProcAddress(libeayHandleM, "X509_STORE_CTX_set_trust")) == NULL) goto err;
//	if ((X509_STORE_CTX_set_verify_cb_FP = (X509_STORE_CTX_set_verify_cb_TYPE)GetProcAddress(libeayHandleM, "X509_STORE_CTX_set_verify_cb")) == NULL) goto err;
//	if ((X509_STORE_CTX_trusted_stack_FP = (X509_STORE_CTX_trusted_stack_TYPE)GetProcAddress(libeayHandleM, "X509_STORE_CTX_trusted_stack")) == NULL) goto err;
//	if ((X509_STORE_add_cert_FP = (X509_STORE_add_cert_TYPE)GetProcAddress(libeayHandleM, "X509_STORE_add_cert")) == NULL) goto err;
//	if ((X509_STORE_add_crl_FP = (X509_STORE_add_crl_TYPE)GetProcAddress(libeayHandleM, "X509_STORE_add_crl")) == NULL) goto err;
//	if ((X509_STORE_add_lookup_FP = (X509_STORE_add_lookup_TYPE)GetProcAddress(libeayHandleM, "X509_STORE_add_lookup")) == NULL) goto err;
//	if ((X509_STORE_free_FP = (X509_STORE_free_TYPE)GetProcAddress(libeayHandleM, "X509_STORE_free")) == NULL) goto err;
//	if ((X509_STORE_get_by_subject_FP = (X509_STORE_get_by_subject_TYPE)GetProcAddress(libeayHandleM, "X509_STORE_get_by_subject")) == NULL) goto err;
//	if ((X509_STORE_load_locations_FP = (X509_STORE_load_locations_TYPE)GetProcAddress(libeayHandleM, "X509_STORE_load_locations")) == NULL) goto err;
//	if ((X509_STORE_new_FP = (X509_STORE_new_TYPE)GetProcAddress(libeayHandleM, "X509_STORE_new")) == NULL) goto err;
//	if ((X509_STORE_set_default_paths_FP = (X509_STORE_set_default_paths_TYPE)GetProcAddress(libeayHandleM, "X509_STORE_set_default_paths")) == NULL) goto err;
//	if ((X509_STORE_set_flags_FP = (X509_STORE_set_flags_TYPE)GetProcAddress(libeayHandleM, "X509_STORE_set_flags")) == NULL) goto err;
//	if ((X509_STORE_set_purpose_FP = (X509_STORE_set_purpose_TYPE)GetProcAddress(libeayHandleM, "X509_STORE_set_purpose")) == NULL) goto err;
//	if ((X509_STORE_set_trust_FP = (X509_STORE_set_trust_TYPE)GetProcAddress(libeayHandleM, "X509_STORE_set_trust")) == NULL) goto err;
//	if ((X509_TRUST_add_FP = (X509_TRUST_add_TYPE)GetProcAddress(libeayHandleM, "X509_TRUST_add")) == NULL) goto err;
//	if ((X509_TRUST_cleanup_FP = (X509_TRUST_cleanup_TYPE)GetProcAddress(libeayHandleM, "X509_TRUST_cleanup")) == NULL) goto err;
//	if ((X509_TRUST_get0_FP = (X509_TRUST_get0_TYPE)GetProcAddress(libeayHandleM, "X509_TRUST_get0")) == NULL) goto err;
//	if ((X509_TRUST_get0_name_FP = (X509_TRUST_get0_name_TYPE)GetProcAddress(libeayHandleM, "X509_TRUST_get0_name")) == NULL) goto err;
//	if ((X509_TRUST_get_by_id_FP = (X509_TRUST_get_by_id_TYPE)GetProcAddress(libeayHandleM, "X509_TRUST_get_by_id")) == NULL) goto err;
//	if ((X509_TRUST_get_count_FP = (X509_TRUST_get_count_TYPE)GetProcAddress(libeayHandleM, "X509_TRUST_get_count")) == NULL) goto err;
//	if ((X509_TRUST_get_flags_FP = (X509_TRUST_get_flags_TYPE)GetProcAddress(libeayHandleM, "X509_TRUST_get_flags")) == NULL) goto err;
//	if ((X509_TRUST_get_trust_FP = (X509_TRUST_get_trust_TYPE)GetProcAddress(libeayHandleM, "X509_TRUST_get_trust")) == NULL) goto err;
//	if ((X509_TRUST_set_FP = (X509_TRUST_set_TYPE)GetProcAddress(libeayHandleM, "X509_TRUST_set")) == NULL) goto err;
//	if ((X509_TRUST_set_default_FP = (X509_TRUST_set_default_TYPE)GetProcAddress(libeayHandleM, "X509_TRUST_set_default")) == NULL) goto err;
//	if ((X509_VAL_free_FP = (X509_VAL_free_TYPE)GetProcAddress(libeayHandleM, "X509_VAL_free")) == NULL) goto err;
//	if ((X509_VAL_it_FP = (X509_VAL_it_TYPE)GetProcAddress(libeayHandleM, "X509_VAL_it")) == NULL) goto err;
//	if ((X509_VAL_new_FP = (X509_VAL_new_TYPE)GetProcAddress(libeayHandleM, "X509_VAL_new")) == NULL) goto err;
//	if ((X509_add1_ext_i2d_FP = (X509_add1_ext_i2d_TYPE)GetProcAddress(libeayHandleM, "X509_add1_ext_i2d")) == NULL) goto err;
//	if ((X509_add1_reject_object_FP = (X509_add1_reject_object_TYPE)GetProcAddress(libeayHandleM, "X509_add1_reject_object")) == NULL) goto err;
//	if ((X509_add1_trust_object_FP = (X509_add1_trust_object_TYPE)GetProcAddress(libeayHandleM, "X509_add1_trust_object")) == NULL) goto err;
//	if ((X509_add_ext_FP = (X509_add_ext_TYPE)GetProcAddress(libeayHandleM, "X509_add_ext")) == NULL) goto err;
//	if ((X509_alias_get0_FP = (X509_alias_get0_TYPE)GetProcAddress(libeayHandleM, "X509_alias_get0")) == NULL) goto err;
//	if ((X509_alias_set1_FP = (X509_alias_set1_TYPE)GetProcAddress(libeayHandleM, "X509_alias_set1")) == NULL) goto err;
//	if ((X509_asn1_meth_FP = (X509_asn1_meth_TYPE)GetProcAddress(libeayHandleM, "X509_asn1_meth")) == NULL) goto err;
//	if ((X509_certificate_type_FP = (X509_certificate_type_TYPE)GetProcAddress(libeayHandleM, "X509_certificate_type")) == NULL) goto err;
//	if ((X509_check_issued_FP = (X509_check_issued_TYPE)GetProcAddress(libeayHandleM, "X509_check_issued")) == NULL) goto err;
//	if ((X509_check_private_key_FP = (X509_check_private_key_TYPE)GetProcAddress(libeayHandleM, "X509_check_private_key")) == NULL) goto err;
//	if ((X509_check_purpose_FP = (X509_check_purpose_TYPE)GetProcAddress(libeayHandleM, "X509_check_purpose")) == NULL) goto err;
//	if ((X509_check_trust_FP = (X509_check_trust_TYPE)GetProcAddress(libeayHandleM, "X509_check_trust")) == NULL) goto err;
//	if ((X509_cmp_FP = (X509_cmp_TYPE)GetProcAddress(libeayHandleM, "X509_cmp")) == NULL) goto err;
//	if ((X509_cmp_current_time_FP = (X509_cmp_current_time_TYPE)GetProcAddress(libeayHandleM, "X509_cmp_current_time")) == NULL) goto err;
//	if ((X509_cmp_time_FP = (X509_cmp_time_TYPE)GetProcAddress(libeayHandleM, "X509_cmp_time")) == NULL) goto err;
//	if ((X509_delete_ext_FP = (X509_delete_ext_TYPE)GetProcAddress(libeayHandleM, "X509_delete_ext")) == NULL) goto err;
//	if ((X509_digest_FP = (X509_digest_TYPE)GetProcAddress(libeayHandleM, "X509_digest")) == NULL) goto err;
	if ((X509_dup_FP = (X509_dup_TYPE)GetProcAddress(libeayHandleM, "X509_dup")) == NULL) goto err;
//	if ((X509_email_free_FP = (X509_email_free_TYPE)GetProcAddress(libeayHandleM, "X509_email_free")) == NULL) goto err;
//	if ((X509_find_by_issuer_and_serial_FP = (X509_find_by_issuer_and_serial_TYPE)GetProcAddress(libeayHandleM, "X509_find_by_issuer_and_serial")) == NULL) goto err;
//	if ((X509_find_by_subject_FP = (X509_find_by_subject_TYPE)GetProcAddress(libeayHandleM, "X509_find_by_subject")) == NULL) goto err;
	if ((X509_free_FP = (X509_free_TYPE)GetProcAddress(libeayHandleM, "X509_free")) == NULL) goto err;
//	if ((X509_get0_pubkey_bitstr_FP = (X509_get0_pubkey_bitstr_TYPE)GetProcAddress(libeayHandleM, "X509_get0_pubkey_bitstr")) == NULL) goto err;
//	if ((X509_get1_email_FP = (X509_get1_email_TYPE)GetProcAddress(libeayHandleM, "X509_get1_email")) == NULL) goto err;
//	if ((X509_get_default_cert_area_FP = (X509_get_default_cert_area_TYPE)GetProcAddress(libeayHandleM, "X509_get_default_cert_area")) == NULL) goto err;
//	if ((X509_get_default_cert_dir_FP = (X509_get_default_cert_dir_TYPE)GetProcAddress(libeayHandleM, "X509_get_default_cert_dir")) == NULL) goto err;
//	if ((X509_get_default_cert_dir_env_FP = (X509_get_default_cert_dir_env_TYPE)GetProcAddress(libeayHandleM, "X509_get_default_cert_dir_env")) == NULL) goto err;
//	if ((X509_get_default_cert_file_FP = (X509_get_default_cert_file_TYPE)GetProcAddress(libeayHandleM, "X509_get_default_cert_file")) == NULL) goto err;
//	if ((X509_get_default_cert_file_env_FP = (X509_get_default_cert_file_env_TYPE)GetProcAddress(libeayHandleM, "X509_get_default_cert_file_env")) == NULL) goto err;
//	if ((X509_get_default_private_dir_FP = (X509_get_default_private_dir_TYPE)GetProcAddress(libeayHandleM, "X509_get_default_private_dir")) == NULL) goto err;
//	if ((X509_get_ex_data_FP = (X509_get_ex_data_TYPE)GetProcAddress(libeayHandleM, "X509_get_ex_data")) == NULL) goto err;
//	if ((X509_get_ex_new_index_FP = (X509_get_ex_new_index_TYPE)GetProcAddress(libeayHandleM, "X509_get_ex_new_index")) == NULL) goto err;
//	if ((X509_get_ext_FP = (X509_get_ext_TYPE)GetProcAddress(libeayHandleM, "X509_get_ext")) == NULL) goto err;
//	if ((X509_get_ext_by_NID_FP = (X509_get_ext_by_NID_TYPE)GetProcAddress(libeayHandleM, "X509_get_ext_by_NID")) == NULL) goto err;
//	if ((X509_get_ext_by_OBJ_FP = (X509_get_ext_by_OBJ_TYPE)GetProcAddress(libeayHandleM, "X509_get_ext_by_OBJ")) == NULL) goto err;
//	if ((X509_get_ext_by_critical_FP = (X509_get_ext_by_critical_TYPE)GetProcAddress(libeayHandleM, "X509_get_ext_by_critical")) == NULL) goto err;
//	if ((X509_get_ext_count_FP = (X509_get_ext_count_TYPE)GetProcAddress(libeayHandleM, "X509_get_ext_count")) == NULL) goto err;
//	if ((X509_get_ext_d2i_FP = (X509_get_ext_d2i_TYPE)GetProcAddress(libeayHandleM, "X509_get_ext_d2i")) == NULL) goto err;
	if ((X509_get_issuer_name_FP = (X509_get_issuer_name_TYPE)GetProcAddress(libeayHandleM, "X509_get_issuer_name")) == NULL) goto err;
	if ((X509_get_pubkey_FP = (X509_get_pubkey_TYPE)GetProcAddress(libeayHandleM, "X509_get_pubkey")) == NULL) goto err;
//	if ((X509_get_pubkey_parameters_FP = (X509_get_pubkey_parameters_TYPE)GetProcAddress(libeayHandleM, "X509_get_pubkey_parameters")) == NULL) goto err;
	if ((X509_get_serialNumber_FP = (X509_get_serialNumber_TYPE)GetProcAddress(libeayHandleM, "X509_get_serialNumber")) == NULL) goto err;
	if ((X509_get_subject_name_FP = (X509_get_subject_name_TYPE)GetProcAddress(libeayHandleM, "X509_get_subject_name")) == NULL) goto err;
//	if ((X509_gmtime_adj_FP = (X509_gmtime_adj_TYPE)GetProcAddress(libeayHandleM, "X509_gmtime_adj")) == NULL) goto err;
//	if ((X509_issuer_and_serial_cmp_FP = (X509_issuer_and_serial_cmp_TYPE)GetProcAddress(libeayHandleM, "X509_issuer_and_serial_cmp")) == NULL) goto err;
//	if ((X509_issuer_and_serial_hash_FP = (X509_issuer_and_serial_hash_TYPE)GetProcAddress(libeayHandleM, "X509_issuer_and_serial_hash")) == NULL) goto err;
//	if ((X509_issuer_name_cmp_FP = (X509_issuer_name_cmp_TYPE)GetProcAddress(libeayHandleM, "X509_issuer_name_cmp")) == NULL) goto err;
//	if ((X509_issuer_name_hash_FP = (X509_issuer_name_hash_TYPE)GetProcAddress(libeayHandleM, "X509_issuer_name_hash")) == NULL) goto err;
	if ((X509_it_FP = (X509_it_TYPE)GetProcAddress(libeayHandleM, "X509_it")) == NULL) goto err;
//	if ((X509_keyid_set1_FP = (X509_keyid_set1_TYPE)GetProcAddress(libeayHandleM, "X509_keyid_set1")) == NULL) goto err;
//	if ((X509_load_cert_crl_file_FP = (X509_load_cert_crl_file_TYPE)GetProcAddress(libeayHandleM, "X509_load_cert_crl_file")) == NULL) goto err;
//	if ((X509_load_cert_file_FP = (X509_load_cert_file_TYPE)GetProcAddress(libeayHandleM, "X509_load_cert_file")) == NULL) goto err;
//	if ((X509_load_crl_file_FP = (X509_load_crl_file_TYPE)GetProcAddress(libeayHandleM, "X509_load_crl_file")) == NULL) goto err;
	if ((X509_new_FP = (X509_new_TYPE)GetProcAddress(libeayHandleM, "X509_new")) == NULL) goto err;
//	if ((X509_ocspid_print_FP = (X509_ocspid_print_TYPE)GetProcAddress(libeayHandleM, "X509_ocspid_print")) == NULL) goto err;
//	if ((X509_print_FP = (X509_print_TYPE)GetProcAddress(libeayHandleM, "X509_print")) == NULL) goto err;
//	if ((X509_print_ex_FP = (X509_print_ex_TYPE)GetProcAddress(libeayHandleM, "X509_print_ex")) == NULL) goto err;
//	if ((X509_print_ex_fp_FP = (X509_print_ex_fp_TYPE)GetProcAddress(libeayHandleM, "X509_print_ex_fp")) == NULL) goto err;
//	if ((X509_print_fp_FP = (X509_print_fp_TYPE)GetProcAddress(libeayHandleM, "X509_print_fp")) == NULL) goto err;
//	if ((X509_pubkey_digest_FP = (X509_pubkey_digest_TYPE)GetProcAddress(libeayHandleM, "X509_pubkey_digest")) == NULL) goto err;
//	if ((X509_reject_clear_FP = (X509_reject_clear_TYPE)GetProcAddress(libeayHandleM, "X509_reject_clear")) == NULL) goto err;
//	if ((X509_set_ex_data_FP = (X509_set_ex_data_TYPE)GetProcAddress(libeayHandleM, "X509_set_ex_data")) == NULL) goto err;
	if ((X509_set_issuer_name_FP = (X509_set_issuer_name_TYPE)GetProcAddress(libeayHandleM, "X509_set_issuer_name")) == NULL) goto err;
//	if ((X509_set_notAfter_FP = (X509_set_notAfter_TYPE)GetProcAddress(libeayHandleM, "X509_set_notAfter")) == NULL) goto err;
//	if ((X509_set_notBefore_FP = (X509_set_notBefore_TYPE)GetProcAddress(libeayHandleM, "X509_set_notBefore")) == NULL) goto err;
	if ((X509_set_pubkey_FP = (X509_set_pubkey_TYPE)GetProcAddress(libeayHandleM, "X509_set_pubkey")) == NULL) goto err;
//	if ((X509_set_serialNumber_FP = (X509_set_serialNumber_TYPE)GetProcAddress(libeayHandleM, "X509_set_serialNumber")) == NULL) goto err;
	if ((X509_set_subject_name_FP = (X509_set_subject_name_TYPE)GetProcAddress(libeayHandleM, "X509_set_subject_name")) == NULL) goto err;
	if ((X509_set_version_FP = (X509_set_version_TYPE)GetProcAddress(libeayHandleM, "X509_set_version")) == NULL) goto err;
	if ((X509_sign_FP = (X509_sign_TYPE)GetProcAddress(libeayHandleM, "X509_sign")) == NULL) goto err;
//	if ((X509_signature_print_FP = (X509_signature_print_TYPE)GetProcAddress(libeayHandleM, "X509_signature_print")) == NULL) goto err;
//	if ((X509_subject_name_cmp_FP = (X509_subject_name_cmp_TYPE)GetProcAddress(libeayHandleM, "X509_subject_name_cmp")) == NULL) goto err;
//	if ((X509_subject_name_hash_FP = (X509_subject_name_hash_TYPE)GetProcAddress(libeayHandleM, "X509_subject_name_hash")) == NULL) goto err;
//	if ((X509_supported_extension_FP = (X509_supported_extension_TYPE)GetProcAddress(libeayHandleM, "X509_supported_extension")) == NULL) goto err;
	if ((X509_time_adj_FP = (X509_time_adj_TYPE)GetProcAddress(libeayHandleM, "X509_time_adj")) == NULL) goto err;
//	if ((X509_to_X509_REQ_FP = (X509_to_X509_REQ_TYPE)GetProcAddress(libeayHandleM, "X509_to_X509_REQ")) == NULL) goto err;
//	if ((X509_trust_clear_FP = (X509_trust_clear_TYPE)GetProcAddress(libeayHandleM, "X509_trust_clear")) == NULL) goto err;
//	if ((X509_verify_FP = (X509_verify_TYPE)GetProcAddress(libeayHandleM, "X509_verify")) == NULL) goto err;
//	if ((X509_verify_cert_FP = (X509_verify_cert_TYPE)GetProcAddress(libeayHandleM, "X509_verify_cert")) == NULL) goto err;
	if ((X509_verify_cert_error_string_FP = (X509_verify_cert_error_string_TYPE)GetProcAddress(libeayHandleM, "X509_verify_cert_error_string")) == NULL) goto err;
//	if ((X509at_add1_attr_FP = (X509at_add1_attr_TYPE)GetProcAddress(libeayHandleM, "X509at_add1_attr")) == NULL) goto err;
//	if ((X509at_add1_attr_by_NID_FP = (X509at_add1_attr_by_NID_TYPE)GetProcAddress(libeayHandleM, "X509at_add1_attr_by_NID")) == NULL) goto err;
//	if ((X509at_add1_attr_by_OBJ_FP = (X509at_add1_attr_by_OBJ_TYPE)GetProcAddress(libeayHandleM, "X509at_add1_attr_by_OBJ")) == NULL) goto err;
//	if ((X509at_add1_attr_by_txt_FP = (X509at_add1_attr_by_txt_TYPE)GetProcAddress(libeayHandleM, "X509at_add1_attr_by_txt")) == NULL) goto err;
//	if ((X509at_delete_attr_FP = (X509at_delete_attr_TYPE)GetProcAddress(libeayHandleM, "X509at_delete_attr")) == NULL) goto err;
//	if ((X509at_get_attr_FP = (X509at_get_attr_TYPE)GetProcAddress(libeayHandleM, "X509at_get_attr")) == NULL) goto err;
//	if ((X509at_get_attr_by_NID_FP = (X509at_get_attr_by_NID_TYPE)GetProcAddress(libeayHandleM, "X509at_get_attr_by_NID")) == NULL) goto err;
//	if ((X509at_get_attr_by_OBJ_FP = (X509at_get_attr_by_OBJ_TYPE)GetProcAddress(libeayHandleM, "X509at_get_attr_by_OBJ")) == NULL) goto err;
//	if ((X509at_get_attr_count_FP = (X509at_get_attr_count_TYPE)GetProcAddress(libeayHandleM, "X509at_get_attr_count")) == NULL) goto err;
//	if ((X509v3_add_ext_FP = (X509v3_add_ext_TYPE)GetProcAddress(libeayHandleM, "X509v3_add_ext")) == NULL) goto err;
//	if ((X509v3_delete_ext_FP = (X509v3_delete_ext_TYPE)GetProcAddress(libeayHandleM, "X509v3_delete_ext")) == NULL) goto err;
//	if ((X509v3_get_ext_FP = (X509v3_get_ext_TYPE)GetProcAddress(libeayHandleM, "X509v3_get_ext")) == NULL) goto err;
//	if ((X509v3_get_ext_by_NID_FP = (X509v3_get_ext_by_NID_TYPE)GetProcAddress(libeayHandleM, "X509v3_get_ext_by_NID")) == NULL) goto err;
//	if ((X509v3_get_ext_by_OBJ_FP = (X509v3_get_ext_by_OBJ_TYPE)GetProcAddress(libeayHandleM, "X509v3_get_ext_by_OBJ")) == NULL) goto err;
//	if ((X509v3_get_ext_by_critical_FP = (X509v3_get_ext_by_critical_TYPE)GetProcAddress(libeayHandleM, "X509v3_get_ext_by_critical")) == NULL) goto err;
//	if ((X509v3_get_ext_count_FP = (X509v3_get_ext_count_TYPE)GetProcAddress(libeayHandleM, "X509v3_get_ext_count")) == NULL) goto err;
//	if ((ZLONG_it_FP = (ZLONG_it_TYPE)GetProcAddress(libeayHandleM, "ZLONG_it")) == NULL) goto err;
//	if ((_ossl_096_des_random_seed_FP = (_ossl_096_des_random_seed_TYPE)GetProcAddress(libeayHandleM, "_ossl_096_des_random_seed")) == NULL) goto err;
//	if ((_ossl_old_crypt_FP = (_ossl_old_crypt_TYPE)GetProcAddress(libeayHandleM, "_ossl_old_crypt")) == NULL) goto err;
//	if ((_ossl_old_des_cbc_cksum_FP = (_ossl_old_des_cbc_cksum_TYPE)GetProcAddress(libeayHandleM, "_ossl_old_des_cbc_cksum")) == NULL) goto err;
//	if ((_ossl_old_des_cbc_encrypt_FP = (_ossl_old_des_cbc_encrypt_TYPE)GetProcAddress(libeayHandleM, "_ossl_old_des_cbc_encrypt")) == NULL) goto err;
//	if ((_ossl_old_des_cfb64_encrypt_FP = (_ossl_old_des_cfb64_encrypt_TYPE)GetProcAddress(libeayHandleM, "_ossl_old_des_cfb64_encrypt")) == NULL) goto err;
//	if ((_ossl_old_des_cfb_encrypt_FP = (_ossl_old_des_cfb_encrypt_TYPE)GetProcAddress(libeayHandleM, "_ossl_old_des_cfb_encrypt")) == NULL) goto err;
//	if ((_ossl_old_des_crypt_FP = (_ossl_old_des_crypt_TYPE)GetProcAddress(libeayHandleM, "_ossl_old_des_crypt")) == NULL) goto err;
//	if ((_ossl_old_des_decrypt3_FP = (_ossl_old_des_decrypt3_TYPE)GetProcAddress(libeayHandleM, "_ossl_old_des_decrypt3")) == NULL) goto err;
//	if ((_ossl_old_des_ecb3_encrypt_FP = (_ossl_old_des_ecb3_encrypt_TYPE)GetProcAddress(libeayHandleM, "_ossl_old_des_ecb3_encrypt")) == NULL) goto err;
//	if ((_ossl_old_des_ecb_encrypt_FP = (_ossl_old_des_ecb_encrypt_TYPE)GetProcAddress(libeayHandleM, "_ossl_old_des_ecb_encrypt")) == NULL) goto err;
//	if ((_ossl_old_des_ede3_cbc_encrypt_FP = (_ossl_old_des_ede3_cbc_encrypt_TYPE)GetProcAddress(libeayHandleM, "_ossl_old_des_ede3_cbc_encrypt")) == NULL) goto err;
//	if ((_ossl_old_des_ede3_cfb64_encrypt_FP = (_ossl_old_des_ede3_cfb64_encrypt_TYPE)GetProcAddress(libeayHandleM, "_ossl_old_des_ede3_cfb64_encrypt")) == NULL) goto err;
//	if ((_ossl_old_des_ede3_ofb64_encrypt_FP = (_ossl_old_des_ede3_ofb64_encrypt_TYPE)GetProcAddress(libeayHandleM, "_ossl_old_des_ede3_ofb64_encrypt")) == NULL) goto err;
//	if ((_ossl_old_des_enc_read_FP = (_ossl_old_des_enc_read_TYPE)GetProcAddress(libeayHandleM, "_ossl_old_des_enc_read")) == NULL) goto err;
//	if ((_ossl_old_des_enc_write_FP = (_ossl_old_des_enc_write_TYPE)GetProcAddress(libeayHandleM, "_ossl_old_des_enc_write")) == NULL) goto err;
//	if ((_ossl_old_des_encrypt2_FP = (_ossl_old_des_encrypt2_TYPE)GetProcAddress(libeayHandleM, "_ossl_old_des_encrypt2")) == NULL) goto err;
//	if ((_ossl_old_des_encrypt3_FP = (_ossl_old_des_encrypt3_TYPE)GetProcAddress(libeayHandleM, "_ossl_old_des_encrypt3")) == NULL) goto err;
//	if ((_ossl_old_des_encrypt_FP = (_ossl_old_des_encrypt_TYPE)GetProcAddress(libeayHandleM, "_ossl_old_des_encrypt")) == NULL) goto err;
//	if ((_ossl_old_des_fcrypt_FP = (_ossl_old_des_fcrypt_TYPE)GetProcAddress(libeayHandleM, "_ossl_old_des_fcrypt")) == NULL) goto err;
//	if ((_ossl_old_des_is_weak_key_FP = (_ossl_old_des_is_weak_key_TYPE)GetProcAddress(libeayHandleM, "_ossl_old_des_is_weak_key")) == NULL) goto err;
//	if ((_ossl_old_des_key_sched_FP = (_ossl_old_des_key_sched_TYPE)GetProcAddress(libeayHandleM, "_ossl_old_des_key_sched")) == NULL) goto err;
//	if ((_ossl_old_des_ncbc_encrypt_FP = (_ossl_old_des_ncbc_encrypt_TYPE)GetProcAddress(libeayHandleM, "_ossl_old_des_ncbc_encrypt")) == NULL) goto err;
//	if ((_ossl_old_des_ofb64_encrypt_FP = (_ossl_old_des_ofb64_encrypt_TYPE)GetProcAddress(libeayHandleM, "_ossl_old_des_ofb64_encrypt")) == NULL) goto err;
//	if ((_ossl_old_des_ofb_encrypt_FP = (_ossl_old_des_ofb_encrypt_TYPE)GetProcAddress(libeayHandleM, "_ossl_old_des_ofb_encrypt")) == NULL) goto err;
//	if ((_ossl_old_des_options_FP = (_ossl_old_des_options_TYPE)GetProcAddress(libeayHandleM, "_ossl_old_des_options")) == NULL) goto err;
//	if ((_ossl_old_des_pcbc_encrypt_FP = (_ossl_old_des_pcbc_encrypt_TYPE)GetProcAddress(libeayHandleM, "_ossl_old_des_pcbc_encrypt")) == NULL) goto err;
//	if ((_ossl_old_des_quad_cksum_FP = (_ossl_old_des_quad_cksum_TYPE)GetProcAddress(libeayHandleM, "_ossl_old_des_quad_cksum")) == NULL) goto err;
//	if ((_ossl_old_des_random_key_FP = (_ossl_old_des_random_key_TYPE)GetProcAddress(libeayHandleM, "_ossl_old_des_random_key")) == NULL) goto err;
//	if ((_ossl_old_des_random_seed_FP = (_ossl_old_des_random_seed_TYPE)GetProcAddress(libeayHandleM, "_ossl_old_des_random_seed")) == NULL) goto err;
//	if ((_ossl_old_des_read_2passwords_FP = (_ossl_old_des_read_2passwords_TYPE)GetProcAddress(libeayHandleM, "_ossl_old_des_read_2passwords")) == NULL) goto err;
//	if ((_ossl_old_des_read_password_FP = (_ossl_old_des_read_password_TYPE)GetProcAddress(libeayHandleM, "_ossl_old_des_read_password")) == NULL) goto err;
//	if ((_ossl_old_des_read_pw_FP = (_ossl_old_des_read_pw_TYPE)GetProcAddress(libeayHandleM, "_ossl_old_des_read_pw")) == NULL) goto err;
//	if ((_ossl_old_des_read_pw_string_FP = (_ossl_old_des_read_pw_string_TYPE)GetProcAddress(libeayHandleM, "_ossl_old_des_read_pw_string")) == NULL) goto err;
//	if ((_ossl_old_des_set_key_FP = (_ossl_old_des_set_key_TYPE)GetProcAddress(libeayHandleM, "_ossl_old_des_set_key")) == NULL) goto err;
//	if ((_ossl_old_des_set_odd_parity_FP = (_ossl_old_des_set_odd_parity_TYPE)GetProcAddress(libeayHandleM, "_ossl_old_des_set_odd_parity")) == NULL) goto err;
//	if ((_ossl_old_des_string_to_2keys_FP = (_ossl_old_des_string_to_2keys_TYPE)GetProcAddress(libeayHandleM, "_ossl_old_des_string_to_2keys")) == NULL) goto err;
//	if ((_ossl_old_des_string_to_key_FP = (_ossl_old_des_string_to_key_TYPE)GetProcAddress(libeayHandleM, "_ossl_old_des_string_to_key")) == NULL) goto err;
//	if ((_ossl_old_des_xcbc_encrypt_FP = (_ossl_old_des_xcbc_encrypt_TYPE)GetProcAddress(libeayHandleM, "_ossl_old_des_xcbc_encrypt")) == NULL) goto err;
//	if ((_ossl_old_des_xwhite_in2out_FP = (_ossl_old_des_xwhite_in2out_TYPE)GetProcAddress(libeayHandleM, "_ossl_old_des_xwhite_in2out")) == NULL) goto err;
//	if ((_shadow_DES_check_key_FP = (_shadow_DES_check_key_TYPE)GetProcAddress(libeayHandleM, "_shadow_DES_check_key")) == NULL) goto err;
//	if ((_shadow_DES_rw_mode_FP = (_shadow_DES_rw_mode_TYPE)GetProcAddress(libeayHandleM, "_shadow_DES_rw_mode")) == NULL) goto err;
//	if ((a2d_ASN1_OBJECT_FP = (a2d_ASN1_OBJECT_TYPE)GetProcAddress(libeayHandleM, "a2d_ASN1_OBJECT")) == NULL) goto err;
//	if ((a2i_ASN1_ENUMERATED_FP = (a2i_ASN1_ENUMERATED_TYPE)GetProcAddress(libeayHandleM, "a2i_ASN1_ENUMERATED")) == NULL) goto err;
//	if ((a2i_ASN1_INTEGER_FP = (a2i_ASN1_INTEGER_TYPE)GetProcAddress(libeayHandleM, "a2i_ASN1_INTEGER")) == NULL) goto err;
//	if ((a2i_ASN1_STRING_FP = (a2i_ASN1_STRING_TYPE)GetProcAddress(libeayHandleM, "a2i_ASN1_STRING")) == NULL) goto err;
//	if ((asc2uni_FP = (asc2uni_TYPE)GetProcAddress(libeayHandleM, "asc2uni")) == NULL) goto err;
//	if ((asn1_Finish_FP = (asn1_Finish_TYPE)GetProcAddress(libeayHandleM, "asn1_Finish")) == NULL) goto err;
//	if ((asn1_GetSequence_FP = (asn1_GetSequence_TYPE)GetProcAddress(libeayHandleM, "asn1_GetSequence")) == NULL) goto err;
//	if ((asn1_add_error_FP = (asn1_add_error_TYPE)GetProcAddress(libeayHandleM, "asn1_add_error")) == NULL) goto err;
//	if ((asn1_do_adb_FP = (asn1_do_adb_TYPE)GetProcAddress(libeayHandleM, "asn1_do_adb")) == NULL) goto err;
//	if ((asn1_do_lock_FP = (asn1_do_lock_TYPE)GetProcAddress(libeayHandleM, "asn1_do_lock")) == NULL) goto err;
//	if ((asn1_enc_free_FP = (asn1_enc_free_TYPE)GetProcAddress(libeayHandleM, "asn1_enc_free")) == NULL) goto err;
//	if ((asn1_enc_init_FP = (asn1_enc_init_TYPE)GetProcAddress(libeayHandleM, "asn1_enc_init")) == NULL) goto err;
//	if ((asn1_enc_restore_FP = (asn1_enc_restore_TYPE)GetProcAddress(libeayHandleM, "asn1_enc_restore")) == NULL) goto err;
//	if ((asn1_enc_save_FP = (asn1_enc_save_TYPE)GetProcAddress(libeayHandleM, "asn1_enc_save")) == NULL) goto err;
//	if ((asn1_ex_c2i_FP = (asn1_ex_c2i_TYPE)GetProcAddress(libeayHandleM, "asn1_ex_c2i")) == NULL) goto err;
//	if ((asn1_ex_i2c_FP = (asn1_ex_i2c_TYPE)GetProcAddress(libeayHandleM, "asn1_ex_i2c")) == NULL) goto err;
//	if ((asn1_get_choice_selector_FP = (asn1_get_choice_selector_TYPE)GetProcAddress(libeayHandleM, "asn1_get_choice_selector")) == NULL) goto err;
//	if ((asn1_get_field_ptr_FP = (asn1_get_field_ptr_TYPE)GetProcAddress(libeayHandleM, "asn1_get_field_ptr")) == NULL) goto err;
//	if ((asn1_set_choice_selector_FP = (asn1_set_choice_selector_TYPE)GetProcAddress(libeayHandleM, "asn1_set_choice_selector")) == NULL) goto err;
//	if ((bn_add_words_FP = (bn_add_words_TYPE)GetProcAddress(libeayHandleM, "bn_add_words")) == NULL) goto err;
//	if ((bn_div_words_FP = (bn_div_words_TYPE)GetProcAddress(libeayHandleM, "bn_div_words")) == NULL) goto err;
//	if ((bn_dup_expand_FP = (bn_dup_expand_TYPE)GetProcAddress(libeayHandleM, "bn_dup_expand")) == NULL) goto err;
//	if ((bn_expand2_FP = (bn_expand2_TYPE)GetProcAddress(libeayHandleM, "bn_expand2")) == NULL) goto err;
//	if ((bn_mul_add_words_FP = (bn_mul_add_words_TYPE)GetProcAddress(libeayHandleM, "bn_mul_add_words")) == NULL) goto err;
//	if ((bn_mul_words_FP = (bn_mul_words_TYPE)GetProcAddress(libeayHandleM, "bn_mul_words")) == NULL) goto err;
//	if ((bn_sqr_words_FP = (bn_sqr_words_TYPE)GetProcAddress(libeayHandleM, "bn_sqr_words")) == NULL) goto err;
//	if ((bn_sub_words_FP = (bn_sub_words_TYPE)GetProcAddress(libeayHandleM, "bn_sub_words")) == NULL) goto err;
//	if ((c2i_ASN1_BIT_STRING_FP = (c2i_ASN1_BIT_STRING_TYPE)GetProcAddress(libeayHandleM, "c2i_ASN1_BIT_STRING")) == NULL) goto err;
//	if ((c2i_ASN1_INTEGER_FP = (c2i_ASN1_INTEGER_TYPE)GetProcAddress(libeayHandleM, "c2i_ASN1_INTEGER")) == NULL) goto err;
//	if ((c2i_ASN1_OBJECT_FP = (c2i_ASN1_OBJECT_TYPE)GetProcAddress(libeayHandleM, "c2i_ASN1_OBJECT")) == NULL) goto err;
//	if ((d2i_ACCESS_DESCRIPTION_FP = (d2i_ACCESS_DESCRIPTION_TYPE)GetProcAddress(libeayHandleM, "d2i_ACCESS_DESCRIPTION")) == NULL) goto err;
//	if ((d2i_ASN1_BIT_STRING_FP = (d2i_ASN1_BIT_STRING_TYPE)GetProcAddress(libeayHandleM, "d2i_ASN1_BIT_STRING")) == NULL) goto err;
//	if ((d2i_ASN1_BMPSTRING_FP = (d2i_ASN1_BMPSTRING_TYPE)GetProcAddress(libeayHandleM, "d2i_ASN1_BMPSTRING")) == NULL) goto err;
//	if ((d2i_ASN1_BOOLEAN_FP = (d2i_ASN1_BOOLEAN_TYPE)GetProcAddress(libeayHandleM, "d2i_ASN1_BOOLEAN")) == NULL) goto err;
//	if ((d2i_ASN1_ENUMERATED_FP = (d2i_ASN1_ENUMERATED_TYPE)GetProcAddress(libeayHandleM, "d2i_ASN1_ENUMERATED")) == NULL) goto err;
//	if ((d2i_ASN1_GENERALIZEDTIME_FP = (d2i_ASN1_GENERALIZEDTIME_TYPE)GetProcAddress(libeayHandleM, "d2i_ASN1_GENERALIZEDTIME")) == NULL) goto err;
//	if ((d2i_ASN1_GENERALSTRING_FP = (d2i_ASN1_GENERALSTRING_TYPE)GetProcAddress(libeayHandleM, "d2i_ASN1_GENERALSTRING")) == NULL) goto err;
//	if ((d2i_ASN1_HEADER_FP = (d2i_ASN1_HEADER_TYPE)GetProcAddress(libeayHandleM, "d2i_ASN1_HEADER")) == NULL) goto err;
//	if ((d2i_ASN1_IA5STRING_FP = (d2i_ASN1_IA5STRING_TYPE)GetProcAddress(libeayHandleM, "d2i_ASN1_IA5STRING")) == NULL) goto err;
//	if ((d2i_ASN1_INTEGER_FP = (d2i_ASN1_INTEGER_TYPE)GetProcAddress(libeayHandleM, "d2i_ASN1_INTEGER")) == NULL) goto err;
//	if ((d2i_ASN1_NULL_FP = (d2i_ASN1_NULL_TYPE)GetProcAddress(libeayHandleM, "d2i_ASN1_NULL")) == NULL) goto err;
//	if ((d2i_ASN1_OBJECT_FP = (d2i_ASN1_OBJECT_TYPE)GetProcAddress(libeayHandleM, "d2i_ASN1_OBJECT")) == NULL) goto err;
//	if ((d2i_ASN1_OCTET_STRING_FP = (d2i_ASN1_OCTET_STRING_TYPE)GetProcAddress(libeayHandleM, "d2i_ASN1_OCTET_STRING")) == NULL) goto err;
//	if ((d2i_ASN1_PRINTABLESTRING_FP = (d2i_ASN1_PRINTABLESTRING_TYPE)GetProcAddress(libeayHandleM, "d2i_ASN1_PRINTABLESTRING")) == NULL) goto err;
//	if ((d2i_ASN1_PRINTABLE_FP = (d2i_ASN1_PRINTABLE_TYPE)GetProcAddress(libeayHandleM, "d2i_ASN1_PRINTABLE")) == NULL) goto err;
//	if ((d2i_ASN1_SET_FP = (d2i_ASN1_SET_TYPE)GetProcAddress(libeayHandleM, "d2i_ASN1_SET")) == NULL) goto err;
//	if ((d2i_ASN1_T61STRING_FP = (d2i_ASN1_T61STRING_TYPE)GetProcAddress(libeayHandleM, "d2i_ASN1_T61STRING")) == NULL) goto err;
//	if ((d2i_ASN1_TIME_FP = (d2i_ASN1_TIME_TYPE)GetProcAddress(libeayHandleM, "d2i_ASN1_TIME")) == NULL) goto err;
	if ((d2i_ASN1_TYPE_FP = (d2i_ASN1_TYPE_TYPE)GetProcAddress(libeayHandleM, "d2i_ASN1_TYPE")) == NULL) goto err;
//	if ((d2i_ASN1_UINTEGER_FP = (d2i_ASN1_UINTEGER_TYPE)GetProcAddress(libeayHandleM, "d2i_ASN1_UINTEGER")) == NULL) goto err;
//	if ((d2i_ASN1_UNIVERSALSTRING_FP = (d2i_ASN1_UNIVERSALSTRING_TYPE)GetProcAddress(libeayHandleM, "d2i_ASN1_UNIVERSALSTRING")) == NULL) goto err;
//	if ((d2i_ASN1_UTCTIME_FP = (d2i_ASN1_UTCTIME_TYPE)GetProcAddress(libeayHandleM, "d2i_ASN1_UTCTIME")) == NULL) goto err;
//	if ((d2i_ASN1_UTF8STRING_FP = (d2i_ASN1_UTF8STRING_TYPE)GetProcAddress(libeayHandleM, "d2i_ASN1_UTF8STRING")) == NULL) goto err;
//	if ((d2i_ASN1_VISIBLESTRING_FP = (d2i_ASN1_VISIBLESTRING_TYPE)GetProcAddress(libeayHandleM, "d2i_ASN1_VISIBLESTRING")) == NULL) goto err;
//	if ((d2i_ASN1_bytes_FP = (d2i_ASN1_bytes_TYPE)GetProcAddress(libeayHandleM, "d2i_ASN1_bytes")) == NULL) goto err;
//	if ((d2i_ASN1_type_bytes_FP = (d2i_ASN1_type_bytes_TYPE)GetProcAddress(libeayHandleM, "d2i_ASN1_type_bytes")) == NULL) goto err;
//	if ((d2i_AUTHORITY_INFO_ACCESS_FP = (d2i_AUTHORITY_INFO_ACCESS_TYPE)GetProcAddress(libeayHandleM, "d2i_AUTHORITY_INFO_ACCESS")) == NULL) goto err;
//	if ((d2i_AUTHORITY_KEYID_FP = (d2i_AUTHORITY_KEYID_TYPE)GetProcAddress(libeayHandleM, "d2i_AUTHORITY_KEYID")) == NULL) goto err;
//	if ((d2i_AutoPrivateKey_FP = (d2i_AutoPrivateKey_TYPE)GetProcAddress(libeayHandleM, "d2i_AutoPrivateKey")) == NULL) goto err;
//	if ((d2i_BASIC_CONSTRAINTS_FP = (d2i_BASIC_CONSTRAINTS_TYPE)GetProcAddress(libeayHandleM, "d2i_BASIC_CONSTRAINTS")) == NULL) goto err;
//	if ((d2i_CERTIFICATEPOLICIES_FP = (d2i_CERTIFICATEPOLICIES_TYPE)GetProcAddress(libeayHandleM, "d2i_CERTIFICATEPOLICIES")) == NULL) goto err;
//	if ((d2i_CRL_DIST_POINTS_FP = (d2i_CRL_DIST_POINTS_TYPE)GetProcAddress(libeayHandleM, "d2i_CRL_DIST_POINTS")) == NULL) goto err;
//	if ((d2i_DHparams_FP = (d2i_DHparams_TYPE)GetProcAddress(libeayHandleM, "d2i_DHparams")) == NULL) goto err;
//	if ((d2i_DIRECTORYSTRING_FP = (d2i_DIRECTORYSTRING_TYPE)GetProcAddress(libeayHandleM, "d2i_DIRECTORYSTRING")) == NULL) goto err;
//	if ((d2i_DISPLAYTEXT_FP = (d2i_DISPLAYTEXT_TYPE)GetProcAddress(libeayHandleM, "d2i_DISPLAYTEXT")) == NULL) goto err;
//	if ((d2i_DIST_POINT_FP = (d2i_DIST_POINT_TYPE)GetProcAddress(libeayHandleM, "d2i_DIST_POINT")) == NULL) goto err;
//	if ((d2i_DIST_POINT_NAME_FP = (d2i_DIST_POINT_NAME_TYPE)GetProcAddress(libeayHandleM, "d2i_DIST_POINT_NAME")) == NULL) goto err;
//	if ((d2i_DSAPrivateKey_FP = (d2i_DSAPrivateKey_TYPE)GetProcAddress(libeayHandleM, "d2i_DSAPrivateKey")) == NULL) goto err;
//	if ((d2i_DSAPrivateKey_bio_FP = (d2i_DSAPrivateKey_bio_TYPE)GetProcAddress(libeayHandleM, "d2i_DSAPrivateKey_bio")) == NULL) goto err;
//	if ((d2i_DSAPrivateKey_fp_FP = (d2i_DSAPrivateKey_fp_TYPE)GetProcAddress(libeayHandleM, "d2i_DSAPrivateKey_fp")) == NULL) goto err;
//	if ((d2i_DSAPublicKey_FP = (d2i_DSAPublicKey_TYPE)GetProcAddress(libeayHandleM, "d2i_DSAPublicKey")) == NULL) goto err;
//	if ((d2i_DSA_PUBKEY_FP = (d2i_DSA_PUBKEY_TYPE)GetProcAddress(libeayHandleM, "d2i_DSA_PUBKEY")) == NULL) goto err;
//	if ((d2i_DSA_PUBKEY_bio_FP = (d2i_DSA_PUBKEY_bio_TYPE)GetProcAddress(libeayHandleM, "d2i_DSA_PUBKEY_bio")) == NULL) goto err;
//	if ((d2i_DSA_PUBKEY_fp_FP = (d2i_DSA_PUBKEY_fp_TYPE)GetProcAddress(libeayHandleM, "d2i_DSA_PUBKEY_fp")) == NULL) goto err;
//	if ((d2i_DSA_SIG_FP = (d2i_DSA_SIG_TYPE)GetProcAddress(libeayHandleM, "d2i_DSA_SIG")) == NULL) goto err;
//	if ((d2i_DSAparams_FP = (d2i_DSAparams_TYPE)GetProcAddress(libeayHandleM, "d2i_DSAparams")) == NULL) goto err;
//	if ((d2i_EDIPARTYNAME_FP = (d2i_EDIPARTYNAME_TYPE)GetProcAddress(libeayHandleM, "d2i_EDIPARTYNAME")) == NULL) goto err;
//	if ((d2i_EXTENDED_KEY_USAGE_FP = (d2i_EXTENDED_KEY_USAGE_TYPE)GetProcAddress(libeayHandleM, "d2i_EXTENDED_KEY_USAGE")) == NULL) goto err;
//	if ((d2i_GENERAL_NAMES_FP = (d2i_GENERAL_NAMES_TYPE)GetProcAddress(libeayHandleM, "d2i_GENERAL_NAMES")) == NULL) goto err;
//	if ((d2i_GENERAL_NAME_FP = (d2i_GENERAL_NAME_TYPE)GetProcAddress(libeayHandleM, "d2i_GENERAL_NAME")) == NULL) goto err;
//	if ((d2i_KRB5_APREQBODY_FP = (d2i_KRB5_APREQBODY_TYPE)GetProcAddress(libeayHandleM, "d2i_KRB5_APREQBODY")) == NULL) goto err;
//	if ((d2i_KRB5_APREQ_FP = (d2i_KRB5_APREQ_TYPE)GetProcAddress(libeayHandleM, "d2i_KRB5_APREQ")) == NULL) goto err;
//	if ((d2i_KRB5_AUTHDATA_FP = (d2i_KRB5_AUTHDATA_TYPE)GetProcAddress(libeayHandleM, "d2i_KRB5_AUTHDATA")) == NULL) goto err;
//	if ((d2i_KRB5_AUTHENTBODY_FP = (d2i_KRB5_AUTHENTBODY_TYPE)GetProcAddress(libeayHandleM, "d2i_KRB5_AUTHENTBODY")) == NULL) goto err;
//	if ((d2i_KRB5_AUTHENT_FP = (d2i_KRB5_AUTHENT_TYPE)GetProcAddress(libeayHandleM, "d2i_KRB5_AUTHENT")) == NULL) goto err;
//	if ((d2i_KRB5_CHECKSUM_FP = (d2i_KRB5_CHECKSUM_TYPE)GetProcAddress(libeayHandleM, "d2i_KRB5_CHECKSUM")) == NULL) goto err;
//	if ((d2i_KRB5_ENCDATA_FP = (d2i_KRB5_ENCDATA_TYPE)GetProcAddress(libeayHandleM, "d2i_KRB5_ENCDATA")) == NULL) goto err;
//	if ((d2i_KRB5_ENCKEY_FP = (d2i_KRB5_ENCKEY_TYPE)GetProcAddress(libeayHandleM, "d2i_KRB5_ENCKEY")) == NULL) goto err;
//	if ((d2i_KRB5_PRINCNAME_FP = (d2i_KRB5_PRINCNAME_TYPE)GetProcAddress(libeayHandleM, "d2i_KRB5_PRINCNAME")) == NULL) goto err;
//	if ((d2i_KRB5_TICKET_FP = (d2i_KRB5_TICKET_TYPE)GetProcAddress(libeayHandleM, "d2i_KRB5_TICKET")) == NULL) goto err;
//	if ((d2i_KRB5_TKTBODY_FP = (d2i_KRB5_TKTBODY_TYPE)GetProcAddress(libeayHandleM, "d2i_KRB5_TKTBODY")) == NULL) goto err;
//	if ((d2i_NETSCAPE_CERT_SEQUENCE_FP = (d2i_NETSCAPE_CERT_SEQUENCE_TYPE)GetProcAddress(libeayHandleM, "d2i_NETSCAPE_CERT_SEQUENCE")) == NULL) goto err;
//	if ((d2i_NETSCAPE_SPKAC_FP = (d2i_NETSCAPE_SPKAC_TYPE)GetProcAddress(libeayHandleM, "d2i_NETSCAPE_SPKAC")) == NULL) goto err;
//	if ((d2i_NETSCAPE_SPKI_FP = (d2i_NETSCAPE_SPKI_TYPE)GetProcAddress(libeayHandleM, "d2i_NETSCAPE_SPKI")) == NULL) goto err;
//	if ((d2i_NOTICEREF_FP = (d2i_NOTICEREF_TYPE)GetProcAddress(libeayHandleM, "d2i_NOTICEREF")) == NULL) goto err;
//	if ((d2i_Netscape_RSA_FP = (d2i_Netscape_RSA_TYPE)GetProcAddress(libeayHandleM, "d2i_Netscape_RSA")) == NULL) goto err;
//	if ((d2i_OCSP_BASICRESP_FP = (d2i_OCSP_BASICRESP_TYPE)GetProcAddress(libeayHandleM, "d2i_OCSP_BASICRESP")) == NULL) goto err;
//	if ((d2i_OCSP_CERTID_FP = (d2i_OCSP_CERTID_TYPE)GetProcAddress(libeayHandleM, "d2i_OCSP_CERTID")) == NULL) goto err;
//	if ((d2i_OCSP_CERTSTATUS_FP = (d2i_OCSP_CERTSTATUS_TYPE)GetProcAddress(libeayHandleM, "d2i_OCSP_CERTSTATUS")) == NULL) goto err;
//	if ((d2i_OCSP_CRLID_FP = (d2i_OCSP_CRLID_TYPE)GetProcAddress(libeayHandleM, "d2i_OCSP_CRLID")) == NULL) goto err;
//	if ((d2i_OCSP_ONEREQ_FP = (d2i_OCSP_ONEREQ_TYPE)GetProcAddress(libeayHandleM, "d2i_OCSP_ONEREQ")) == NULL) goto err;
//	if ((d2i_OCSP_REQINFO_FP = (d2i_OCSP_REQINFO_TYPE)GetProcAddress(libeayHandleM, "d2i_OCSP_REQINFO")) == NULL) goto err;
//	if ((d2i_OCSP_REQUEST_FP = (d2i_OCSP_REQUEST_TYPE)GetProcAddress(libeayHandleM, "d2i_OCSP_REQUEST")) == NULL) goto err;
//	if ((d2i_OCSP_RESPBYTES_FP = (d2i_OCSP_RESPBYTES_TYPE)GetProcAddress(libeayHandleM, "d2i_OCSP_RESPBYTES")) == NULL) goto err;
//	if ((d2i_OCSP_RESPDATA_FP = (d2i_OCSP_RESPDATA_TYPE)GetProcAddress(libeayHandleM, "d2i_OCSP_RESPDATA")) == NULL) goto err;
//	if ((d2i_OCSP_RESPID_FP = (d2i_OCSP_RESPID_TYPE)GetProcAddress(libeayHandleM, "d2i_OCSP_RESPID")) == NULL) goto err;
//	if ((d2i_OCSP_RESPONSE_FP = (d2i_OCSP_RESPONSE_TYPE)GetProcAddress(libeayHandleM, "d2i_OCSP_RESPONSE")) == NULL) goto err;
//	if ((d2i_OCSP_REVOKEDINFO_FP = (d2i_OCSP_REVOKEDINFO_TYPE)GetProcAddress(libeayHandleM, "d2i_OCSP_REVOKEDINFO")) == NULL) goto err;
//	if ((d2i_OCSP_SERVICELOC_FP = (d2i_OCSP_SERVICELOC_TYPE)GetProcAddress(libeayHandleM, "d2i_OCSP_SERVICELOC")) == NULL) goto err;
//	if ((d2i_OCSP_SIGNATURE_FP = (d2i_OCSP_SIGNATURE_TYPE)GetProcAddress(libeayHandleM, "d2i_OCSP_SIGNATURE")) == NULL) goto err;
//	if ((d2i_OCSP_SINGLERESP_FP = (d2i_OCSP_SINGLERESP_TYPE)GetProcAddress(libeayHandleM, "d2i_OCSP_SINGLERESP")) == NULL) goto err;
//	if ((d2i_OTHERNAME_FP = (d2i_OTHERNAME_TYPE)GetProcAddress(libeayHandleM, "d2i_OTHERNAME")) == NULL) goto err;
//	if ((d2i_PBE2PARAM_FP = (d2i_PBE2PARAM_TYPE)GetProcAddress(libeayHandleM, "d2i_PBE2PARAM")) == NULL) goto err;
//	if ((d2i_PBEPARAM_FP = (d2i_PBEPARAM_TYPE)GetProcAddress(libeayHandleM, "d2i_PBEPARAM")) == NULL) goto err;
//	if ((d2i_PBKDF2PARAM_FP = (d2i_PBKDF2PARAM_TYPE)GetProcAddress(libeayHandleM, "d2i_PBKDF2PARAM")) == NULL) goto err;
//	if ((d2i_PKCS12_FP = (d2i_PKCS12_TYPE)GetProcAddress(libeayHandleM, "d2i_PKCS12")) == NULL) goto err;
//	if ((d2i_PKCS12_BAGS_FP = (d2i_PKCS12_BAGS_TYPE)GetProcAddress(libeayHandleM, "d2i_PKCS12_BAGS")) == NULL) goto err;
//	if ((d2i_PKCS12_MAC_DATA_FP = (d2i_PKCS12_MAC_DATA_TYPE)GetProcAddress(libeayHandleM, "d2i_PKCS12_MAC_DATA")) == NULL) goto err;
//	if ((d2i_PKCS12_SAFEBAG_FP = (d2i_PKCS12_SAFEBAG_TYPE)GetProcAddress(libeayHandleM, "d2i_PKCS12_SAFEBAG")) == NULL) goto err;
	if ((d2i_PKCS12_bio_FP = (d2i_PKCS12_bio_TYPE)GetProcAddress(libeayHandleM, "d2i_PKCS12_bio")) == NULL) goto err;
//	if ((d2i_PKCS12_fp_FP = (d2i_PKCS12_fp_TYPE)GetProcAddress(libeayHandleM, "d2i_PKCS12_fp")) == NULL) goto err;
//	if ((d2i_PKCS7_FP = (d2i_PKCS7_TYPE)GetProcAddress(libeayHandleM, "d2i_PKCS7")) == NULL) goto err;
//	if ((d2i_PKCS7_DIGEST_FP = (d2i_PKCS7_DIGEST_TYPE)GetProcAddress(libeayHandleM, "d2i_PKCS7_DIGEST")) == NULL) goto err;
//	if ((d2i_PKCS7_ENCRYPT_FP = (d2i_PKCS7_ENCRYPT_TYPE)GetProcAddress(libeayHandleM, "d2i_PKCS7_ENCRYPT")) == NULL) goto err;
//	if ((d2i_PKCS7_ENC_CONTENT_FP = (d2i_PKCS7_ENC_CONTENT_TYPE)GetProcAddress(libeayHandleM, "d2i_PKCS7_ENC_CONTENT")) == NULL) goto err;
//	if ((d2i_PKCS7_ENVELOPE_FP = (d2i_PKCS7_ENVELOPE_TYPE)GetProcAddress(libeayHandleM, "d2i_PKCS7_ENVELOPE")) == NULL) goto err;
//	if ((d2i_PKCS7_ISSUER_AND_SERIAL_FP = (d2i_PKCS7_ISSUER_AND_SERIAL_TYPE)GetProcAddress(libeayHandleM, "d2i_PKCS7_ISSUER_AND_SERIAL")) == NULL) goto err;
//	if ((d2i_PKCS7_RECIP_INFO_FP = (d2i_PKCS7_RECIP_INFO_TYPE)GetProcAddress(libeayHandleM, "d2i_PKCS7_RECIP_INFO")) == NULL) goto err;
//	if ((d2i_PKCS7_SIGNED_FP = (d2i_PKCS7_SIGNED_TYPE)GetProcAddress(libeayHandleM, "d2i_PKCS7_SIGNED")) == NULL) goto err;
//	if ((d2i_PKCS7_SIGNER_INFO_FP = (d2i_PKCS7_SIGNER_INFO_TYPE)GetProcAddress(libeayHandleM, "d2i_PKCS7_SIGNER_INFO")) == NULL) goto err;
//	if ((d2i_PKCS7_SIGN_ENVELOPE_FP = (d2i_PKCS7_SIGN_ENVELOPE_TYPE)GetProcAddress(libeayHandleM, "d2i_PKCS7_SIGN_ENVELOPE")) == NULL) goto err;
	if ((d2i_PKCS7_bio_FP = (d2i_PKCS7_bio_TYPE)GetProcAddress(libeayHandleM, "d2i_PKCS7_bio")) == NULL) goto err;
//	if ((d2i_PKCS7_fp_FP = (d2i_PKCS7_fp_TYPE)GetProcAddress(libeayHandleM, "d2i_PKCS7_fp")) == NULL) goto err;
//	if ((d2i_PKCS8PrivateKey_bio_FP = (d2i_PKCS8PrivateKey_bio_TYPE)GetProcAddress(libeayHandleM, "d2i_PKCS8PrivateKey_bio")) == NULL) goto err;
//	if ((d2i_PKCS8PrivateKey_fp_FP = (d2i_PKCS8PrivateKey_fp_TYPE)GetProcAddress(libeayHandleM, "d2i_PKCS8PrivateKey_fp")) == NULL) goto err;
	if ((d2i_PKCS8_PRIV_KEY_INFO_FP = (d2i_PKCS8_PRIV_KEY_INFO_TYPE)GetProcAddress(libeayHandleM, "d2i_PKCS8_PRIV_KEY_INFO")) == NULL) goto err;
//	if ((d2i_PKCS8_PRIV_KEY_INFO_bio_FP = (d2i_PKCS8_PRIV_KEY_INFO_bio_TYPE)GetProcAddress(libeayHandleM, "d2i_PKCS8_PRIV_KEY_INFO_bio")) == NULL) goto err;
//	if ((d2i_PKCS8_PRIV_KEY_INFO_fp_FP = (d2i_PKCS8_PRIV_KEY_INFO_fp_TYPE)GetProcAddress(libeayHandleM, "d2i_PKCS8_PRIV_KEY_INFO_fp")) == NULL) goto err;
//	if ((d2i_PKCS8_bio_FP = (d2i_PKCS8_bio_TYPE)GetProcAddress(libeayHandleM, "d2i_PKCS8_bio")) == NULL) goto err;
//	if ((d2i_PKCS8_fp_FP = (d2i_PKCS8_fp_TYPE)GetProcAddress(libeayHandleM, "d2i_PKCS8_fp")) == NULL) goto err;
//	if ((d2i_PKEY_USAGE_PERIOD_FP = (d2i_PKEY_USAGE_PERIOD_TYPE)GetProcAddress(libeayHandleM, "d2i_PKEY_USAGE_PERIOD")) == NULL) goto err;
//	if ((d2i_POLICYINFO_FP = (d2i_POLICYINFO_TYPE)GetProcAddress(libeayHandleM, "d2i_POLICYINFO")) == NULL) goto err;
//	if ((d2i_POLICYQUALINFO_FP = (d2i_POLICYQUALINFO_TYPE)GetProcAddress(libeayHandleM, "d2i_POLICYQUALINFO")) == NULL) goto err;
//	if ((d2i_PUBKEY_FP = (d2i_PUBKEY_TYPE)GetProcAddress(libeayHandleM, "d2i_PUBKEY")) == NULL) goto err;
//	if ((d2i_PUBKEY_bio_FP = (d2i_PUBKEY_bio_TYPE)GetProcAddress(libeayHandleM, "d2i_PUBKEY_bio")) == NULL) goto err;
//	if ((d2i_PUBKEY_fp_FP = (d2i_PUBKEY_fp_TYPE)GetProcAddress(libeayHandleM, "d2i_PUBKEY_fp")) == NULL) goto err;
	if ((d2i_PrivateKey_FP = (d2i_PrivateKey_TYPE)GetProcAddress(libeayHandleM, "d2i_PrivateKey")) == NULL) goto err;
//	if ((d2i_PrivateKey_bio_FP = (d2i_PrivateKey_bio_TYPE)GetProcAddress(libeayHandleM, "d2i_PrivateKey_bio")) == NULL) goto err;
//	if ((d2i_PrivateKey_fp_FP = (d2i_PrivateKey_fp_TYPE)GetProcAddress(libeayHandleM, "d2i_PrivateKey_fp")) == NULL) goto err;
//	if ((d2i_PublicKey_FP = (d2i_PublicKey_TYPE)GetProcAddress(libeayHandleM, "d2i_PublicKey")) == NULL) goto err;
//	if ((d2i_RSAPrivateKey_FP = (d2i_RSAPrivateKey_TYPE)GetProcAddress(libeayHandleM, "d2i_RSAPrivateKey")) == NULL) goto err;
//	if ((d2i_RSAPrivateKey_bio_FP = (d2i_RSAPrivateKey_bio_TYPE)GetProcAddress(libeayHandleM, "d2i_RSAPrivateKey_bio")) == NULL) goto err;
//	if ((d2i_RSAPrivateKey_fp_FP = (d2i_RSAPrivateKey_fp_TYPE)GetProcAddress(libeayHandleM, "d2i_RSAPrivateKey_fp")) == NULL) goto err;
//	if ((d2i_RSAPublicKey_FP = (d2i_RSAPublicKey_TYPE)GetProcAddress(libeayHandleM, "d2i_RSAPublicKey")) == NULL) goto err;
//	if ((d2i_RSAPublicKey_bio_FP = (d2i_RSAPublicKey_bio_TYPE)GetProcAddress(libeayHandleM, "d2i_RSAPublicKey_bio")) == NULL) goto err;
//	if ((d2i_RSAPublicKey_fp_FP = (d2i_RSAPublicKey_fp_TYPE)GetProcAddress(libeayHandleM, "d2i_RSAPublicKey_fp")) == NULL) goto err;
//	if ((d2i_RSA_NET_FP = (d2i_RSA_NET_TYPE)GetProcAddress(libeayHandleM, "d2i_RSA_NET")) == NULL) goto err;
//	if ((d2i_RSA_PUBKEY_FP = (d2i_RSA_PUBKEY_TYPE)GetProcAddress(libeayHandleM, "d2i_RSA_PUBKEY")) == NULL) goto err;
//	if ((d2i_RSA_PUBKEY_bio_FP = (d2i_RSA_PUBKEY_bio_TYPE)GetProcAddress(libeayHandleM, "d2i_RSA_PUBKEY_bio")) == NULL) goto err;
//	if ((d2i_RSA_PUBKEY_fp_FP = (d2i_RSA_PUBKEY_fp_TYPE)GetProcAddress(libeayHandleM, "d2i_RSA_PUBKEY_fp")) == NULL) goto err;
//	if ((d2i_SXNETID_FP = (d2i_SXNETID_TYPE)GetProcAddress(libeayHandleM, "d2i_SXNETID")) == NULL) goto err;
//	if ((d2i_SXNET_FP = (d2i_SXNET_TYPE)GetProcAddress(libeayHandleM, "d2i_SXNET")) == NULL) goto err;
//	if ((d2i_USERNOTICE_FP = (d2i_USERNOTICE_TYPE)GetProcAddress(libeayHandleM, "d2i_USERNOTICE")) == NULL) goto err;
	if ((d2i_X509_FP = (d2i_X509_TYPE)GetProcAddress(libeayHandleM, "d2i_X509")) == NULL) goto err;
//	if ((d2i_X509_ALGOR_FP = (d2i_X509_ALGOR_TYPE)GetProcAddress(libeayHandleM, "d2i_X509_ALGOR")) == NULL) goto err;
//	if ((d2i_X509_ATTRIBUTE_FP = (d2i_X509_ATTRIBUTE_TYPE)GetProcAddress(libeayHandleM, "d2i_X509_ATTRIBUTE")) == NULL) goto err;
//	if ((d2i_X509_AUX_FP = (d2i_X509_AUX_TYPE)GetProcAddress(libeayHandleM, "d2i_X509_AUX")) == NULL) goto err;
//	if ((d2i_X509_CERT_AUX_FP = (d2i_X509_CERT_AUX_TYPE)GetProcAddress(libeayHandleM, "d2i_X509_CERT_AUX")) == NULL) goto err;
//	if ((d2i_X509_CINF_FP = (d2i_X509_CINF_TYPE)GetProcAddress(libeayHandleM, "d2i_X509_CINF")) == NULL) goto err;
//	if ((d2i_X509_CRL_FP = (d2i_X509_CRL_TYPE)GetProcAddress(libeayHandleM, "d2i_X509_CRL")) == NULL) goto err;
//	if ((d2i_X509_CRL_INFO_FP = (d2i_X509_CRL_INFO_TYPE)GetProcAddress(libeayHandleM, "d2i_X509_CRL_INFO")) == NULL) goto err;
//	if ((d2i_X509_CRL_bio_FP = (d2i_X509_CRL_bio_TYPE)GetProcAddress(libeayHandleM, "d2i_X509_CRL_bio")) == NULL) goto err;
//	if ((d2i_X509_CRL_fp_FP = (d2i_X509_CRL_fp_TYPE)GetProcAddress(libeayHandleM, "d2i_X509_CRL_fp")) == NULL) goto err;
//	if ((d2i_X509_EXTENSION_FP = (d2i_X509_EXTENSION_TYPE)GetProcAddress(libeayHandleM, "d2i_X509_EXTENSION")) == NULL) goto err;
//	if ((d2i_X509_NAME_FP = (d2i_X509_NAME_TYPE)GetProcAddress(libeayHandleM, "d2i_X509_NAME")) == NULL) goto err;
//	if ((d2i_X509_NAME_ENTRY_FP = (d2i_X509_NAME_ENTRY_TYPE)GetProcAddress(libeayHandleM, "d2i_X509_NAME_ENTRY")) == NULL) goto err;
//	if ((d2i_X509_PKEY_FP = (d2i_X509_PKEY_TYPE)GetProcAddress(libeayHandleM, "d2i_X509_PKEY")) == NULL) goto err;
//	if ((d2i_X509_PUBKEY_FP = (d2i_X509_PUBKEY_TYPE)GetProcAddress(libeayHandleM, "d2i_X509_PUBKEY")) == NULL) goto err;
//	if ((d2i_X509_REQ_FP = (d2i_X509_REQ_TYPE)GetProcAddress(libeayHandleM, "d2i_X509_REQ")) == NULL) goto err;
//	if ((d2i_X509_REQ_INFO_FP = (d2i_X509_REQ_INFO_TYPE)GetProcAddress(libeayHandleM, "d2i_X509_REQ_INFO")) == NULL) goto err;
//	if ((d2i_X509_REQ_bio_FP = (d2i_X509_REQ_bio_TYPE)GetProcAddress(libeayHandleM, "d2i_X509_REQ_bio")) == NULL) goto err;
//	if ((d2i_X509_REQ_fp_FP = (d2i_X509_REQ_fp_TYPE)GetProcAddress(libeayHandleM, "d2i_X509_REQ_fp")) == NULL) goto err;
//	if ((d2i_X509_REVOKED_FP = (d2i_X509_REVOKED_TYPE)GetProcAddress(libeayHandleM, "d2i_X509_REVOKED")) == NULL) goto err;
	if ((d2i_X509_SIG_FP = (d2i_X509_SIG_TYPE)GetProcAddress(libeayHandleM, "d2i_X509_SIG")) == NULL) goto err;
//	if ((d2i_X509_VAL_FP = (d2i_X509_VAL_TYPE)GetProcAddress(libeayHandleM, "d2i_X509_VAL")) == NULL) goto err;
//	if ((d2i_X509_bio_FP = (d2i_X509_bio_TYPE)GetProcAddress(libeayHandleM, "d2i_X509_bio")) == NULL) goto err;
//	if ((d2i_X509_fp_FP = (d2i_X509_fp_TYPE)GetProcAddress(libeayHandleM, "d2i_X509_fp")) == NULL) goto err;
//	if ((hex_to_string_FP = (hex_to_string_TYPE)GetProcAddress(libeayHandleM, "hex_to_string")) == NULL) goto err;
//	if ((i2a_ACCESS_DESCRIPTION_FP = (i2a_ACCESS_DESCRIPTION_TYPE)GetProcAddress(libeayHandleM, "i2a_ACCESS_DESCRIPTION")) == NULL) goto err;
//	if ((i2a_ASN1_ENUMERATED_FP = (i2a_ASN1_ENUMERATED_TYPE)GetProcAddress(libeayHandleM, "i2a_ASN1_ENUMERATED")) == NULL) goto err;
//	if ((i2a_ASN1_INTEGER_FP = (i2a_ASN1_INTEGER_TYPE)GetProcAddress(libeayHandleM, "i2a_ASN1_INTEGER")) == NULL) goto err;
//	if ((i2a_ASN1_OBJECT_FP = (i2a_ASN1_OBJECT_TYPE)GetProcAddress(libeayHandleM, "i2a_ASN1_OBJECT")) == NULL) goto err;
//	if ((i2a_ASN1_STRING_FP = (i2a_ASN1_STRING_TYPE)GetProcAddress(libeayHandleM, "i2a_ASN1_STRING")) == NULL) goto err;
//	if ((i2c_ASN1_BIT_STRING_FP = (i2c_ASN1_BIT_STRING_TYPE)GetProcAddress(libeayHandleM, "i2c_ASN1_BIT_STRING")) == NULL) goto err;
//	if ((i2c_ASN1_INTEGER_FP = (i2c_ASN1_INTEGER_TYPE)GetProcAddress(libeayHandleM, "i2c_ASN1_INTEGER")) == NULL) goto err;
//	if ((i2d_ACCESS_DESCRIPTION_FP = (i2d_ACCESS_DESCRIPTION_TYPE)GetProcAddress(libeayHandleM, "i2d_ACCESS_DESCRIPTION")) == NULL) goto err;
//	if ((i2d_ASN1_BIT_STRING_FP = (i2d_ASN1_BIT_STRING_TYPE)GetProcAddress(libeayHandleM, "i2d_ASN1_BIT_STRING")) == NULL) goto err;
//	if ((i2d_ASN1_BMPSTRING_FP = (i2d_ASN1_BMPSTRING_TYPE)GetProcAddress(libeayHandleM, "i2d_ASN1_BMPSTRING")) == NULL) goto err;
//	if ((i2d_ASN1_BOOLEAN_FP = (i2d_ASN1_BOOLEAN_TYPE)GetProcAddress(libeayHandleM, "i2d_ASN1_BOOLEAN")) == NULL) goto err;
//	if ((i2d_ASN1_ENUMERATED_FP = (i2d_ASN1_ENUMERATED_TYPE)GetProcAddress(libeayHandleM, "i2d_ASN1_ENUMERATED")) == NULL) goto err;
//	if ((i2d_ASN1_GENERALIZEDTIME_FP = (i2d_ASN1_GENERALIZEDTIME_TYPE)GetProcAddress(libeayHandleM, "i2d_ASN1_GENERALIZEDTIME")) == NULL) goto err;
//	if ((i2d_ASN1_GENERALSTRING_FP = (i2d_ASN1_GENERALSTRING_TYPE)GetProcAddress(libeayHandleM, "i2d_ASN1_GENERALSTRING")) == NULL) goto err;
//	if ((i2d_ASN1_HEADER_FP = (i2d_ASN1_HEADER_TYPE)GetProcAddress(libeayHandleM, "i2d_ASN1_HEADER")) == NULL) goto err;
//	if ((i2d_ASN1_IA5STRING_FP = (i2d_ASN1_IA5STRING_TYPE)GetProcAddress(libeayHandleM, "i2d_ASN1_IA5STRING")) == NULL) goto err;
//	if ((i2d_ASN1_INTEGER_FP = (i2d_ASN1_INTEGER_TYPE)GetProcAddress(libeayHandleM, "i2d_ASN1_INTEGER")) == NULL) goto err;
//	if ((i2d_ASN1_NULL_FP = (i2d_ASN1_NULL_TYPE)GetProcAddress(libeayHandleM, "i2d_ASN1_NULL")) == NULL) goto err;
//	if ((i2d_ASN1_OBJECT_FP = (i2d_ASN1_OBJECT_TYPE)GetProcAddress(libeayHandleM, "i2d_ASN1_OBJECT")) == NULL) goto err;
//	if ((i2d_ASN1_OCTET_STRING_FP = (i2d_ASN1_OCTET_STRING_TYPE)GetProcAddress(libeayHandleM, "i2d_ASN1_OCTET_STRING")) == NULL) goto err;
//	if ((i2d_ASN1_PRINTABLESTRING_FP = (i2d_ASN1_PRINTABLESTRING_TYPE)GetProcAddress(libeayHandleM, "i2d_ASN1_PRINTABLESTRING")) == NULL) goto err;
//	if ((i2d_ASN1_PRINTABLE_FP = (i2d_ASN1_PRINTABLE_TYPE)GetProcAddress(libeayHandleM, "i2d_ASN1_PRINTABLE")) == NULL) goto err;
//	if ((i2d_ASN1_SET_FP = (i2d_ASN1_SET_TYPE)GetProcAddress(libeayHandleM, "i2d_ASN1_SET")) == NULL) goto err;
//	if ((i2d_ASN1_T61STRING_FP = (i2d_ASN1_T61STRING_TYPE)GetProcAddress(libeayHandleM, "i2d_ASN1_T61STRING")) == NULL) goto err;
//	if ((i2d_ASN1_TIME_FP = (i2d_ASN1_TIME_TYPE)GetProcAddress(libeayHandleM, "i2d_ASN1_TIME")) == NULL) goto err;
//	if ((i2d_ASN1_TYPE_FP = (i2d_ASN1_TYPE_TYPE)GetProcAddress(libeayHandleM, "i2d_ASN1_TYPE")) == NULL) goto err;
//	if ((i2d_ASN1_UNIVERSALSTRING_FP = (i2d_ASN1_UNIVERSALSTRING_TYPE)GetProcAddress(libeayHandleM, "i2d_ASN1_UNIVERSALSTRING")) == NULL) goto err;
//	if ((i2d_ASN1_UTCTIME_FP = (i2d_ASN1_UTCTIME_TYPE)GetProcAddress(libeayHandleM, "i2d_ASN1_UTCTIME")) == NULL) goto err;
//	if ((i2d_ASN1_UTF8STRING_FP = (i2d_ASN1_UTF8STRING_TYPE)GetProcAddress(libeayHandleM, "i2d_ASN1_UTF8STRING")) == NULL) goto err;
//	if ((i2d_ASN1_VISIBLESTRING_FP = (i2d_ASN1_VISIBLESTRING_TYPE)GetProcAddress(libeayHandleM, "i2d_ASN1_VISIBLESTRING")) == NULL) goto err;
//	if ((i2d_ASN1_bytes_FP = (i2d_ASN1_bytes_TYPE)GetProcAddress(libeayHandleM, "i2d_ASN1_bytes")) == NULL) goto err;
//	if ((i2d_AUTHORITY_INFO_ACCESS_FP = (i2d_AUTHORITY_INFO_ACCESS_TYPE)GetProcAddress(libeayHandleM, "i2d_AUTHORITY_INFO_ACCESS")) == NULL) goto err;
//	if ((i2d_AUTHORITY_KEYID_FP = (i2d_AUTHORITY_KEYID_TYPE)GetProcAddress(libeayHandleM, "i2d_AUTHORITY_KEYID")) == NULL) goto err;
//	if ((i2d_BASIC_CONSTRAINTS_FP = (i2d_BASIC_CONSTRAINTS_TYPE)GetProcAddress(libeayHandleM, "i2d_BASIC_CONSTRAINTS")) == NULL) goto err;
//	if ((i2d_CERTIFICATEPOLICIES_FP = (i2d_CERTIFICATEPOLICIES_TYPE)GetProcAddress(libeayHandleM, "i2d_CERTIFICATEPOLICIES")) == NULL) goto err;
//	if ((i2d_CRL_DIST_POINTS_FP = (i2d_CRL_DIST_POINTS_TYPE)GetProcAddress(libeayHandleM, "i2d_CRL_DIST_POINTS")) == NULL) goto err;
//	if ((i2d_DHparams_FP = (i2d_DHparams_TYPE)GetProcAddress(libeayHandleM, "i2d_DHparams")) == NULL) goto err;
//	if ((i2d_DIRECTORYSTRING_FP = (i2d_DIRECTORYSTRING_TYPE)GetProcAddress(libeayHandleM, "i2d_DIRECTORYSTRING")) == NULL) goto err;
//	if ((i2d_DISPLAYTEXT_FP = (i2d_DISPLAYTEXT_TYPE)GetProcAddress(libeayHandleM, "i2d_DISPLAYTEXT")) == NULL) goto err;
//	if ((i2d_DIST_POINT_FP = (i2d_DIST_POINT_TYPE)GetProcAddress(libeayHandleM, "i2d_DIST_POINT")) == NULL) goto err;
//	if ((i2d_DIST_POINT_NAME_FP = (i2d_DIST_POINT_NAME_TYPE)GetProcAddress(libeayHandleM, "i2d_DIST_POINT_NAME")) == NULL) goto err;
//	if ((i2d_DSAPrivateKey_FP = (i2d_DSAPrivateKey_TYPE)GetProcAddress(libeayHandleM, "i2d_DSAPrivateKey")) == NULL) goto err;
//	if ((i2d_DSAPrivateKey_bio_FP = (i2d_DSAPrivateKey_bio_TYPE)GetProcAddress(libeayHandleM, "i2d_DSAPrivateKey_bio")) == NULL) goto err;
//	if ((i2d_DSAPrivateKey_fp_FP = (i2d_DSAPrivateKey_fp_TYPE)GetProcAddress(libeayHandleM, "i2d_DSAPrivateKey_fp")) == NULL) goto err;
//	if ((i2d_DSAPublicKey_FP = (i2d_DSAPublicKey_TYPE)GetProcAddress(libeayHandleM, "i2d_DSAPublicKey")) == NULL) goto err;
//	if ((i2d_DSA_PUBKEY_FP = (i2d_DSA_PUBKEY_TYPE)GetProcAddress(libeayHandleM, "i2d_DSA_PUBKEY")) == NULL) goto err;
//	if ((i2d_DSA_PUBKEY_bio_FP = (i2d_DSA_PUBKEY_bio_TYPE)GetProcAddress(libeayHandleM, "i2d_DSA_PUBKEY_bio")) == NULL) goto err;
//	if ((i2d_DSA_PUBKEY_fp_FP = (i2d_DSA_PUBKEY_fp_TYPE)GetProcAddress(libeayHandleM, "i2d_DSA_PUBKEY_fp")) == NULL) goto err;
//	if ((i2d_DSA_SIG_FP = (i2d_DSA_SIG_TYPE)GetProcAddress(libeayHandleM, "i2d_DSA_SIG")) == NULL) goto err;
//	if ((i2d_DSAparams_FP = (i2d_DSAparams_TYPE)GetProcAddress(libeayHandleM, "i2d_DSAparams")) == NULL) goto err;
//	if ((i2d_EDIPARTYNAME_FP = (i2d_EDIPARTYNAME_TYPE)GetProcAddress(libeayHandleM, "i2d_EDIPARTYNAME")) == NULL) goto err;
//	if ((i2d_EXTENDED_KEY_USAGE_FP = (i2d_EXTENDED_KEY_USAGE_TYPE)GetProcAddress(libeayHandleM, "i2d_EXTENDED_KEY_USAGE")) == NULL) goto err;
//	if ((i2d_GENERAL_NAMES_FP = (i2d_GENERAL_NAMES_TYPE)GetProcAddress(libeayHandleM, "i2d_GENERAL_NAMES")) == NULL) goto err;
//	if ((i2d_GENERAL_NAME_FP = (i2d_GENERAL_NAME_TYPE)GetProcAddress(libeayHandleM, "i2d_GENERAL_NAME")) == NULL) goto err;
//	if ((i2d_KRB5_APREQBODY_FP = (i2d_KRB5_APREQBODY_TYPE)GetProcAddress(libeayHandleM, "i2d_KRB5_APREQBODY")) == NULL) goto err;
//	if ((i2d_KRB5_APREQ_FP = (i2d_KRB5_APREQ_TYPE)GetProcAddress(libeayHandleM, "i2d_KRB5_APREQ")) == NULL) goto err;
//	if ((i2d_KRB5_AUTHDATA_FP = (i2d_KRB5_AUTHDATA_TYPE)GetProcAddress(libeayHandleM, "i2d_KRB5_AUTHDATA")) == NULL) goto err;
//	if ((i2d_KRB5_AUTHENTBODY_FP = (i2d_KRB5_AUTHENTBODY_TYPE)GetProcAddress(libeayHandleM, "i2d_KRB5_AUTHENTBODY")) == NULL) goto err;
//	if ((i2d_KRB5_AUTHENT_FP = (i2d_KRB5_AUTHENT_TYPE)GetProcAddress(libeayHandleM, "i2d_KRB5_AUTHENT")) == NULL) goto err;
//	if ((i2d_KRB5_CHECKSUM_FP = (i2d_KRB5_CHECKSUM_TYPE)GetProcAddress(libeayHandleM, "i2d_KRB5_CHECKSUM")) == NULL) goto err;
//	if ((i2d_KRB5_ENCDATA_FP = (i2d_KRB5_ENCDATA_TYPE)GetProcAddress(libeayHandleM, "i2d_KRB5_ENCDATA")) == NULL) goto err;
//	if ((i2d_KRB5_ENCKEY_FP = (i2d_KRB5_ENCKEY_TYPE)GetProcAddress(libeayHandleM, "i2d_KRB5_ENCKEY")) == NULL) goto err;
//	if ((i2d_KRB5_PRINCNAME_FP = (i2d_KRB5_PRINCNAME_TYPE)GetProcAddress(libeayHandleM, "i2d_KRB5_PRINCNAME")) == NULL) goto err;
//	if ((i2d_KRB5_TICKET_FP = (i2d_KRB5_TICKET_TYPE)GetProcAddress(libeayHandleM, "i2d_KRB5_TICKET")) == NULL) goto err;
//	if ((i2d_KRB5_TKTBODY_FP = (i2d_KRB5_TKTBODY_TYPE)GetProcAddress(libeayHandleM, "i2d_KRB5_TKTBODY")) == NULL) goto err;
//	if ((i2d_NETSCAPE_CERT_SEQUENCE_FP = (i2d_NETSCAPE_CERT_SEQUENCE_TYPE)GetProcAddress(libeayHandleM, "i2d_NETSCAPE_CERT_SEQUENCE")) == NULL) goto err;
//	if ((i2d_NETSCAPE_SPKAC_FP = (i2d_NETSCAPE_SPKAC_TYPE)GetProcAddress(libeayHandleM, "i2d_NETSCAPE_SPKAC")) == NULL) goto err;
//	if ((i2d_NETSCAPE_SPKI_FP = (i2d_NETSCAPE_SPKI_TYPE)GetProcAddress(libeayHandleM, "i2d_NETSCAPE_SPKI")) == NULL) goto err;
//	if ((i2d_NOTICEREF_FP = (i2d_NOTICEREF_TYPE)GetProcAddress(libeayHandleM, "i2d_NOTICEREF")) == NULL) goto err;
//	if ((i2d_Netscape_RSA_FP = (i2d_Netscape_RSA_TYPE)GetProcAddress(libeayHandleM, "i2d_Netscape_RSA")) == NULL) goto err;
//	if ((i2d_OCSP_BASICRESP_FP = (i2d_OCSP_BASICRESP_TYPE)GetProcAddress(libeayHandleM, "i2d_OCSP_BASICRESP")) == NULL) goto err;
//	if ((i2d_OCSP_CERTID_FP = (i2d_OCSP_CERTID_TYPE)GetProcAddress(libeayHandleM, "i2d_OCSP_CERTID")) == NULL) goto err;
//	if ((i2d_OCSP_CERTSTATUS_FP = (i2d_OCSP_CERTSTATUS_TYPE)GetProcAddress(libeayHandleM, "i2d_OCSP_CERTSTATUS")) == NULL) goto err;
//	if ((i2d_OCSP_CRLID_FP = (i2d_OCSP_CRLID_TYPE)GetProcAddress(libeayHandleM, "i2d_OCSP_CRLID")) == NULL) goto err;
//	if ((i2d_OCSP_ONEREQ_FP = (i2d_OCSP_ONEREQ_TYPE)GetProcAddress(libeayHandleM, "i2d_OCSP_ONEREQ")) == NULL) goto err;
//	if ((i2d_OCSP_REQINFO_FP = (i2d_OCSP_REQINFO_TYPE)GetProcAddress(libeayHandleM, "i2d_OCSP_REQINFO")) == NULL) goto err;
//	if ((i2d_OCSP_REQUEST_FP = (i2d_OCSP_REQUEST_TYPE)GetProcAddress(libeayHandleM, "i2d_OCSP_REQUEST")) == NULL) goto err;
//	if ((i2d_OCSP_RESPBYTES_FP = (i2d_OCSP_RESPBYTES_TYPE)GetProcAddress(libeayHandleM, "i2d_OCSP_RESPBYTES")) == NULL) goto err;
//	if ((i2d_OCSP_RESPDATA_FP = (i2d_OCSP_RESPDATA_TYPE)GetProcAddress(libeayHandleM, "i2d_OCSP_RESPDATA")) == NULL) goto err;
//	if ((i2d_OCSP_RESPID_FP = (i2d_OCSP_RESPID_TYPE)GetProcAddress(libeayHandleM, "i2d_OCSP_RESPID")) == NULL) goto err;
//	if ((i2d_OCSP_RESPONSE_FP = (i2d_OCSP_RESPONSE_TYPE)GetProcAddress(libeayHandleM, "i2d_OCSP_RESPONSE")) == NULL) goto err;
//	if ((i2d_OCSP_REVOKEDINFO_FP = (i2d_OCSP_REVOKEDINFO_TYPE)GetProcAddress(libeayHandleM, "i2d_OCSP_REVOKEDINFO")) == NULL) goto err;
//	if ((i2d_OCSP_SERVICELOC_FP = (i2d_OCSP_SERVICELOC_TYPE)GetProcAddress(libeayHandleM, "i2d_OCSP_SERVICELOC")) == NULL) goto err;
//	if ((i2d_OCSP_SIGNATURE_FP = (i2d_OCSP_SIGNATURE_TYPE)GetProcAddress(libeayHandleM, "i2d_OCSP_SIGNATURE")) == NULL) goto err;
//	if ((i2d_OCSP_SINGLERESP_FP = (i2d_OCSP_SINGLERESP_TYPE)GetProcAddress(libeayHandleM, "i2d_OCSP_SINGLERESP")) == NULL) goto err;
//	if ((i2d_OTHERNAME_FP = (i2d_OTHERNAME_TYPE)GetProcAddress(libeayHandleM, "i2d_OTHERNAME")) == NULL) goto err;
//	if ((i2d_PBE2PARAM_FP = (i2d_PBE2PARAM_TYPE)GetProcAddress(libeayHandleM, "i2d_PBE2PARAM")) == NULL) goto err;
//	if ((i2d_PBEPARAM_FP = (i2d_PBEPARAM_TYPE)GetProcAddress(libeayHandleM, "i2d_PBEPARAM")) == NULL) goto err;
//	if ((i2d_PBKDF2PARAM_FP = (i2d_PBKDF2PARAM_TYPE)GetProcAddress(libeayHandleM, "i2d_PBKDF2PARAM")) == NULL) goto err;
//	if ((i2d_PKCS12_FP = (i2d_PKCS12_TYPE)GetProcAddress(libeayHandleM, "i2d_PKCS12")) == NULL) goto err;
//	if ((i2d_PKCS12_BAGS_FP = (i2d_PKCS12_BAGS_TYPE)GetProcAddress(libeayHandleM, "i2d_PKCS12_BAGS")) == NULL) goto err;
//	if ((i2d_PKCS12_MAC_DATA_FP = (i2d_PKCS12_MAC_DATA_TYPE)GetProcAddress(libeayHandleM, "i2d_PKCS12_MAC_DATA")) == NULL) goto err;
//	if ((i2d_PKCS12_SAFEBAG_FP = (i2d_PKCS12_SAFEBAG_TYPE)GetProcAddress(libeayHandleM, "i2d_PKCS12_SAFEBAG")) == NULL) goto err;
//	if ((i2d_PKCS12_bio_FP = (i2d_PKCS12_bio_TYPE)GetProcAddress(libeayHandleM, "i2d_PKCS12_bio")) == NULL) goto err;
//	if ((i2d_PKCS12_fp_FP = (i2d_PKCS12_fp_TYPE)GetProcAddress(libeayHandleM, "i2d_PKCS12_fp")) == NULL) goto err;
//	if ((i2d_PKCS7_FP = (i2d_PKCS7_TYPE)GetProcAddress(libeayHandleM, "i2d_PKCS7")) == NULL) goto err;
//	if ((i2d_PKCS7_DIGEST_FP = (i2d_PKCS7_DIGEST_TYPE)GetProcAddress(libeayHandleM, "i2d_PKCS7_DIGEST")) == NULL) goto err;
//	if ((i2d_PKCS7_ENCRYPT_FP = (i2d_PKCS7_ENCRYPT_TYPE)GetProcAddress(libeayHandleM, "i2d_PKCS7_ENCRYPT")) == NULL) goto err;
//	if ((i2d_PKCS7_ENC_CONTENT_FP = (i2d_PKCS7_ENC_CONTENT_TYPE)GetProcAddress(libeayHandleM, "i2d_PKCS7_ENC_CONTENT")) == NULL) goto err;
//	if ((i2d_PKCS7_ENVELOPE_FP = (i2d_PKCS7_ENVELOPE_TYPE)GetProcAddress(libeayHandleM, "i2d_PKCS7_ENVELOPE")) == NULL) goto err;
//	if ((i2d_PKCS7_ISSUER_AND_SERIAL_FP = (i2d_PKCS7_ISSUER_AND_SERIAL_TYPE)GetProcAddress(libeayHandleM, "i2d_PKCS7_ISSUER_AND_SERIAL")) == NULL) goto err;
//	if ((i2d_PKCS7_RECIP_INFO_FP = (i2d_PKCS7_RECIP_INFO_TYPE)GetProcAddress(libeayHandleM, "i2d_PKCS7_RECIP_INFO")) == NULL) goto err;
//	if ((i2d_PKCS7_SIGNED_FP = (i2d_PKCS7_SIGNED_TYPE)GetProcAddress(libeayHandleM, "i2d_PKCS7_SIGNED")) == NULL) goto err;
//	if ((i2d_PKCS7_SIGNER_INFO_FP = (i2d_PKCS7_SIGNER_INFO_TYPE)GetProcAddress(libeayHandleM, "i2d_PKCS7_SIGNER_INFO")) == NULL) goto err;
//	if ((i2d_PKCS7_SIGN_ENVELOPE_FP = (i2d_PKCS7_SIGN_ENVELOPE_TYPE)GetProcAddress(libeayHandleM, "i2d_PKCS7_SIGN_ENVELOPE")) == NULL) goto err;
//	if ((i2d_PKCS7_bio_FP = (i2d_PKCS7_bio_TYPE)GetProcAddress(libeayHandleM, "i2d_PKCS7_bio")) == NULL) goto err;
//	if ((i2d_PKCS7_fp_FP = (i2d_PKCS7_fp_TYPE)GetProcAddress(libeayHandleM, "i2d_PKCS7_fp")) == NULL) goto err;
//	if ((i2d_PKCS8PrivateKeyInfo_bio_FP = (i2d_PKCS8PrivateKeyInfo_bio_TYPE)GetProcAddress(libeayHandleM, "i2d_PKCS8PrivateKeyInfo_bio")) == NULL) goto err;
//	if ((i2d_PKCS8PrivateKeyInfo_fp_FP = (i2d_PKCS8PrivateKeyInfo_fp_TYPE)GetProcAddress(libeayHandleM, "i2d_PKCS8PrivateKeyInfo_fp")) == NULL) goto err;
//	if ((i2d_PKCS8PrivateKey_bio_FP = (i2d_PKCS8PrivateKey_bio_TYPE)GetProcAddress(libeayHandleM, "i2d_PKCS8PrivateKey_bio")) == NULL) goto err;
//	if ((i2d_PKCS8PrivateKey_fp_FP = (i2d_PKCS8PrivateKey_fp_TYPE)GetProcAddress(libeayHandleM, "i2d_PKCS8PrivateKey_fp")) == NULL) goto err;
//	if ((i2d_PKCS8PrivateKey_nid_bio_FP = (i2d_PKCS8PrivateKey_nid_bio_TYPE)GetProcAddress(libeayHandleM, "i2d_PKCS8PrivateKey_nid_bio")) == NULL) goto err;
//	if ((i2d_PKCS8PrivateKey_nid_fp_FP = (i2d_PKCS8PrivateKey_nid_fp_TYPE)GetProcAddress(libeayHandleM, "i2d_PKCS8PrivateKey_nid_fp")) == NULL) goto err;
//	if ((i2d_PKCS8_PRIV_KEY_INFO_FP = (i2d_PKCS8_PRIV_KEY_INFO_TYPE)GetProcAddress(libeayHandleM, "i2d_PKCS8_PRIV_KEY_INFO")) == NULL) goto err;
//	if ((i2d_PKCS8_PRIV_KEY_INFO_bio_FP = (i2d_PKCS8_PRIV_KEY_INFO_bio_TYPE)GetProcAddress(libeayHandleM, "i2d_PKCS8_PRIV_KEY_INFO_bio")) == NULL) goto err;
//	if ((i2d_PKCS8_PRIV_KEY_INFO_fp_FP = (i2d_PKCS8_PRIV_KEY_INFO_fp_TYPE)GetProcAddress(libeayHandleM, "i2d_PKCS8_PRIV_KEY_INFO_fp")) == NULL) goto err;
//	if ((i2d_PKCS8_bio_FP = (i2d_PKCS8_bio_TYPE)GetProcAddress(libeayHandleM, "i2d_PKCS8_bio")) == NULL) goto err;
//	if ((i2d_PKCS8_fp_FP = (i2d_PKCS8_fp_TYPE)GetProcAddress(libeayHandleM, "i2d_PKCS8_fp")) == NULL) goto err;
//	if ((i2d_PKEY_USAGE_PERIOD_FP = (i2d_PKEY_USAGE_PERIOD_TYPE)GetProcAddress(libeayHandleM, "i2d_PKEY_USAGE_PERIOD")) == NULL) goto err;
//	if ((i2d_POLICYINFO_FP = (i2d_POLICYINFO_TYPE)GetProcAddress(libeayHandleM, "i2d_POLICYINFO")) == NULL) goto err;
//	if ((i2d_POLICYQUALINFO_FP = (i2d_POLICYQUALINFO_TYPE)GetProcAddress(libeayHandleM, "i2d_POLICYQUALINFO")) == NULL) goto err;
//	if ((i2d_PUBKEY_FP = (i2d_PUBKEY_TYPE)GetProcAddress(libeayHandleM, "i2d_PUBKEY")) == NULL) goto err;
//	if ((i2d_PUBKEY_bio_FP = (i2d_PUBKEY_bio_TYPE)GetProcAddress(libeayHandleM, "i2d_PUBKEY_bio")) == NULL) goto err;
//	if ((i2d_PUBKEY_fp_FP = (i2d_PUBKEY_fp_TYPE)GetProcAddress(libeayHandleM, "i2d_PUBKEY_fp")) == NULL) goto err;
//	if ((i2d_PrivateKey_FP = (i2d_PrivateKey_TYPE)GetProcAddress(libeayHandleM, "i2d_PrivateKey")) == NULL) goto err;
//	if ((i2d_PrivateKey_bio_FP = (i2d_PrivateKey_bio_TYPE)GetProcAddress(libeayHandleM, "i2d_PrivateKey_bio")) == NULL) goto err;
//	if ((i2d_PrivateKey_fp_FP = (i2d_PrivateKey_fp_TYPE)GetProcAddress(libeayHandleM, "i2d_PrivateKey_fp")) == NULL) goto err;
//	if ((i2d_PublicKey_FP = (i2d_PublicKey_TYPE)GetProcAddress(libeayHandleM, "i2d_PublicKey")) == NULL) goto err;
//	if ((i2d_RSAPrivateKey_FP = (i2d_RSAPrivateKey_TYPE)GetProcAddress(libeayHandleM, "i2d_RSAPrivateKey")) == NULL) goto err;
//	if ((i2d_RSAPrivateKey_bio_FP = (i2d_RSAPrivateKey_bio_TYPE)GetProcAddress(libeayHandleM, "i2d_RSAPrivateKey_bio")) == NULL) goto err;
//	if ((i2d_RSAPrivateKey_fp_FP = (i2d_RSAPrivateKey_fp_TYPE)GetProcAddress(libeayHandleM, "i2d_RSAPrivateKey_fp")) == NULL) goto err;
//	if ((i2d_RSAPublicKey_FP = (i2d_RSAPublicKey_TYPE)GetProcAddress(libeayHandleM, "i2d_RSAPublicKey")) == NULL) goto err;
//	if ((i2d_RSAPublicKey_bio_FP = (i2d_RSAPublicKey_bio_TYPE)GetProcAddress(libeayHandleM, "i2d_RSAPublicKey_bio")) == NULL) goto err;
//	if ((i2d_RSAPublicKey_fp_FP = (i2d_RSAPublicKey_fp_TYPE)GetProcAddress(libeayHandleM, "i2d_RSAPublicKey_fp")) == NULL) goto err;
//	if ((i2d_RSA_NET_FP = (i2d_RSA_NET_TYPE)GetProcAddress(libeayHandleM, "i2d_RSA_NET")) == NULL) goto err;
//	if ((i2d_RSA_PUBKEY_FP = (i2d_RSA_PUBKEY_TYPE)GetProcAddress(libeayHandleM, "i2d_RSA_PUBKEY")) == NULL) goto err;
//	if ((i2d_RSA_PUBKEY_bio_FP = (i2d_RSA_PUBKEY_bio_TYPE)GetProcAddress(libeayHandleM, "i2d_RSA_PUBKEY_bio")) == NULL) goto err;
//	if ((i2d_RSA_PUBKEY_fp_FP = (i2d_RSA_PUBKEY_fp_TYPE)GetProcAddress(libeayHandleM, "i2d_RSA_PUBKEY_fp")) == NULL) goto err;
//	if ((i2d_SXNETID_FP = (i2d_SXNETID_TYPE)GetProcAddress(libeayHandleM, "i2d_SXNETID")) == NULL) goto err;
//	if ((i2d_SXNET_FP = (i2d_SXNET_TYPE)GetProcAddress(libeayHandleM, "i2d_SXNET")) == NULL) goto err;
//	if ((i2d_USERNOTICE_FP = (i2d_USERNOTICE_TYPE)GetProcAddress(libeayHandleM, "i2d_USERNOTICE")) == NULL) goto err;
//	if ((i2d_X509_FP = (i2d_X509_TYPE)GetProcAddress(libeayHandleM, "i2d_X509")) == NULL) goto err;
//	if ((i2d_X509_ALGOR_FP = (i2d_X509_ALGOR_TYPE)GetProcAddress(libeayHandleM, "i2d_X509_ALGOR")) == NULL) goto err;
//	if ((i2d_X509_ATTRIBUTE_FP = (i2d_X509_ATTRIBUTE_TYPE)GetProcAddress(libeayHandleM, "i2d_X509_ATTRIBUTE")) == NULL) goto err;
//	if ((i2d_X509_AUX_FP = (i2d_X509_AUX_TYPE)GetProcAddress(libeayHandleM, "i2d_X509_AUX")) == NULL) goto err;
//	if ((i2d_X509_CERT_AUX_FP = (i2d_X509_CERT_AUX_TYPE)GetProcAddress(libeayHandleM, "i2d_X509_CERT_AUX")) == NULL) goto err;
//	if ((i2d_X509_CINF_FP = (i2d_X509_CINF_TYPE)GetProcAddress(libeayHandleM, "i2d_X509_CINF")) == NULL) goto err;
//	if ((i2d_X509_CRL_FP = (i2d_X509_CRL_TYPE)GetProcAddress(libeayHandleM, "i2d_X509_CRL")) == NULL) goto err;
//	if ((i2d_X509_CRL_INFO_FP = (i2d_X509_CRL_INFO_TYPE)GetProcAddress(libeayHandleM, "i2d_X509_CRL_INFO")) == NULL) goto err;
//	if ((i2d_X509_CRL_bio_FP = (i2d_X509_CRL_bio_TYPE)GetProcAddress(libeayHandleM, "i2d_X509_CRL_bio")) == NULL) goto err;
//	if ((i2d_X509_CRL_fp_FP = (i2d_X509_CRL_fp_TYPE)GetProcAddress(libeayHandleM, "i2d_X509_CRL_fp")) == NULL) goto err;
//	if ((i2d_X509_EXTENSION_FP = (i2d_X509_EXTENSION_TYPE)GetProcAddress(libeayHandleM, "i2d_X509_EXTENSION")) == NULL) goto err;
//	if ((i2d_X509_NAME_FP = (i2d_X509_NAME_TYPE)GetProcAddress(libeayHandleM, "i2d_X509_NAME")) == NULL) goto err;
//	if ((i2d_X509_NAME_ENTRY_FP = (i2d_X509_NAME_ENTRY_TYPE)GetProcAddress(libeayHandleM, "i2d_X509_NAME_ENTRY")) == NULL) goto err;
//	if ((i2d_X509_PKEY_FP = (i2d_X509_PKEY_TYPE)GetProcAddress(libeayHandleM, "i2d_X509_PKEY")) == NULL) goto err;
//	if ((i2d_X509_PUBKEY_FP = (i2d_X509_PUBKEY_TYPE)GetProcAddress(libeayHandleM, "i2d_X509_PUBKEY")) == NULL) goto err;
//	if ((i2d_X509_REQ_FP = (i2d_X509_REQ_TYPE)GetProcAddress(libeayHandleM, "i2d_X509_REQ")) == NULL) goto err;
//	if ((i2d_X509_REQ_INFO_FP = (i2d_X509_REQ_INFO_TYPE)GetProcAddress(libeayHandleM, "i2d_X509_REQ_INFO")) == NULL) goto err;
//	if ((i2d_X509_REQ_bio_FP = (i2d_X509_REQ_bio_TYPE)GetProcAddress(libeayHandleM, "i2d_X509_REQ_bio")) == NULL) goto err;
//	if ((i2d_X509_REQ_fp_FP = (i2d_X509_REQ_fp_TYPE)GetProcAddress(libeayHandleM, "i2d_X509_REQ_fp")) == NULL) goto err;
//	if ((i2d_X509_REVOKED_FP = (i2d_X509_REVOKED_TYPE)GetProcAddress(libeayHandleM, "i2d_X509_REVOKED")) == NULL) goto err;
//	if ((i2d_X509_SIG_FP = (i2d_X509_SIG_TYPE)GetProcAddress(libeayHandleM, "i2d_X509_SIG")) == NULL) goto err;
//	if ((i2d_X509_VAL_FP = (i2d_X509_VAL_TYPE)GetProcAddress(libeayHandleM, "i2d_X509_VAL")) == NULL) goto err;
//	if ((i2d_X509_bio_FP = (i2d_X509_bio_TYPE)GetProcAddress(libeayHandleM, "i2d_X509_bio")) == NULL) goto err;
//	if ((i2d_X509_fp_FP = (i2d_X509_fp_TYPE)GetProcAddress(libeayHandleM, "i2d_X509_fp")) == NULL) goto err;
//	if ((i2s_ASN1_ENUMERATED_FP = (i2s_ASN1_ENUMERATED_TYPE)GetProcAddress(libeayHandleM, "i2s_ASN1_ENUMERATED")) == NULL) goto err;
//	if ((i2s_ASN1_ENUMERATED_TABLE_FP = (i2s_ASN1_ENUMERATED_TABLE_TYPE)GetProcAddress(libeayHandleM, "i2s_ASN1_ENUMERATED_TABLE")) == NULL) goto err;
//	if ((i2s_ASN1_INTEGER_FP = (i2s_ASN1_INTEGER_TYPE)GetProcAddress(libeayHandleM, "i2s_ASN1_INTEGER")) == NULL) goto err;
//	if ((i2s_ASN1_OCTET_STRING_FP = (i2s_ASN1_OCTET_STRING_TYPE)GetProcAddress(libeayHandleM, "i2s_ASN1_OCTET_STRING")) == NULL) goto err;
	if ((i2t_ASN1_OBJECT_FP = (i2t_ASN1_OBJECT_TYPE)GetProcAddress(libeayHandleM, "i2t_ASN1_OBJECT")) == NULL) goto err;
//	if ((i2v_GENERAL_NAMES_FP = (i2v_GENERAL_NAMES_TYPE)GetProcAddress(libeayHandleM, "i2v_GENERAL_NAMES")) == NULL) goto err;
//	if ((i2v_GENERAL_NAME_FP = (i2v_GENERAL_NAME_TYPE)GetProcAddress(libeayHandleM, "i2v_GENERAL_NAME")) == NULL) goto err;
//	if ((lh_delete_FP = (lh_delete_TYPE)GetProcAddress(libeayHandleM, "lh_delete")) == NULL) goto err;
//	if ((lh_doall_FP = (lh_doall_TYPE)GetProcAddress(libeayHandleM, "lh_doall")) == NULL) goto err;
//	if ((lh_doall_arg_FP = (lh_doall_arg_TYPE)GetProcAddress(libeayHandleM, "lh_doall_arg")) == NULL) goto err;
//	if ((lh_free_FP = (lh_free_TYPE)GetProcAddress(libeayHandleM, "lh_free")) == NULL) goto err;
//	if ((lh_insert_FP = (lh_insert_TYPE)GetProcAddress(libeayHandleM, "lh_insert")) == NULL) goto err;
//	if ((lh_new_FP = (lh_new_TYPE)GetProcAddress(libeayHandleM, "lh_new")) == NULL) goto err;
//	if ((lh_node_stats_FP = (lh_node_stats_TYPE)GetProcAddress(libeayHandleM, "lh_node_stats")) == NULL) goto err;
//	if ((lh_node_stats_bio_FP = (lh_node_stats_bio_TYPE)GetProcAddress(libeayHandleM, "lh_node_stats_bio")) == NULL) goto err;
//	if ((lh_node_usage_stats_FP = (lh_node_usage_stats_TYPE)GetProcAddress(libeayHandleM, "lh_node_usage_stats")) == NULL) goto err;
//	if ((lh_node_usage_stats_bio_FP = (lh_node_usage_stats_bio_TYPE)GetProcAddress(libeayHandleM, "lh_node_usage_stats_bio")) == NULL) goto err;
//	if ((lh_num_items_FP = (lh_num_items_TYPE)GetProcAddress(libeayHandleM, "lh_num_items")) == NULL) goto err;
//	if ((lh_retrieve_FP = (lh_retrieve_TYPE)GetProcAddress(libeayHandleM, "lh_retrieve")) == NULL) goto err;
//	if ((lh_stats_FP = (lh_stats_TYPE)GetProcAddress(libeayHandleM, "lh_stats")) == NULL) goto err;
//	if ((lh_stats_bio_FP = (lh_stats_bio_TYPE)GetProcAddress(libeayHandleM, "lh_stats_bio")) == NULL) goto err;
//	if ((lh_strhash_FP = (lh_strhash_TYPE)GetProcAddress(libeayHandleM, "lh_strhash")) == NULL) goto err;
//	if ((ms_time_cmp_FP = (ms_time_cmp_TYPE)GetProcAddress(libeayHandleM, "ms_time_cmp")) == NULL) goto err;
//	if ((ms_time_diff_FP = (ms_time_diff_TYPE)GetProcAddress(libeayHandleM, "ms_time_diff")) == NULL) goto err;
//	if ((ms_time_free_FP = (ms_time_free_TYPE)GetProcAddress(libeayHandleM, "ms_time_free")) == NULL) goto err;
//	if ((ms_time_get_FP = (ms_time_get_TYPE)GetProcAddress(libeayHandleM, "ms_time_get")) == NULL) goto err;
//	if ((ms_time_new_FP = (ms_time_new_TYPE)GetProcAddress(libeayHandleM, "ms_time_new")) == NULL) goto err;
//	if ((name_cmp_FP = (name_cmp_TYPE)GetProcAddress(libeayHandleM, "name_cmp")) == NULL) goto err;
//	if ((s2i_ASN1_INTEGER_FP = (s2i_ASN1_INTEGER_TYPE)GetProcAddress(libeayHandleM, "s2i_ASN1_INTEGER")) == NULL) goto err;
//	if ((s2i_ASN1_OCTET_STRING_FP = (s2i_ASN1_OCTET_STRING_TYPE)GetProcAddress(libeayHandleM, "s2i_ASN1_OCTET_STRING")) == NULL) goto err;
	if ((sk_delete_FP = (sk_delete_TYPE)GetProcAddress(libeayHandleM, "sk_delete")) == NULL) goto err;
//	if ((sk_delete_ptr_FP = (sk_delete_ptr_TYPE)GetProcAddress(libeayHandleM, "sk_delete_ptr")) == NULL) goto err;
//	if ((sk_dup_FP = (sk_dup_TYPE)GetProcAddress(libeayHandleM, "sk_dup")) == NULL) goto err;
//	if ((sk_find_FP = (sk_find_TYPE)GetProcAddress(libeayHandleM, "sk_find")) == NULL) goto err;
//	if ((sk_free_FP = (sk_free_TYPE)GetProcAddress(libeayHandleM, "sk_free")) == NULL) goto err;
	if ((sk_insert_FP = (sk_insert_TYPE)GetProcAddress(libeayHandleM, "sk_insert")) == NULL) goto err;
//	if ((sk_new_FP = (sk_new_TYPE)GetProcAddress(libeayHandleM, "sk_new")) == NULL) goto err;
	if ((sk_new_null_FP = (sk_new_null_TYPE)GetProcAddress(libeayHandleM, "sk_new_null")) == NULL) goto err;
	if ((sk_num_FP = (sk_num_TYPE)GetProcAddress(libeayHandleM, "sk_num")) == NULL) goto err;
	if ((sk_pop_FP = (sk_pop_TYPE)GetProcAddress(libeayHandleM, "sk_pop")) == NULL) goto err;
	if ((sk_pop_free_FP = (sk_pop_free_TYPE)GetProcAddress(libeayHandleM, "sk_pop_free")) == NULL) goto err;
	if ((sk_push_FP = (sk_push_TYPE)GetProcAddress(libeayHandleM, "sk_push")) == NULL) goto err;
//	if ((sk_set_FP = (sk_set_TYPE)GetProcAddress(libeayHandleM, "sk_set")) == NULL) goto err;
//	if ((sk_set_cmp_func_FP = (sk_set_cmp_func_TYPE)GetProcAddress(libeayHandleM, "sk_set_cmp_func")) == NULL) goto err;
	if ((sk_shift_FP = (sk_shift_TYPE)GetProcAddress(libeayHandleM, "sk_shift")) == NULL) goto err;
//	if ((sk_sort_FP = (sk_sort_TYPE)GetProcAddress(libeayHandleM, "sk_sort")) == NULL) goto err;
//	if ((sk_unshift_FP = (sk_unshift_TYPE)GetProcAddress(libeayHandleM, "sk_unshift")) == NULL) goto err;
	if ((sk_value_FP = (sk_value_TYPE)GetProcAddress(libeayHandleM, "sk_value")) == NULL) goto err;
//	if ((sk_zero_FP = (sk_zero_TYPE)GetProcAddress(libeayHandleM, "sk_zero")) == NULL) goto err;
//	if ((string_to_hex_FP = (string_to_hex_TYPE)GetProcAddress(libeayHandleM, "string_to_hex")) == NULL) goto err;
//	if ((uni2asc_FP = (uni2asc_TYPE)GetProcAddress(libeayHandleM, "uni2asc")) == NULL) goto err;
//	if ((v2i_GENERAL_NAMES_FP = (v2i_GENERAL_NAMES_TYPE)GetProcAddress(libeayHandleM, "v2i_GENERAL_NAMES")) == NULL) goto err;
//	if ((v2i_GENERAL_NAME_FP = (v2i_GENERAL_NAME_TYPE)GetProcAddress(libeayHandleM, "v2i_GENERAL_NAME")) == NULL) goto err;

	return true;

err:
	if (libeayHandleM != NULL)
	{
		FreeLibrary(libeayHandleM);
		libeayHandleM = NULL;
	}

	return false;
}