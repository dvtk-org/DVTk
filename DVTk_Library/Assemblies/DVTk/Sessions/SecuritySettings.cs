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

namespace Dvtk.Sessions
{
    using System;
    using System.ComponentModel;
    //
    // Aliases for types
    //
    using FileName = System.String;
    using System.Windows.Forms;
    using System.Reflection;

    internal class TlsVersionFlagsConverter : EnumConverter
    {
        // Here we are defining the values that the API expect.
        // Note: public read-only constants are still a great way
        // of exposing a strict set of valid values. I just don’t
        // want to expose them to users of my webservices or 
        // my businesslogic layer.
        #region Constants
        public const string TLSv1_0 = "TLSv1";
        public const string TLSv1_1 = "TLSv1.1";
        public const string TLSv1_2 = "TLSv1.2";
        public const string TLSv1_3 = "TLSv1.3";
        public const string SSLv3 = "SSLv3";
        public const string Seperator = ":";
        #endregion

        #region ctors
        // The default constructor (define the enum this class supports).
        public TlsVersionFlagsConverter() : base(typeof(TlsVersionFlags)) { }
        #endregion ctors

        // Overrides the CanConvertFrom method of TypeConverter.
        // The ITypeDescriptorContext interface provides the context for the
        // conversion. Typically, this interface is used at design time to 
        // provide information about the design-time container.
        public override bool CanConvertFrom(ITypeDescriptorContext context,
            Type sourceType)
        {
            if (sourceType == typeof(string)) return true;
            return base.CanConvertFrom(context, sourceType);
        }
        // Overrides the ConvertFrom method of TypeConverter.
        public override object ConvertFrom(ITypeDescriptorContext context,
            System.Globalization.CultureInfo culture, object value)
        {
            if (value is string)
            {
                // Convert from <"TLSv1">:<"SSLv3"> to TlsVersionFlags
                string enumString = (string)value;
                switch (enumString)
                {
                    case TLSv1_0:
                        enumString = TlsVersionFlags.TLS_VERSION_TLSv1_0.ToString();
                        break;
                    case TLSv1_1:
                        enumString = TlsVersionFlags.TLS_VERSION_TLSv1_1.ToString();
                        break;
                    case TLSv1_2:
                        enumString = TlsVersionFlags.TLS_VERSION_TLSv1_2.ToString();
                        break;
                    case TLSv1_3:
                        enumString = TlsVersionFlags.TLS_VERSION_TLSv1_3.ToString();
                        break;
                }
                TlsVersionFlags tlsVersionFlags =
                    (TlsVersionFlags)System.Enum.Parse(typeof(TlsVersionFlags), enumString);
                return tlsVersionFlags;
            }
            return base.ConvertFrom(context, culture, value);
        }
        // Overrides the ConvertTo method of TypeConverter.
        public override object ConvertTo(ITypeDescriptorContext context,
            System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                // Convert from TlsVersionFlags to <"TLSv1">:<"SSLv3">
                TlsVersionFlags tlsVersionFlags = (TlsVersionFlags)value;
                // Generate ", " seperated list of flagged values.
                string libraryString = System.Enum.Format(typeof(TlsVersionFlags), value, "g");
                // Replace ", " seperations by ":" seperations.
                libraryString = libraryString.Replace(", ", Seperator);
                // Replace enum string values by library string values.
                libraryString = libraryString.Replace(TlsVersionFlags.TLS_VERSION_TLSv1_0.ToString(), TLSv1_0);
                libraryString = libraryString.Replace(TlsVersionFlags.TLS_VERSION_TLSv1_1.ToString(), TLSv1_1);
                libraryString = libraryString.Replace(TlsVersionFlags.TLS_VERSION_TLSv1_2.ToString(), TLSv1_2);
                libraryString = libraryString.Replace(TlsVersionFlags.TLS_VERSION_TLSv1_3.ToString(), TLSv1_3);

                return libraryString;
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
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
    [Flags]
    [TypeConverter(typeof(TlsVersionFlagsConverter))]
    public enum TlsVersionFlags
    {
        /// <summary>
        /// TLS version 1.0
        /// </summary>
        /// <remarks>
        /// Required by DICOM [DICOM, 3.15, B.1] and IHE [IHE2, 4.32.4.1.2]. (default)
        /// </remarks>
        TLS_VERSION_TLSv1_0 = 0, // "TLSv1"
        TLS_VERSION_TLSv1_1,
        TLS_VERSION_TLSv1_2,
        TLS_VERSION_TLSv1_3,
    }

    internal class CipherFlagsConverter : EnumConverter
    {
        // Here we are defining the values that the API expect.
        // Note: public read-only constants are still a great way
        // of exposing a strict set of valid values. I just don’t
        // want to expose them to users of my webservices or 
        // my businesslogic layer.
        #region Constants
        public const string
            aRSA = "aRSA",
            aDSS = "aDSS",
            kRSA = "kRSA",
            DH = "DH",
            SHA1 = "SHA1",
            MD5 = "MD5",
            eNULL = "eNULL",
            tripleDES = "3DES",
            AES = "AES",
            NoAES128 = "-DHE-RSA-AES128-SHA:-DHE-DSS-AES128-SHA:-AES128-SHA",
            NoAES256 = "-DHE-RSA-AES256-SHA:-DHE-DSS-AES256-SHA:-AES256-SHA",
            TlsPostFix = ":@STRENGTH:-SSLv2",
            //TlsPostFix = "+TLSv1.2",
            SecElementSeperator = "+",
            SecGroupSeperator = ":";
        #endregion

        #region ctors
        // The default constructor (define the enum this class supports).
        public CipherFlagsConverter() : base(typeof(CipherFlags)) { }
        #endregion ctors
        // Overrides the CanConvertFrom method of TypeConverter.
        // The ITypeDescriptorContext interface provides the context for the
        // conversion. Typically, this interface is used at design time to 
        // provide information about the design-time container.
        public override bool CanConvertFrom(ITypeDescriptorContext context,
            Type sourceType)
        {
            if (sourceType == typeof(string)) return true;
            return base.CanConvertFrom(context, sourceType);
        }
        // Overrides the ConvertFrom method of TypeConverter.

        public bool getSecuritySettingsChangedStatus()
        {
            return securitySettingsChanged;
        }


        public bool isFirstcheck = true;
        public bool securitySettingsChanged;
        public override object ConvertFrom(ITypeDescriptorContext context,
            System.Globalization.CultureInfo culture, object value)
        {
            if (value is string)
            {
                securitySettingsChanged = false;
                string stringValue = (string)value;
                // Search for AES128 and AES256 disabling settings
                bool bDisableAES128 = (stringValue.IndexOf(NoAES128) != -1);
                bool bDisableAES256 = (stringValue.IndexOf(NoAES256) != -1);

                int count = 0;
                CipherFlags cipherFlags = 0;
                // Split library string into security groups
                string[] secGroups = stringValue.Split(new char[] { ':' });
                // Analyze the first security group only for settings

                if (secGroups[0].IndexOf(aRSA) != -1 & secGroups[0].IndexOf(aDSS) != -1)
                {
                    cipherFlags |= CipherFlags.TLS_AUTHENICATION_METHOD_RSA;
                    securitySettingsChanged = true;
                    // none aan
                }
                //else if (secGroups[0].IndexOf(aRSA) == -1 & secGroups[0].IndexOf(aDSS) == -1)
                //{
                //    cipherFlags |= CipherFlags.TLS_AUTHENICATION_METHOD_RSA;
                //    securitySettingsChanged = true;
                //    // none aan
                //}
                else
                {
                    if (secGroups[0].IndexOf(aRSA) != -1) cipherFlags |= CipherFlags.TLS_AUTHENICATION_METHOD_RSA;
                    if (secGroups[0].IndexOf(aDSS) != -1) cipherFlags |= CipherFlags.TLS_AUTHENICATION_METHOD_DSA;
                }

                if (secGroups[0].IndexOf(kRSA) != -1 & secGroups[0].IndexOf("+" + DH) != -1)
                {
                    cipherFlags |= CipherFlags.TLS_KEY_EXCHANGE_METHOD_RSA;
                    securitySettingsChanged = true;
                }
                //else if (secGroups[0].IndexOf(kRSA) == -1 & secGroups[0].IndexOf("+" + DH) == -1)
                //{
                //    cipherFlags |= CipherFlags.TLS_KEY_EXCHANGE_METHOD_RSA;
                //    securitySettingsChanged = true;
                //}
                else
                {
                    if (secGroups[0].IndexOf(kRSA) != -1) cipherFlags |= CipherFlags.TLS_KEY_EXCHANGE_METHOD_RSA;
                    if (secGroups[0].IndexOf("+" + DH) != -1) cipherFlags |= CipherFlags.TLS_KEY_EXCHANGE_METHOD_DH;
                }


                if (secGroups[0].IndexOf(SHA1) != -1 & secGroups[0].IndexOf("+" + MD5) != -1)
                {
                    cipherFlags |= CipherFlags.TLS_DATA_INTEGRITY_METHOD_SHA1;
                    securitySettingsChanged = true;
                }
                //else if (secGroups[0].IndexOf(SHA1) == -1 & secGroups[0].IndexOf("+" + MD5) == -1)
                //{
                //    cipherFlags |= CipherFlags.TLS_DATA_INTEGRITY_METHOD_SHA1;
                //    securitySettingsChanged = true;
                //}
                else
                {
                    if (secGroups[0].IndexOf(SHA1) != -1) cipherFlags |= CipherFlags.TLS_DATA_INTEGRITY_METHOD_SHA1;
                    if (secGroups[0].IndexOf(MD5) != -1) cipherFlags |= CipherFlags.TLS_DATA_INTEGRITY_METHOD_MD5;
                }


                if (secGroups[0].IndexOf(eNULL) != -1) count++;
                if (secGroups[0].IndexOf(tripleDES) != -1) count++;
                if (secGroups[0].IndexOf(AES) != -1)
                {
                    if (!bDisableAES128) count++;
                    if (!bDisableAES256) count++;
                }


                if (count < 2)
                {
                    if (secGroups[0].IndexOf(eNULL) != -1) cipherFlags |= CipherFlags.TLS_ENCRYPTION_METHOD_NONE;
                    if (secGroups[0].IndexOf(tripleDES) != -1) cipherFlags |= CipherFlags.TLS_ENCRYPTION_METHOD_3DES;
                    if (secGroups[0].IndexOf(AES) != -1)
                    {
                        if (!bDisableAES128) cipherFlags |= CipherFlags.TLS_ENCRYPTION_METHOD_AES128;
                        if (!bDisableAES256) cipherFlags |= CipherFlags.TLS_ENCRYPTION_METHOD_AES256;
                    }
                }
                else
                {
                    securitySettingsChanged = true;
                    // Set 3DES as default
                    cipherFlags |= CipherFlags.TLS_ENCRYPTION_METHOD_3DES;
                }

                if (securitySettingsChanged & isFirstcheck)
                {
                    MessageBox.Show("Configuration not supported by new openssl version, default configuration is loaded", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    isFirstcheck = false;
                }
                else if (!securitySettingsChanged & !isFirstcheck)
                {
                    isFirstcheck = true;
                }

                return cipherFlags;
            }
            return base.ConvertFrom(context, culture, value);
        }
        // Overrides the ConvertTo method of TypeConverter.
        public override object ConvertTo(ITypeDescriptorContext context,
            System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                CipherFlags cipherFlags = (CipherFlags)value;
                // Determine AES128 and AES256 flag settings
                bool bAES128 = ((cipherFlags & CipherFlags.TLS_ENCRYPTION_METHOD_AES128) != 0);
                bool bAES256 = ((cipherFlags & CipherFlags.TLS_ENCRYPTION_METHOD_AES256) != 0);
                // Remove AES128 and AES256 flags. These are handled seperately.
                cipherFlags &= ~CipherFlags.TLS_ENCRYPTION_METHOD_AES128;
                cipherFlags &= ~CipherFlags.TLS_ENCRYPTION_METHOD_AES256;
                // Generate ', ' seperated list of flagged values.
                string libraryString = System.Enum.Format(typeof(CipherFlags), cipherFlags, "g");
                // Replace ", " seperations by "+" seperations
                libraryString = libraryString.Replace(", ", SecElementSeperator);
                // Replace enum string values by library string values.
                libraryString = libraryString.Replace(CipherFlags.TLS_AUTHENICATION_METHOD_RSA.ToString(), aRSA);
                libraryString = libraryString.Replace(CipherFlags.TLS_AUTHENICATION_METHOD_DSA.ToString(), aDSS);
                libraryString = libraryString.Replace(CipherFlags.TLS_KEY_EXCHANGE_METHOD_RSA.ToString(), kRSA);
                libraryString = libraryString.Replace(CipherFlags.TLS_KEY_EXCHANGE_METHOD_DH.ToString(), DH);
                libraryString = libraryString.Replace(CipherFlags.TLS_DATA_INTEGRITY_METHOD_SHA1.ToString(), SHA1);
                libraryString = libraryString.Replace(CipherFlags.TLS_DATA_INTEGRITY_METHOD_MD5.ToString(), MD5);
                libraryString = libraryString.Replace(CipherFlags.TLS_ENCRYPTION_METHOD_NONE.ToString(), eNULL);
                libraryString = libraryString.Replace(CipherFlags.TLS_ENCRYPTION_METHOD_3DES.ToString(), tripleDES);
                if (bAES128 || bAES256) libraryString += (SecElementSeperator + AES);
                // Append AES128 and AES256 disabling settings
                if (!bAES128 && bAES256) libraryString += (SecGroupSeperator + NoAES128);
                if (bAES128 && !bAES256) libraryString += (SecGroupSeperator + NoAES256);
                // Append post fix
                libraryString += TlsPostFix;
                return libraryString;
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    /// <summary>
    /// Flags indicating the type of security used.
    /// </summary>
    /// <remarks>
    /// Combination of;
    /// <list type="bullet">
    /// <item>
    ///  <term>Entity Authentication</term>
    ///  <description>Mechanisms that allow for checking the integrity of the data transmitted.</description>
    /// </item>
    /// <item>
    ///  <term>Key Exchange</term>
    ///  <description>
    ///  DICOM calls this “Exchange of Master Secrets”. 
    ///  This is the method that is used to exchange the symmetric key to be used 
    ///  for encoding the messages
    ///  </description>
    /// </item>
    /// <item>
    ///  <term>Data Integrity</term>
    ///  <description>Mechanisms that allow for checking the integrity of the data transmitted.</description>
    /// </item>
    /// <item>
    ///  <term>Privacy</term>
    ///  <description>Mechanisms that allow for encrypting the data sent.</description>
    /// </item>
    /// </list>
    /// </remarks>
    [Flags]
    [TypeConverter(typeof(CipherFlagsConverter))]
    public enum CipherFlags
    {
        /// <summary>
        /// Rivest-Shamir-Adleman.  
        /// A public key algorithm that provides both digital signatures and encryption.  
        /// Also the name of the company that developed this and other algorithms.
        /// </summary>
        TLS_AUTHENICATION_METHOD_RSA = 1 << 0, // "aRSA"
        /// <summary>
        /// DSA – Digital Signature Algorithm.
        /// A method for computing digital signatures.
        /// </summary>
        TLS_AUTHENICATION_METHOD_DSA = 1 << 1, // "aDSS"
        /// <summary>
        /// Rivest-Shamir-Adleman.
        /// A public key algorithm that provides both digital signatures and encryption.
        /// Also the name of the company that developed this and other algorithms.
        /// </summary>
        TLS_KEY_EXCHANGE_METHOD_RSA = 1 << 2, // "kRSA"
        /// <summary>
        /// DH – Diffie-Hellman algorithm.
        /// An algorithm used to exchange symmetric keys.
        /// </summary>
        TLS_KEY_EXCHANGE_METHOD_DH = 1 << 3, // "DH"
        /// <summary>
        /// Secure Hash Algorithm. 
        /// Used to generate a cryptographic hash (or checksum). 
        /// FIPS 180-1.
        /// </summary>
        TLS_DATA_INTEGRITY_METHOD_SHA1 = 1 << 4, // "SHA1"
        /// <summary>
        /// MD5 – Message Digest 5.
        /// Used to generate a cryptographic hash (or checksum).
        /// RFC 1321.
        /// </summary>
        TLS_DATA_INTEGRITY_METHOD_MD5 = 1 << 5, // "MD5"
        /// <summary>
        /// No encryption.
        /// </summary>
        TLS_ENCRYPTION_METHOD_NONE = 1 << 6, // "eNULL"
        /// <summary>
        /// Triple DES (also known as 3DES) – 
        /// DES applied 3 times in a row (encode, decode, encode).
        /// Highly secure, well trusted symmetric key encryption algorithm.
        /// </summary>
        TLS_ENCRYPTION_METHOD_3DES = 1 << 7, // "3DES"
        /// <summary>
        /// AES – Advanced Encryption Standard.
        /// Newer symmetric key encryption algorithm.
        /// Expected to replace Triple DES.
        /// FIPS (Federal Information Processing Standard) 197.
        /// </summary>
        TLS_ENCRYPTION_METHOD_AES128 = 1 << 8, // "AES128"
        /// <summary>
        /// AES – Advanced Encryption Standard.
        /// Newer symmetric key encryption algorithm.
        /// Expected to replace Triple DES.
        /// FIPS (Federal Information Processing Standard) 197.
        /// </summary>
        TLS_ENCRYPTION_METHOD_AES256 = 1 << 9, // "AES256"
    }
    /// <summary>
    /// Access to settings for security
    /// </summary>
    public interface ISecuritySettings
    {
        /// <summary>
        /// Enable or disable secure socket communication
        /// </summary>
        System.Boolean SecureSocketsEnabled
        {
            get;
            set;
        }
        /// <summary>
        /// Protocol password
        /// </summary>
        System.String TlsPassword
        {
            get;
            set;
        }
        /// <summary>
        /// Protocol(s) Supported
        /// </summary>

        TlsVersionFlags MaxTlsVersionFlags
        {
            get;
            set;
        }


        TlsVersionFlags MinTlsVersionFlags
        {
            get;
            set;
        }
        /// <summary>
        /// Verifies the SSL peer and fail the connection if the peer has no ceritificate.
        /// </summary>
        System.Boolean CheckRemoteCertificate
        {
            get;
            set;
        }
        /// <summary>
        /// Flags indicating the type of security used.
        /// </summary>
        CipherFlags CipherFlags
        {
            get;
            set;
        }

        bool securitySettingsChanged
        {
            get;
        }
        /// <summary>
        /// Session caching will be optionally supported.
        /// It can be turned on or off.
        /// DVT will only support in memory caching.
        /// Caching will be enabled by default.
        /// </summary>
        System.Boolean CacheTlsSessions
        {
            get;
            set;
        }
        /// <summary>
        /// Sets the session cache timeout time used by the TLS server.
        /// </summary>
        System.UInt16 TlsCacheTimeout
        {
            get;
            set;
        }
        /// <summary>
        /// Credentials file name
        /// </summary>
        FileName CredentialsFileName
        {
            get;
            set;
        }
        /// <summary>
        /// Certificate file name
        /// </summary>
        FileName CertificateFileName
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates that the contents of one of the files has changed 
        /// </summary>
        void ApplyChangedSocketParameters();
    }
    internal class SecuritySettings : ISecuritySettings
    {
        internal SecuritySettings(Wrappers.MBaseSession adaptee)
        {
            if (adaptee == null) throw new System.ArgumentNullException();

            m_adaptee = adaptee;
        }
        protected Wrappers.MBaseSession m_adaptee = null;

        public System.Boolean SecureSocketsEnabled
        {
            get
            {
                return this.m_adaptee.UseSecureSockets;
            }
            set
            {
                this.m_adaptee.UseSecureSockets = value;
            }
        }
        public System.String TlsPassword
        {
            get
            {
                return this.m_adaptee.TlsPassword;
            }
            set
            {
                if (value == null) throw new System.ArgumentNullException();
                this.m_adaptee.TlsPassword = value;
            }
        }

        public TlsVersionFlags MaxTlsVersionFlags
        {
            get
            {
                return (TlsVersionFlags)
                    TypeDescriptor.
                    GetConverter(typeof(TlsVersionFlags)).
                    ConvertFromString(this.m_adaptee.MaxTlsVersion);
            }
            set
            {
                this.m_adaptee.MaxTlsVersion =
                    TypeDescriptor.GetConverter(value).ConvertToString(value);
            }
        }

        public TlsVersionFlags MinTlsVersionFlags
        {
            get
            {
                return (TlsVersionFlags)
                    TypeDescriptor.
                    GetConverter(typeof(TlsVersionFlags)).
                    ConvertFromString(this.m_adaptee.MinTlsVersion);
            }
            set
            {
                this.m_adaptee.MinTlsVersion =
                    TypeDescriptor.GetConverter(value).ConvertToString(value);
            }
        }

        CipherFlagsConverter bla = new CipherFlagsConverter();

        public bool securitySettingsChanged
        {
            get
            {

                Type type = typeof(CipherFlagsConverter);
                MethodInfo info = type.GetMethod("getSecuritySettingsChangedStatus");

                object bla2 = info.Invoke(bla, new object[] { });
                return (bool)bla2;


            }
        }

        public System.Boolean CheckRemoteCertificate
        {
            get
            {
                return this.m_adaptee.CheckRemoteCertificate;
            }
            set
            {
                this.m_adaptee.CheckRemoteCertificate = value;
            }
        }
        public CipherFlags CipherFlags
        {
            get
            {
                return (CipherFlags)bla.ConvertFrom(this.m_adaptee.CipherList);
                //TypeDescriptor.
                //GetConverter(typeof(CipherFlags)).
                //ConvertFromString(this.m_adaptee.CipherList); 
            }
            set
            {
                //if (
                //    (value & CipherFlags.TLS_AUTHENICATION_METHOD_RSA) == 0 &&
                //    (value & CipherFlags.TLS_AUTHENICATION_METHOD_DSA) == 0
                //    ) throw new System.ArgumentException("Select at least one Authentication method");
                //if (
                //    (value & CipherFlags.TLS_KEY_EXCHANGE_METHOD_RSA) == 0 &&
                //    (value & CipherFlags.TLS_KEY_EXCHANGE_METHOD_DH) == 0
                //    ) throw new System.ArgumentException("Select at least one Key Exchange method");
                //if (
                //    (value & CipherFlags.TLS_DATA_INTEGRITY_METHOD_SHA1) == 0 &&
                //    (value & CipherFlags.TLS_DATA_INTEGRITY_METHOD_MD5) == 0
                //    ) throw new System.ArgumentException("Select at least one Data Integrity method");
                //if (
                //    (value & CipherFlags.TLS_ENCRYPTION_METHOD_NONE) == 0 &&
                //    (value & CipherFlags.TLS_ENCRYPTION_METHOD_3DES) == 0 &&
                //    (value & CipherFlags.TLS_ENCRYPTION_METHOD_AES128) == 0 &&
                //    (value & CipherFlags.TLS_ENCRYPTION_METHOD_AES256) == 0
                //    ) throw new System.ArgumentException("Select at least one Encryption method");
                if (!_ValidCipherFlags(value))
                    throw new System.ArgumentException("Configuration does not match any cipher suites");
                this.m_adaptee.CipherList =
                    TypeDescriptor.GetConverter(value).ConvertToString(value);
            }
        }
        private System.Boolean _ValidCipherFlags(CipherFlags value)
        {
            string cipherList = TypeDescriptor.GetConverter(value).ConvertToString(value);
            return this.m_adaptee.IsValidCipherList(cipherList);
        }
        public System.Boolean CacheTlsSessions
        {
            get
            {
                return this.m_adaptee.CacheTlsSessions;
            }
            set
            {
                this.m_adaptee.CacheTlsSessions = value;
            }
        }
        public System.UInt16 TlsCacheTimeout
        {
            get
            {
                return this.m_adaptee.TlsCacheTimeout;
            }
            set
            {
                this.m_adaptee.TlsCacheTimeout = value;
            }
        }
        public FileName CredentialsFileName
        {
            get
            {
                return this.m_adaptee.CredentialsFileName;
            }
            set
            {
                if (value == null) throw new System.ArgumentNullException();
                this.m_adaptee.CredentialsFileName = value;
            }
        }
        public FileName CertificateFileName
        {
            get
            {
                return this.m_adaptee.CertificateFileName;
            }
            set
            {
                if (value == null) throw new System.ArgumentNullException();
                this.m_adaptee.CertificateFileName = value;
            }
        }
        public void ApplyChangedSocketParameters()
        {
            this.m_adaptee.SocketParametersChanged = true;
        }
    }
}