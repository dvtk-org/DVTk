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
using System.Windows.Forms;
using System.IO;
using System.Collections;
using DvtkApplicationLayer;

namespace Dvt
{
	/// <summary>
	/// 
	/// </summary>
    public class SopClassesManager
	{
		void Initialize()
		{
			DataGridBoolColumn theBoolColumn = null;
			DataGridTextBoxColumn theTextColumn = null;
			DataGridTableStyle theStyle = new DataGridTableStyle();
			theStyle.AllowSorting = true;
			theStyle.RowHeadersVisible = false;
			theStyle.MappingName = "ArrayList";

			// We set the column to readonly and handle the mouse events ourselves
			// in the MouseUp event handler. We want to circumvent the select cell
			// first before you can enable/disable a checkbox.
			theBoolColumn = new DataGridBoolColumn();
			theBoolColumn.MappingName = "Loaded";
			theBoolColumn.HeaderText = "Loaded";
			theBoolColumn.Width = 50;
			theBoolColumn.AllowNull = false;
			theBoolColumn.ReadOnly = false;
			theStyle.GridColumnStyles.Add (theBoolColumn);
      
			_theDefinitionFileTextColumn.MappingName = "Filename";
			_theDefinitionFileTextColumn.HeaderText = "Definition filename";
			_theDefinitionFileTextColumn.Width = 250;
			_theDefinitionFileTextColumn.ReadOnly = true;
			_theDefinitionFileTextColumn.TextBox.DoubleClick += new System.EventHandler(this.DefinitionFile_DoubleClick);
			theStyle.GridColumnStyles.Add(_theDefinitionFileTextColumn);

			theTextColumn = new DataGridTextBoxColumn();
			theTextColumn.MappingName = "SOPClassName";
			theTextColumn.HeaderText = "SOP class name";
			theTextColumn.Width = 125;
			theTextColumn.ReadOnly = true;
			theStyle.GridColumnStyles.Add(theTextColumn);

			theTextColumn = new DataGridTextBoxColumn();
			theTextColumn.MappingName = "SOPClassUID";
			theTextColumn.HeaderText = "SOP class UID";
			theTextColumn.Width = 125;
			theTextColumn.ReadOnly = true;
			theStyle.GridColumnStyles.Add(theTextColumn);

			theTextColumn = new DataGridTextBoxColumn();
			theTextColumn.MappingName = "AETitle";
			theTextColumn.HeaderText = "AE title";
			theTextColumn.Width = 100;
			theTextColumn.ReadOnly = true;
			theStyle.GridColumnStyles.Add(theTextColumn);

			theTextColumn = new DataGridTextBoxColumn();
			theTextColumn.MappingName = "AEVersion";
			theTextColumn.HeaderText = "AE version";
			theTextColumn.Width = 50;
			theTextColumn.ReadOnly = true;
			theStyle.GridColumnStyles.Add(theTextColumn);

			theTextColumn = new DataGridTextBoxColumn();
			theTextColumn.MappingName = "DefinitionRoot";
			theTextColumn.HeaderText = "Definition root";
			theTextColumn.Width = 200;
			theTextColumn.ReadOnly = true;
			theStyle.GridColumnStyles.Add(theTextColumn);

			_DataGridSopClasses.TableStyles.Add(theStyle);			
		}

		public string DefinitionName = null ;
		
		public void DefinitionFile_DoubleClick(object sender, System.EventArgs e)
		{
			OpenDefinitionFile();
		}

		public void OpenDefinitionFile()
		{
			// Open the definition file in notepad.
			System.Diagnostics.Process theProcess  = new System.Diagnostics.Process();

			// Get full definition file name.
			string theFileName = DefinitionName ;
			//DefinitionName = fileName ;
			string theFullFileName = "";
			foreach (DefinitionFile theDefinitionFile in  _DefinitionFilesInfoForDataGrid)
			{
				if (theDefinitionFile.Filename == theFileName)
				{
					theFullFileName = System.IO.Path.Combine(theDefinitionFile.DefinitionRoot, theDefinitionFile.Filename);
					break;
				}
			}

			theProcess.StartInfo.FileName= "Notepad.exe";
			theProcess.StartInfo.Arguments = theFullFileName;
			theProcess.Start();
		}

		/// <summary>
		/// Update the Specify SOP classes tab.
		/// </summary>
		public void Update()
		{
			if (_SessionUsedForContentsOfTabSpecifySopClasses == GetSessionTreeViewManager().GetSession())
			{
				// No need to update, this tab already contains the correct session information.
				_DataGridSopClasses.SetDataBinding(_DefinitionFilesInfoForDataGrid, "");
			}
			else
			{	
				Cursor.Current = Cursors.WaitCursor;

				DvtkApplicationLayer.Session theSession = GetSessionTreeViewManager().GetSession();
				Dvtk.Sessions.DefinitionFileDirectoryList theDefinitionFileDirectoryList = theSession.Implementation.DefinitionManagement.DefinitionFileDirectoryList;
               

				// Update the definition file directories list box.
				_ListBoxDefinitionFileDirectories.Items.Clear();

				UpdateRemoveButton();
				
				foreach(string theDefinitionFileDirectory in theDefinitionFileDirectoryList)
				{
					_ListBoxDefinitionFileDirectories.Items.Add(theDefinitionFileDirectory);
				}

				// Update the SOP classes data grid.
				// For every definition file directory, use the .def files present in the directory.
				_RichTextBoxInfo.Clear();
				_ComboBoxAeTitle.Items.Clear();
				_ComboBoxAeTitle.Items.Add(_DefaultAeTitleVersion);
				_DefinitionFilesInfoForDataGrid.Clear();
		
				// Add the correct information to the datagrid by inspecting the definition directory.
				// When doing this, also fill the combobox with available ae title - version combinations.
				AddSopClassesToDataGridFromDirectories();

				// Add the correct information to the datagrid by inspecting the loaded definitions.
				// When doing this, also fill the combobox with available ae title - version combinations.
				// Only add an entry if it does not already exist.
				AddSopClassesToDataGridFromLoadedDefinitions();

				// Make the correct AE title - version in the combo box the current one.
				SetSelectedAeTitleVersion();

				// Workaround for following problem:
				// If the SOP classes tab has already been filled, and another session contains less
				// records for the datagrid, windows gets confused and thinks there are more records
				// then actually present in _DefinitionFilesInfoForDataGrid resulting in an assertion.
				_DataGridSopClasses.SetDataBinding(null, "");

				// Actually refresh the data grid.
				_DataGridSopClasses.SetDataBinding(_DefinitionFilesInfoForDataGrid, "");
				//_DataGridSopClasses.DataSource = _DefinitionFilesInfoForDataGrid;

				_DataGridSopClasses.Refresh();

				_SessionUsedForContentsOfTabSpecifySopClasses = GetSessionTreeViewManager().GetSession();

				Cursor.Current = Cursors.Default;
			}
		}

		public void SetSelectedAeTitleVersion()
		{
			DvtkApplicationLayer.Session theSession = GetSessionTreeViewManager().GetSession();

			AeTitleVersion theCurrentAeTitleVersionInComboBox = null;

			foreach (AeTitleVersion theAeTitleVersion in _ComboBoxAeTitle.Items)
			{
				if ( (theAeTitleVersion._AeTitle == theSession.Implementation.DefinitionManagement.ApplicationEntityName) &&
					(theAeTitleVersion._Version == theSession.Implementation.DefinitionManagement.ApplicationEntityVersion) )
				{
					theCurrentAeTitleVersionInComboBox = theAeTitleVersion;
					break;
				}
			}
			
			if (theCurrentAeTitleVersionInComboBox != null)
			{
				_ComboBoxAeTitle.SelectedItem = theCurrentAeTitleVersionInComboBox;
			}
			else
			{
				_ComboBoxAeTitle.SelectedItem = _DefaultAeTitleVersion;
			}
		}

		private void AddSopClassesToDataGridFromDirectories()
		{
			DvtkApplicationLayer.Session theSession = GetSessionTreeViewManager().GetSession();
			Dvtk.Sessions.DefinitionFileDirectoryList theDefinitionFileDirectoryList = theSession.Implementation.DefinitionManagement.DefinitionFileDirectoryList;

			foreach(string theDefinitionFileDirectory in theDefinitionFileDirectoryList)
			{
				DirectoryInfo theDirectoryInfo = new DirectoryInfo(theDefinitionFileDirectory);

				if (theDirectoryInfo.Exists)
				{
					FileInfo[] theFilesInfo;

					theFilesInfo = theDirectoryInfo.GetFiles("*.def");

					foreach(FileInfo theDefinitionFileInfo in theFilesInfo)
					{
						try
						{
							AddSopClassToDataGridFromDefinitionFile(theDefinitionFileInfo.FullName);
						}
						catch(Exception exception)
						{
							string theErrorText;

							theErrorText = string.Format("Definition file {0} could not be interpreted while reading directory:\n{1}\n\n", theDefinitionFileInfo.FullName, exception.Message);

							_RichTextBoxInfo.AppendText(theErrorText);
						}						
					}
				}
			}
		}

		private void AddSopClassesToDataGridFromLoadedDefinitions()
		{
			DvtkApplicationLayer.Session theSession = GetSessionTreeViewManager().GetSession();

			foreach (string theLoadedDefinitionFileName in theSession.Implementation.DefinitionManagement.LoadedDefinitionFileNames)
			{
				string theLoadedDefinitionFullFileName = theLoadedDefinitionFileName;

				// If theLoadedDefinitionFileName does not contain a '\', this is a file name only.
				// Append the Root directory to get the full path name.
				if (theLoadedDefinitionFileName.LastIndexOf("\\") == -1)
				{
					theLoadedDefinitionFullFileName = System.IO.Path.Combine(theSession.Implementation.DefinitionManagement.DefinitionFileRootDirectory, theLoadedDefinitionFileName);
				}

				// Check if this definition file is already present in the data grid.
				// Do this by comparing the full file name of the definition file with the full file names
				// present in the data grid.
				bool IsDefinitionAlreadyPresent = false;

				foreach (DefinitionFile theDefinitionFile in  _DefinitionFilesInfoForDataGrid)
				{
					string defFileForDataGrid = System.IO.Path.Combine(theDefinitionFile.DefinitionRoot, theDefinitionFile.Filename);
					if ( defFileForDataGrid.ToLower() == theLoadedDefinitionFullFileName.ToLower())
					{
						IsDefinitionAlreadyPresent = true;
						break;
					}
				}

				if (!IsDefinitionAlreadyPresent)
				{
					try
					{
						AddSopClassToDataGridFromDefinitionFile(theLoadedDefinitionFullFileName);
					}
					catch(Exception exception)
					{
						string theErrorText;

						theErrorText = string.Format("Definition file {0} could not be interpreted while reading definition present in session:\n{1}\n\n", theLoadedDefinitionFullFileName, exception.Message);

						_RichTextBoxInfo.AppendText(theErrorText);
					}					
				}
			}
		}

		/// <summary>
		/// The information from a definition file is added to the datagrid and possibly to the combo box.
		/// 
		/// An excpetion is thrown when retrieving details for the definition file fails.
		/// </summary>
		/// <param name="theDefinitionFullFileName">the full file name of the definition file.</param>
		private void AddSopClassToDataGridFromDefinitionFile(string theDefinitionFullFileName)
		{
			DvtkApplicationLayer.Session theSession = GetSessionTreeViewManager().GetSession();
			Dvtk.Sessions.DefinitionFileDetails theDefinitionFileDetails;

			// Try to get detailed information about this definition file.
			theDefinitionFileDetails = theSession.Implementation.DefinitionManagement.GetDefinitionFileDetails(theDefinitionFullFileName);

			// No excpetion thrown when calling GetDefinitionFileDetails (otherwise this statement would not have been reached)
			// so this is a valid definition file. Add it to the data frid.
			DefinitionFile theDataGridDefinitionFileInfo = 
				new DefinitionFile(IsDefinitionFileLoaded(theDefinitionFullFileName),
				System.IO.Path.GetFileName(theDefinitionFullFileName),
				theDefinitionFileDetails.SOPClassName,
				theDefinitionFileDetails.SOPClassUID,
				theDefinitionFileDetails.ApplicationEntityName,
				theDefinitionFileDetails.ApplicationEntityVersion,
				System.IO.Path.GetDirectoryName(theDefinitionFullFileName));

            if(!theDefinitionFullFileName.Contains("AllDimseCommands.def"))
                _DefinitionFilesInfoForDataGrid.Add(theDataGridDefinitionFileInfo);

			// If the AE title - version does not yet exist in the combo box, add it.
			bool IsAeTitleVersionAlreadyInComboBox = false;

			foreach (AeTitleVersion theAeTitleVersion in _ComboBoxAeTitle.Items)
			{
				if ( (theAeTitleVersion._AeTitle == theDefinitionFileDetails.ApplicationEntityName) &&
					(theAeTitleVersion._Version == theDefinitionFileDetails.ApplicationEntityVersion) )
				{
					IsAeTitleVersionAlreadyInComboBox = true;
					break;
				}
			}

			if (!IsAeTitleVersionAlreadyInComboBox)
			{
				AeTitleVersion theAeTitleVersion = new AeTitleVersion(theDefinitionFileDetails.ApplicationEntityName, theDefinitionFileDetails.ApplicationEntityVersion);
				_ComboBoxAeTitle.Items.Add(theAeTitleVersion);
			}
		}

		private bool IsDefinitionFileLoaded(string theFullDefinitionFileName)
		{
			bool theReturnValue = false;

			DvtkApplicationLayer.Session theSession = GetSessionTreeViewManager().GetSession();

			foreach (string theLoadedDefinitionFileName in theSession.Implementation.DefinitionManagement.LoadedDefinitionFileNames)
			{
				string thetheLoadedDefinitionFullFileName = theLoadedDefinitionFileName;

				// If theLoadedDefinitionFileName does not contain a '\', this is a file name only.
				// Append the Root directory to get the full path name.
				if (theLoadedDefinitionFileName.LastIndexOf("\\") == -1)
				{
					thetheLoadedDefinitionFullFileName = System.IO.Path.Combine(theSession.Implementation.DefinitionManagement.DefinitionFileRootDirectory, theLoadedDefinitionFileName);
				}

				if (thetheLoadedDefinitionFullFileName.ToLower() == theFullDefinitionFileName.ToLower())
				{
					theReturnValue = true;
					break;
				}
			}

			return (theReturnValue);
		}
		
		/// <summary>
		/// Get the session tree view manager associated with the tab control.
		/// </summary>
		/// <returns>The session tree view manager</returns>
		private UserControlSessionTree GetSessionTreeViewManager() 
		{
			return(_UserControlSessionTree);
		}

		/// <summary>
		/// Add a definition file direcotory.
		/// </summary>
		public void AddDefinitionFileDirectory()
		{
			DvtkApplicationLayer.Session theSession = GetSessionTreeViewManager().GetSession();
			Dvtk.Sessions.DefinitionFileDirectoryList theDefinitionFileDirectoryList = theSession.Implementation.DefinitionManagement.DefinitionFileDirectoryList;

			FolderBrowserDialog theFolderBrowserDialog = new FolderBrowserDialog();

			theFolderBrowserDialog.Description = "Select the directory where definition files are located.";

			if (theFolderBrowserDialog.ShowDialog() == DialogResult.OK)
			{
				bool isExistingDirectory = false;
				string theNewDirectory = GetDirectoryWithTrailingSlashesRemoved(theFolderBrowserDialog.SelectedPath);

				// Find out if this new directory already exists.
				foreach (string theExistingDirectory in theDefinitionFileDirectoryList)
				{
					if (theNewDirectory == GetDirectoryWithTrailingSlashesRemoved(theExistingDirectory))
					{
						isExistingDirectory = true;
						break;
					}
				}

				theNewDirectory = theNewDirectory + "\\";

				// Only add this new directory if it does not already exist.
				if (!isExistingDirectory)
				{
					DirectoryInfo theDirectoryInfo = new DirectoryInfo(theNewDirectory);

					// If the new directory is not valid, show an error message.
					if (!theDirectoryInfo.Exists)
					{
						MessageBox.Show("The directory \"" + theNewDirectory + "\" is not a valid directory.",
							"Directory not added",
							MessageBoxButtons.OK,
							MessageBoxIcon.Information);
					}
					else
					{
						theDefinitionFileDirectoryList.Add(theNewDirectory);

						// Notify the rest of the world.
						// This will implicitly also update this specify SOP classes tab.
						
						SessionChange theSessionChange = new SessionChange(theSession, SessionChange.SessionChangeSubTypEnum.SOP_CLASSES_OTHER);
						Notify(theSessionChange);
					}
				}
				else
				{
					MessageBox.Show("The directory \"" + theNewDirectory + "\" is already present in\nthe list of definition file directories.",
						"Directory not added",
						MessageBoxButtons.OK,
						MessageBoxIcon.Information);
				}
			}
		}

		public void RemoveDefinitionFileDirectory()
		{
			string theSelectedDirectory;
			DvtkApplicationLayer.Session theSession = GetSessionTreeViewManager().GetSession();
			Dvtk.Sessions.DefinitionFileDirectoryList theDefinitionFileDirectoryList = theSession.Implementation.DefinitionManagement.DefinitionFileDirectoryList;
			theSelectedDirectory = (string)_ListBoxDefinitionFileDirectories.SelectedItem;
			theDefinitionFileDirectoryList.Remove(theSelectedDirectory);
			theSession.HasSessionChanged = true ;
			// Notify the rest of the world.
			// This will implicitly also update this specify SOP classes tab.
			// TO DO
			Update();
			SessionChange theSessionChange = new SessionChange(theSession, SessionChange.SessionChangeSubTypEnum.SOP_CLASSES_OTHER);
			Notify(theSessionChange);
		}

		private string GetDirectoryWithTrailingSlashesRemoved(string theDirectory)
		{
			string theReturnValue = theDirectory;
			while (theReturnValue.EndsWith("/") || theReturnValue.EndsWith("\\"))
			{
				theReturnValue = theReturnValue.Substring(0, theReturnValue.Length - 1);
			}
			return(theReturnValue);
		}

		private void Notify(object theEvent)
		{
			_ProjectForm2.Notify(theEvent);
		}

		public void SetSessionChanged(DvtkApplicationLayer.Session theSession)
		{
			if (theSession == _SessionUsedForContentsOfTabSpecifySopClasses)
			{
				_SessionUsedForContentsOfTabSpecifySopClasses = null;
			}
		}

		public void SelectedAeTitleVersionChanged()
		{
			DvtkApplicationLayer.Session theSession = GetSessionTreeViewManager().GetSession();

			AeTitleVersion theAeTitleVersion = (AeTitleVersion)_ComboBoxAeTitle.SelectedItem;

			if ( (theAeTitleVersion._AeTitle != theSession.Implementation.DefinitionManagement.ApplicationEntityName) ||
				(theAeTitleVersion._Version != theSession.Implementation.DefinitionManagement.ApplicationEntityVersion) )
			{
				theSession.Implementation.DefinitionManagement.ApplicationEntityName = theAeTitleVersion._AeTitle;
				theSession.Implementation.DefinitionManagement.ApplicationEntityVersion = theAeTitleVersion._Version;
				// TO DO
				SessionChange theSessionChange = new SessionChange(theSession, SessionChange.SessionChangeSubTypEnum.SOP_CLASSES_AE_TITLE_VERSION);
				Notify(theSessionChange);
			}
		}

		/// <summary>
		/// The reason we handle the mouseup event ourselves, is because with the normal
		/// grid, you first have to select a cell, and only then you can enable/disable a
		/// checkbox. We want to enable/disable the checkbox with 1 click.
		/// </summary>
		/// <param name="e"></param>
		public void DataGrid_MouseDown(System.Windows.Forms.MouseEventArgs e)
		{
			System.Windows.Forms.DataGrid.HitTestInfo theHitTestInfo;
			DvtkApplicationLayer.Session theSession = GetSessionTreeViewManager().GetSession();

			// Find out what part of the data grid is below the mouse pointer.
			theHitTestInfo = _DataGridSopClasses.HitTest(e.X, e.Y);

			switch (theHitTestInfo.Type)
			{
				case System.Windows.Forms.DataGrid.HitTestType.Cell:
					// If this is the "loaded" column...
					if (theHitTestInfo.Column == 0)
					{
						// Remember the cell we've changed. We don't want to change this cell when we move the mouse.
						// (see DataGrid_MouseMove).
						_MouseEventInfoForDataGrid_LoadedStateChangedForRow = theHitTestInfo.Row;

						// Get the column style for the "loaded" column.
						DataGridColumnStyle theDataGridColumnStyle;
						theDataGridColumnStyle = _DataGridSopClasses.TableStyles[0].GridColumnStyles[0];
						
						// Change the "loaded" stated in _DefinitionFilesInfoForDataGrid and the data grid itself.
						_DataGridSopClasses.BeginEdit(theDataGridColumnStyle, theHitTestInfo.Row);
						DefinitionFile theDefinitionFile = (DefinitionFile)_DefinitionFilesInfoForDataGrid[theHitTestInfo.Row];
						theDefinitionFile.Loaded = !theDefinitionFile.Loaded;
						_DataGridSopClasses.EndEdit (theDataGridColumnStyle, theHitTestInfo.Row, false);


						// Change the "loaded" state in the session object.
						string theFullFileName = System.IO.Path.Combine(theDefinitionFile.DefinitionRoot, theDefinitionFile.Filename);

						if (theDefinitionFile.Loaded)
						{
							// The definition file was not loaded yet. Load it now.
							theSession.Implementation.DefinitionManagement.LoadDefinitionFile(theFullFileName);
						}
						else
						{
							// The definition file was loaded. Unload it now.
							theSession.Implementation.DefinitionManagement.UnLoadDefinitionFile(theFullFileName);
						}

						// Remember the new "loaded" state for the case where the mouse is moved over other
						// "loaded" checkboxes while keeping the left mouse button pressed.
						_MouseEventInfoForDataGrid_LoadedStatedAfterMouseDown = theDefinitionFile.Loaded;

						// Notify the rest of the world.
						SessionChange theSessionChange = new SessionChange(theSession, SessionChange.SessionChangeSubTypEnum.SOP_CLASSES_LOADED_STATE);
						Notify(theSessionChange);
					}

					if (theHitTestInfo.Column == 1)
					{
						DefinitionFile theDefinitionFile = (DefinitionFile)_DefinitionFilesInfoForDataGrid[theHitTestInfo.Row];
						DefinitionName = theDefinitionFile.Filename;
					}
					break;
			}
		}

		/// <summary>
		/// When moving the mouse over other "loaded" check box while keeping the left mouse button pressed,
		/// change the "loaded" state to the new state when the left mouse button was pressed.
		/// </summary>
		/// <param name="e"></param>
		public void DataGrid_MouseMove(System.Windows.Forms.MouseEventArgs e)
		{
			DvtkApplicationLayer.Session theSession = GetSessionTreeViewManager().GetSession();

			if (e.Button == MouseButtons.Left)
			{
				System.Windows.Forms.DataGrid.HitTestInfo theHitTestInfo;

				theHitTestInfo = _DataGridSopClasses.HitTest(e.X, e.Y);

				switch (theHitTestInfo.Type)
				{
					case System.Windows.Forms.DataGrid.HitTestType.Cell:
						// If this is the "loaded" column...
						if (theHitTestInfo.Column == 0)
						{
							// If another "loaded" check box is pointed to, not the one where the state has
							// last been changed...
							if (theHitTestInfo.Row != _MouseEventInfoForDataGrid_LoadedStateChangedForRow)
							{
								DefinitionFile theDefinitionFile = (DefinitionFile)_DefinitionFilesInfoForDataGrid[theHitTestInfo.Row];
								bool theCurrentLoadedState = theDefinitionFile.Loaded;

								// Only if the state will change...
								if (theCurrentLoadedState != _MouseEventInfoForDataGrid_LoadedStatedAfterMouseDown)
								{
									// Get the column style for the "loaded" column.
									DataGridColumnStyle theDataGridColumnStyle;
									theDataGridColumnStyle = _DataGridSopClasses.TableStyles[0].GridColumnStyles[0];

									// Change the "loaded" stated in _DefinitionFilesInfoForDataGrid and the data grid itself.
									_DataGridSopClasses.BeginEdit(theDataGridColumnStyle, theHitTestInfo.Row);
									theDefinitionFile.Loaded = _MouseEventInfoForDataGrid_LoadedStatedAfterMouseDown;
									_DataGridSopClasses.EndEdit (theDataGridColumnStyle, theHitTestInfo.Row, false);

									// Change the "loaded" state in the session object.
									string theFullFileName = System.IO.Path.Combine(theDefinitionFile.DefinitionRoot, theDefinitionFile.Filename);

									if (_MouseEventInfoForDataGrid_LoadedStatedAfterMouseDown)
									{
										// The definition file was not loaded yet. Load it now.
										theSession.Implementation.DefinitionManagement.LoadDefinitionFile(theFullFileName);
									}
									else
									{
										// The definition file was loaded. Unload it now.
										theSession.Implementation.DefinitionManagement.UnLoadDefinitionFile(theFullFileName);
									}

									// Remember the cell we've changed. We don't want to change the loaded
									// state with each minor mouse move.
									_MouseEventInfoForDataGrid_LoadedStateChangedForRow = theHitTestInfo.Row;

									// Notify the rest of the world.
									// TO DO
									SessionChange theSessionChange = new SessionChange(theSession, SessionChange.SessionChangeSubTypEnum.SOP_CLASSES_LOADED_STATE);
									Notify(theSessionChange);
								}
								else
									// State will not change. The cell under the mouse will not be selected. This
									// results in scrolling that will not work when the mouse is moved to the bottom
									// of the datagrid.
									//
									// To solve this, explicitly make the cell under the mouse selected.
								{
									DataGridCell currentSelectedCell = _DataGridSopClasses.CurrentCell;
									DataGridCell newSelectedCell = new DataGridCell(theHitTestInfo.Row, currentSelectedCell.ColumnNumber);

									_DataGridSopClasses.CurrentCell = newSelectedCell;
								}
							}
						}
						break;
				}
			}
		}

		public void DataGrid_MouseUp(System.Windows.Forms.MouseEventArgs e)
		{
			_MouseEventInfoForDataGrid_LoadedStateChangedForRow = -1;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="theDataGridSopClasses">The DataGrid that is managed by this class.</param>
		/// <param name="theSessionTreeViewManager">The associated session tree view manager.</param>
		public SopClassesManager(ProjectForm2 theProjectForm2, System.Windows.Forms.DataGrid theDataGridSopClasses, System.Windows.Forms.ComboBox theComboBoxAeTitle, System.Windows.Forms.ListBox theListBoxDefinitionFileDirectories, System.Windows.Forms.RichTextBox theRichTextBoxInfo, UserControlSessionTree theUserControlSessionTree, System.Windows.Forms.Button theButtonRemoveDirectory)
		{
			_ProjectForm2 = theProjectForm2;
			_DataGridSopClasses = theDataGridSopClasses;
			_ComboBoxAeTitle = theComboBoxAeTitle;
			_ListBoxDefinitionFileDirectories = theListBoxDefinitionFileDirectories;
			_RichTextBoxInfo = theRichTextBoxInfo;
			_ButtonRemoveDirectory = theButtonRemoveDirectory;
			_UserControlSessionTree = theUserControlSessionTree;
			_theDefinitionFileTextColumn = new DataGridTextBoxColumn();
			_DefinitionFilesInfoForDataGrid = new ArrayList();
			_DefaultAeTitleVersion = new AeTitleVersion("DICOM", "3.0");

			Initialize();
		}

		private System.Windows.Forms.DataGrid _DataGridSopClasses;
		private System.Windows.Forms.ComboBox _ComboBoxAeTitle;
		private System.Windows.Forms.ListBox _ListBoxDefinitionFileDirectories;
		private System.Windows.Forms.RichTextBox _RichTextBoxInfo;
		private DataGridTextBoxColumn _theDefinitionFileTextColumn;
		private UserControlSessionTree _UserControlSessionTree;
		private ProjectForm2 _ProjectForm2;
		private System.Windows.Forms.Button _ButtonRemoveDirectory;
		private AeTitleVersion _DefaultAeTitleVersion;
		private ArrayList _DefinitionFilesInfoForDataGrid;
		private int _MouseEventInfoForDataGrid_LoadedStateChangedForRow = -1;
		private bool _MouseEventInfoForDataGrid_LoadedStatedAfterMouseDown = true;
		private DvtkApplicationLayer.Session _SessionUsedForContentsOfTabSpecifySopClasses = null;

		public void UpdateRemoveButton()
		{
			bool removeButtonEnabled = false;

			if (_ListBoxDefinitionFileDirectories.SelectedItem == null)
			{
				removeButtonEnabled = false;
			}
			else
			{
				if (_ListBoxDefinitionFileDirectories.Items.Count > 1)
				{
					removeButtonEnabled = true;
				}
				else
				{
					removeButtonEnabled = false;
				}
			}
			_ButtonRemoveDirectory.Enabled = removeButtonEnabled;
		}

		public void RemoveDynamicDataBinding()
		{
			_DataGridSopClasses.SetDataBinding(null, "");
		}

		public void SelectAllDefinitionFiles()
		{ 
			int tempCount = 0;
			DvtkApplicationLayer.Session theSession = GetSessionTreeViewManager().GetSession();
			DataGridBoolColumn dataBoolColumn = new DataGridBoolColumn();
			foreach (DefinitionFile theDefinitionFile in  _DefinitionFilesInfoForDataGrid)
			{
				// Get the column style for the "loaded" column.
				DataGridColumnStyle theColStyle = _DataGridSopClasses.TableStyles[0].GridColumnStyles[0];

				// Change the "loaded" stated in _DefinitionFilesInfoForDataGrid and the data grid itself.
				_DataGridSopClasses.BeginEdit(theColStyle, tempCount);
				theDefinitionFile.Loaded = true;
				_DataGridSopClasses.EndEdit (theColStyle, tempCount, false);

				//_DataGridSopClasses[tempCount,0] = dataBoolColumn.TrueValue;
				string theFullFileName = System.IO.Path.Combine(theDefinitionFile.DefinitionRoot, theDefinitionFile.Filename);
				theSession.Implementation.DefinitionManagement.LoadDefinitionFile(theFullFileName);
				tempCount++ ;
			}

			_DataGridSopClasses.SetDataBinding(_DefinitionFilesInfoForDataGrid , "");
			// Notify the rest of the world.
			SessionChange theSessionChange = new SessionChange(theSession, SessionChange.SessionChangeSubTypEnum.SOP_CLASSES_LOADED_STATE);
			Notify(theSessionChange);			
		}

		public void UnSelectAllDefinitionFiles()
		{
			int tempCount = 0;
			DvtkApplicationLayer.Session theSession = GetSessionTreeViewManager().GetSession();
			DataGridBoolColumn dataBoolColumn = new DataGridBoolColumn();
			foreach (DefinitionFile theDefinitionFile in  _DefinitionFilesInfoForDataGrid)
			{
				string theFullFileName = System.IO.Path.Combine(theDefinitionFile.DefinitionRoot, theDefinitionFile.Filename);
				theSession.Implementation.DefinitionManagement.UnLoadDefinitionFile(theFullFileName);
				//_DataGridSopClasses[tempCount,0] = dataBoolColumn.FalseValue;
				// Get the column style for the "loaded" column.
				DataGridColumnStyle theColStyle = _DataGridSopClasses.TableStyles[0].GridColumnStyles[0];

				// Change the "loaded" stated in _DefinitionFilesInfoForDataGrid and the data grid itself.
				_DataGridSopClasses.BeginEdit(theColStyle, tempCount);
				theDefinitionFile.Loaded = false;
				_DataGridSopClasses.EndEdit (theColStyle, tempCount, false);

				tempCount++ ;
			}

			_DataGridSopClasses.SetDataBinding(_DefinitionFilesInfoForDataGrid , "");
			// Notify the rest of the world.
			SessionChange theSessionChange = new SessionChange(theSession, SessionChange.SessionChangeSubTypEnum.SOP_CLASSES_LOADED_STATE);
			Notify(theSessionChange);						
		}		
	}

	public class AeTitleVersion: object
	{
		public override string ToString()
		{
			return (_AeTitle + " - " + _Version);
		}
		public AeTitleVersion(string theAeTitle, string theVersion)
		{
			_AeTitle = theAeTitle;
			_Version = theVersion;
		}
		public string _AeTitle;
		public string _Version;
	}	
}
