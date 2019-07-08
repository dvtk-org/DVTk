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
using System.Windows.Forms;


namespace Dvt
{
	/// <summary>
	/// Summary description for ChangePasswordForm.
	/// </summary>
	public class ChangePasswordForm : System.Windows.Forms.Form
	{
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox TextBoxCurrentPassword;
        private System.Windows.Forms.TextBox TextBoxNewPassword;
        private System.Windows.Forms.TextBox TextBoxReEnterPassword;
        private System.Windows.Forms.Button ButtonOK;
        private System.Windows.Forms.Button ButtonCancel;
        private System.Windows.Forms.CheckBox CheckBoxUseDefaultPassword;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

        

        
        


        public ChangePasswordForm(string current_password)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

            this._current_password = current_password;
            if (current_password == MainForm.DEFAULT_CERTIFICATE_FILE_PASSWORD)
                this.CheckBoxUseDefaultPassword.Checked = true;
            else
                this.CheckBoxUseDefaultPassword.Checked = false;
        }

        string _current_password;

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

        public string password
        {
            get
            {
                if (this.CheckBoxUseDefaultPassword.Checked)
                    return MainForm.DEFAULT_CERTIFICATE_FILE_PASSWORD;
                else
                    return this.TextBoxNewPassword.Text;
            }
        }

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.CheckBoxUseDefaultPassword = new System.Windows.Forms.CheckBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.TextBoxCurrentPassword = new System.Windows.Forms.TextBox();
			this.TextBoxNewPassword = new System.Windows.Forms.TextBox();
			this.TextBoxReEnterPassword = new System.Windows.Forms.TextBox();
			this.ButtonOK = new System.Windows.Forms.Button();
			this.ButtonCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 48);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(136, 23);
			this.label1.TabIndex = 6;
			this.label1.Text = "Enter current password:";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(16, 88);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(136, 23);
			this.label2.TabIndex = 8;
			this.label2.Text = "Enter new password:";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(16, 120);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(136, 23);
			this.label3.TabIndex = 9;
			this.label3.Text = "Re-enter new password:";
			// 
			// CheckBoxUseDefaultPassword
			// 
			this.CheckBoxUseDefaultPassword.Location = new System.Drawing.Point(16, 8);
			this.CheckBoxUseDefaultPassword.Name = "CheckBoxUseDefaultPassword";
			this.CheckBoxUseDefaultPassword.Size = new System.Drawing.Size(272, 24);
			this.CheckBoxUseDefaultPassword.TabIndex = 0;
			this.CheckBoxUseDefaultPassword.Text = "Use default password";
			this.CheckBoxUseDefaultPassword.CheckedChanged += new System.EventHandler(this.CheckBoxUseDefaultPassword_CheckedChanged);
			// 
			// groupBox1
			// 
			this.groupBox1.Location = new System.Drawing.Point(16, 64);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(272, 8);
			this.groupBox1.TabIndex = 7;
			this.groupBox1.TabStop = false;
			// 
			// TextBoxCurrentPassword
			// 
			this.TextBoxCurrentPassword.Location = new System.Drawing.Point(152, 40);
			this.TextBoxCurrentPassword.Name = "TextBoxCurrentPassword";
			this.TextBoxCurrentPassword.PasswordChar = '*';
			this.TextBoxCurrentPassword.Size = new System.Drawing.Size(136, 20);
			this.TextBoxCurrentPassword.TabIndex = 1;
			this.TextBoxCurrentPassword.Text = "";
			// 
			// TextBoxNewPassword
			// 
			this.TextBoxNewPassword.Location = new System.Drawing.Point(152, 80);
			this.TextBoxNewPassword.Name = "TextBoxNewPassword";
			this.TextBoxNewPassword.PasswordChar = '*';
			this.TextBoxNewPassword.Size = new System.Drawing.Size(136, 20);
			this.TextBoxNewPassword.TabIndex = 2;
			this.TextBoxNewPassword.Text = "";
			// 
			// TextBoxReEnterPassword
			// 
			this.TextBoxReEnterPassword.Location = new System.Drawing.Point(152, 112);
			this.TextBoxReEnterPassword.Name = "TextBoxReEnterPassword";
			this.TextBoxReEnterPassword.PasswordChar = '*';
			this.TextBoxReEnterPassword.Size = new System.Drawing.Size(136, 20);
			this.TextBoxReEnterPassword.TabIndex = 3;
			this.TextBoxReEnterPassword.Text = "";
			// 
			// ButtonOK
			// 
			this.ButtonOK.Location = new System.Drawing.Point(120, 152);
			this.ButtonOK.Name = "ButtonOK";
			this.ButtonOK.TabIndex = 4;
			this.ButtonOK.Text = "OK";
			this.ButtonOK.Click += new System.EventHandler(this.ButtonOK_Click);
			// 
			// ButtonCancel
			// 
			this.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.ButtonCancel.Location = new System.Drawing.Point(208, 152);
			this.ButtonCancel.Name = "ButtonCancel";
			this.ButtonCancel.TabIndex = 5;
			this.ButtonCancel.Text = "Cancel";
			// 
			// ChangePasswordForm
			// 
			this.AcceptButton = this.ButtonOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.ButtonCancel;
			this.ClientSize = new System.Drawing.Size(304, 192);
			this.Controls.Add(this.ButtonOK);
			this.Controls.Add(this.TextBoxCurrentPassword);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.CheckBoxUseDefaultPassword);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.TextBoxNewPassword);
			this.Controls.Add(this.TextBoxReEnterPassword);
			this.Controls.Add(this.ButtonCancel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ChangePasswordForm";
			this.ShowInTaskbar = false;
			this.Text = "Change password";
			this.ResumeLayout(false);

		}
		#endregion

        private void CheckBoxUseDefaultPassword_CheckedChanged(object sender, System.EventArgs e)
        {
            this.TextBoxCurrentPassword.Enabled = !this.CheckBoxUseDefaultPassword.Checked;
            this.TextBoxNewPassword.Enabled = !this.CheckBoxUseDefaultPassword.Checked;
            this.TextBoxReEnterPassword.Enabled = !this.CheckBoxUseDefaultPassword.Checked;
        }

        private void ButtonOK_Click(object sender, System.EventArgs e)
        {
            if (String.Compare(this.TextBoxCurrentPassword.Text, this._current_password, false) != 0)
            {
                MessageBox.Show("Current Password Incorrect! ");
            }
            else if (String.Compare(this.TextBoxNewPassword.Text, this.TextBoxReEnterPassword.Text, false) != 0)
            {
                MessageBox.Show("New password entered donot match!","Warning ",MessageBoxButtons.OK ,MessageBoxIcon.Warning);
            }
            else
            {
                this.DialogResult = DialogResult.OK;
                
                this.Close ();
            }
        }
	}
}
