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
using System.IO;
using System.Management;

namespace SnifferUI
{
	/// <summary>
	/// Summary description for UserOptions.
	/// </summary>
	public class UserOptions : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBoxViewerPath;
		private System.Windows.Forms.Button buttonOk;
		private System.Windows.Forms.Button buttonBrowse;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
        private CheckBox retainInfoCheckBox;
		string userOptionsFilePath;
        //static public int kernelBufferSize=100;
        static public bool retainOldAssociationsInfo = false;

		public UserOptions()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

            string dataDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\DVTk\DICOM Network Analyzer";
            userOptionsFilePath = dataDirectory + @"\UserOptions.txt";

           // kernelBufferSizeBox.Text = kernelBufferSize.ToString();
            retainInfoCheckBox.Checked = retainOldAssociationsInfo;
			// Set default viewer path from user option text file
			StreamReader reader = new StreamReader(userOptionsFilePath);
			if(reader != null)
			{
				string line = reader.ReadLine().Trim();
				if(line != "")
					textBoxViewerPath.Text = line;
				reader.Close();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserOptions));
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxViewerPath = new System.Windows.Forms.TextBox();
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonBrowse = new System.Windows.Forms.Button();
            this.retainInfoCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(0, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "DICOM Pixel Viewer";
            // 
            // textBoxViewerPath
            // 
            this.textBoxViewerPath.Location = new System.Drawing.Point(2, 40);
            this.textBoxViewerPath.Name = "textBoxViewerPath";
            this.textBoxViewerPath.Size = new System.Drawing.Size(352, 20);
            this.textBoxViewerPath.TabIndex = 1;
            // 
            // buttonOk
            // 
            this.buttonOk.Location = new System.Drawing.Point(180, 129);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 2;
            this.buttonOk.Text = "OK";
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // buttonBrowse
            // 
            this.buttonBrowse.Location = new System.Drawing.Point(360, 40);
            this.buttonBrowse.Name = "buttonBrowse";
            this.buttonBrowse.Size = new System.Drawing.Size(75, 23);
            this.buttonBrowse.TabIndex = 3;
            this.buttonBrowse.Text = "Browse...";
            this.buttonBrowse.Click += new System.EventHandler(this.button1_Click);
            // 
            // retainInfoCheckBox
            // 
            this.retainInfoCheckBox.AutoSize = true;
            this.retainInfoCheckBox.Location = new System.Drawing.Point(3, 97);
            this.retainInfoCheckBox.Name = "retainInfoCheckBox";
            this.retainInfoCheckBox.Size = new System.Drawing.Size(410, 17);
            this.retainInfoCheckBox.TabIndex = 54;
            this.retainInfoCheckBox.Text = "Retain the DICOM Associations from Earlier Captures while starting a new capture";
            this.retainInfoCheckBox.UseVisualStyleBackColor = true;
            this.retainInfoCheckBox.CheckedChanged += new System.EventHandler(this.retainInfoCheckBox_CheckedChanged);
            // 
            // UserOptions
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(448, 173);
            this.Controls.Add(this.retainInfoCheckBox);
            this.Controls.Add(this.buttonBrowse);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.textBoxViewerPath);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UserOptions";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "User Options";
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		public string DICOMViewerPath
		{
			get
			{
				return textBoxViewerPath.Text;
			}
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			OpenFileDialog theFileDialog = new OpenFileDialog();

			theFileDialog.Filter = "Executables (*.exe) |*.exe";
			theFileDialog.Multiselect = false;
			theFileDialog.Title = "Select viewer application";

			if (theFileDialog.ShowDialog () == DialogResult.OK)
			{
				textBoxViewerPath.Text = theFileDialog.FileName;
			}		
		}

		private void buttonOk_Click(object sender, System.EventArgs e)
		{
            //int userInputBufferSize;
            
            
            //userInputBufferSize = Int32.Parse(kernelBufferSizeBox.Text.ToString());
            
            ////Maximum size of the Buffer cannot exceed 4GB.
            //if (userInputBufferSize < 4096)
            //{
            //    //The value of the Buffer entered by the user should be less then 50% of the Physical Memory available on the machine.
            //    if (userInputBufferSize > (int)((DICOMSniffer.totalPhysicalMemoryAvailable) / (1024 * 1024*2)))
            //    {
            //        this.Show();
            //        MessageBox.Show("Kernel Buffer size cannot exceed half the size of the Physical Memory present on the local machine", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        //If incorrect value is entered,reset Buffer Size to old value.
            //        kernelBufferSizeBox.Text = kernelBufferSize.ToString(); ;
            //    }
            //    else
            //    {
            //        //Set the kernel buffer size
            //        kernelBufferSize = userInputBufferSize;
            //    }
            //}
            //else
            //{
            //    this.Show();
            //    MessageBox.Show("Maximum permitted size of Buffer is 4096 MB(4GB).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    kernelBufferSizeBox.Text = "100";
            //}
            
            StreamWriter writer = new StreamWriter(userOptionsFilePath);
			writer.WriteLine(textBoxViewerPath.Text);
			writer.Close();

			this.DialogResult = DialogResult.OK;
			this.Close();
		}

        //private void kernelBuffercheckBox_CheckedChanged_1(object sender, EventArgs e)
        //{
        //    if (kernelBuffercheckBox.Checked)
        //    {
        //        kernelBufferSizeBox.Enabled = true;
                
        //    }
        //    else
        //    {
        //        kernelBufferSizeBox.Enabled = false;
                
        //    }
        //}

        private void retainInfoCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (retainInfoCheckBox.Checked)
            {
                retainOldAssociationsInfo = true;
            }
            else
            {
                retainOldAssociationsInfo = false;
            }
        }
	}
}
