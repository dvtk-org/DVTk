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
	/// Summary description for FindForm.
	/// </summary>
	public class FindForm : System.Windows.Forms.Form
	{
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TextBoxFindString;
        private System.Windows.Forms.CheckBox CheckBoxMatchCase;
        private System.Windows.Forms.Button ButtonFindNext;
        private System.Windows.Forms.Button ButtonClose;
        private System.Windows.Forms.CheckBox CheckBoxMatchWholeWords;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

        public FindForm(MainForm theMainForm)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

            _MainForm = theMainForm;
		}

        private MainForm _MainForm;

        public string search_string
        {
            get { return this.TextBoxFindString.Text; }
        }

        public bool search_match_case
        {
            get { return this.CheckBoxMatchCase.Checked; }
        }

        public bool search_match_whole_word
        {
            get { return this.CheckBoxMatchWholeWords.Checked; }
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
			this.label1 = new System.Windows.Forms.Label();
			this.TextBoxFindString = new System.Windows.Forms.TextBox();
			this.CheckBoxMatchCase = new System.Windows.Forms.CheckBox();
			this.CheckBoxMatchWholeWords = new System.Windows.Forms.CheckBox();
			this.ButtonFindNext = new System.Windows.Forms.Button();
			this.ButtonClose = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(64, 23);
			this.label1.TabIndex = 0;
			this.label1.Text = "Find what:";
			// 
			// TextBoxFindString
			// 
			this.TextBoxFindString.Location = new System.Drawing.Point(80, 8);
			this.TextBoxFindString.Name = "TextBoxFindString";
			this.TextBoxFindString.Size = new System.Drawing.Size(176, 20);
			this.TextBoxFindString.TabIndex = 1;
			this.TextBoxFindString.Text = "";
			this.TextBoxFindString.TextChanged += new System.EventHandler(this.TextBoxFindString_TextChanged);
			// 
			// CheckBoxMatchCase
			// 
			this.CheckBoxMatchCase.Location = new System.Drawing.Point(16, 32);
			this.CheckBoxMatchCase.Name = "CheckBoxMatchCase";
			this.CheckBoxMatchCase.TabIndex = 2;
			this.CheckBoxMatchCase.Text = "Match case";
			// 
			// CheckBoxMatchWholeWords
			// 
			this.CheckBoxMatchWholeWords.Location = new System.Drawing.Point(16, 56);
			this.CheckBoxMatchWholeWords.Name = "CheckBoxMatchWholeWords";
			this.CheckBoxMatchWholeWords.Size = new System.Drawing.Size(120, 24);
			this.CheckBoxMatchWholeWords.TabIndex = 3;
			this.CheckBoxMatchWholeWords.Text = "Match whole words";
			// 
			// ButtonFindNext
			// 
			this.ButtonFindNext.Enabled = false;
			this.ButtonFindNext.Location = new System.Drawing.Point(272, 8);
			this.ButtonFindNext.Name = "ButtonFindNext";
			this.ButtonFindNext.TabIndex = 4;
			this.ButtonFindNext.Text = "Find Next";
			this.ButtonFindNext.Click += new System.EventHandler(this.ButtonFindNext_Click);
			// 
			// ButtonClose
			// 
			this.ButtonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.ButtonClose.Location = new System.Drawing.Point(272, 40);
			this.ButtonClose.Name = "ButtonClose";
			this.ButtonClose.TabIndex = 5;
			this.ButtonClose.Text = "Close";
			this.ButtonClose.Click += new System.EventHandler(this.ButtonClose_Click);
			// 
			// FindForm
			// 
			this.AcceptButton = this.ButtonFindNext;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.ButtonClose;
			this.ClientSize = new System.Drawing.Size(360, 86);
			this.Controls.Add(this.ButtonClose);
			this.Controls.Add(this.ButtonFindNext);
			this.Controls.Add(this.CheckBoxMatchWholeWords);
			this.Controls.Add(this.CheckBoxMatchCase);
			this.Controls.Add(this.TextBoxFindString);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FindForm";
			this.ShowInTaskbar = false;
			this.Text = "Find";
			this.Activated += new System.EventHandler(this.FindForm_Activated);
			this.ResumeLayout(false);

		}
		#endregion

		private void ButtonFindNext_Click(object sender, System.EventArgs e)
		{
			_MainForm.GetActiveProjectForm().TCM_GetValidationResultsManager().FindNextText(TextBoxFindString.Text, CheckBoxMatchWholeWords.Checked, CheckBoxMatchCase.Checked);
		}

        private void ButtonClose_Click(object sender, System.EventArgs e)
        {
            this.Close ();
        }

        private void TextBoxFindString_TextChanged(object sender, System.EventArgs e)
        {
            if (this.TextBoxFindString.Text.Length > 0)
                this.ButtonFindNext.Enabled = true;
            else
                this.ButtonFindNext.Enabled = false;
        }

		private void FindForm_Activated(object sender, System.EventArgs e)
		{
			TextBoxFindString.Focus();
			TextBoxFindString.SelectAll();
		}
	}
}
