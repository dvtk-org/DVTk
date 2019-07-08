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

namespace Dvtk.Security
{
    /// <summary>
    /// Access to information of the security item.
    /// </summary>
    /// <remarks>
    /// The security item may be either a <see cref="Certificate"/> or <see cref="Credential"/>.
    /// </remarks>
    public interface ISecurityItem
    {
        /// <summary>
        /// Effective date
        /// </summary>
        /// <remarks>
        /// returns <see langword="null"/> if no date is found.
        /// </remarks>
        object EffectiveDate                { get; }
        /// <summary>
        /// Expiration date
        /// </summary>
        /// <remarks>
        /// returns <see langword="null"/> if no date is found.
        /// </remarks>
        object ExpirationDate               { get; }
        /// <summary>
        /// Issuer
        /// </summary>
        System.String Issuer                { get; }
        /// <summary>
        /// Serial number
        /// </summary>
        System.String SerialNumber          { get; }
        /// <summary>
        /// Signature algorithm
        /// </summary>
        System.String SignatureAlgorithm    { get; }
        /// <summary>
        /// Signature key length
        /// </summary>
        System.Int32 SignatureKeyLength     { get; }
        /// <summary>
        /// Subject
        /// </summary>
        System.String Subject               { get; }
        /// <summary>
        /// Version
        /// </summary>
        System.Int32 Version                { get; }
    }
    /// <summary>
    /// Certificate security item.
    /// </summary>
    public class Certificate : ISecurityItem
    {
        readonly private Wrappers.MCertificate _certificate;
        // ctor
        internal Certificate(Wrappers.MCertificate certificate)
        {
            _certificate = certificate;
        }
        #region ISecurityItem
        /// <summary>
        /// <see cref="ISecurityItem.EffectiveDate"/>
        /// </summary>
        public object EffectiveDate
        { 
            get 
            { 
                System.DateTime effectiveDate = _certificate.EffectiveDate;
                if (effectiveDate == System.DateTime.MinValue)
                {
                    return null;
                }
                else
                {
                    return effectiveDate;
                }
            }
        }
        /// <summary>
        /// <see cref="ISecurityItem.ExpirationDate"/>
        /// </summary>
        public object ExpirationDate
        {
            get 
            { 
                System.DateTime expirationDate = _certificate.ExpirationDate;
                if (expirationDate == System.DateTime.MinValue)
                {
                    return null;
                }
                else
                {
                    return expirationDate;
                }
            }
        }
        /// <summary>
        /// <see cref="ISecurityItem.Issuer"/>
        /// </summary>
        public System.String Issuer
        {
            get 
            { 
                return _certificate.Issuer; 
            }
        }
        /// <summary>
        /// <see cref="ISecurityItem.SerialNumber"/>
        /// </summary>
        public System.String SerialNumber
        {
            get 
            { 
                return _certificate.SerialNumber; 
            }
        }
        /// <summary>
        /// <see cref="ISecurityItem.SignatureAlgorithm"/>
        /// </summary>
        public System.String SignatureAlgorithm
        {
            get 
            { 
                return _certificate.SignatureAlgorithm; 
            }
        }
        /// <summary>
        /// <see cref="ISecurityItem.SignatureKeyLength"/>
        /// </summary>
        public System.Int32 SignatureKeyLength
        {
            get 
            { 
                return _certificate.SignatureKeyLength; 
            }
        }
        /// <summary>
        /// <see cref="ISecurityItem.Subject"/>
        /// </summary>
        public System.String Subject
        {
            get 
            { 
                return _certificate.Subject; 
            }
        }
        /// <summary>
        /// <see cref="ISecurityItem.Version"/>
        /// </summary>
        public System.Int32 Version
        {
            get 
            { 
                return _certificate.Version; 
            }
        }
        #endregion ISecurityItem
    }
    /// <summary>
    /// Credential security item.
    /// </summary>
    public class Credential : ISecurityItem
    {
        readonly private Wrappers.MCertificate _credential;
        // ctor
        internal Credential(Wrappers.MCertificate credential)
        {
            _credential = credential;
        }
        #region ISecurityItem
        /// <summary>
        /// <see cref="ISecurityItem.EffectiveDate"/>
        /// </summary>
        public object EffectiveDate
        { 
            get 
            { 
                System.DateTime effectiveDate = _credential.EffectiveDate;
                if (effectiveDate == System.DateTime.MinValue)
                {
                    return null;
                }
                else
                {
                    return effectiveDate;
                }
            }
        }
        /// <summary>
        /// <see cref="ISecurityItem.ExpirationDate"/>
        /// </summary>
        public object ExpirationDate
        {
            get 
            { 
                System.DateTime expirationDate = _credential.ExpirationDate;
                if (expirationDate == System.DateTime.MinValue)
                {
                    return null;
                }
                else
                {
                    return expirationDate;
                }
            }
        }
        /// <summary>
        /// <see cref="ISecurityItem.Issuer"/>
        /// </summary>
        public System.String Issuer
        {
            get 
            { 
                return _credential.Issuer; 
            }
        }
        /// <summary>
        /// <see cref="ISecurityItem.SerialNumber"/>
        /// </summary>
        public System.String SerialNumber
        {
            get 
            { 
                return _credential.SerialNumber; 
            }
        }
        /// <summary>
        /// <see cref="ISecurityItem.SignatureAlgorithm"/>
        /// </summary>
        public System.String SignatureAlgorithm
        {
            get 
            { 
                return _credential.SignatureAlgorithm; 
            }
        }
        /// <summary>
        /// <see cref="ISecurityItem.SignatureKeyLength"/>
        /// </summary>
        public System.Int32 SignatureKeyLength
        {
            get 
            { 
                return _credential.SignatureKeyLength; 
            }
        }
        /// <summary>
        /// <see cref="ISecurityItem.Subject"/>
        /// </summary>
        public System.String Subject
        {
            get 
            { 
                return _credential.Subject; 
            }
        }
        /// <summary>
        /// <see cref="ISecurityItem.Version"/>
        /// </summary>
        public System.Int32 Version
        {
            get
            { 
                return _credential.Version; 
            }
        }
        #endregion ISecurityItem
    }
    /// <summary>
    /// Access to certificate file handling.
    /// </summary>
    public interface ICertificateHandling
    {
        /// <summary>
        /// List of certificates
        /// </summary>
        System.Collections.ArrayList Certificates { get; }
        /// <summary>
        /// File name
        /// </summary>
        System.String FileName { get; set; }
        /// <summary>
        /// Password
        /// </summary>
        System.String Password { get; set; }
        /// <summary>
        /// Import certificate from an other file.
        /// </summary>
        /// <param name="fileName">file name</param>
        /// <param name="password">password</param>
        void ImportCertificateFromOtherFile(
            System.String fileName, System.String password);
        /// <summary>
        /// Remove certificates from file.
        /// </summary>
        /// <param name="indexes">certificates to be removed</param>
        /// <returns>returns <see langword="true"/> on success</returns>
        System.Boolean RemoveCertificatesFromFile(params System.UInt32[] indexes);
        /// <summary>
        /// Load certificate file
        /// </summary>
        /// <param name="fileName">file name</param>
        /// <param name="password">password</param>
        /// <exception cref="System.ApplicationException">Loaded certificate file can not be verified.</exception>
        void LoadCertificateFile(System.String fileName, System.String password);
        /// <summary>
        /// Verify whether the changes result in a valid certificate file.
        /// </summary>
        /// <param name="verificationMessage">
        /// When not valid, the verification message states additional 
        /// information about the verification
        /// </param>
        /// <returns>
        /// Returns <see langword="true"/> if valid else <see langword="false"/>
        /// </returns>
        System.Boolean VerifyChanges(out System.String verificationMessage);
        /// <summary>
        /// Save certificate to file.
        /// </summary>
        /// <remarks>
        /// The file will not be saved if the changes are not valid.
        /// </remarks>
        /// <param name="verificationMessage">
        /// When not valid, the verification message states additional 
        /// information about the verification
        /// </param>
        /// <returns>
        /// Returns <see langword="true"/> if valid else <see langword="false"/>
        /// </returns>
        System.Boolean SaveCertificateFile(out System.String verificationMessage);
        /// <summary>
        /// Save certificate to file. Even if the changes are not valid.
        /// </summary>
        /// <returns></returns>
        System.Boolean ForcedSaveCertificateFile();
    }
    // Usage scenario:
    // 1) Load (Unsaved
    // 2) Add|Remove certificates*
    // 3) Verify
    // 4) Save
    internal class CertificateHandling : ICertificateHandling
    {
        //
        // Specify the security item which is handled in by this class.
        //
        private const Wrappers.WrappedSecurityItemType SecurityItemType = 
            Wrappers.WrappedSecurityItemType.Certificate;

        /// <summary>
        /// <see cref="ICertificateHandling.Certificates"/>
        /// </summary>
        public System.Collections.ArrayList Certificates
        {
            //
            // return ReadOnly variant of the internal ArrayList.
            //
            get 
            { 
                return System.Collections.ArrayList.ReadOnly(_certificateList); 
            }
        }
        private System.Collections.ArrayList _certificateList = 
            new System.Collections.ArrayList();

        private void _RefreshCertificateListFromFile()
        {
            this._certificateList.Clear();
            // Determine number of certificates in the certificate file.
            System.UInt32 nrOfCertificates = this._certificateFile.NumberOfCertificates;
            for (System.UInt32 index = 0; index < nrOfCertificates; index++)
            {
                Wrappers.MCertificate internalCertificate = this._certificateFile.get_Certificate(index);
                this._certificateList.Add(new Certificate(internalCertificate));
            }
            return;
        }
        //
        // Service Access Point - on session.
        //
        private Dvtk.Sessions.ISecuritySettings _securitySettings = null;
        //
        // ctor
        //
        internal CertificateHandling(
            Dvtk.Sessions.ISecure secureSession)
        {
            this._securitySettings = secureSession.SecuritySettings;
            this._password = this._securitySettings.TlsPassword;
            this._fileName = this._securitySettings.CertificateFileName;
            // Auto load certificate file using the session settings.
            this.LoadCertificateFile(this._fileName, this._password);
        }
        // dtor
        ~CertificateHandling()
        {
        }
        /// <summary>
        /// <see cref="ICertificateHandling.FileName"/>
        /// </summary>
        public System.String FileName
        {
            get 
            { 
                return _fileName; 
            }
            set 
            { 
                _fileName = value; 
            }
        }
        private System.String _fileName = System.String.Empty;
        /// <summary>
        /// <see cref="ICertificateHandling.Password"/>
        /// </summary>
        public System.String Password
        {
            get 
            { 
                return _password; 
            }
            set 
            { 
                _password = value; 
            }
        }
        private System.String _password = System.String.Empty;
        /// <summary>
        /// <see cref="ICertificateHandling.ImportCertificateFromOtherFile"/>
        /// </summary>
        public void ImportCertificateFromOtherFile(
            System.String fileName, System.String password)
        {
            this._certificateFile.AddCertificate(
                Wrappers.WrappedSecurityItemType.Certificate,
                fileName,
                password);
            _RefreshCertificateListFromFile();
            return;
        }
        /// <summary>
        /// <see cref="ICertificateHandling.RemoveCertificatesFromFile"/>
        /// </summary>
        public System.Boolean RemoveCertificatesFromFile(params System.UInt32[] indexes)
        {
            if (this._certificateFile == null) return false;
            System.Boolean success = true;
            foreach (System.UInt32 index in indexes)
            {
                if (!success) break;
                success = this._certificateFile.RemoveCertificate(index);
            }
            _RefreshCertificateListFromFile();
            return success;
        }
        /// <summary>
        /// <see cref="ICertificateHandling.LoadCertificateFile"/>
        /// </summary>
        public void LoadCertificateFile(System.String fileName, System.String password)
        {
            this._certificateFile = null;
            this._fileName = System.String.Empty;
            // reset before try{} to load
            this._certificateFile = 
                new Wrappers.MCertificateFile(
                SecurityItemType,
                fileName,
                password);
            System.String verificationMessage = System.String.Empty;
            bool bValid = this._certificateFile.Verify(SecurityItemType, out verificationMessage);
            if (!bValid) throw new System.ApplicationException(verificationMessage);
            this._fileName = fileName;
            this._password = password;
            _RefreshCertificateListFromFile();
        }
        private Wrappers.MCertificateFile _certificateFile = null;

        private System.Boolean _CheckForChanges(
            out System.Boolean sessionChanged,
            out System.Boolean certificateFileChanged)
        {
            sessionChanged = false;
            certificateFileChanged = false;
            bool ignoreCase = true;
            if (
                System.String.Compare(
                this._fileName,
                this._securitySettings.CertificateFileName,
                ignoreCase) != 0)
            {
                sessionChanged = true;
            }
            if (this._certificateFile.HasChanged)
            {
                certificateFileChanged = true;
            }
            return (certificateFileChanged || sessionChanged);
        }
        /// <summary>
        /// <see cref="ICertificateHandling.VerifyChanges"/>
        /// </summary>
        public System.Boolean VerifyChanges(out System.String verificationMessage)
        {
            System.Boolean bValid = true;
            verificationMessage = System.String.Empty;
            System.Boolean sessionChanged;
            System.Boolean certificateFileChanged;
            _CheckForChanges(out sessionChanged, out certificateFileChanged);
            if (certificateFileChanged)
            {
                bValid = this._certificateFile.Verify(
                    Wrappers.WrappedSecurityItemType.Certificate,
                    out verificationMessage);
                if (bValid)
                {
                    verificationMessage = 
                        "The trusted certificate file \"" +
                        _fileName +
                        "\" has changed\n\n";
                }
                else
                {
                    verificationMessage = 
                        "The trusted certificate file \"" +
                        _fileName +
                        "\" has changed and is invalid\n\n" +
                        verificationMessage +
                        "\n\nA valid trusted certificate file contains the trusted certificates" +
                        " and certificate chains always terminating with a root (self signed) certificate\n\n";
                }
            }
            return bValid;
        }
        /// <summary>
        /// <see cref="ICertificateHandling.SaveCertificateFile"/>
        /// </summary>
        public System.Boolean SaveCertificateFile(out System.String verificationMessage)
        {
            System.Boolean forcedSave = false;
            return _SaveCertificateFile(forcedSave, out verificationMessage);
        }
        /// <summary>
        /// <see cref="ICertificateHandling.ForcedSaveCertificateFile"/>
        /// </summary>
        public System.Boolean ForcedSaveCertificateFile()
        {
            System.String verificationMessage;
            System.Boolean forcedSave = true;
            return _SaveCertificateFile(forcedSave, out verificationMessage);
        }
        private System.Boolean _SaveCertificateFile(
            System.Boolean forcedSave, 
            out System.String verificationMessage)
        {
            System.Boolean sessionChanged;
            System.Boolean certFileChanged;
            _CheckForChanges(out sessionChanged, out certFileChanged);
            if (!VerifyChanges(out verificationMessage))
            {
                if (!forcedSave) return false;
            }
            if (sessionChanged)
            {
                // Apply changes to the session.
                this._securitySettings.CertificateFileName = this._fileName;
            }
            if (certFileChanged)
            {
                this._certificateFile.WriteFile(this._password);
                // the certificate file changed, so there are changes that affect the socket
                //sessionM_ptr->setSocketParametersChanged(true);
            }
            return true;
        }
    }
    /// <summary>
    /// Access to credential file handling.
    /// </summary>
    public interface ICredentialHandling
    {
        /// <summary>
        /// List of credentials
        /// </summary>
        System.Collections.ArrayList Credentials { get; }
        /// <summary>
        /// File name
        /// </summary>
        System.String FileName { get; set; }
        /// <summary>
        /// Password
        /// </summary>
        System.String Password { get; set; }
        /// <summary>
        /// Import credential from an other file.
        /// </summary>
        /// <param name="fileName">file name</param>
        /// <param name="password">password</param>
        void ImportCredentialFromOtherFile(
            System.String fileName, System.String password);
        /// <summary>
        /// Remove credentials from file.
        /// </summary>
        /// <param name="indexes">credentials to be removed</param>
        /// <returns>returns <see langword="true"/> on success</returns>
        System.Boolean RemoveCredentialFromFile(params System.UInt32[] indexes);
        /// <summary>
        /// Load credential file
        /// </summary>
        /// <param name="fileName">file name</param>
        /// <param name="password">password</param>
        void LoadCredentialFile(System.String fileName, System.String password);
        /// <summary>
        /// Verify whether the changes result in a valid credential file.
        /// </summary>
        /// <param name="verificationMessage">
        /// When not valid, the verification message states additional 
        /// information about the verification
        /// </param>
        /// <returns>
        /// Returns <see langword="true"/> if valid else <see langword="false"/>
        /// </returns>
        System.Boolean VerifyChanges(out System.String verificationMessage);
        /// <summary>
        /// Save credential to file.
        /// </summary>
        /// <remarks>
        /// The file will not be saved if the changes are not valid.
        /// </remarks>
        /// <param name="verificationMessage">
        /// When not valid, the verification message states additional 
        /// information about the verification
        /// </param>
        /// <returns>
        /// Returns <see langword="true"/> if valid else <see langword="false"/>
        /// </returns>
        System.Boolean SaveCredentialFile(out System.String verificationMessage);
        /// <summary>
        /// Save credential to file. Even if the changes are not valid.
        /// </summary>
        /// <returns></returns>
        System.Boolean ForcedSaveCredentialFile();
    }
    // Usage scenario:
    // 1) Load (Unsaved
    // 2) Add|Remove credential*
    // 3) Verify
    // 4) Save
    internal class CredentialHandling : ICredentialHandling
    {
        // Specify the security item which is handled in by this class.
        private const Wrappers.WrappedSecurityItemType SecurityItemType = 
            Wrappers.WrappedSecurityItemType.Credential;

        public System.Collections.ArrayList Credentials
        {
            // return ReadOnly variant of the internal ArrayList.
            get { return System.Collections.ArrayList.ReadOnly(_credentialList); }
        }
        private System.Collections.ArrayList _credentialList = 
            new System.Collections.ArrayList();

        private void _RefreshCredentialListFromFile()
        {
            this._credentialList.Clear();
            // Determine number of credentials in the credentials file.
            System.UInt32 nrOfCredentials = this._credentialFile.NumberOfCertificates;
            for (System.UInt32 index = 0; index < nrOfCredentials; index++)
            {
                Wrappers.MCertificate internalCredential = this._credentialFile.get_Certificate(index);
                this._credentialList.Add(new Credential(internalCredential));
            }
            return;
        }

        // Service Access Point - on session.
        private Dvtk.Sessions.ISecuritySettings _securitySettings = null;
        // ctor
        internal CredentialHandling(
            Dvtk.Sessions.ISecure secureSession)
        {
            this._securitySettings = secureSession.SecuritySettings;
            this._password = this._securitySettings.TlsPassword;
            this._fileName = this._securitySettings.CredentialsFileName;
            // Auto load credential file using the session settings.
            this.LoadCredentialFile(this._fileName, this._password);
        }
        // dtor
        ~CredentialHandling()
        {}

        public System.String FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }
        private System.String _fileName = System.String.Empty;

        public System.String Password
        {
            get { return _password; }
            set { _password = value; }
        }
        private System.String _password = System.String.Empty;

        public void ImportCredentialFromOtherFile(
            System.String fileName, System.String password)
        {
            this._credentialFile.AddCertificate(
                Wrappers.WrappedSecurityItemType.Credential,
                fileName,
                password);
            _RefreshCredentialListFromFile();
            return;
        }

        public System.Boolean RemoveCredentialFromFile(params System.UInt32[] indexes)
        {
            if (this._credentialFile == null) return false;
            System.Boolean success = true;
            foreach (System.UInt32 index in indexes)
            {
                if (!success) break;
                success = this._credentialFile.RemoveCertificate(index);
            }
            _RefreshCredentialListFromFile();
            return success;
        }

        public void LoadCredentialFile(System.String fileName, System.String password)
        {
            this._credentialFile = null;
            this._fileName = System.String.Empty;
            // reset before try{} to load
            this._credentialFile = 
                new Wrappers.MCertificateFile(
                SecurityItemType,
                fileName,
                password);
            System.String verificationMessage = System.String.Empty;
            bool bValid = this._credentialFile.Verify(SecurityItemType, out verificationMessage);
            if (!bValid) throw new System.ApplicationException(verificationMessage);
            this._fileName = fileName;
            this._password = password;
            _RefreshCredentialListFromFile();
        }
        private Wrappers.MCertificateFile _credentialFile = null;

        private System.Boolean _CheckForChanges(
            out System.Boolean sessionChanged,
            out System.Boolean credentialFileChanged)
        {
            sessionChanged = false;
            credentialFileChanged = false;
            bool ignoreCase = true;
            if (
                System.String.Compare(
                this._fileName,
                this._securitySettings.CredentialsFileName,
                ignoreCase) != 0)
            {
                sessionChanged = true;
            }
            if (
                System.String.Compare(
                this._password,
                this._securitySettings.TlsPassword) != 0)
            {
                // the password in the session changed
                sessionChanged = true;
            }
            if (
                System.String.Compare(
                this._password,
                this._credentialFile.Password) != 0)
            {
                // the password in the credential file changed
                credentialFileChanged = true;
            }
            if (this._credentialFile.HasChanged)
            {
                credentialFileChanged = true;
            }
            return (credentialFileChanged || sessionChanged);
        }

        public System.Boolean VerifyChanges(out System.String verificationMessage)
        {
            System.Boolean bValid = true;
            verificationMessage = System.String.Empty;
            System.Boolean sessionChanged;
            System.Boolean credentialFileChanged;
            _CheckForChanges(out sessionChanged, out credentialFileChanged);
            if (credentialFileChanged)
            {
                bValid = this._credentialFile.Verify(
                    Wrappers.WrappedSecurityItemType.Credential,
                    out verificationMessage);
                if (bValid)
                {
                    verificationMessage = 
                        "The security credential file \"" +
                        _fileName +
                        "\" has changed\n\n";
                }
                else
                {
                    verificationMessage = 
                        "The security credential file \"" +
                        _fileName +
                        "\" has changed and is invalid\n\n" +
                        verificationMessage +
                        "\n\nA valid credentials file is structured as:\n" +
                        "    private key\n" +
                        "    certificate corresponding to the private key\n" +
                        "    certificate chain from the certificate back to the root (self signed) certificate\n" +
                        "    optionally a second private key of the other type (RSA or DSA) than the first private key\n" +
                        "    the certificate and chain for the second private key\n\n";
                }
            }
            return bValid;
        }
        public System.Boolean SaveCredentialFile(out System.String verificationMessage)
        {
            System.Boolean forcedSave = false;
            return _SaveCredentialFile(forcedSave, out verificationMessage);
        }
        public System.Boolean ForcedSaveCredentialFile()
        {
            System.String verificationMessage;
            System.Boolean forcedSave = true;
            return _SaveCredentialFile(forcedSave, out verificationMessage);
        }
        private System.Boolean _SaveCredentialFile(
            System.Boolean forcedSave, 
            out System.String verificationMessage)
        {
            System.Boolean sessionChanged;
            System.Boolean certFileChanged;
            _CheckForChanges(out sessionChanged, out certFileChanged);
            if (!VerifyChanges(out verificationMessage))
            {
                if (!forcedSave) return false;
            }
            if (sessionChanged)
            {
                // Apply changes to the session.
                this._securitySettings.CredentialsFileName = this._fileName;
                this._securitySettings.TlsPassword = this._password;
            }
            if (certFileChanged)
            {
                this._credentialFile.WriteFile(this._password);
                // the credential file changed, so there are changes that affect the socket
                //sessionM_ptr->setSocketParametersChanged(true);
            }
            return true;
        }
    }
}