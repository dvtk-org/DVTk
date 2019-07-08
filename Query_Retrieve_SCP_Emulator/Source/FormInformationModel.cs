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
using System.ComponentModel;
using System.Windows.Forms;

namespace QR_Emulator
{
	/// <summary>
	/// Summary description for FormInformationModel.
	/// </summary>
	public class FormInformationModel : System.Windows.Forms.Form
    {
        internal DvtkHighLevelInterface.InformationModel.InformationModelControl informationModelControl1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public FormInformationModel()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormInformationModel));
            this.informationModelControl1 = new DvtkHighLevelInterface.InformationModel.InformationModelControl();
            this.SuspendLayout();
            // 
            // informationModelControl1
            // 
            this.informationModelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.informationModelControl1.Location = new System.Drawing.Point(0, 0);
            this.informationModelControl1.Name = "informationModelControl1";
            this.informationModelControl1.Size = new System.Drawing.Size(992, 566);
            this.informationModelControl1.TabIndex = 1;
            // 
            // FormInformationModel
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.ClientSize = new System.Drawing.Size(992, 566);
            this.Controls.Add(this.informationModelControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(720, 577);
            this.Name = "FormInformationModel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormInformationModel";
            this.Resize += new System.EventHandler(this.FormInformationModel_Resize);
            this.ResumeLayout(false);

		}
		#endregion

        private void FormInformationModel_Resize(object sender, EventArgs e)
        {
            informationModelControl1.Resize(this.Height, this.Width);   
        }
	}
}
