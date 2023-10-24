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

namespace Dvtk.Certificates
{
    /// <summary>
    /// Type of security key algorithm.
    /// </summary>
    public enum SignatureAlgorithm
    {
        /// <summary>
        /// RSA – Rivest-Shamir-Adleman.
        /// A public key algorithm that provides both digital signatures and encryption. 
        /// Also the name of the company that developed this and other algorithms.
        /// </summary>
        RSA = 0,
        /// <summary>
        /// DSA – Digital Signature Algorithm.
        /// A method for computing digital signatures.
        /// </summary>
        DSA,
    };

    /// <summary>
    /// Certificate Generator - Generates certificates in a specified certificate file.
    /// </summary>
    /// <remarks>
    /// Various properties of the certificate can be set before generation.
    /// </remarks>
    public class CertificateGenerator
    {
        //
        // Use something that does not look like a password.
        //
        private const System.String DEFAULT_CERTIFICATE_FILE_PASSWORD =
            ":-DHE-RSA-AES256-SHA:-DHE-DSS-AES256-SHA:-AES256-SHA";

        /// <summary>
        /// Generate a certificate with the current settings.
        /// </summary>
        public void Generate()
        {
            //
            // Check pre-conditions for properties.
            //
            // Check the key length for specific key types
            //
            switch (this.SignatureAlgorithm)
            {
                case SignatureAlgorithm.RSA:
                    {
                        if ((this.SignatureKeyLength < 512) || (this.SignatureKeyLength > 2048))
                        {
                            throw new System.ArgumentOutOfRangeException(
                                "RSA key length must be between 512 and 2048");
                        }
                        break;
                    }
                case SignatureAlgorithm.DSA:
                    {
                        if ((this.SignatureKeyLength < 512) || (this.SignatureKeyLength > 1024))
                        {
                            throw new System.ArgumentOutOfRangeException(
                                "DSA key length must be between 512 and 1024");
                        }
                        break;
                    }
                default:
                    //
                    // Unknown SignatureAlgorithm
                    //
                    throw new System.NotImplementedException();
            }
            //
            // Check for illegal characters in CommonName
            //
            if (this.CommonName.IndexOf("/=") != -1)
            {
                throw new System.ArgumentOutOfRangeException(
                    "The characters \"/\" and \"=\" are not allowed in a name field");
            }
            //
            // Check for illegal characters in OrganizationalUnit
            //
            if (this.OrganizationalUnit.IndexOf("/=") != -1)
            {
                throw new System.ArgumentOutOfRangeException(
                    "The characters \"/\" and \"=\" are not allowed in a name field");
            }
            //
            // Check for illegal characters in Organizational
            //
            if (this.Organization.IndexOf("/=") != -1)
            {
                throw new System.ArgumentOutOfRangeException(
                    "The characters \"/\" and \"=\" are not allowed in a name field");
            }
            //
            // Check for illegal characters in Locality
            //
            if (this.Locality.IndexOf("/=") != -1)
            {
                throw new System.ArgumentOutOfRangeException(
                    "The characters \"/\" and \"=\" are not allowed in a name field");
            }
            //
            // Check for illegal characters in State
            //
            if (this.State.IndexOf("/=") != -1)
            {
                throw new System.ArgumentOutOfRangeException(
                    "The characters \"/\" and \"=\" are not allowed in a name field");
            }
            //
            // Check for illegal characters in Country
            //
            if (this.Country.IndexOf("/=") != -1)
            {
                throw new System.ArgumentOutOfRangeException(
                    "The characters \"/\" and \"=\" are not allowed in a name field");
            }
            //
            // Create certificate
            //
            Wrappers.MCertificate certificate = new Wrappers.MCertificate();
            //
            // Initialize certificate
            //
            // Only generating version 1 certificates
            //
            certificate.Version = 1;
            certificate.Subject =
                BuildSubjectName(
                this.CommonName,
                this.OrganizationalUnit,
                this.Organization,
                this.Locality,
                this.State,
                this.Country);
            certificate.EffectiveDate = this.ValidFromDate;
            certificate.ExpirationDate = this.ValidToDate;
            switch (this.SignatureAlgorithm)
            {
                case SignatureAlgorithm.RSA:
                    certificate.SignatureAlgorithm = "RSA";
                    break;
                case SignatureAlgorithm.DSA:
                    certificate.SignatureAlgorithm = "DSA";
                    break;
                default:
                    //
                    // Unknown SignatureAlgorithm
                    //
                    throw new System.NotImplementedException();
            }
            certificate.SignatureKeyLength = this.SignatureKeyLength;
            System.String credentialsFilePassword;
            if (
                this.CredentialsFilePassword == null ||
                this.CredentialsFilePassword == System.String.Empty)
            {
                //
                // use the default password for the credentials file if none is specified
                //
                credentialsFilePassword = DEFAULT_CERTIFICATE_FILE_PASSWORD;
            }
            else
            {
                credentialsFilePassword = this.CredentialsFilePassword;
            }
            System.String credentialsFile;
            if (this.SelfSign)
            {
                credentialsFile = null;
            }
            else
            {
                credentialsFile = this.CredentialsFile;
            }
            certificate.GenerateFiles(
                credentialsFile,
                credentialsFilePassword,
                this.PrivateKeyPassword,
                this.PrivateKeyFile,
                this.CertificateFile);
        }

        static private System.String BuildSubjectName(
            System.String commonName,
            System.String organizationalUnit,
            System.String organization,
            System.String locality,
            System.String state,
            System.String country)
        {
            //
            // Check preconditions
            //
            System.String subjectName = System.String.Empty;
            if (
                commonName != null &&
                commonName != System.String.Empty
                )
            {
                subjectName += ("/CN=" + commonName);
            }
            if (
                organizationalUnit != null &&
                organizationalUnit != System.String.Empty
                )
            {
                subjectName += ("/OU=" + organizationalUnit);
            }
            if (
                organization != null &&
                organization != System.String.Empty
                )
            {
                subjectName += ("/O=" + organization);
            }
            if (
                locality != null &&
                locality != System.String.Empty
                )
            {
                subjectName += ("/L=" + locality);
            }
            if (
                state != null &&
                state != System.String.Empty
                )
            {
                subjectName += ("/ST=" + state);
            }
            if (
                country != null &&
                country != System.String.Empty
                )
            {
                subjectName += ("/C=" + country);
            }
            return subjectName;
        }
        /// <summary>
        /// Certificate file to be generated.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Argument is a <see langword="null"/> reference.</exception>
        public System.String CertificateFile
        {
            get
            {
                return _CertificateFile;
            }
            set
            {
                if (value == null) throw new System.ArgumentNullException();
                if (value.Length == 0) throw new System.ArgumentOutOfRangeException();
                _CertificateFile = value;
            }
        }
        private System.String _CertificateFile = DefaultCertificateFile;
        /// <summary>
        /// Certificate file name.
        /// </summary>
        public const System.String DefaultCertificateFile = "new.pem";

        /// <summary>
        /// Common Name
        /// </summary>
        /// <remarks>
        /// The name of your server as it appears in the server's URL (for example, www.acme.com). 
        /// This name must be identical to the fully qualified domain name of the server for 
        /// which you are requesting a certificate. 
        /// Do not include the protocol specifier (http://) or any port numbers or pathnames 
        /// in the common name. Do not use use wildcards such as * or ?, and do not use an IP address.
        /// </remarks>
        public System.String CommonName
        {
            get
            {
                return _CommonName;
            }
            set
            {
                _CommonName = value;
            }
        }
        private System.String _CommonName = null;
        /// <summary>
        /// Country
        /// </summary>
        /// <remarks>
        /// The two-letter ISO abbreviation for the country (for example, US for the United States).
        /// </remarks>
        public System.String Country
        {
            get
            {
                return _Country;
            }
            set
            {
                _Country = value;
            }
        }
        private System.String _Country = null;
        /// <summary>
        /// Locality
        /// </summary>
        /// <remarks>
        /// Usually the name of the city in which your organization has its head office.
        /// </remarks>
        public System.String Locality
        {
            get
            {
                return _Locality;
            }
            set
            {
                _Locality = value;
            }
        }
        private System.String _Locality = null;
        /// <summary>
        /// Organization
        /// </summary>
        /// <remarks>
        /// The name under which your organization is registered. 
        /// This organization must own the domain name that appears in common name of your server. 
        /// </remarks>
        public System.String Organization
        {
            get
            {
                return _Organization;
            }
            set
            {
                _Organization = value;
            }
        }
        private System.String _Organization = null;
        /// <summary>
        /// Organizational Unit
        /// </summary>
        /// <remarks>
        /// Normally the name of the department or group that will be using the secure server.
        /// </remarks>
        public System.String OrganizationalUnit
        {
            get
            {
                return _OrganizationalUnit;
            }
            set
            {
                _OrganizationalUnit = value;
            }
        }
        private System.String _OrganizationalUnit = null;
        /// <summary>
        /// State
        /// </summary>
        /// <remarks>
        /// Usually the name of the state in which your organization has its head office. 
        /// </remarks>
        public System.String State
        {
            get
            {
                return _State;
            }
            set
            {
                _State = value;
            }
        }
        private System.String _State = null;
        /// <summary>
        /// The signer's credentials file - may be <see langword="null"/>.
        /// </summary>
        /// <remarks>
        /// If credential file <see langword="null"/>, a self signed certificate will be generated.
        /// </remarks>
        public System.String CredentialsFile
        {
            get
            {
                return _CredentialsFile;
            }
            set
            {
                _CredentialsFile = value;
            }
        }
        private System.String _CredentialsFile = DefaultCredentialsFile;
        /// <summary>
        /// Default Credentials File name
        /// </summary>
        public const System.String DefaultCredentialsFile = "credentials.pem";

        /// <summary>
        /// Password for the signer's credentials file
        /// </summary>
        public System.String CredentialsFilePassword
        {
            get
            {
                return _CredentialsFilePassword;
            }
            set
            {
                _CredentialsFilePassword = value;
            }
        }
        private System.String _CredentialsFilePassword = System.String.Empty;
        /// <summary>
        /// Private Key file to be generated
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Argument is a <see langword="null"/> reference.</exception>
        public System.String PrivateKeyFile
        {
            get
            {
                return _PrivateKeyFile;
            }
            set
            {
                if (value == null) throw new System.ArgumentNullException();
                if (value.Length == 0) throw new System.ArgumentOutOfRangeException();
                _PrivateKeyFile = value;
            }
        }
        private System.String _PrivateKeyFile = DefaultPrivateKeyFile;
        /// <summary>
        /// Default PrivateKey File name
        /// </summary>
        public const System.String DefaultPrivateKeyFile = "newkey.pem";

        /// <summary>
        /// Private key password
        /// </summary>
        public System.String PrivateKeyPassword
        {
            get
            {
                return _KeyPassword;
            }
            set
            {
                _KeyPassword = value;
            }
        }
        private System.String _KeyPassword = System.String.Empty;
        /// <summary>
        /// Number of bits use in the signing algorithm
        /// </summary>
        public System.Int32 SignatureKeyLength
        {
            get
            {
                return _KeyLength;
            }
            set
            {
                _KeyLength = value;
            }
        }
        private System.Int32 _KeyLength = 1024;
        /// <summary>
        /// Signature Algorithm
        /// </summary>
        public SignatureAlgorithm SignatureAlgorithm
        {
            get
            {
                return _SignatureAlgorithm;
            }
            set
            {
                _SignatureAlgorithm = value;
            }
        }
        private SignatureAlgorithm _SignatureAlgorithm = SignatureAlgorithm.RSA;
        /// <summary>
        /// The beginning date for the validity of the certificate.
        /// </summary>
        public System.DateTime ValidFromDate
        {
            get
            {
                return _ValidFromDate;
            }
            set
            {
                _ValidFromDate = value;
            }
        }
        private System.DateTime _ValidFromDate = System.DateTime.Now;
        /// <summary>
        /// The ending date for the validity of the certificate.
        /// </summary>
        public System.DateTime ValidToDate
        {
            get
            {
                return _ValidToDate;
            }
            set
            {
                _ValidToDate = value;
            }
        }
        private System.DateTime _ValidToDate = System.DateTime.Now.AddYears(2);
        /// <summary>
        /// Specify whether to use self signing
        /// </summary>
        public System.Boolean SelfSign
        {
            get
            {
                return _SelfSign;
            }
            set
            {
                _SelfSign = value;
            }
        }
        private System.Boolean _SelfSign = false;
    }
}