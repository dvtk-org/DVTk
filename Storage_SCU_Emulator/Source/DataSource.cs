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

namespace StorageSCUEmulator
{
	/// <summary>
	/// Summary description for DataSource.
	/// </summary>
	public class DataSource : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton radioButtonDir;
		private System.Windows.Forms.RadioButton radioButtonFiles;
		private System.Windows.Forms.RadioButton radioButtonDICOMDIR;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public DataSource()
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataSource));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButtonDICOMDIR = new System.Windows.Forms.RadioButton();
            this.radioButtonFiles = new System.Windows.Forms.RadioButton();
            this.radioButtonDir = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButtonDICOMDIR);
            this.groupBox1.Controls.Add(this.radioButtonFiles);
            this.groupBox1.Controls.Add(this.radioButtonDir);
            this.groupBox1.Location = new System.Drawing.Point(19, 18);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(403, 176);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Data Source";
            // 
            // radioButtonDICOMDIR
            // 
            this.radioButtonDICOMDIR.Location = new System.Drawing.Point(38, 138);
            this.radioButtonDICOMDIR.Name = "radioButtonDICOMDIR";
            this.radioButtonDICOMDIR.Size = new System.Drawing.Size(125, 28);
            this.radioButtonDICOMDIR.TabIndex = 2;
            this.radioButtonDICOMDIR.Text = "DICOMDIR";
            this.radioButtonDICOMDIR.CheckedChanged += new System.EventHandler(this.radioButtonDICOMDIR_CheckedChanged);
            // 
            // radioButtonFiles
            // 
            this.radioButtonFiles.Location = new System.Drawing.Point(38, 83);
            this.radioButtonFiles.Name = "radioButtonFiles";
            this.radioButtonFiles.Size = new System.Drawing.Size(125, 28);
            this.radioButtonFiles.TabIndex = 1;
            this.radioButtonFiles.Text = "DICOM Files";
            this.radioButtonFiles.CheckedChanged += new System.EventHandler(this.radioButtonFiles_CheckedChanged);
            // 
            // radioButtonDir
            // 
            this.radioButtonDir.Location = new System.Drawing.Point(38, 28);
            this.radioButtonDir.Name = "radioButtonDir";
            this.radioButtonDir.Size = new System.Drawing.Size(356, 27);
            this.radioButtonDir.TabIndex = 0;
            this.radioButtonDir.Text = "Data Directory (Recursively search all sub directories)";
            this.radioButtonDir.CheckedChanged += new System.EventHandler(this.radioButtonDir_CheckedChanged);
            // 
            // DataSource
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.ClientSize = new System.Drawing.Size(370, 184);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DataSource";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select Data Source to Export";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		public int GetDataSouceMode()
		{
			int data = 1;
			if(radioButtonDir.Checked)
				data = 0;

			if(radioButtonFiles.Checked)
				data = 1;

			if(radioButtonDICOMDIR.Checked)
				data = 2;

			return data;
		}

		private void radioButtonDir_CheckedChanged(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void radioButtonFiles_CheckedChanged(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void radioButtonDICOMDIR_CheckedChanged(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			this.Close();
		}
	}
}
