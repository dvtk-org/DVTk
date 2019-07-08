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
	/// Summary description for FormInformationModel.
	/// </summary>
	public class FormInformationModel : System.Windows.Forms.Form
	{
		private System.Windows.Forms.RichTextBox richTextBoxInformationModel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public FormInformationModel(String textToDisplay)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			this.richTextBoxInformationModel.AppendText(textToDisplay);
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
            this.richTextBoxInformationModel = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // richTextBoxInformationModel
            // 
            this.richTextBoxInformationModel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxInformationModel.Location = new System.Drawing.Point(0, 0);
            this.richTextBoxInformationModel.Name = "richTextBoxInformationModel";
            this.richTextBoxInformationModel.ReadOnly = true;
            this.richTextBoxInformationModel.Size = new System.Drawing.Size(536, 406);
            this.richTextBoxInformationModel.TabIndex = 0;
            this.richTextBoxInformationModel.Text = "";
            // 
            // FormInformationModel
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.ClientSize = new System.Drawing.Size(536, 406);
            this.Controls.Add(this.richTextBoxInformationModel);
            this.Name = "FormInformationModel";
            this.Text = "FormInformationModel";
            this.ResumeLayout(false);

		}
		#endregion
	}
}
