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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using Dvtk;
using DvtkApplicationLayer;

namespace Dvt
{
	/// <summary>
	/// Summary description for CredentialsCertificatesForm.
	/// </summary>
	public class CredentialsCertificatesForm : System.Windows.Forms.Form
	{
        private System.Windows.Forms.Button ButtonCancel;
        private System.Windows.Forms.DataGrid DataGridCertificates;
        private System.Windows.Forms.OpenFileDialog DialogOpenFile;
        private System.Windows.Forms.Button ButtonAddCertificate;
        private System.Windows.Forms.Button ButtonRemoveCertificate;
        private System.Windows.Forms.Button ButtonChangePassword;
        private System.Windows.Forms.Button ButtonOK;
        private System.Windows.Forms.OpenFileDialog DialogAddCertificate;
        private System.Windows.Forms.DataGridTableStyle GridTableStyle;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

        private Dvtk.Security.ICertificateHandling  _cert_handling;
        private Dvtk.Security.ICredentialHandling   _cred_handling;
		private string _FileContainingKeys = "";

		private ArrayList _KeysInformation = new ArrayList();
		private string _ExceptionText = "";

		public CredentialsCertificatesForm(DvtkApplicationLayer.Session session, bool show_credentials)
		{
            //
			// Required for Windows Form Designer support
			//
			InitializeComponent();

            if (session.Implementation is Dvtk.Sessions.ISecure)
            {
                this._show_credentials = show_credentials;
                this._session = session;
                this.ButtonRemoveCertificate.Enabled = false;

                if (show_credentials)
                {
                    this.Text = "DVT Private Keys (credentials)";
					this.ButtonChangePassword.Visible = true ;
                    _FileContainingKeys = (session.Implementation as Dvtk.Sessions.ISecure).SecuritySettings.CredentialsFileName;
                    this._cred_handling = (session.Implementation as Dvtk.Sessions.ISecure).CreateSecurityCredentialHandler ();
                }
                else
                {
                    this.Text = "SUT Public Keys (certificates)";
					this.ButtonChangePassword.Visible = false ;
                    _FileContainingKeys = (session.Implementation as Dvtk.Sessions.ISecure).SecuritySettings.CertificateFileName;
                    this._cert_handling = (session.Implementation as Dvtk.Sessions.ISecure).CreateSecurityCertificateHandler ();
                }
                FileInfo cert_file = new FileInfo (_FileContainingKeys);

                if (cert_file.Exists)
                {
                    try
                    {
                        if (show_credentials)
                        {
                            FileInfo    file;

                            file = new FileInfo (this._cred_handling.FileName);
                            _FileContainingKeys = this._cred_handling.FileName;

							if (file.Exists)
							{
								_ExceptionText = "";
								_KeysInformation = ConvertToKeyInformationArrayList(_cred_handling.Credentials);
								DataGridCertificates.DataSource = _KeysInformation;
								Debug.Assert(_ExceptionText.Length == 0, "Following errors occured while filling datagrid:\n\n" + _ExceptionText);
								
							}

                            if (this._cred_handling.Credentials.Count > 0)
                                this.ButtonRemoveCertificate.Enabled = true;
                        }
                        else
                        {
                            FileInfo    file;

                            file = new FileInfo (this._cert_handling.FileName);
                            _FileContainingKeys = this._cert_handling.FileName;

							if (file.Exists)
							{
								_ExceptionText = "";
								_KeysInformation = ConvertToKeyInformationArrayList(_cert_handling.Certificates);
								DataGridCertificates.DataSource = _KeysInformation;
								Debug.Assert(_ExceptionText.Length == 0, "Following errors occured while filling datagrid:\n\n" + _ExceptionText);
							}
                            if (this._cert_handling.Certificates.Count > 0)
                                this.ButtonRemoveCertificate.Enabled = true;
                        }
                    }
                    catch
                    {
                        this.DataGridCertificates.DataSource = null;
                    }
                }
            }
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.ButtonAddCertificate = new System.Windows.Forms.Button();
			this.ButtonRemoveCertificate = new System.Windows.Forms.Button();
			this.ButtonChangePassword = new System.Windows.Forms.Button();
			this.ButtonOK = new System.Windows.Forms.Button();
			this.ButtonCancel = new System.Windows.Forms.Button();
			this.DataGridCertificates = new System.Windows.Forms.DataGrid();
			this.GridTableStyle = new System.Windows.Forms.DataGridTableStyle();
			this.DialogOpenFile = new System.Windows.Forms.OpenFileDialog();
			this.DialogAddCertificate = new System.Windows.Forms.OpenFileDialog();
			((System.ComponentModel.ISupportInitialize)(this.DataGridCertificates)).BeginInit();
			this.SuspendLayout();
			// 
			// ButtonAddCertificate
			// 
			this.ButtonAddCertificate.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.ButtonAddCertificate.Location = new System.Drawing.Point(8, 200);
			this.ButtonAddCertificate.Name = "ButtonAddCertificate";
			this.ButtonAddCertificate.Size = new System.Drawing.Size(112, 23);
			this.ButtonAddCertificate.TabIndex = 0;
			this.ButtonAddCertificate.Text = "Import Key";
			this.ButtonAddCertificate.Click += new System.EventHandler(this.ButtonAddCertificate_Click);
			// 
			// ButtonRemoveCertificate
			// 
			this.ButtonRemoveCertificate.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.ButtonRemoveCertificate.Location = new System.Drawing.Point(128, 200);
			this.ButtonRemoveCertificate.Name = "ButtonRemoveCertificate";
			this.ButtonRemoveCertificate.Size = new System.Drawing.Size(112, 23);
			this.ButtonRemoveCertificate.TabIndex = 1;
			this.ButtonRemoveCertificate.Text = "Remove Key";
			this.ButtonRemoveCertificate.Click += new System.EventHandler(this.ButtonRemoveCertificate_Click);
			// 
			// ButtonChangePassword
			// 
			this.ButtonChangePassword.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.ButtonChangePassword.Location = new System.Drawing.Point(248, 200);
			this.ButtonChangePassword.Name = "ButtonChangePassword";
			this.ButtonChangePassword.Size = new System.Drawing.Size(112, 23);
			this.ButtonChangePassword.TabIndex = 2;
			this.ButtonChangePassword.Text = "Change password";
			this.ButtonChangePassword.Click += new System.EventHandler(this.ButtonChangePassword_Click);
			// 
			// ButtonOK
			// 
			this.ButtonOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.ButtonOK.Location = new System.Drawing.Point(472, 200);
			this.ButtonOK.Name = "ButtonOK";
			this.ButtonOK.TabIndex = 3;
			this.ButtonOK.Text = "Ok";
			this.ButtonOK.Click += new System.EventHandler(this.Ok_Click);
			// 
			// ButtonCancel
			// 
			this.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.ButtonCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.ButtonCancel.Location = new System.Drawing.Point(552, 200);
			this.ButtonCancel.Name = "ButtonCancel";
			this.ButtonCancel.TabIndex = 4;
			this.ButtonCancel.Text = "Cancel";
			this.ButtonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
			// 
			// DataGridCertificates
			// 
			this.DataGridCertificates.AllowNavigation = false;
			this.DataGridCertificates.CaptionVisible = false;
			this.DataGridCertificates.DataMember = "";
			this.DataGridCertificates.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.DataGridCertificates.Location = new System.Drawing.Point(8, 40);
			this.DataGridCertificates.Name = "DataGridCertificates";
			this.DataGridCertificates.ParentRowsVisible = false;
			this.DataGridCertificates.ReadOnly = true;
			this.DataGridCertificates.RowHeadersVisible = false;
			this.DataGridCertificates.Size = new System.Drawing.Size(624, 152);
			this.DataGridCertificates.TabIndex = 2;
			this.DataGridCertificates.TableStyles.AddRange(new System.Windows.Forms.DataGridTableStyle[] {
																											 this.GridTableStyle});
			this.DataGridCertificates.TabStop = false;
			// 
			// GridTableStyle
			// 
			this.GridTableStyle.DataGrid = this.DataGridCertificates;
			this.GridTableStyle.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.GridTableStyle.MappingName = "ArrayList";
			// 
			// DialogOpenFile
			// 
			this.DialogOpenFile.Filter = "PEM Certificate files (*.pem;*.cer)|*.pem;*.cer";
			// 
			// DialogAddCertificate
			// 
			this.DialogAddCertificate.Filter = "PEM Certificate files (*.pem;*.cer)|*.pem;*.cer|DER Certificate files (*.cer)|*.c" +
				"er|PKCS#12 files (*.p12;*.pfx)|*.p12;*.pfx|PKCS#7 files (*.p7b;*.p7c)|*.p7b;*.p7" +
				"c";
			// 
			// CredentialsCertificatesForm
			// 
			this.AcceptButton = this.ButtonOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.ButtonCancel;
			this.ClientSize = new System.Drawing.Size(642, 240);
			this.Controls.Add(this.DataGridCertificates);
			this.Controls.Add(this.ButtonAddCertificate);
			this.Controls.Add(this.ButtonRemoveCertificate);
			this.Controls.Add(this.ButtonChangePassword);
			this.Controls.Add(this.ButtonOK);
			this.Controls.Add(this.ButtonCancel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CredentialsCertificatesForm";
			this.ShowInTaskbar = false;
			this.Text = "CredentialsCertificatesForm";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.CredentialsCertificatesForm_Closing);
			((System.ComponentModel.ISupportInitialize)(this.DataGridCertificates)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

        private bool _show_credentials;
        private DvtkApplicationLayer.Session _session;
        private bool file_changed = false;
        private bool session_changed = false;

        private void ButtonCancel_Click(object sender, System.EventArgs e)
        {
            this.Close ();
        }

        private void Ok_Click(object sender, System.EventArgs e)
        {
            if (this.file_changed)
            {
                string message;
                string error_message;
                if (this._show_credentials)
                {
                    if (this._cred_handling.VerifyChanges (out error_message))
                    {
                        message = 
                            string.Format(
                            "The security credentials file \"{0}\" has changed.\n\n", 
                            _FileContainingKeys);
                    }
                    else
                    {
                        message = 
                            string.Format(
                            "The security credentials file \"{0}\" " +
                            "has changed and is invalid.\n\n" +
                            error_message +
                            "\n\nA valid credentials file is structured as:\n" +
                            "  private key\n" +
                            "  certificate corresponding to the private key\n" +
                            "  certificate chain from the certificate back to the root (self signed) certificate\n" +
                            "  optionally a second private key of the other type (RSA or DSA) than the first private key\n" +
                            "  the certificate and chain for the second private key.\n\n",
                            _FileContainingKeys);
                    }
                }
                else
                {
                    if (this._cert_handling.VerifyChanges (out error_message))
                    {
                        message = 
                            string.Format(
                            "The trusted certificate file \"{0}\" has changed.\n\n",
                            _FileContainingKeys);
                    }
                    else
                    {
                        message = 
                            string.Format(
                            "The trusted certificate file \"{0}\" has changed and is invalid.\n" +
                            "\n" +
                            error_message +
                            "\n" +
                            "\n" +
                            "A valid trusted certifcicate file contains the trusted certificates" +
                            " and certificate chain always terminating with a root (self signed) certificate.\n\n",
                            _FileContainingKeys);
                    }
                }
                message += "Do you want to save the changes?";

                switch (MessageBox.Show (this,
                    message,
                    "Save changes?",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button1))
                {
                    case DialogResult.Yes:
                        string verify_msg;
                        if (this._show_credentials)
                            this._cred_handling.SaveCredentialFile (out verify_msg);
                        else
                            this._cert_handling.SaveCertificateFile (out verify_msg);
                        break;
                    case DialogResult.No:
                        break;
                    case DialogResult.Cancel:
                        return;
                    default:
                        break;
                }
            }
            if (this.session_changed)
                this.DialogResult = DialogResult.OK;
            else
                this.DialogResult = DialogResult.Cancel;    // Nothing changed, no need for notifying the session about this.

            this.Close ();
        }

		private void ButtonAddCertificate_Click(object sender, System.EventArgs e)
		{
            bool passed = false;
            bool displayPassword = true;
            System.String ImprtKeyPassword = "";

			if (this._show_credentials)
			{
				this.DialogAddCertificate.Title = "Select Credentials File to Import";
				if (this.DialogAddCertificate.ShowDialog (this) == DialogResult.OK)
				{
                    String ext = this.DialogAddCertificate.FileName.Substring(this.DialogAddCertificate.FileName.LastIndexOf("."));
                    if ((ext == ".cer"))
                        throw new System.ApplicationException("Not a valid credential to import");
                    while (!passed)
                    {
                        try
                        {
                            this._cred_handling.ImportCredentialFromOtherFile(this.DialogAddCertificate.FileName, ImprtKeyPassword);
                            passed = true;
                            displayPassword = false;
                        }
                        catch (Exception excep)
                        {

                            if (excep.GetType().FullName == "Wrappers.Exceptions.PasswordExpection")
                            {
                                passed = false;
                                PassWordForm passWordForm = new PassWordForm( this._session, displayPassword);
                                displayPassword = false;
                                DialogResult res = passWordForm.ShowDialog();
                                if ( res != DialogResult.OK)
                                {                                    
                                    passed = true;
                                    displayPassword = false;
                                }
                                else /* when result is ok */
                                {
                                    ImprtKeyPassword = passWordForm.passWordEntered;
                                }
                            }
                            else
                            {
                                MessageBox.Show(excep.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                passed = true;
                            }

//                            MessageBox.Show(excep.Message);
                        }
                    }
					// Touch.
					this.DataGridCertificates.DataSource = null;

					_ExceptionText = "";
					_KeysInformation = ConvertToKeyInformationArrayList(_cred_handling.Credentials);
					DataGridCertificates.DataSource = _KeysInformation;
					Debug.Assert(_ExceptionText.Length == 0, "Following errors occured while filling datagrid:\n\n" + _ExceptionText);

					this.ButtonRemoveCertificate.Enabled = true;
					this.file_changed = true;
				}
			}
			else
			{
				this.DialogAddCertificate.Title = "Select Certificate File to Import";
				if (this.DialogAddCertificate.ShowDialog (this) == DialogResult.OK)
				{	
					try 
					{
                        String ext = this.DialogAddCertificate.FileName.Substring(this.DialogAddCertificate.FileName.LastIndexOf("."));

                        if (ext == ".pfx") 
                            throw new System.ApplicationException("Not a valid certificate file "); 
						this._cert_handling.ImportCertificateFromOtherFile (this.DialogAddCertificate.FileName, "");
					}
					catch (Exception Message)
					{
						MessageBox.Show(Message.Message);
					}	
					// Touch.
					this.DataGridCertificates.DataSource = null;
						
					_ExceptionText = "";
					_KeysInformation = ConvertToKeyInformationArrayList(_cert_handling.Certificates);
					DataGridCertificates.DataSource = _KeysInformation;
					Debug.Assert(_ExceptionText.Length == 0, "Following errors occured while filling datagrid:\n\n" + _ExceptionText);

					this.ButtonRemoveCertificate.Enabled = true;
					this.file_changed = true;
				}
				
			}
		}

        private void ButtonRemoveCertificate_Click(object sender, System.EventArgs e)
        {
            if (this._show_credentials)
            {
                this._cred_handling.RemoveCredentialFromFile (Convert.ToUInt16 (this.DataGridCertificates.CurrentRowIndex));

				_ExceptionText = "";
				_KeysInformation = ConvertToKeyInformationArrayList(_cred_handling.Credentials);
                DataGridCertificates.DataSource = _KeysInformation;
				Debug.Assert(_ExceptionText.Length == 0, "Following errors occured while filling datagrid:\n\n" + _ExceptionText);

                if (this._cred_handling.Credentials.Count == 0)
                    this.ButtonRemoveCertificate.Enabled = false;
            }
            else
            {
                this._cert_handling.RemoveCertificatesFromFile (Convert.ToUInt16 (this.DataGridCertificates.CurrentRowIndex));

				_ExceptionText = "";
				_KeysInformation = ConvertToKeyInformationArrayList(_cert_handling.Certificates);
                DataGridCertificates.DataSource = _KeysInformation;
				Debug.Assert(_ExceptionText.Length == 0, "Following errors occured while filling datagrid:\n\n" + _ExceptionText);

                if (this._cert_handling.Certificates.Count == 0)
                    this.ButtonRemoveCertificate.Enabled = false;
            }
            this.file_changed = true;
        }

        private void ButtonChangePassword_Click(object sender, System.EventArgs e)
        {
            if (this._show_credentials)
            {
                ChangePasswordForm password_form = new ChangePasswordForm (this._cred_handling.Password);
                if (password_form.ShowDialog (this) == DialogResult.OK)
                {
                    if (this._cred_handling.Password != password_form.password)
                    {
                        this._cred_handling.Password = password_form.password;
                        this.session_changed = true;
                        this.file_changed = true;
                    }
                }
            }
            else
            {
                ChangePasswordForm password_form = new ChangePasswordForm (this._cert_handling.Password);
                if (password_form.ShowDialog (this) == DialogResult.OK)
                {
                    this._cert_handling.Password = password_form.password;
                    this.session_changed = true;
                }
            }
        }

        private void CredentialsCertificatesForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this._show_credentials)
                (this._session.Implementation as Dvtk.Sessions.ISecure).DisposeSecurityCredentialHandler ();
            else
                (this._session.Implementation as Dvtk.Sessions.ISecure).DisposeSecurityCertificateHandler ();
        }

		private ArrayList ConvertToKeyInformationArrayList(ArrayList theKeys)
		{
			_ExceptionText = "";
			ArrayList theKeysInformation = new ArrayList();

			foreach(Dvtk.Security.ISecurityItem theISecurityItem in theKeys)
			{
				theKeysInformation.Add(ConvertToKeyInformation(theISecurityItem));
			}

			return(theKeysInformation);
		}

		private KeyInformation ConvertToKeyInformation(Dvtk.Security.ISecurityItem  theISecurityItem)
		{
			KeyInformation theKeyInformation = new KeyInformation();

			try
			{
				theKeyInformation.Issuer = theISecurityItem.Issuer;
			}
			catch(Exception theException)
			{
				theKeyInformation.Issuer = KeyInformation.UNKNOWN;
				_ExceptionText += "\nIssuer: " + theException.Message + "\n";
			}

			try
			{
				theKeyInformation.ExpirationDate = theISecurityItem.ExpirationDate.ToString();
			}
			catch(Exception theException)
			{
				theKeyInformation.ExpirationDate = KeyInformation.UNKNOWN;
				_ExceptionText += "\nExpirationDate: " + theException.Message + "\n";
			}

			try
			{
				theKeyInformation.Version = theISecurityItem.Version.ToString();
			}
			catch(Exception theException)
			{
				theKeyInformation.Version = KeyInformation.UNKNOWN;
				_ExceptionText += "\nVersion: " + theException.Message + "\n";
			}

			try
			{
				theKeyInformation.SerialNumber = theISecurityItem.SerialNumber;
			}
			catch(Exception theException)
			{
				theKeyInformation.SerialNumber = KeyInformation.UNKNOWN;
				_ExceptionText += "\nSerialNumber: " + theException.Message + "\n";
			}

			try
			{
				theKeyInformation.Subject = theISecurityItem.Subject;
			}
			catch(Exception theException)
			{
				theKeyInformation.Subject = KeyInformation.UNKNOWN;
				_ExceptionText += "\nSubject: " + theException.Message + "\n";
			}

			try
			{
				theKeyInformation.EffectiveDate = theISecurityItem.EffectiveDate.ToString();
			}
			catch(Exception theException)
			{
				theKeyInformation.EffectiveDate = KeyInformation.UNKNOWN;
				_ExceptionText += "\nEffectiveDate: " + theException.Message + "\n";
			}

			try
			{
				theKeyInformation.SignatureKeyLength = theISecurityItem.SignatureKeyLength.ToString();
			}
			catch(Exception theException)
			{
				theKeyInformation.SignatureKeyLength = KeyInformation.UNKNOWN;
				_ExceptionText += "\nSignatureKeyLength " + theException.Message + "\n";
			}

			try
			{
				theKeyInformation.SignatureAlgorithm = theISecurityItem.SignatureAlgorithm;
			}
			catch(Exception theException)
			{
				theKeyInformation.SignatureAlgorithm = KeyInformation.UNKNOWN;
				_ExceptionText += "\nSignatureAlgorithm: " + theException.Message + "\n";
			}

			return(theKeyInformation);
		}

		class KeyInformation
		{
			public const string UNKNOWN = "?";

			public string Issuer
			{
				get
				{
					return _Issuer;
				}

				set
				{
					_Issuer = value;
				}
			}

			public string ExpirationDate
			{
				get
				{
					return _ExpirationDate;
				}

				set
				{
					_ExpirationDate = value;
				}
			}

			public string Version
			{
				get
				{
					return _Version;
				}

				set
				{
					_Version = value;
				}
			}

			public string SerialNumber
			{
				get
				{
					return _SerialNumber;
				}

				set
				{
					_SerialNumber = value;
				}
			}

			public string Subject
			{
				get
				{
					return _Subject;
				}

				set
				{
					_Subject = value;
				}
			}

			public string EffectiveDate
			{
				get
				{
					return _EffectiveDate;
				}

				set
				{
					_EffectiveDate = value;
				}
			}

			public string SignatureKeyLength
			{
				get
				{
					return _SignatureKeyLength;
				}

				set
				{
					_SignatureKeyLength = value;
				}
			}

			public string SignatureAlgorithm
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

			public string _Issuer = "";
			public string _ExpirationDate = "";
			public string _Version = "";
			public string _SerialNumber = "";
			public string _Subject = "";
			public string _EffectiveDate = "";
			public string _SignatureKeyLength = "";
			public string _SignatureAlgorithm = "";
		}

	}
}
