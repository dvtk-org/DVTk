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

namespace RIS_Emulator
{
	/// <summary>
	/// Summary description for SelectMode.
	/// </summary>
	public class SelectMode : System.Windows.Forms.Form
	{
		private System.Windows.Forms.RadioButton radioButtonMWL;
		private System.Windows.Forms.RadioButton radioButton2;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TextBox textBoxNrOfRsp;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.Button buttonOk;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.PictureBox pictureBoxRIS;
		private System.Windows.Forms.ToolTip toolTipRIS;
		bool isMWLRsp = true;

		public SelectMode()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			textBoxNrOfRsp.Visible = false;
			label1.Visible = false;
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectMode));
            this.radioButtonMWL = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pictureBoxRIS = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxNrOfRsp = new System.Windows.Forms.TextBox();
            this.buttonOk = new System.Windows.Forms.Button();
            this.toolTipRIS = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRIS)).BeginInit();
            this.SuspendLayout();
            // 
            // radioButtonMWL
            // 
            this.radioButtonMWL.Checked = true;
            this.radioButtonMWL.Location = new System.Drawing.Point(29, 37);
            this.radioButtonMWL.Name = "radioButtonMWL";
            this.radioButtonMWL.Size = new System.Drawing.Size(288, 28);
            this.radioButtonMWL.TabIndex = 0;
            this.radioButtonMWL.TabStop = true;
            this.radioButtonMWL.Text = "Using Modality Worklist Information Model";
            this.radioButtonMWL.CheckedChanged += new System.EventHandler(this.radioButtonMWL_CheckedChanged);
            // 
            // radioButton2
            // 
            this.radioButton2.Location = new System.Drawing.Point(29, 83);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(240, 28);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.Text = "Using Randomized DICOM objects";
            this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.pictureBoxRIS);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textBoxNrOfRsp);
            this.groupBox1.Controls.Add(this.radioButtonMWL);
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Location = new System.Drawing.Point(19, 18);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(336, 167);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Send C-FIND Responses";
            // 
            // pictureBoxRIS
            // 
            this.pictureBoxRIS.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxRIS.Image")));
            this.pictureBoxRIS.Location = new System.Drawing.Point(278, 83);
            this.pictureBoxRIS.Name = "pictureBoxRIS";
            this.pictureBoxRIS.Size = new System.Drawing.Size(20, 19);
            this.pictureBoxRIS.TabIndex = 37;
            this.pictureBoxRIS.TabStop = false;
            this.toolTipRIS.SetToolTip(this.pictureBoxRIS, "Pl select DCM object from \"Edit DICOM Files\" tab and insert \'@\' charcter in desir" +
        "ed attributes values.");
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(48, 129);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(154, 19);
            this.label1.TabIndex = 3;
            this.label1.Text = "Number Of Responses:";
            // 
            // textBoxNrOfRsp
            // 
            this.textBoxNrOfRsp.Location = new System.Drawing.Point(202, 129);
            this.textBoxNrOfRsp.Name = "textBoxNrOfRsp";
            this.textBoxNrOfRsp.Size = new System.Drawing.Size(57, 22);
            this.textBoxNrOfRsp.TabIndex = 2;
            this.textBoxNrOfRsp.Text = "1";
            // 
            // buttonOk
            // 
            this.buttonOk.Location = new System.Drawing.Point(134, 194);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(90, 26);
            this.buttonOk.TabIndex = 3;
            this.buttonOk.Text = "OK";
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // toolTipRIS
            // 
            this.toolTipRIS.AutomaticDelay = 0;
            this.toolTipRIS.AutoPopDelay = 5000;
            this.toolTipRIS.InitialDelay = 1;
            this.toolTipRIS.ReshowDelay = 100;
            this.toolTipRIS.ShowAlways = true;
            // 
            // SelectMode
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.ClientSize = new System.Drawing.Size(385, 238);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelectMode";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select Mode";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRIS)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		private void radioButtonMWL_CheckedChanged(object sender, System.EventArgs e)
		{
			isMWLRsp = true;
		}

		private void radioButton2_CheckedChanged(object sender, System.EventArgs e)
		{
			textBoxNrOfRsp.Visible = true;
			label1.Visible = true;
			isMWLRsp = false;
		}

		private void buttonOk_Click(object sender, System.EventArgs e)
		{
			this.Close();
			this.DialogResult = DialogResult.OK;
		}		

		public bool C_FIND_RSP_MODE
		{
			get
			{
				return isMWLRsp;
			}
		}

		public int NrOfResponses
		{
			get
			{
				int nrOfRsps = 0;
				if(textBoxNrOfRsp.Text != "")
				{
					nrOfRsps = int.Parse(textBoxNrOfRsp.Text);
				}
				else
				{
					MessageBox.Show("Please specify the Nr of responses value.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				}
				return nrOfRsps;
			}
			set
			{
				textBoxNrOfRsp.Text = value.ToString();
			}
		}
	}
}
