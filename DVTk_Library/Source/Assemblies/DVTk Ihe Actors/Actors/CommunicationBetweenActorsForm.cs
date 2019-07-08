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
using System.IO;
using System.Windows.Forms;
using System.Threading;

namespace Dvtk.IheActors.Actors
{
	/// <summary>
	/// Summary description for CommunicationBetweenActorsForm.
	/// </summary>
	public class CommunicationBetweenActorsForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label labelActorName1;
		private System.Windows.Forms.Label labelActorName2;
		private System.Windows.Forms.PictureBox pictureBoxActorPicture1;
		private System.Windows.Forms.PictureBox pictureBox2;
		private System.Windows.Forms.PictureBox pictureBoxActorPicture2;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.Label labelTransactionName;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public CommunicationBetweenActorsForm(String transactionName, String actorDescription1, String pictureFullFileName1, String actorDescription2, String pictureFullFileName2)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			this.labelActorName1.Text = actorDescription1;
			this.labelActorName2.Text = actorDescription2;
			this.labelTransactionName.Text = transactionName;

			if (File.Exists(pictureFullFileName1))
			{
				this.pictureBoxActorPicture1.Image = Image.FromFile(pictureFullFileName1);
			}

			if (File.Exists(pictureFullFileName2))
			{
				this.pictureBoxActorPicture2.Image = Image.FromFile(pictureFullFileName2);
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CommunicationBetweenActorsForm));
			this.labelActorName1 = new System.Windows.Forms.Label();
			this.labelActorName2 = new System.Windows.Forms.Label();
			this.pictureBoxActorPicture1 = new System.Windows.Forms.PictureBox();
			this.pictureBoxActorPicture2 = new System.Windows.Forms.PictureBox();
			this.pictureBox2 = new System.Windows.Forms.PictureBox();
			this.buttonOK = new System.Windows.Forms.Button();
			this.labelTransactionName = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// labelActorName1
			// 
			this.labelActorName1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.labelActorName1.Location = new System.Drawing.Point(16, 16);
			this.labelActorName1.Name = "labelActorName1";
			this.labelActorName1.Size = new System.Drawing.Size(344, 23);
			this.labelActorName1.TabIndex = 0;
			this.labelActorName1.Text = "-";
			this.labelActorName1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// labelActorName2
			// 
			this.labelActorName2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.labelActorName2.Location = new System.Drawing.Point(528, 16);
			this.labelActorName2.Name = "labelActorName2";
			this.labelActorName2.Size = new System.Drawing.Size(336, 23);
			this.labelActorName2.TabIndex = 1;
			this.labelActorName2.Text = "-";
			this.labelActorName2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// pictureBoxActorPicture1
			// 
			this.pictureBoxActorPicture1.BackColor = System.Drawing.SystemColors.Window;
			this.pictureBoxActorPicture1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pictureBoxActorPicture1.Location = new System.Drawing.Point(16, 56);
			this.pictureBoxActorPicture1.Name = "pictureBoxActorPicture1";
			this.pictureBoxActorPicture1.Size = new System.Drawing.Size(334, 206);
			this.pictureBoxActorPicture1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.pictureBoxActorPicture1.TabIndex = 4;
			this.pictureBoxActorPicture1.TabStop = false;
			// 
			// pictureBoxActorPicture2
			// 
			this.pictureBoxActorPicture2.BackColor = System.Drawing.SystemColors.Window;
			this.pictureBoxActorPicture2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pictureBoxActorPicture2.Location = new System.Drawing.Point(528, 56);
			this.pictureBoxActorPicture2.Name = "pictureBoxActorPicture2";
			this.pictureBoxActorPicture2.Size = new System.Drawing.Size(334, 206);
			this.pictureBoxActorPicture2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.pictureBoxActorPicture2.TabIndex = 5;
			this.pictureBoxActorPicture2.TabStop = false;
			// 
			// pictureBox2
			// 
			this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
			this.pictureBox2.Location = new System.Drawing.Point(360, 136);
			this.pictureBox2.Name = "pictureBox2";
			this.pictureBox2.Size = new System.Drawing.Size(160, 50);
			this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.pictureBox2.TabIndex = 6;
			this.pictureBox2.TabStop = false;
			// 
			// buttonOK
			// 
			this.buttonOK.Location = new System.Drawing.Point(784, 288);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.TabIndex = 7;
			this.buttonOK.Text = "OK";
			this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
			// 
			// labelTransactionName
			// 
			this.labelTransactionName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.labelTransactionName.Location = new System.Drawing.Point(256, 280);
			this.labelTransactionName.Name = "labelTransactionName";
			this.labelTransactionName.Size = new System.Drawing.Size(368, 23);
			this.labelTransactionName.TabIndex = 8;
			this.labelTransactionName.Text = "-";
			this.labelTransactionName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// CommunicationBetweenActorsForm
			// 
            this.AutoScaleMode = AutoScaleMode.None;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(872, 326);
			this.Controls.Add(this.labelTransactionName);
			this.Controls.Add(this.buttonOK);
			this.Controls.Add(this.pictureBox2);
			this.Controls.Add(this.pictureBoxActorPicture2);
			this.Controls.Add(this.pictureBoxActorPicture1);
			this.Controls.Add(this.labelActorName2);
			this.Controls.Add(this.labelActorName1);
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(880, 360);
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(880, 360);
			this.Name = "CommunicationBetweenActorsForm";
			this.Text = "Communication between Actors";
			this.ResumeLayout(false);

		}
		#endregion

		private void buttonOK_Click(object sender, System.EventArgs e)
		{
			Close();
		}
	}
}
