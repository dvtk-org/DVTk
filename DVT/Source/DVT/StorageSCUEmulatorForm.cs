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
using System.IO;
using Dvtk;
using System.Threading;

namespace Dvt
{
	/// <summary>
	/// Summary description for StorageSCUEmulatorForm.
	/// </summary>
	public class StorageSCUEmulatorForm : System.Windows.Forms.Form
	{
        private System.Windows.Forms.ListBox ListBoxFiles;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ButtonAdd;
        private System.Windows.Forms.Button ButtonRemove;
        private System.Windows.Forms.GroupBox GroupBoxNrAssociations;
        private System.Windows.Forms.GroupBox GroupBoxOptions;
        private System.Windows.Forms.Button ButtonCancel;
        private System.Windows.Forms.Button ButtonSend;
        private System.Windows.Forms.Button ButtonEcho;
        private System.Windows.Forms.RadioButton RadioButtonSingleAssoc;
        private System.Windows.Forms.RadioButton RadioButtonMultAssoc;
        private System.Windows.Forms.CheckBox CheckBoxValidateOnImport;
        private System.Windows.Forms.CheckBox CheckBoxRepeat;
		private System.Windows.Forms.CheckBox CheckBoxNewStudy;
        private System.Windows.Forms.NumericUpDown NummericRepeat;
        private System.Windows.Forms.OpenFileDialog OpenMediaFileDialog;
		private System.Windows.Forms.FolderBrowserDialog mediaDirectoryBrowserDialog;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		// Manually added members.
		System.AsyncCallback _StorageScuAsyncCallback = null;
        private DvtkApplicationLayer.EmulatorSession _EmulatorSession ;
		private System.Windows.Forms.Button buttonMediaDirectory;
		bool _SendButtonClicked = false;

		public StorageSCUEmulatorForm(System.AsyncCallback theStorageScuAsyncCallback)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			_StorageScuAsyncCallback = theStorageScuAsyncCallback;
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
			this.ListBoxFiles = new System.Windows.Forms.ListBox();
			this.label1 = new System.Windows.Forms.Label();
			this.ButtonAdd = new System.Windows.Forms.Button();
			this.ButtonRemove = new System.Windows.Forms.Button();
			this.GroupBoxNrAssociations = new System.Windows.Forms.GroupBox();
			this.RadioButtonSingleAssoc = new System.Windows.Forms.RadioButton();
			this.RadioButtonMultAssoc = new System.Windows.Forms.RadioButton();
			this.GroupBoxOptions = new System.Windows.Forms.GroupBox();
			this.CheckBoxNewStudy = new System.Windows.Forms.CheckBox();
			this.NummericRepeat = new System.Windows.Forms.NumericUpDown();
			this.CheckBoxValidateOnImport = new System.Windows.Forms.CheckBox();
			this.CheckBoxRepeat = new System.Windows.Forms.CheckBox();
			this.ButtonCancel = new System.Windows.Forms.Button();
			this.ButtonSend = new System.Windows.Forms.Button();
			this.ButtonEcho = new System.Windows.Forms.Button();
			this.OpenMediaFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.mediaDirectoryBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
			this.buttonMediaDirectory = new System.Windows.Forms.Button();
			this.GroupBoxNrAssociations.SuspendLayout();
			this.GroupBoxOptions.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.NummericRepeat)).BeginInit();
			this.SuspendLayout();
			// 
			// ListBoxFiles
			// 
			this.ListBoxFiles.HorizontalScrollbar = true;
			this.ListBoxFiles.Location = new System.Drawing.Point(16, 40);
			this.ListBoxFiles.Name = "ListBoxFiles";
			this.ListBoxFiles.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.ListBoxFiles.Size = new System.Drawing.Size(336, 95);
			this.ListBoxFiles.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(336, 23);
			this.label1.TabIndex = 0;
			this.label1.Text = "Files to send:";
			// 
			// ButtonAdd
			// 
			this.ButtonAdd.Location = new System.Drawing.Point(368, 40);
			this.ButtonAdd.Name = "ButtonAdd";
			this.ButtonAdd.TabIndex = 0;
			this.ButtonAdd.Text = "Add";
			this.ButtonAdd.Click += new System.EventHandler(this.ButtonAdd_Click);
			// 
			// ButtonRemove
			// 
			this.ButtonRemove.Enabled = false;
			this.ButtonRemove.Location = new System.Drawing.Point(368, 72);
			this.ButtonRemove.Name = "ButtonRemove";
			this.ButtonRemove.TabIndex = 1;
			this.ButtonRemove.Text = "Remove";
			this.ButtonRemove.Click += new System.EventHandler(this.ButtonRemove_Click);
			// 
			// GroupBoxNrAssociations
			// 
			this.GroupBoxNrAssociations.Controls.Add(this.RadioButtonSingleAssoc);
			this.GroupBoxNrAssociations.Controls.Add(this.RadioButtonMultAssoc);
			this.GroupBoxNrAssociations.Location = new System.Drawing.Point(16, 152);
			this.GroupBoxNrAssociations.Name = "GroupBoxNrAssociations";
			this.GroupBoxNrAssociations.Size = new System.Drawing.Size(200, 72);
			this.GroupBoxNrAssociations.TabIndex = 3;
			this.GroupBoxNrAssociations.TabStop = false;
			this.GroupBoxNrAssociations.Text = "Number of associations";
			// 
			// RadioButtonSingleAssoc
			// 
			this.RadioButtonSingleAssoc.Checked = true;
			this.RadioButtonSingleAssoc.Enabled = false;
			this.RadioButtonSingleAssoc.Location = new System.Drawing.Point(16, 16);
			this.RadioButtonSingleAssoc.Name = "RadioButtonSingleAssoc";
			this.RadioButtonSingleAssoc.Size = new System.Drawing.Size(176, 24);
			this.RadioButtonSingleAssoc.TabIndex = 0;
			this.RadioButtonSingleAssoc.TabStop = true;
			this.RadioButtonSingleAssoc.Text = "Single association";
			// 
			// RadioButtonMultAssoc
			// 
			this.RadioButtonMultAssoc.Enabled = false;
			this.RadioButtonMultAssoc.Location = new System.Drawing.Point(16, 40);
			this.RadioButtonMultAssoc.Name = "RadioButtonMultAssoc";
			this.RadioButtonMultAssoc.Size = new System.Drawing.Size(176, 24);
			this.RadioButtonMultAssoc.TabIndex = 1;
			this.RadioButtonMultAssoc.Text = "Multiple associations";
			// 
			// GroupBoxOptions
			// 
			this.GroupBoxOptions.Controls.Add(this.CheckBoxNewStudy);
			this.GroupBoxOptions.Controls.Add(this.NummericRepeat);
			this.GroupBoxOptions.Controls.Add(this.CheckBoxValidateOnImport);
			this.GroupBoxOptions.Controls.Add(this.CheckBoxRepeat);
			this.GroupBoxOptions.Location = new System.Drawing.Point(240, 152);
			this.GroupBoxOptions.Name = "GroupBoxOptions";
			this.GroupBoxOptions.Size = new System.Drawing.Size(200, 104);
			this.GroupBoxOptions.TabIndex = 4;
			this.GroupBoxOptions.TabStop = false;
			this.GroupBoxOptions.Text = "Options";
			// 
			// CheckBoxNewStudy
			// 
			this.CheckBoxNewStudy.Location = new System.Drawing.Point(16, 72);
			this.CheckBoxNewStudy.Name = "CheckBoxNewStudy";
			this.CheckBoxNewStudy.Size = new System.Drawing.Size(168, 16);
			this.CheckBoxNewStudy.TabIndex = 3;
			this.CheckBoxNewStudy.Text = "Send data under new study";
			// 
			// NummericRepeat
			// 
			this.NummericRepeat.Enabled = false;
			this.NummericRepeat.Location = new System.Drawing.Point(136, 40);
			this.NummericRepeat.Minimum = new System.Decimal(new int[] {
																		   1,
																		   0,
																		   0,
																		   0});
			this.NummericRepeat.Name = "NummericRepeat";
			this.NummericRepeat.Size = new System.Drawing.Size(48, 20);
			this.NummericRepeat.TabIndex = 2;
			this.NummericRepeat.Value = new System.Decimal(new int[] {
																		 1,
																		 0,
																		 0,
																		 0});
			// 
			// CheckBoxValidateOnImport
			// 
			this.CheckBoxValidateOnImport.Location = new System.Drawing.Point(16, 16);
			this.CheckBoxValidateOnImport.Name = "CheckBoxValidateOnImport";
			this.CheckBoxValidateOnImport.Size = new System.Drawing.Size(176, 24);
			this.CheckBoxValidateOnImport.TabIndex = 0;
			this.CheckBoxValidateOnImport.Text = "Validate before export";
			// 
			// CheckBoxRepeat
			// 
			this.CheckBoxRepeat.Location = new System.Drawing.Point(16, 40);
			this.CheckBoxRepeat.Name = "CheckBoxRepeat";
			this.CheckBoxRepeat.TabIndex = 1;
			this.CheckBoxRepeat.Text = "Repeat x:";
			this.CheckBoxRepeat.CheckedChanged += new System.EventHandler(this.CheckBoxRepeat_CheckedChanged);
			// 
			// ButtonCancel
			// 
			this.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.ButtonCancel.Location = new System.Drawing.Point(368, 264);
			this.ButtonCancel.Name = "ButtonCancel";
			this.ButtonCancel.TabIndex = 8;
			this.ButtonCancel.Text = "Cancel";
			// 
			// ButtonSend
			// 
			this.ButtonSend.Enabled = false;
			this.ButtonSend.Location = new System.Drawing.Point(280, 264);
			this.ButtonSend.Name = "ButtonSend";
			this.ButtonSend.TabIndex = 7;
			this.ButtonSend.Text = "Send";
			this.ButtonSend.Click += new System.EventHandler(this.ButtonSend_Click);
			// 
			// ButtonEcho
			// 
			this.ButtonEcho.Location = new System.Drawing.Point(192, 264);
			this.ButtonEcho.Name = "ButtonEcho";
			this.ButtonEcho.TabIndex = 6;
			this.ButtonEcho.Text = "Echo";
			this.ButtonEcho.Click += new System.EventHandler(this.ButtonEcho_Click);
			// 
			// OpenMediaFileDialog
			// 
			this.OpenMediaFileDialog.Filter = "DICOM media files (*.dcm)|*.dcm|All files (*.*)|*.*";
			this.OpenMediaFileDialog.Multiselect = true;
			this.OpenMediaFileDialog.Title = "Select media files to send";
			// 
			// buttonMediaDirectory
			// 
			this.buttonMediaDirectory.Location = new System.Drawing.Point(32, 264);
			this.buttonMediaDirectory.Name = "buttonMediaDirectory";
			this.buttonMediaDirectory.Size = new System.Drawing.Size(120, 23);
			this.buttonMediaDirectory.TabIndex = 5;
			this.buttonMediaDirectory.Text = "Send media directory";
			this.buttonMediaDirectory.Click += new System.EventHandler(this.buttonMediaDirectory_Click);
			// 
			// StorageSCUEmulatorForm
			// 
			this.AcceptButton = this.ButtonSend;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.ButtonCancel;
			this.ClientSize = new System.Drawing.Size(456, 304);
			this.ControlBox = false;
			this.Controls.Add(this.buttonMediaDirectory);
			this.Controls.Add(this.GroupBoxNrAssociations);
			this.Controls.Add(this.ButtonAdd);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.ListBoxFiles);
			this.Controls.Add(this.ButtonRemove);
			this.Controls.Add(this.GroupBoxOptions);
			this.Controls.Add(this.ButtonCancel);
			this.Controls.Add(this.ButtonSend);
			this.Controls.Add(this.ButtonEcho);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "StorageSCUEmulatorForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Storage SCU Emulator";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.StorageSCUEmulatorForm_Closing);
			this.GroupBoxNrAssociations.ResumeLayout(false);
			this.GroupBoxOptions.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.NummericRepeat)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

        private void ButtonAdd_Click(object sender, System.EventArgs e)
        {
			try 
			{
				if (this.OpenMediaFileDialog.ShowDialog (this) == DialogResult.OK) 
				{				
					this.ListBoxFiles.Items.AddRange (this.OpenMediaFileDialog.FileNames);
					if (this.ListBoxFiles.Items.Count > 1) 
					{
						this.RadioButtonMultAssoc.Enabled = true;
						this.RadioButtonSingleAssoc.Enabled = true;
					}
					else if (this.ListBoxFiles.Items.Count == 1)
					{
						this.RadioButtonMultAssoc.Enabled = false;
						this.RadioButtonSingleAssoc.Enabled = false;
						this.RadioButtonSingleAssoc.Checked = true;
					}
					this.ButtonRemove.Enabled = true;
					this.ButtonSend.Enabled = true;
				}
			}
			catch (InvalidOperationException ioe) 
			{
				//
				// Catch InvalidOperationException thrown by OpenFileDialog.ShowDialog method in
				// case too many files (more than approx. 800) are selected in the OpenFileDialog.
				//
				MessageBox.Show(ioe.Message ,"Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
			catch (Exception theException) 
			{
				MessageBox.Show(theException.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
        }

        private void ButtonRemove_Click(object sender, System.EventArgs e)
        {
            if (this.ListBoxFiles.SelectedItems.Count > 0)
            {
                if (MessageBox.Show (this,
                    "Are you sure you want to delete the selected media files?",
                    "Remove selected media files?",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    // Create a copy of the selected items. This is needed because the
                    // selected items list is dynamically updated.
                    ArrayList list = new ArrayList (this.ListBoxFiles.SelectedItems);
                    foreach (object item in list)
                        this.ListBoxFiles.Items.Remove (item);

                    if (this.ListBoxFiles.Items.Count > 1)
                    {
                        this.RadioButtonMultAssoc.Enabled = true;
                        this.RadioButtonSingleAssoc.Enabled = true;
                    }
                    else if (this.ListBoxFiles.Items.Count == 1)
                    {
                        this.RadioButtonMultAssoc.Enabled = false;
                        this.RadioButtonSingleAssoc.Enabled = false;
                        this.RadioButtonSingleAssoc.Checked = true;
                    }
                    else
                    {
                        this.RadioButtonMultAssoc.Enabled = false;
                        this.RadioButtonSingleAssoc.Enabled = false;
                        this.ButtonRemove.Enabled = false;
                        this.ButtonSend.Enabled = false;
                    }
                }
            }
        }

        private void OnEmulateVerificationScuDone(IAsyncResult ar)
        {
			try
			{
				_EmulatorSession.EmulatorSessionImplementation.EndEmulateVerificationSCU(ar);
			}
			catch (Exception ex)
			{
				//
				// Problem:
				// Errors thrown from a workerthread are eaten by the .NET 1.x CLR.
				// Workaround:
				// Directly call the global (untrapped) exception handler callback.
				// Do NOT rely on 
				// either
				// - System.AppDomain.CurrentDomain.UnhandledException
				// or
				// - System.Windows.Forms.Application.ThreadException
				// These events will only be triggered for the main thread not for worker threads.
				//
//				CustomExceptionHandler eh = new CustomExceptionHandler();
//				System.Threading.ThreadExceptionEventArgs args = new ThreadExceptionEventArgs(ex);
//				eh.OnThreadException(this, args);
				//
				// Rethrow. This rethrow may work in the future .NET 2.x CLR.
				// Currently eaten.
				//
				throw ex;
			}
        }

        private void ButtonEcho_Click(object sender, System.EventArgs e)
        {
            System.AsyncCallback cb = new AsyncCallback(this.OnEmulateVerificationScuDone);
            System.IAsyncResult ar = 
                _EmulatorSession.EmulatorSessionImplementation.BeginEmulateVerificationSCU(cb);
        }

        private void ButtonSend_Click(object sender, System.EventArgs e)
        {
			string[] files = new string[this.ListBoxFiles.Items.Count];

            for (int i=0 ; i<this.ListBoxFiles.Items.Count ; i++)
                files[i] = this.ListBoxFiles.Items[i].ToString();

            System.IAsyncResult ar = 
                _EmulatorSession.EmulatorSessionImplementation.BeginEmulateStorageSCU(
                files,
                this.RadioButtonMultAssoc.Checked,
                this.CheckBoxValidateOnImport.Checked,
				this.CheckBoxNewStudy.Checked,
                Convert.ToUInt16 (this.NummericRepeat.Value),
                _StorageScuAsyncCallback);

			// Close this form.
			// The handling of the callback method will be done in the SessionTreeViewManager.
			_SendButtonClicked = true;
			Close();
        }

        private void CheckBoxRepeat_CheckedChanged(object sender, System.EventArgs e)
        {
			if(CheckBoxRepeat.Checked == false)
			{
				NummericRepeat.Value=NummericRepeat.Minimum;
			}
			
			this.NummericRepeat.Enabled = this.CheckBoxRepeat.Checked;
        }

		public DialogResult ShowDialog(IWin32Window theIWin32Window, DvtkApplicationLayer.EmulatorSession theEmulatorSession)
		{
			_EmulatorSession = theEmulatorSession;
			return(ShowDialog(theIWin32Window));
		}

		private void StorageSCUEmulatorForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (_SendButtonClicked)
			{
				DialogResult = DialogResult.OK;
				_SendButtonClicked = false;
			}
		}

		private void buttonMediaDirectory_Click(object sender, System.EventArgs e)
		{
			mediaDirectoryBrowserDialog.Description = "Select the directory contains media files:";
			if (mediaDirectoryBrowserDialog.ShowDialog (this) == DialogResult.OK) 
			{
				DirectoryInfo theDirectoryInfo = new DirectoryInfo(this.mediaDirectoryBrowserDialog.SelectedPath);
				FileInfo[] dcmFiles = null;
				if (theDirectoryInfo != null) 
				{
					// Get all the subdirectories
					FileSystemInfo[] infos = theDirectoryInfo.GetFileSystemInfos();
					ArrayList allDCMFiles = new ArrayList();
					foreach (FileSystemInfo f in infos)
					{
						if (f is DirectoryInfo) 
						{
							// Get all the files in a specific directory
							FileInfo[] dcmFilesInSubDir = ((DirectoryInfo)f).GetFiles();
							if(dcmFilesInSubDir.Length != 0)
							{
								foreach (FileInfo fileNext in dcmFilesInSubDir) 
								{
									if ((fileNext.Extension.ToLower() == ".dcm")||(fileNext.Extension == null) || (fileNext.Extension == ""))
										allDCMFiles.Add(fileNext);
								}
							}                             
						}
						else if (f is FileInfo) 
						{
							if ((f.Extension.ToLower() == ".dcm")||(f.Extension == null) || (f.Extension == ""))
								allDCMFiles.Add(f);
						}
					}

					dcmFiles = (FileInfo[])allDCMFiles.ToArray(typeof(FileInfo));
					allDCMFiles.Clear();
				}

				if(dcmFiles.Length != 0)
				{
					string[] filesToSend = new string[dcmFiles.Length];

					for (int i=0 ; i<dcmFiles.Length ; i++)
						filesToSend[i] = dcmFiles[i].FullName;

					System.IAsyncResult ar = 
						_EmulatorSession.EmulatorSessionImplementation.BeginEmulateStorageSCU(
						filesToSend,
						this.RadioButtonMultAssoc.Checked,
						this.CheckBoxValidateOnImport.Checked,
						this.CheckBoxNewStudy.Checked,
						Convert.ToUInt16 (this.NummericRepeat.Value),
						_StorageScuAsyncCallback);
				
					// Close this form.
					_SendButtonClicked = true;
					Close();
				}
				else
				{
					string msg =
						string.Format(
						"The selected directory {0} doesn't contain any media file.\n Please select other directory.", 
						this.mediaDirectoryBrowserDialog.SelectedPath);
					MessageBox.Show(msg, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
			}			
		}
	}
}
