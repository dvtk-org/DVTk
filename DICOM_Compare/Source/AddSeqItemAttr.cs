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
	/// Summary description for AddSeqItemAttr.
	/// </summary>
	public class AddSeqItemAttr : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox textBoxSeqAttrGroup;
		private System.Windows.Forms.TextBox textBoxSeqAttrElement;
		private System.Windows.Forms.TextBox textBoxAttrGroup;
		private System.Windows.Forms.TextBox textBoxAttrElement;
		private System.Windows.Forms.Button buttonOk;
		private System.Windows.Forms.TextBox textBoxSeqItemNr;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public AddSeqItemAttr()
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(AddSeqItemAttr));
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.textBoxSeqAttrGroup = new System.Windows.Forms.TextBox();
			this.textBoxSeqAttrElement = new System.Windows.Forms.TextBox();
			this.textBoxAttrGroup = new System.Windows.Forms.TextBox();
			this.textBoxAttrElement = new System.Windows.Forms.TextBox();
			this.buttonOk = new System.Windows.Forms.Button();
			this.textBoxSeqItemNr = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(144, 23);
			this.label1.TabIndex = 0;
			this.label1.Text = "Parent Seq Attribute Group:";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 56);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(160, 23);
			this.label2.TabIndex = 1;
			this.label2.Text = "Parent Seq Attribute Element:";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(40, 160);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(88, 23);
			this.label3.TabIndex = 2;
			this.label3.Text = "Attribute Group:";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(32, 200);
			this.label4.Name = "label4";
			this.label4.TabIndex = 3;
			this.label4.Text = "Attribute Element:";
			// 
			// textBoxSeqAttrGroup
			// 
			this.textBoxSeqAttrGroup.Location = new System.Drawing.Point(160, 16);
			this.textBoxSeqAttrGroup.Name = "textBoxSeqAttrGroup";
			this.textBoxSeqAttrGroup.Size = new System.Drawing.Size(104, 20);
			this.textBoxSeqAttrGroup.TabIndex = 4;
			this.textBoxSeqAttrGroup.Text = "";
			// 
			// textBoxSeqAttrElement
			// 
			this.textBoxSeqAttrElement.Location = new System.Drawing.Point(160, 56);
			this.textBoxSeqAttrElement.Name = "textBoxSeqAttrElement";
			this.textBoxSeqAttrElement.Size = new System.Drawing.Size(104, 20);
			this.textBoxSeqAttrElement.TabIndex = 5;
			this.textBoxSeqAttrElement.Text = "";
			// 
			// textBoxAttrGroup
			// 
			this.textBoxAttrGroup.Location = new System.Drawing.Point(160, 160);
			this.textBoxAttrGroup.Name = "textBoxAttrGroup";
			this.textBoxAttrGroup.Size = new System.Drawing.Size(104, 20);
			this.textBoxAttrGroup.TabIndex = 7;
			this.textBoxAttrGroup.Text = "";
			// 
			// textBoxAttrElement
			// 
			this.textBoxAttrElement.Location = new System.Drawing.Point(160, 200);
			this.textBoxAttrElement.Name = "textBoxAttrElement";
			this.textBoxAttrElement.Size = new System.Drawing.Size(104, 20);
			this.textBoxAttrElement.TabIndex = 8;
			this.textBoxAttrElement.Text = "";
			// 
			// buttonOk
			// 
			this.buttonOk.Location = new System.Drawing.Point(104, 264);
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.TabIndex = 9;
			this.buttonOk.Text = "OK";
			this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
			// 
			// textBoxSeqItemNr
			// 
			this.textBoxSeqItemNr.Location = new System.Drawing.Point(160, 96);
			this.textBoxSeqItemNr.Name = "textBoxSeqItemNr";
			this.textBoxSeqItemNr.Size = new System.Drawing.Size(56, 20);
			this.textBoxSeqItemNr.TabIndex = 6;
			this.textBoxSeqItemNr.Text = "";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(48, 96);
			this.label5.Name = "label5";
			this.label5.TabIndex = 10;
			this.label5.Text = "Seq Item Number:";
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(0, 128);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(168, 16);
			this.label6.TabIndex = 11;
			this.label6.Text = "Note: Seq Item Nr is one based.";
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(0, 232);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(264, 16);
			this.label7.TabIndex = 12;
			this.label7.Text = "Note: Enter group and element values as e.g. 0008";
			// 
			// AddSeqItemAttr
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(290, 296);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.textBoxSeqItemNr);
			this.Controls.Add(this.buttonOk);
			this.Controls.Add(this.textBoxAttrElement);
			this.Controls.Add(this.textBoxAttrGroup);
			this.Controls.Add(this.textBoxSeqAttrElement);
			this.Controls.Add(this.textBoxSeqAttrGroup);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AddSeqItemAttr";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Add Seq Item attribute to the filter";
			this.ResumeLayout(false);

		}
		#endregion

		private void buttonOk_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		public UInt16 SeqAttributeGroup
		{
			get
			{
				return UInt16.Parse(textBoxSeqAttrGroup.Text,System.Globalization.NumberStyles.HexNumber);
			}
		}

		public UInt16 SeqAttributeElement
		{
			get
			{
				return UInt16.Parse(textBoxSeqAttrElement.Text,System.Globalization.NumberStyles.HexNumber);
			}
		}

		public string SeqItemNr
		{
			get
			{
				return textBoxSeqItemNr.Text;
			}
		}

		public UInt16 AttributeGroup
		{
			get
			{
				return UInt16.Parse(textBoxAttrGroup.Text,System.Globalization.NumberStyles.HexNumber);
			}
		}

		public UInt16 AttributeElement
		{
			get
			{
				return UInt16.Parse(textBoxAttrElement.Text,System.Globalization.NumberStyles.HexNumber);
			}
		}
	}
}
