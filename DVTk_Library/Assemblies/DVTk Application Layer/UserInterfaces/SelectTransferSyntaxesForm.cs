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
    /// Summary description for SelectTransferSyntaxesForm.
    /// </summary>
    public class SelectTransferSyntaxesForm : System.Windows.Forms.Form
    {
        private System.Windows.Forms.Button ButtonOk;
        private System.Windows.Forms.Button ButtonCancel;
        private System.Windows.Forms.TreeView TreeViewSTS;
        private System.Windows.Forms.Label LabelSelectTS;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        /// <summary>
        /// Used in UI applications for TS selection change
        /// </summary>
        public bool _SelectionChanged = false;
        private System.Windows.Forms.Button buttonSelectAll;
        private System.Windows.Forms.Button buttonDeselectAll;

        /// <summary>
        /// Constructor
        /// </summary>
        public SelectTransferSyntaxesForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
        }

        /// <summary>
        /// Select TS supported as we display dialog
        /// </summary>
        public void selectSupportedTS()
        {
            foreach (DvtkData.Dul.TransferSyntax ts in ts_list)
            {
                TreeNode node = this.TreeViewSTS.Nodes.Add(ts.ToString());
                node.Text = ts.ToString() + "(" + ts.UID + ")";

                // Set the item to 'checked' if the transfer syntax is supported 
                // by the emulator
                if (tsList.Contains(ts))
                {
                    node.Checked = true;
                }
            }
        }

        /// <summary>
        /// Default Transfer Syntaxes list displayed
        /// </summary>
        public ArrayList DefaultTransferSyntaxesList
        {
            set { ts_list = value; }
        }
        private ArrayList ts_list = new ArrayList();

        /// <summary>
        /// Supported Transfer Syntaxes
        /// </summary>
        public DvtkData.Dul.TransferSyntaxList SupportedTransferSyntaxes
        {
            get { return tsList; }
        }
        private DvtkData.Dul.TransferSyntaxList tsList =
            new DvtkData.Dul.TransferSyntaxList();

        /// <summary>
        /// Provide button to Select All Transfer Syntaxes
        /// </summary>
        public bool DisplaySelectAllButton
        {
            set { buttonSelectAll.Visible = value; }
        }

        /// <summary>
        /// Provide button to De-Select All Transfer Syntaxes
        /// </summary>
        public bool DisplayDeSelectAllButton
        {
            set { buttonDeselectAll.Visible = value; }
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
            this.ButtonOk = new System.Windows.Forms.Button();
            this.ButtonCancel = new System.Windows.Forms.Button();
            this.TreeViewSTS = new System.Windows.Forms.TreeView();
            this.LabelSelectTS = new System.Windows.Forms.Label();
            this.buttonSelectAll = new System.Windows.Forms.Button();
            this.buttonDeselectAll = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ButtonOk
            // 
            this.ButtonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonOk.Location = new System.Drawing.Point(240, 184);
            this.ButtonOk.Name = "ButtonOk";
            this.ButtonOk.TabIndex = 1;
            this.ButtonOk.Text = "OK";
            this.ButtonOk.Click += new System.EventHandler(this.ButtonOk_Click);
            // 
            // ButtonCancel
            // 
            this.ButtonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonCancel.Location = new System.Drawing.Point(344, 182);
            this.ButtonCancel.Name = "ButtonCancel";
            this.ButtonCancel.TabIndex = 2;
            this.ButtonCancel.Text = "Cancel";
            // 
            // TreeViewSTS
            // 
            this.TreeViewSTS.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this.TreeViewSTS.CheckBoxes = true;
            this.TreeViewSTS.ImageIndex = -1;
            this.TreeViewSTS.Location = new System.Drawing.Point(8, 48);
            this.TreeViewSTS.Name = "TreeViewSTS";
            this.TreeViewSTS.SelectedImageIndex = -1;
            this.TreeViewSTS.ShowLines = false;
            this.TreeViewSTS.ShowPlusMinus = false;
            this.TreeViewSTS.ShowRootLines = false;
            this.TreeViewSTS.Size = new System.Drawing.Size(424, 118);
            this.TreeViewSTS.TabIndex = 0;
            // 
            // LabelSelectTS
            // 
            this.LabelSelectTS.Location = new System.Drawing.Point(8, 24);
            this.LabelSelectTS.Name = "LabelSelectTS";
            this.LabelSelectTS.Size = new System.Drawing.Size(368, 16);
            this.LabelSelectTS.TabIndex = 3;
            this.LabelSelectTS.Text = "Select the Transfer Syntaxes you want to be supported by the emulator:";
            // 
            // buttonSelectAll
            // 
            this.buttonSelectAll.Location = new System.Drawing.Point(24, 184);
            this.buttonSelectAll.Name = "buttonSelectAll";
            this.buttonSelectAll.TabIndex = 4;
            this.buttonSelectAll.Text = "Select All";
            this.buttonSelectAll.Click += new System.EventHandler(this.buttonSelectAll_Click);
            // 
            // buttonDeselectAll
            // 
            this.buttonDeselectAll.Location = new System.Drawing.Point(128, 184);
            this.buttonDeselectAll.Name = "buttonDeselectAll";
            this.buttonDeselectAll.Size = new System.Drawing.Size(80, 23);
            this.buttonDeselectAll.TabIndex = 5;
            this.buttonDeselectAll.Text = "De-select All";
            this.buttonDeselectAll.Click += new System.EventHandler(this.buttonDeselectAll_Click);
            // 
            // SelectTransferSyntaxesForm
            // 
            this.AcceptButton = this.ButtonOk;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.ButtonCancel;
            this.ClientSize = new System.Drawing.Size(440, 222);
            this.Controls.Add(this.buttonDeselectAll);
            this.Controls.Add(this.buttonSelectAll);
            this.Controls.Add(this.LabelSelectTS);
            this.Controls.Add(this.TreeViewSTS);
            this.Controls.Add(this.ButtonOk);
            this.Controls.Add(this.ButtonCancel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(392, 168);
            this.Name = "SelectTransferSyntaxesForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Specify Supported Transfer Syntaxes";
            this.ResumeLayout(false);

        }
        #endregion

        private void ButtonOk_Click(object sender, System.EventArgs e)
        {
            tsList.Clear();
            foreach (DvtkData.Dul.TransferSyntax ts in ts_list)
            {
                foreach (TreeNode node in this.TreeViewSTS.Nodes)
                {
                    string tsString = node.Text.Substring(0, node.Text.IndexOf("("));
                    if ((ts.ToString() == tsString) && (node.Checked))
                        tsList.Add(ts);
                }
            }

            _SelectionChanged = true;
            this.Close();
        }

        private void buttonSelectAll_Click(object sender, System.EventArgs e)
        {
            tsList.Clear();
            foreach (TreeNode node in this.TreeViewSTS.Nodes)
            {
                node.Checked = true;
                string tsString = node.Text.Substring(0, node.Text.IndexOf("("));
                foreach (DvtkData.Dul.TransferSyntax ts in ts_list)
                {
                    if (ts.ToString() == tsString)
                        tsList.Add(ts);
                }
            }
        }

        private void buttonDeselectAll_Click(object sender, System.EventArgs e)
        {
            tsList.Clear();
            foreach (TreeNode node in this.TreeViewSTS.Nodes)
            {
                node.Checked = false;
            }
        }
    }
}
