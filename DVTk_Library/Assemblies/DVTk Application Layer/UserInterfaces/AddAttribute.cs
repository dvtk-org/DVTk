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
    /// Summary description for AddAttribute.
    /// </summary>
    public class AddAttribute : System.Windows.Forms.Form
    {
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_attrTag;
        private System.Windows.Forms.TextBox textBox_attrVR;
        private System.Windows.Forms.TextBox textBox_attrValue;
        private System.Windows.Forms.Button button_ok;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button1;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        private string attributeTag;
        private string attributeVR;
        private string attributeValue;

        /// <summary>
        /// 
        /// </summary>
        public AddAttribute()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddAttribute));
            this.textBox_attrTag = new System.Windows.Forms.TextBox();
            this.textBox_attrVR = new System.Windows.Forms.TextBox();
            this.textBox_attrValue = new System.Windows.Forms.TextBox();
            this.button_ok = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox_attrTag
            // 
            this.textBox_attrTag.Location = new System.Drawing.Point(112, 16);
            this.textBox_attrTag.Name = "textBox_attrTag";
            this.textBox_attrTag.Size = new System.Drawing.Size(128, 20);
            this.textBox_attrTag.TabIndex = 1;
            // 
            // textBox_attrVR
            // 
            this.textBox_attrVR.Location = new System.Drawing.Point(112, 80);
            this.textBox_attrVR.Name = "textBox_attrVR";
            this.textBox_attrVR.Size = new System.Drawing.Size(40, 20);
            this.textBox_attrVR.TabIndex = 3;
            // 
            // textBox_attrValue
            // 
            this.textBox_attrValue.Location = new System.Drawing.Point(112, 112);
            this.textBox_attrValue.Name = "textBox_attrValue";
            this.textBox_attrValue.Size = new System.Drawing.Size(176, 20);
            this.textBox_attrValue.TabIndex = 4;
            // 
            // button_ok
            // 
            this.button_ok.Location = new System.Drawing.Point(64, 176);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(64, 23);
            this.button_ok.TabIndex = 5;
            this.button_ok.Text = "OK";
            this.button_ok.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(24, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 23);
            this.label1.TabIndex = 7;
            this.label1.Text = "Attribute Tag";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(24, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 23);
            this.label3.TabIndex = 9;
            this.label3.Text = "Attribute VR";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(24, 112);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 23);
            this.label4.TabIndex = 10;
            this.label4.Text = "Attribute Value";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(16, 48);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(296, 23);
            this.label5.TabIndex = 13;
            this.label5.Text = "Note: Specify Attribute Tag as:(gggg,eeee) or gggg,eeee";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(16, 144);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(280, 23);
            this.label6.TabIndex = 14;
            this.label6.Text = "Note: Specify mutliple attribute values seperated by \'\\\'";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(176, 176);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 15;
            this.button1.Text = "Cancel";
            this.button1.Click += new System.EventHandler(this.buttonCancle_Click);
            // 
            // AddAttribute
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(328, 214);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBox_attrValue);
            this.Controls.Add(this.textBox_attrVR);
            this.Controls.Add(this.textBox_attrTag);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button_ok);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddAttribute";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Add New Attribute";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private void buttonOk_Click(object sender, System.EventArgs e)
        {
            bool ok = true;
            attributeTag = textBox_attrTag.Text;
            if ((!attributeTag.StartsWith("(")) && (!attributeTag.StartsWith("(")))
            {
                attributeTag = "(" + attributeTag + ")";
            }

            string vrString = textBox_attrVR.Text.Trim().ToUpper();
            if (CheckVR(vrString))
            {
                attributeVR = vrString;
            }
            else
            {
                string msg = "Provide valid VR e.g. AE, AT.\n";
                DialogResult theDialogResult = MessageBox.Show(msg, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                ok = false;
                textBox_attrVR.Focus();
            }

            attributeValue = textBox_attrValue.Text;

            if (ok)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string NewAttributeTag
        {
            get { return this.attributeTag; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string NewAttributeVR
        {
            get { return this.attributeVR; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string NewAttributeValue
        {
            get { return this.attributeValue; }
        }

        private bool CheckVR(string vrString)
        {
            bool isVRValid = false;
            if ((vrString == "AE")
                || (vrString == "AS")
                || (vrString == "AT")
                || (vrString == "CS")
                || (vrString == "DA")
                || (vrString == "DS")
                || (vrString == "DT")
                || (vrString == "FL")
                || (vrString == "FD")
                || (vrString == "IS")
                || (vrString == "LO")
                || (vrString == "LT")
                || (vrString == "OB")
                || (vrString == "OF")
                || (vrString == "OW")
                || (vrString == "PN")
                || (vrString == "SH")
                || (vrString == "SL")
                || (vrString == "SQ")
                || (vrString == "SS")
                || (vrString == "ST")
                || (vrString == "TM")
                || (vrString == "UI")
                || (vrString == "UL")
                || (vrString == "UN")
                || (vrString == "US")
                || (vrString == "UT"))
            {
                isVRValid = true;
            }
            return isVRValid;
        }

        //private void linkLabel1_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        //{
        //    // Get full definition file name.
        //    string definitionDir = Environment.GetEnvironmentVariable("COMMONPROGRAMFILES") + @"\DVTk\Definition Files\DICOM\";
        //    string theFullFileName = definitionDir + "Allotherattributes.def";

        //    // Open the definition file in notepad.
        //    System.Diagnostics.Process theProcess  = new System.Diagnostics.Process();

        //    theProcess.StartInfo.FileName= "Notepad.exe";
        //    theProcess.StartInfo.Arguments = theFullFileName;

        //    theProcess.Start();			
        //}

        private void buttonCancle_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }
    }
}
