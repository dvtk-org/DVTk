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

namespace SnifferUI
{
	/// <summary>
	/// Summary description for PDUOverview.
	/// </summary>
	public class PDUOverview : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label labelPDUType;
		private System.Windows.Forms.Label PDULength;
		private System.Windows.Forms.TextBox textBoxPDUType;
		private System.Windows.Forms.TextBox textBoxPDULength;
		private System.Windows.Forms.RichTextBox richTextBoxPDU;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// Construtor
		/// </summary>
		/// <param name="pduDetail"></param>
		/// <param name="pduType"></param>
		/// <param name="pduLength"></param>
		public PDUOverview(string pduDetail, string pduType, string pduLength)
		{
			InitializeComponent();
			richTextBoxPDU.Text = pduDetail;
			textBoxPDUType.Text = pduType;
			textBoxPDULength.Text = pduLength;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PDUOverview));
            this.labelPDUType = new System.Windows.Forms.Label();
            this.PDULength = new System.Windows.Forms.Label();
            this.textBoxPDUType = new System.Windows.Forms.TextBox();
            this.textBoxPDULength = new System.Windows.Forms.TextBox();
            this.richTextBoxPDU = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelPDUType
            // 
            this.labelPDUType.Location = new System.Drawing.Point(8, 16);
            this.labelPDUType.Name = "labelPDUType";
            this.labelPDUType.Size = new System.Drawing.Size(64, 23);
            this.labelPDUType.TabIndex = 0;
            this.labelPDUType.Text = "PDU Type:";
            // 
            // PDULength
            // 
            this.PDULength.Location = new System.Drawing.Point(264, 16);
            this.PDULength.Name = "PDULength";
            this.PDULength.Size = new System.Drawing.Size(72, 23);
            this.PDULength.TabIndex = 1;
            this.PDULength.Text = "PDU Length:";
            // 
            // textBoxPDUType
            // 
            this.textBoxPDUType.Location = new System.Drawing.Point(72, 16);
            this.textBoxPDUType.Name = "textBoxPDUType";
            this.textBoxPDUType.ReadOnly = true;
            this.textBoxPDUType.Size = new System.Drawing.Size(136, 20);
            this.textBoxPDUType.TabIndex = 2;
            // 
            // textBoxPDULength
            // 
            this.textBoxPDULength.Location = new System.Drawing.Point(340, 16);
            this.textBoxPDULength.Name = "textBoxPDULength";
            this.textBoxPDULength.ReadOnly = true;
            this.textBoxPDULength.Size = new System.Drawing.Size(80, 20);
            this.textBoxPDULength.TabIndex = 3;
            // 
            // richTextBoxPDU
            // 
            this.richTextBoxPDU.BackColor = System.Drawing.SystemColors.Control;
            this.richTextBoxPDU.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBoxPDU.Location = new System.Drawing.Point(8, 80);
            this.richTextBoxPDU.Name = "richTextBoxPDU";
            this.richTextBoxPDU.ReadOnly = true;
            this.richTextBoxPDU.Size = new System.Drawing.Size(522, 312);
            this.richTextBoxPDU.TabIndex = 4;
            this.richTextBoxPDU.Text = "";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(16, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 23);
            this.label1.TabIndex = 6;
            this.label1.Text = "Bytes:";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(96, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 23);
            this.label2.TabIndex = 7;
            this.label2.Text = "Field Name:";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(248, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 23);
            this.label3.TabIndex = 8;
            this.label3.Text = "Field Value:";
            // 
            // PDUOverview
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(542, 398);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.richTextBoxPDU);
            this.Controls.Add(this.textBoxPDULength);
            this.Controls.Add(this.textBoxPDUType);
            this.Controls.Add(this.PDULength);
            this.Controls.Add(this.labelPDUType);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PDUOverview";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PDU Overview";
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

	}
}
