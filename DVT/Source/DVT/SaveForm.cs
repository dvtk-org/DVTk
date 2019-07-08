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

namespace Dvt
{
	/// <summary>
	/// Summary description for SaveForm.
	/// </summary>
	public class SaveForm : System.Windows.Forms.Form
	{
        private System.Windows.Forms.Button ButtonSaveAll;
        private System.Windows.Forms.Button ButtonSaveSelected;
        private System.Windows.Forms.Button ButtonCancel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
        private System.Windows.Forms.Label LabelSaveChanges;
        private System.Windows.Forms.ListBox ListBoxChangedItems;

		public SaveForm()
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SaveForm));
            this.ButtonSaveAll = new System.Windows.Forms.Button();
            this.ButtonSaveSelected = new System.Windows.Forms.Button();
            this.ButtonCancel = new System.Windows.Forms.Button();
            this.LabelSaveChanges = new System.Windows.Forms.Label();
            this.ListBoxChangedItems = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // ButtonSaveAll
            // 
            this.ButtonSaveAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonSaveAll.Location = new System.Drawing.Point(11, 94);
            this.ButtonSaveAll.Name = "ButtonSaveAll";
            this.ButtonSaveAll.Size = new System.Drawing.Size(115, 27);
            this.ButtonSaveAll.TabIndex = 1;
            this.ButtonSaveAll.Text = "Save All";
            this.ButtonSaveAll.Click += new System.EventHandler(this.ButtonSaveAll_Click);
            // 
            // ButtonSaveSelected
            // 
            this.ButtonSaveSelected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonSaveSelected.Location = new System.Drawing.Point(198, 94);
            this.ButtonSaveSelected.Name = "ButtonSaveSelected";
            this.ButtonSaveSelected.Size = new System.Drawing.Size(116, 27);
            this.ButtonSaveSelected.TabIndex = 2;
            this.ButtonSaveSelected.Text = "Save Selected";
            this.ButtonSaveSelected.Click += new System.EventHandler(this.ButtonSaveSelected_Click);
            // 
            // ButtonCancel
            // 
            this.ButtonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonCancel.Location = new System.Drawing.Point(386, 94);
            this.ButtonCancel.Name = "ButtonCancel";
            this.ButtonCancel.Size = new System.Drawing.Size(115, 27);
            this.ButtonCancel.TabIndex = 3;
            this.ButtonCancel.Text = "Cancel";
            // 
            // LabelSaveChanges
            // 
            this.LabelSaveChanges.Location = new System.Drawing.Point(19, 9);
            this.LabelSaveChanges.Name = "LabelSaveChanges";
            this.LabelSaveChanges.Size = new System.Drawing.Size(250, 27);
            this.LabelSaveChanges.TabIndex = 2;
            this.LabelSaveChanges.Text = "Save changes to the following items?";
            // 
            // ListBoxChangedItems
            // 
            this.ListBoxChangedItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ListBoxChangedItems.ItemHeight = 16;
            this.ListBoxChangedItems.Location = new System.Drawing.Point(19, 37);
            this.ListBoxChangedItems.Name = "ListBoxChangedItems";
            this.ListBoxChangedItems.ScrollAlwaysVisible = true;
            this.ListBoxChangedItems.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.ListBoxChangedItems.Size = new System.Drawing.Size(482, 20);
            this.ListBoxChangedItems.TabIndex = 0;
            // 
            // SaveForm
            // 
            this.AcceptButton = this.ButtonSaveAll;
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.CancelButton = this.ButtonCancel;
            this.ClientSize = new System.Drawing.Size(520, 134);
            this.Controls.Add(this.ListBoxChangedItems);
            this.Controls.Add(this.LabelSaveChanges);
            this.Controls.Add(this.ButtonSaveAll);
            this.Controls.Add(this.ButtonSaveSelected);
            this.Controls.Add(this.ButtonCancel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(538, 157);
            this.Name = "SaveForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Save Changes?";
            this.ResumeLayout(false);

		}
		#endregion

        public void AddChangedItem (string item)
        {
            this.ListBoxChangedItems.Items.Add (item);
        }

        public bool ItemSelectedToSave (string item)
        {
            return true;
        }

        private ArrayList   items_to_save;

        private void ButtonSaveAll_Click(object sender, System.EventArgs e)
        {
            items_to_save = new ArrayList();
            foreach (string item in this.ListBoxChangedItems.Items)
                items_to_save.Add (item);
            this.DialogResult = DialogResult.OK;
            this.Close ();
        }

        private void ButtonSaveSelected_Click(object sender, System.EventArgs e)
        {
            items_to_save = new ArrayList();
            foreach (string item in this.ListBoxChangedItems.SelectedItems)
                items_to_save.Add (item);
            this.DialogResult = DialogResult.OK;
            this.Close ();
        }

        public ArrayList ItemsToSave
        {
            get { return this.items_to_save; }
        }
	}
}
