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
	/// Summary description for PassWordForm.
	/// </summary>
	public class PassWordForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label InvalidPasswordLabel;
		private System.Windows.Forms.Label PasswordLabel;
		private System.Windows.Forms.TextBox PasswordTextBox;
		private System.Windows.Forms.Label MessageLabel;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private DvtkApplicationLayer.Session _session;

        public string passWordEntered = "";

        public PassWordForm()
        {
            InitializeComponent();
        } 
        public PassWordForm(DvtkApplicationLayer.Session session, bool pwdCalledFirst)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			this.PasswordTextBox.Text = "";
			this._session = session;

            if (pwdCalledFirst == true)
            {
                this.InvalidPasswordLabel.Text = "Password";
            }
            else
            {
                this.InvalidPasswordLabel.Text = "Invalid Password ";
            }
			//
			// TODO: Add any constructor code after InitializeComponent call
			//
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PassWordForm));
            this.InvalidPasswordLabel = new System.Windows.Forms.Label();
            this.PasswordLabel = new System.Windows.Forms.Label();
            this.PasswordTextBox = new System.Windows.Forms.TextBox();
            this.MessageLabel = new System.Windows.Forms.Label();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // InvalidPasswordLabel
            // 
            this.InvalidPasswordLabel.Location = new System.Drawing.Point(38, 37);
            this.InvalidPasswordLabel.Name = "InvalidPasswordLabel";
            this.InvalidPasswordLabel.Size = new System.Drawing.Size(404, 28);
            this.InvalidPasswordLabel.TabIndex = 0;
            this.InvalidPasswordLabel.Text = "Invalid password . ";
            // 
            // PasswordLabel
            // 
            this.PasswordLabel.Location = new System.Drawing.Point(38, 83);
            this.PasswordLabel.Name = "PasswordLabel";
            this.PasswordLabel.Size = new System.Drawing.Size(404, 28);
            this.PasswordLabel.TabIndex = 1;
            this.PasswordLabel.Text = "Enter Password";
            // 
            // PasswordTextBox
            // 
            this.PasswordTextBox.Location = new System.Drawing.Point(38, 120);
            this.PasswordTextBox.Name = "PasswordTextBox";
            this.PasswordTextBox.PasswordChar = '*';
            this.PasswordTextBox.Size = new System.Drawing.Size(404, 22);
            this.PasswordTextBox.TabIndex = 2;
            this.PasswordTextBox.UseSystemPasswordChar = true;
            this.PasswordTextBox.TextChanged += new System.EventHandler(this.PasswordTextBox_TextChanged);
            this.PasswordTextBox.DragEnter += new System.Windows.Forms.DragEventHandler(this.PasswordTextBox_DragEnter);
            this.PasswordTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PassWordForm_KeyDown);
            // 
            // MessageLabel
            // 
            this.MessageLabel.Location = new System.Drawing.Point(38, 157);
            this.MessageLabel.Name = "MessageLabel";
            this.MessageLabel.Size = new System.Drawing.Size(404, 26);
            this.MessageLabel.TabIndex = 3;
            this.MessageLabel.Text = "This password will be used as password for  Secure Socket keys";
            this.MessageLabel.Click += new System.EventHandler(this.label3_Click);
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(86, 194);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(125, 37);
            this.okButton.TabIndex = 3;
            this.okButton.Text = "OK";
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            this.okButton.Enter += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(230, 194);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(125, 37);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // PassWordForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.ClientSize = new System.Drawing.Size(488, 270);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.MessageLabel);
            this.Controls.Add(this.PasswordTextBox);
            this.Controls.Add(this.PasswordLabel);
            this.Controls.Add(this.InvalidPasswordLabel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PassWordForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PassWordForm";
            this.Load += new System.EventHandler(this.PassWordForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PassWordForm_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.PassWordForm_KeyPress);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void PassWordForm_Load(object sender, System.EventArgs e)
		{
		
		}

		private void label3_Click(object sender, System.EventArgs e)
		{
		
		}

		private void okButton_Click(object sender, System.EventArgs e)
		{
			(this._session.Implementation as Dvtk.Sessions.ISecure).SecuritySettings.TlsPassword = this.PasswordTextBox.Text ;
            passWordEntered = this.PasswordTextBox.Text.ToString();
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void cancelButton_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void PasswordTextBox_TextChanged(object sender, System.EventArgs e)
		{
		
		}

		private void PasswordTextBox_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
		{
		
		}

		private void PassWordForm_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
		
		}

		private void PassWordForm_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if ((e.KeyCode == Keys.Enter) || (e.KeyCode == Keys.Return))
			{
				(this.okButton as Button).PerformClick();
			}
		}
	}
}
