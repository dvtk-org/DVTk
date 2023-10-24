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

namespace DCMCompare
{
    /// <summary>
    /// Summary description for AddAttribute.
    /// </summary>
    public class AddAttribute : System.Windows.Forms.Form
    {
        private System.Windows.Forms.Label labelGroup;
        private System.Windows.Forms.TextBox textBoxGroup;
        private System.Windows.Forms.TextBox textBoxElement;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Label labelElement;
        private System.Windows.Forms.Label label1;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public AddAttribute()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
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
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(AddAttribute));
            this.labelGroup = new System.Windows.Forms.Label();
            this.labelElement = new System.Windows.Forms.Label();
            this.textBoxGroup = new System.Windows.Forms.TextBox();
            this.textBoxElement = new System.Windows.Forms.TextBox();
            this.buttonOk = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelGroup
            // 
            this.labelGroup.Location = new System.Drawing.Point(8, 24);
            this.labelGroup.Name = "labelGroup";
            this.labelGroup.Size = new System.Drawing.Size(88, 24);
            this.labelGroup.TabIndex = 0;
            this.labelGroup.Text = "Attribute Group:";
            // 
            // labelElement
            // 
            this.labelElement.Location = new System.Drawing.Point(8, 64);
            this.labelElement.Name = "labelElement";
            this.labelElement.Size = new System.Drawing.Size(96, 23);
            this.labelElement.TabIndex = 1;
            this.labelElement.Text = "Attribute Element:";
            // 
            // textBoxGroup
            // 
            this.textBoxGroup.Location = new System.Drawing.Point(104, 24);
            this.textBoxGroup.Name = "textBoxGroup";
            this.textBoxGroup.Size = new System.Drawing.Size(128, 20);
            this.textBoxGroup.TabIndex = 2;
            this.textBoxGroup.Text = "";
            // 
            // textBoxElement
            // 
            this.textBoxElement.Location = new System.Drawing.Point(104, 64);
            this.textBoxElement.Name = "textBoxElement";
            this.textBoxElement.Size = new System.Drawing.Size(128, 20);
            this.textBoxElement.TabIndex = 3;
            this.textBoxElement.Text = "";
            // 
            // buttonOk
            // 
            this.buttonOk.Location = new System.Drawing.Point(96, 136);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.TabIndex = 4;
            this.buttonOk.Text = "OK";
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(0, 104);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(264, 16);
            this.label1.TabIndex = 5;
            this.label1.Text = "Note: Enter group and element values as e.g. 0008";
            // 
            // AddAttribute
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(266, 168);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.textBoxElement);
            this.Controls.Add(this.textBoxGroup);
            this.Controls.Add(this.labelElement);
            this.Controls.Add(this.labelGroup);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddAttribute";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Add Attribute to the filter";
            this.ResumeLayout(false);

        }
        #endregion

        private void buttonOk_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        public UInt16 AttributeGroup
        {
            get
            {
                return UInt16.Parse(textBoxGroup.Text, System.Globalization.NumberStyles.HexNumber);
            }
        }

        public UInt16 AttributeElement
        {
            get
            {
                return UInt16.Parse(textBoxElement.Text, System.Globalization.NumberStyles.HexNumber);
            }
        }
    }
}
