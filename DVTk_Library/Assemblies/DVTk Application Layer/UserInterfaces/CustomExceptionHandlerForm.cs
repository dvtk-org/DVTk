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

namespace DvtkApplicationLayer.UserInterfaces
{
    /// <summary>
    /// Summary description for CustomExceptionHandlerForm.
    /// </summary>
    public class CustomExceptionHandlerForm : System.Windows.Forms.Form
    {
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.TextBox textBoxErrorDescription;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="errorText"></param>
        public CustomExceptionHandlerForm(String errorText)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            this.textBoxErrorDescription.Text = errorText;

            this.buttonClose.Focus();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBoxErrorDescription = new System.Windows.Forms.TextBox();
            this.buttonClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBoxErrorDescription
            // 
            this.textBoxErrorDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxErrorDescription.Location = new System.Drawing.Point(8, 8);
            this.textBoxErrorDescription.Multiline = true;
            this.textBoxErrorDescription.Name = "textBoxErrorDescription";
            this.textBoxErrorDescription.ReadOnly = true;
            this.textBoxErrorDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxErrorDescription.Size = new System.Drawing.Size(536, 432);
            this.textBoxErrorDescription.TabIndex = 1;
            this.textBoxErrorDescription.Text = "";
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.Location = new System.Drawing.Point(240, 448);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.TabIndex = 0;
            this.buttonClose.Text = "Close";
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // CustomExceptionHandlerForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(552, 478);
            this.ControlBox = false;
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.textBoxErrorDescription);
            this.Name = "CustomExceptionHandlerForm";
            this.Text = "An error occurred!";
            this.ResumeLayout(false);

        }
        #endregion

        private void buttonClose_Click(object sender, System.EventArgs e)
        {
            Close();
        }
    }
}
