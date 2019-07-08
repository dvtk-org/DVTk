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
using DvtkSession = Dvtk.Sessions;

namespace DvtkApplicationLayer {
    /// <summary>
    /// Summary description for SecuritySettings.
    /// </summary>
    public class SecuritySettings
	{
		/// <summary>
		/// 
		/// </summary>
        public enum CipherFlags 
		{
            /// <summary>
            /// Rivest-Shamir-Adleman.  
            /// A public key algorithm that provides both digital signatures and encryption.  
            /// Also the name of the company that developed this and other algorithms.
            /// </summary>
            TLS_AUTHENICATION_METHOD_RSA    = 1<<0, // "aRSA"
            /// <summary>
            /// DSA – Digital Signature Algorithm.
            /// A method for computing digital signatures.
            /// </summary>
            TLS_AUTHENICATION_METHOD_DSA    = 1<<1, // "aDSS"
            /// <summary>
            /// Rivest-Shamir-Adleman.
            /// A public key algorithm that provides both digital signatures and encryption.
            /// Also the name of the company that developed this and other algorithms.
            /// </summary>
            TLS_KEY_EXCHANGE_METHOD_RSA     = 1<<2, // "kRSA"
            /// <summary>
            /// DH – Diffie-Hellman algorithm.
            /// An algorithm used to exchange symmetric keys.
            /// </summary>
            TLS_KEY_EXCHANGE_METHOD_DH      = 1<<3, // "DH"
            /// <summary>
            /// Secure Hash Algorithm. 
            /// Used to generate a cryptographic hash (or checksum). 
            /// FIPS 180-1.
            /// </summary>
            TLS_DATA_INTEGRITY_METHOD_SHA1  = 1<<4, // "SHA1"
            /// <summary>
            /// MD5 – Message Digest 5.
            /// Used to generate a cryptographic hash (or checksum).
            /// RFC 1321.
            /// </summary>
            TLS_DATA_INTEGRITY_METHOD_MD5   = 1<<5, // "MD5"
            /// <summary>
            /// No encryption.
            /// </summary>
            TLS_ENCRYPTION_METHOD_NONE      = 1<<6, // "eNULL"
            /// <summary>
            /// Triple DES (also known as 3DES) – 
            /// DES applied 3 times in a row (encode, decode, encode).
            /// Highly secure, well trusted symmetric key encryption algorithm.
            /// </summary>
            TLS_ENCRYPTION_METHOD_3DES      = 1<<7, // "3DES"
            /// <summary>
            /// AES – Advanced Encryption Standard.
            /// Newer symmetric key encryption algorithm.
            /// Expected to replace Triple DES.
            /// FIPS (Federal Information Processing Standard) 197.
            /// </summary>
            TLS_ENCRYPTION_METHOD_AES128    = 1<<8, // "AES128"
            /// <summary>
            /// AES – Advanced Encryption Standard.
            /// Newer symmetric key encryption algorithm.
            /// Expected to replace Triple DES.
            /// FIPS (Federal Information Processing Standard) 197.
            /// </summary>
            TLS_ENCRYPTION_METHOD_AES256    = 1<<9, // "AES256"
			/// <summary>
			/// Default
			/// </summary>
            TLS_Default = 85
        }

        /// <summary>
        /// Protocols Supported
        /// </summary>
        /// <remarks>
        /// <p>
        /// The use of the secure transport is optional and 
        /// must be turned on in the configuration of the session.
        /// </p>
        /// <p>
        /// By default, a new session will not use secure sockets.
        /// </p>
        /// <p>
        /// The following protocols are supported for secure transport: 
        /// <list type="bullet">
        /// <item>
        ///  <term>TLS version 1.0</term>
        ///  <description>
        ///  Transport Layer Security (TLS). 
        ///  Required by DICOM [DICOM, 3.15, B.1] and IHE [IHE2, 4.32.4.1.2]. (default)
        ///  </description>
        /// </item>
        /// <item>
        ///  <term>SSL version 3.0</term>
        ///  <description>
        ///  Secure Socket Layer (SSL).
        ///  Required for compatibility with different implementations.
        ///  </description>
        /// </item>
        /// </list>
        /// </p>
        /// <p>
        /// Which protocol to use will be configurable. 
        /// Multiple of the protocols can be configured at the same time. 
        /// In that case, the server will accept any of the configured protocols. 
        /// A client will use the highest protocol in the order of the list above that the server supports.
        /// If a security failure occurs in establishing a connection or while transmitting data, 
        /// the socket will be closed.  (This is TLS/SSL behavior).
        /// The protocols support the following features, which can use different mechanisms 
        /// to implement the feature. 
        /// DICOM only defines a minimum set of mechanisms that must be supported. 
        /// Additional, undefined by DICOM, mechanisms are allowed.
        /// </p>
        /// </remarks>
        public enum TlsVersionFlags 
		{
            /// <summary>
            /// TLS version 1.0
            /// </summary>
            /// <remarks>
            /// Required by DICOM [DICOM, 3.15, B.1] and IHE [IHE2, 4.32.4.1.2]. (default)
            /// </remarks>
            TLS_VERSION_TLSv1               = 1<<0, // "TLSv1"
            /// <summary>
            /// SSL version 3.0
            /// </summary>
            /// <remarks>
            /// Required for compatibility with different implementations
            /// </remarks>
            TLS_VERSION_SSLv3               = 1<<1, // "SSLv3"
        }
        private bool secureSocketsEnabled ;
        private string tlsPassword ;
        private bool checkRemoteCertificate;
        private bool cacheTlsSessions;
        private string credentialsFileName;
        private string certificateFileName;
        private UInt16 tlsCacheTimeout;
        private CipherFlags cipherFlags = CipherFlags.TLS_Default;
        private TlsVersionFlags tlsVersionFlags ;

		/// <summary>
		/// 
		/// </summary>
        public System.Boolean SecureSocketsEnabled
		{
            get 
			{
                return secureSocketsEnabled;
            }
            set 
			{ 
                secureSocketsEnabled = value;
            }                    
        }

        /// <summary>
        /// Protocol password
        /// </summary>
        public System.String TlsPassword {
            get { 
                
                return tlsPassword;
            }
            set { 
                tlsPassword = value;
            }
        }

        /// <summary>
        /// Protocol(s) Supported
        /// </summary>
        public    TlsVersionFlags TlsVersionFlag
		{
			get 
			{ 	    
				return tlsVersionFlags;
			}
			set 
			{ 
				tlsVersionFlags = value;
			}
		}

        /// <summary>
        /// Verifies the SSL peer and fail the connection if the peer has no ceritificate.
        /// </summary>
        public bool CheckRemoteCertificate 
		{
            get 
			{                 
                return checkRemoteCertificate;
            }
            set 
			{ 
                checkRemoteCertificate = value;
            }
        }

        /// <summary>
        /// Flags indicating the type of security used.
        /// </summary>
        public CipherFlags CipherFlag 
		{
            get 
			{ 
                return cipherFlags;
            }
            set 
			{ 
                cipherFlags = value;
            }
        }

        /// <summary>
        /// Session caching will be optionally supported.
        /// It can be turned on or off.
        /// DVT will only support in memory caching.
        /// Caching will be enabled by default.
        /// </summary>
        public bool CacheTlsSessions 
		{
            get 
			{                 
                return cacheTlsSessions;
            }
            set 
			{ 
                cacheTlsSessions = value;
            }
        }

        /// <summary>
        /// Sets the session cache timeout time used by the TLS server.
        /// </summary>
        public UInt16 TlsCacheTimeout 
		{
            get 
			{                 
                return tlsCacheTimeout;
            }
            set 
			{ 
                tlsCacheTimeout = value;
            }
        }

        /// <summary>
        /// Credentials file name
        /// </summary>
        public string CredentialsFileName 
		{
            get 
			{ 
                
                return credentialsFileName;
            }
            set 
			{ 
                credentialsFileName = value;
            }
        }

        /// <summary>
        /// Certificate file name
        /// </summary>
        public string CertificateFileName 
		{
            get 
			{ 
                
                return certificateFileName;
            }
            set
			{ 
                certificateFileName = value;
            }
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
        public DvtkApplicationLayer.SecuritySettings.CipherFlags Convert(DvtkSession.CipherFlags value) 
		{
            switch (value) 
			{
                case DvtkSession.CipherFlags.TLS_AUTHENICATION_METHOD_DSA :
                    return CipherFlags.TLS_AUTHENICATION_METHOD_DSA;
                case DvtkSession.CipherFlags.TLS_AUTHENICATION_METHOD_RSA :
                    return CipherFlags.TLS_AUTHENICATION_METHOD_RSA;
                case DvtkSession.CipherFlags.TLS_DATA_INTEGRITY_METHOD_MD5:
                    return CipherFlags.TLS_DATA_INTEGRITY_METHOD_MD5;
                case DvtkSession.CipherFlags.TLS_DATA_INTEGRITY_METHOD_SHA1:
                    return CipherFlags.TLS_DATA_INTEGRITY_METHOD_SHA1;
                case DvtkSession.CipherFlags.TLS_ENCRYPTION_METHOD_3DES:
                    return CipherFlags.TLS_ENCRYPTION_METHOD_3DES;
                case DvtkSession.CipherFlags.TLS_ENCRYPTION_METHOD_AES128:
                    return CipherFlags.TLS_ENCRYPTION_METHOD_AES128;
                case DvtkSession.CipherFlags.TLS_ENCRYPTION_METHOD_AES256:
                    return CipherFlags.TLS_ENCRYPTION_METHOD_AES256;
                case DvtkSession.CipherFlags.TLS_ENCRYPTION_METHOD_NONE:
                    return CipherFlags.TLS_ENCRYPTION_METHOD_NONE;
                case DvtkSession.CipherFlags.TLS_KEY_EXCHANGE_METHOD_DH:
                    return CipherFlags.TLS_KEY_EXCHANGE_METHOD_DH;
                case DvtkSession.CipherFlags.TLS_KEY_EXCHANGE_METHOD_RSA:
                    return CipherFlags.TLS_KEY_EXCHANGE_METHOD_RSA ;
                default:
                    throw new System.NotSupportedException();
            }
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
        public TlsVersionFlags Convert(DvtkSession.TlsVersionFlags value) {
            switch (value) {
                case DvtkSession.TlsVersionFlags.TLS_VERSION_SSLv3 :
                    return TlsVersionFlags.TLS_VERSION_SSLv3;
                case DvtkSession.TlsVersionFlags.TLS_VERSION_TLSv1 :
                    return TlsVersionFlags.TLS_VERSION_TLSv1;
                default :
                    throw new System.NotSupportedException();
            }
        }
    }
}


