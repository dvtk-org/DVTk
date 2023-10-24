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
using System.Data;
using System.IO;
using System.Diagnostics;
using Dvtk;
using System.Threading;
using DvtkApplicationLayer;

using System.Windows;


namespace Dvt
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class MainForm : System.Windows.Forms.Form
	{
        public System.Windows.Forms.StatusBar MainStatusBar;
        private System.Windows.Forms.MainMenu MainMenu;
        private System.Windows.Forms.MenuItem MenuWindow;
        private System.Windows.Forms.ToolBar MainToolBar;
        private System.Windows.Forms.ToolBarButton ToolBarNew;
        private System.Windows.Forms.MenuItem MenuHelp;
        private System.Windows.Forms.ImageList ImageListFunctions;
        private System.Windows.Forms.OpenFileDialog DialogOpenProject;
        private System.Windows.Forms.MenuItem WindowNewProjectView;
        private System.Windows.Forms.MenuItem WindowTileViews;
        private System.Windows.Forms.MenuItem WindowCascadeViews;
        private System.Windows.Forms.MenuItem FileSeparator1;
        private System.Windows.Forms.MenuItem FileSeparator2;
        private System.Windows.Forms.MenuItem FileSeparator3;
        private System.Windows.Forms.MenuItem MenuView;
        private System.Windows.Forms.MenuItem MenuEmulator;
        private System.Windows.Forms.MenuItem EmulatorStatusPrintSCPEmulator;
        private System.Windows.Forms.ToolBarButton ToolBarStop;
        private System.Windows.Forms.ToolBarButton tb_sep1;
        private System.Windows.Forms.ToolBarButton ToolBarOpen;
        private System.Windows.Forms.ToolBarButton ToolBarSave;
        private System.Windows.Forms.ToolBarButton tb_sep2;
        private System.Windows.Forms.ToolBarButton ToolBarBack;
        private System.Windows.Forms.ToolBarButton ToolBarForward;
        private System.Windows.Forms.MenuItem MenuEdit;
        private System.Windows.Forms.MenuItem EditCopy;
        private System.Windows.Forms.MenuItem EditSeparator1;
        private System.Windows.Forms.MenuItem EditSelectAll;
        private System.Windows.Forms.MenuItem EditFind;
        private System.Windows.Forms.ToolBarButton tb_sep4;
        private System.Windows.Forms.ToolBarButton tb_sep3;
        private System.Windows.Forms.ToolBarButton ToolBarCopy;
        private System.Windows.Forms.ToolBarButton tb_sep5;
        private System.Windows.Forms.ToolBarButton ToolBarFind;
        private System.Windows.Forms.ToolBarButton ToolBarNextWarning;
        private System.Windows.Forms.ToolBarButton ToolBarNextError;
        private System.Windows.Forms.MenuItem EditFindAgain;
        private System.Windows.Forms.MenuItem WindowNewProjectViewAndTile;
        private System.Windows.Forms.MenuItem HelpAboutDVT;
        private System.Windows.Forms.SaveFileDialog ProjectSaveAsForm;
        private System.Windows.Forms.MenuItem EditSeparator2;
        private System.Windows.Forms.MenuItem EditEditScript;
        private System.Windows.Forms.ToolBarButton ToolBarEdit;
        private System.Windows.Forms.ToolBarButton tb_sep6;
        private System.Windows.Forms.MenuItem ViewRefreshSessionTree;
		private System.Windows.Forms.MenuItem MenuItem_FileProject;
		private System.Windows.Forms.MenuItem MenuItem_FileSession;
		private System.Windows.Forms.MenuItem MenuItem_FileProjectSaveAs;
		private System.Windows.Forms.MenuItem MenuItem_FileSessionSave;
		public System.Windows.Forms.MenuItem MenuItem_FileSessionRemove;
		private System.Windows.Forms.MenuItem MenuItem_FileSessionSaveAs;
		private System.Windows.Forms.MenuItem MenuItem_FileNew;
		private System.Windows.Forms.MenuItem MenuItem_FileOpen;
		private System.Windows.Forms.MenuItem MenuItem_FileExit;
		private System.Windows.Forms.MenuItem MenuItem_FileSave;
		private System.Windows.Forms.MenuItem MenuItem_File;
		private System.Windows.Forms.MenuItem MenuItem_FileProjectSave;
		private System.Windows.Forms.MenuItem MenuItem_FileProjectAddExistingSession;
		private System.Windows.Forms.MenuItem MenuItem_FileProjectAddNewSession;
		private System.Windows.Forms.MenuItem ViewShowEmptySessions;
		private System.Windows.Forms.MenuItem ViewAskForBackupResultsFile;
		private System.Windows.Forms.MenuItem ViewShowDicomScripts;
		private System.Windows.Forms.MenuItem ViewShowDicomSuperScripts;
		private System.Windows.Forms.MenuItem ViewShowVisualBasicScripts;
		private System.Windows.Forms.MenuItem HelpUserGuide;
		private System.Windows.Forms.MenuItem HelpDicomScript;
		private System.Windows.Forms.MenuItem ViewExpandVisualBasicScript;
		public System.Windows.Forms.ToolBarButton ToolBarFilterResults;
		private System.Windows.Forms.ToolBarButton ToolBarRefresh;
		private System.Windows.Forms.MenuItem MenuReport;
		private System.Windows.Forms.MenuItem GenerateReport;
		private System.Windows.Forms.MenuItem MenuItem_Seperator1;
		private System.Windows.Forms.MenuItem MenuItem_Seperator8;
		private System.Windows.Forms.MenuItem MenuItem_Seperator7;
		private System.Windows.Forms.MenuItem MenuItem_Seperator3;
		private System.Windows.Forms.MenuItem menuItemRecentProjs;
		private System.Windows.Forms.MenuItem FileSeparator4;
		private System.Windows.Forms.MenuItem menuItemProj1;
		private System.Windows.Forms.MenuItem menuItemProj2;
		private System.Windows.Forms.MenuItem menuItemProj3;
		private System.Windows.Forms.MenuItem menuItemProj4;
        private System.ComponentModel.IContainer components;
        bool isUserSelectedNo = false;
       

		/*StreamWriter writer = new StreamWriter(filteredAttrFilePath);
		foreach( object listItem in listBoxFilterAttr.Items)
		{
			writer.WriteLine(listItem.ToString());
		}
		writer.Close();*/

		// Constructor.
		public MainForm(System.ComponentModel.IContainer container)
		{
			if( container != null )
				container.Add(this);
		}

		private FindForm _FindForm = null;

		static private string _StartWithProjectFile = "";
		static private string _StartWithSessionFile = "";

		public UserSettings _UserSettings = new UserSettings();

		public MainForm()
		{
			_UserSettings.Load();

			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			Cursor.Current = Cursors.WaitCursor;

            //Dvtk.Setup.Initialize (); // MIGRATION_IN_PROGRESS
			_FindForm = new FindForm(this);

			if (_StartWithProjectFile != "")
			{
				projectApp.display_message = new DvtkApplicationLayer.Project.CallBackMessageDisplay (this.CallBackMessageDisplay);
				if(projectApp.Load(_StartWithProjectFile))
				{
					menuItemProj1.Text = _StartWithProjectFile;
				}
				foreach (Session session in projectApp.Sessions){
                    if (session is MediaSession){
                        MediaSession mediaSession = (MediaSession)session ;
                            mediaSession.CreateMediaFiles();
                    }
                    else if(session is EmulatorSession) {
                        EmulatorSession emulatorSession = (EmulatorSession)session ;
                            emulatorSession.CreateEmulatorFiles();
                    }
                    else {
                        ScriptSession scriptSession = (ScriptSession)session ;
                        scriptSession.CreateScriptFiles();
                    }                                       
                }
			}
			else if (_StartWithSessionFile != "")
			{
				projectApp.display_message = new DvtkApplicationLayer.Project.CallBackMessageDisplay (this.CallBackMessageDisplay);
				projectApp.New(_StartWithSessionFile.ToLower().Replace (".ses", ".pdvt"));
				projectApp.AddSession (_StartWithSessionFile);
			}

			if (projectApp.IsProjectConstructed)
			{
				ProjectForm2 theProjectForm = new ProjectForm2(projectApp, this);

				theProjectForm.MdiParent = this;

				theProjectForm.Show();
				theProjectForm.Activate();

				// Make the project form update itself completely.
				UpdateAll theUpdateAll = new UpdateAll();
				theProjectForm.Update(this, theUpdateAll);
			}

            this.UpdateUIControls ();

			//Update recent projects menu
			/*StreamReader reader = new StreamReader("");
			while (reader.Peek() != -1) 
			{
				ArrayList list = new ArrayList();
				string line = reader.ReadLine().Trim();
				if(line != "")
					list.Add(line);
				//foreach

			}
			reader.Close();*/

			Cursor.Current = Cursors.Default;
		}

		public bool _IsClosing = false;

        // use something that doesn't look like a password
        public const string DEFAULT_CERTIFICATE_FILE_PASSWORD=":-DHE-RSA-AES256-SHA:-DHE-DSS-AES256-SHA:-AES256-SHA";

        public bool DeleteProjectView (ProjectForm2   project_view)
        {
            if (this.MdiChildren.Length == 1)
				// The user wants to close the last project form.
            {
				// User decides what to save.               
                Save(false);

				Notify(this, new Saved());

				if (projectApp.AreProjectOrSessionsChanged())
				{
					return false;
				}

				projectApp.Close(false);
            }
            this.UpdateUIControls ();
            return true;
        }

		// Notify all ProjectForms of the event.
        public void Notify(object theSource, object theEvent) 
        {
            // If this indicates a change in the session, keep this information
            // to be able to be able to tell which sessions need to be saved.
            if (theEvent is SessionChange) 
            {
			    DvtkApplicationLayer.Session tempSession = GetSelectedSessionNew();
                tempSession.SetSessionChanged(((SessionChange)theEvent).SessionApp, true);
                tempSession.SetAllProperties();
            }

            // Update all project forms.
            for (int theIndex = MdiChildren.GetLowerBound(0); theIndex <= MdiChildren.GetUpperBound(0); theIndex++) 
            {
                if (MdiChildren.GetValue(theIndex) is ProjectForm2) {
                    ProjectForm2 theProjectForm = (ProjectForm2)(MdiChildren.GetValue(theIndex));
                    theProjectForm.Update(theSource , theEvent);
                }
            }

            // Update all controls owned directly by the mainform if necessary.
            if ( (theEvent is SessionTreeViewSelectionChange) ||
                (theEvent is StartExecution) ||
                (theEvent is EndExecution) ||
                (theEvent is SelectedTabChangeEvent) ||
                (theEvent is SessionChange) ||
                (theEvent is ProjectFormGetsFocusEvent) ||
                (theEvent is SessionRemovedEvent) ||
                (theEvent is WebNavigationComplete)||
                (theEvent is scriptWebNavigationComplete)
                ) 
            {
                UpdateUIControls();
            }

            if (theEvent is ProjectClosed) 
            {
                projectApp.Close(false);
                UpdateUIControls();
            }
        }

		// Update all controls that are directly owned by the main form.
        public void UpdateUIControls ()
        {
			UpdateTitleBarText();

			UpdateMenu();

			UpdateToolBar();
        }

		/// <summary>
		/// Update the toolbar.
		/// </summary>
		private void UpdateToolBar()
		{
			bool NewEnabledStateNew = false;
			bool NewEnabledStateOpen = false;
			bool NewEnabledStateSave = false;
			bool NewEnabledStateCopy = false;
			bool NewEnabledStateEdit = false;
			bool NewEnabledStateFind = false;
			bool NewEnabledStateNextWarning = false;
			bool NewEnabledStateNextError = false;
			bool NewEnabledStateStop = false;
			bool NewEnabledStateBack = false;
			bool NewEnabledStateForward = false;
			bool NewEnabledStateRefresh = false;
			bool NewEnabledStateFilter = false;

			if ((!projectApp.IsProjectConstructed) || (ActiveMdiChild == null))
				// No project form visible.
			{
				NewEnabledStateNew = true;
				NewEnabledStateOpen = true;
			}
			else
				// One or more project forms visible.
			{
				ProjectForm2 theActiveProjectForm = (ProjectForm2) this.ActiveMdiChild;
				if (projectApp.Sessions.Count !=0 )
				{
					if (IsExecuting(GetSelectedSessionNew()))
						// Executing.
					{
						NewEnabledStateStop = true;
					}
					else
						// Not executing.
					{
						NewEnabledStateNew = true;
						NewEnabledStateOpen = true;
						NewEnabledStateSave = projectApp.AreProjectOrSessionsChanged();

						switch (theActiveProjectForm.GetActiveTab())
						{
							case ProjectForm2.ProjectFormActiveTab.SCRIPT_TAB:
								NewEnabledStateCopy = true;
                                NewEnabledStateBack = theActiveProjectForm.getWebBrowserScript().CanGoBack;
                                NewEnabledStateForward = theActiveProjectForm.getWebBrowserScript().CanGoForward;
								break;

							case ProjectForm2.ProjectFormActiveTab.VALIDATION_RESULTS_TAB:
								NewEnabledStateCopy = true;
								NewEnabledStateFind = true;
								NewEnabledStateFilter = true;
								NewEnabledStateRefresh = true;
								 
                                NewEnabledStateNextWarning = theActiveProjectForm.TCM_GetValidationResultsManager().ContainsWarnings;
                                NewEnabledStateNextError = theActiveProjectForm.TCM_GetValidationResultsManager().ContainsErrors;
								NewEnabledStateBack = theActiveProjectForm.TCM_GetValidationResultsManager().BackEnabled();
								NewEnabledStateForward = theActiveProjectForm.TCM_GetValidationResultsManager().ForwardEnabled();
								break;

							case ProjectForm2.ProjectFormActiveTab.ACTIVITY_LOGGING_TAB:
								NewEnabledStateCopy = true;
								break;

							case ProjectForm2.ProjectFormActiveTab.RESULTS_MANAGER_TAB:
								NewEnabledStateRefresh = true;
								NewEnabledStateFilter = true;
                                NewEnabledStateBack = theActiveProjectForm.getValidationResultsManagerBrowser().CanGoBack;
                                NewEnabledStateForward = theActiveProjectForm.getValidationResultsManagerBrowser().CanGoForward;
								break;

							case ProjectForm2.ProjectFormActiveTab.SESSION_INFORMATION_TAB:							
							case ProjectForm2.ProjectFormActiveTab.SPECIFY_SOP_CLASSES_TAB:
							case ProjectForm2.ProjectFormActiveTab.OTHER_TAB:
							default:
								break;
						}

						if (GetSelectedTag() is Script)
						{
							NewEnabledStateEdit = true;
						}
					}
				}
				if (projectApp.Sessions.Count ==0 )
				{
					NewEnabledStateNew = true;
					NewEnabledStateOpen = true;
					NewEnabledStateSave = projectApp.AreProjectOrSessionsChanged();
				}
			}

			// Do the actual enabling/disabling of the tool bar buttons.
			ToolBarNew.Enabled = NewEnabledStateNew;
			ToolBarOpen.Enabled = NewEnabledStateOpen;
			ToolBarSave.Enabled = NewEnabledStateSave;
			ToolBarCopy.Enabled = NewEnabledStateCopy;
			ToolBarEdit.Enabled = NewEnabledStateEdit;
			ToolBarFind.Enabled = NewEnabledStateFind;
			ToolBarNextWarning.Enabled = NewEnabledStateNextWarning;
			ToolBarNextError.Enabled = NewEnabledStateNextError;
			ToolBarStop.Enabled = NewEnabledStateStop;
			ToolBarBack.Enabled = NewEnabledStateBack;
			ToolBarForward.Enabled = NewEnabledStateForward;
			ToolBarRefresh.Enabled = NewEnabledStateRefresh;
			ToolBarFilterResults.Enabled = NewEnabledStateFilter;			
		}

		/// <summary>
		/// Update the menu's and menu items.
		/// </summary>
		private void UpdateMenu()
		{
			// File menu items.
			bool isFileNewEnabled = false;
			bool isFileOpenEnabled = false;
			bool isFileProjectEnabled = false;
			bool isFileProjectAddExistingSessionEnabled = false;
			bool isFileProjectAddNewSessionEnabled = false;
			bool isFileProjectSaveEnabled = false;
			bool isFileProjectSaveAsEnabled = false;
			bool isFileSaveEnabled = false;
			bool isFileSessionEnabled = false;
			bool isFileSessionRemoveEnabled = false;
			bool isFileSessionSaveEnabled = false;
			bool isFileSessionSaveAsEnabled = false;

			// Other (sub) menu's that might be disabled.
			bool NewEnabledStateMenuEdit = false;
			bool NewEnabledStateMenuView = false;
			bool NewEnabledMenuReport    = false;
			bool NewEnabledStateMenuEmulator = false;
			bool NewEnabledStateMenuWindow = false;
			bool NewEnabledStateMenuGenerateReport = false;

			// Other menu items that might be disabled.
			bool NewEnabledStateEmulatorStatusPrintSCPEmulator = false;
			bool NewEnabledStateEditCopy = false;
			bool NewEnabledStateEditSelectAll = false;
			bool NewEnabledStateEditFind = false;
			bool NewEnabledStateEditFindAgain = false;
			bool NewEnabledStateEditEditScript = false;

			// Determine the state of the File menu items.
			// If no project opened...
			if (!projectApp.IsProjectConstructed)
			{
				isFileNewEnabled = true;
				isFileOpenEnabled = true;
			}
			// If project openend...
			else
			{
				isFileProjectEnabled = true;
				isFileProjectAddExistingSessionEnabled = true;
				isFileProjectAddNewSessionEnabled = true;
				if (projectApp.HasProjectChanged)
				{
					isFileProjectSaveEnabled = true;
				}
				isFileProjectSaveAsEnabled = true;

				// If no sessions are executing...
				if (GetExecutingSessions().Count == 0)
				{
					isFileNewEnabled = true;
					isFileOpenEnabled = true;

					if (projectApp.AreProjectOrSessionsChanged())
					{
						isFileSaveEnabled = true;
					}
				}

				// If a session is selected...
				if (projectApp.Sessions.Count == 0)
				{
					// do nothing 
				}
				else 
				{
					if (GetSelectedSessionNew() != null)
					{
						// If this session is not executing...
						if (!IsExecuting(GetSelectedSessionNew()))
						{
							isFileSessionEnabled = true;
							isFileSessionRemoveEnabled = true;
							GetSelectedSessionNew().ParentProject = projectApp;
							if (GetSelectedSessionNew().GetSessionChanged(GetSelectedSessionNew()))
							{
								isFileSessionSaveEnabled = true;
							}
							isFileSessionSaveAsEnabled = true;						
						}
					}
				}
			}

			// Determine the enabled state of the other menu items.
			ViewAskForBackupResultsFile.Checked = _UserSettings.AskForBackupResultsFile;
			ViewShowDicomScripts.Checked = _UserSettings.ShowDicomScripts;
			ViewShowDicomSuperScripts.Checked = _UserSettings.ShowDicomSuperScripts;
			ViewShowVisualBasicScripts.Checked = _UserSettings.ShowVisualBasicScripts;
			ViewShowEmptySessions.Checked = _UserSettings.ShowEmptySessions;
			ViewExpandVisualBasicScript.Checked = _UserSettings.ExpandVisualBasicScript;

			if ((projectApp.IsProjectConstructed) && (this.ActiveMdiChild != null))
			{
				if ( projectApp.Sessions.Count !=0)
				{
					ProjectForm2 theActiveProjectForm = (ProjectForm2) this.ActiveMdiChild;

					NewEnabledStateMenuWindow = true;

					if (!IsExecuting(GetSelectedSessionNew()))
						// Not executing.
					{
						NewEnabledStateMenuView = true;
						NewEnabledStateMenuGenerateReport = true;
						NewEnabledMenuReport = true;

						if (GetSelectedTag() is Script)
						{
							NewEnabledStateMenuEdit = true;
							NewEnabledStateEditEditScript = true;
							NewEnabledStateMenuGenerateReport = true;
							NewEnabledMenuReport = true;
						}
					}

					//If any session is executing in any project view the view menu should
					// be disabled. DVT Problem Reports item #1251807(Source Forge)
					if(IsExecuting())
					{
						NewEnabledStateMenuView = false;
					}
                
					if (theActiveProjectForm.GetState() == ProjectForm2.ProjectFormState.EXECUTING_PRINT_SCP)
					{
						NewEnabledStateMenuEmulator = true;
						NewEnabledStateEmulatorStatusPrintSCPEmulator = true;
					}

					switch (theActiveProjectForm.GetActiveTab())
					{
						case ProjectForm2.ProjectFormActiveTab.SCRIPT_TAB:
						case ProjectForm2.ProjectFormActiveTab.ACTIVITY_LOGGING_TAB:
							NewEnabledStateMenuEdit = true;
							NewEnabledStateEditCopy = true;
							break;

						case ProjectForm2.ProjectFormActiveTab.VALIDATION_RESULTS_TAB:
							NewEnabledStateMenuEdit = true;
							NewEnabledStateEditCopy = true;
							NewEnabledStateEditFind = true;
							NewEnabledStateMenuGenerateReport = true;
							NewEnabledMenuReport = true;
							if (_FindForm.search_string != "")
							{
								NewEnabledStateEditFindAgain = true;
							}
							break;

						case ProjectForm2.ProjectFormActiveTab.SESSION_INFORMATION_TAB:
						case ProjectForm2.ProjectFormActiveTab.SPECIFY_SOP_CLASSES_TAB:
						case ProjectForm2.ProjectFormActiveTab.OTHER_TAB:
						default:
							break;
					}

					if (theActiveProjectForm.GetSelectedSessionNew() == null)
					{
						MenuItem_FileSessionSave.Text = "Save session";
						MenuItem_FileSessionRemove.Text = "Remove session from project";
					}
					else 
					{
						MenuItem_FileSessionSave.Text = 
							string.Format(
							"Save session \"{0}\"",
							theActiveProjectForm.GetSelectedSessionNew().SessionFileName);


						MenuItem_FileSessionRemove.Text = 
							string.Format(
							"Remove session \"{0}\" from project",
							theActiveProjectForm.GetSelectedSessionNew().SessionFileName);
					}
				}
			}

			// File menu item: do the actual enabling/disabling
			MenuItem_FileNew.Enabled = isFileNewEnabled;
			MenuItem_FileOpen.Enabled = isFileOpenEnabled;
			MenuItem_FileProject.Enabled = isFileProjectEnabled;
			MenuItem_FileProjectAddExistingSession.Enabled = isFileProjectAddExistingSessionEnabled;
			MenuItem_FileProjectAddNewSession.Enabled = isFileProjectAddNewSessionEnabled;
			MenuItem_FileProjectSave.Enabled = isFileProjectSaveEnabled;
			MenuItem_FileProjectSaveAs.Enabled = isFileProjectSaveAsEnabled;
			MenuItem_FileSave.Enabled = isFileSaveEnabled;
			MenuItem_FileSession.Enabled = isFileSessionEnabled;
			MenuItem_FileSessionRemove.Enabled = isFileSessionRemoveEnabled;
			MenuItem_FileSessionSave.Enabled = isFileSessionSaveEnabled;
			MenuItem_FileSessionSaveAs.Enabled = isFileSessionSaveAsEnabled;

			// Others: do the actual enabling/disabling of the menu's and menu items.
			MenuEdit.Enabled = NewEnabledStateMenuEdit;
			MenuView.Enabled = NewEnabledStateMenuView;
			MenuEmulator.Enabled = NewEnabledStateMenuEmulator;
			MenuWindow.Enabled = NewEnabledStateMenuWindow;
			GenerateReport.Enabled = NewEnabledStateMenuGenerateReport;
			MenuReport.Enabled = NewEnabledMenuReport;
			EmulatorStatusPrintSCPEmulator.Enabled = NewEnabledStateEmulatorStatusPrintSCPEmulator;
			EditCopy.Enabled = NewEnabledStateEditCopy;
			EditSelectAll.Enabled = NewEnabledStateEditSelectAll;
			EditFind.Enabled = NewEnabledStateEditFind;
			EditFindAgain.Enabled = NewEnabledStateEditFindAgain;
			EditEditScript.Enabled = NewEnabledStateEditEditScript;
			MenuItem_FileOpen.Enabled = true;
		 }

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.MainStatusBar = new System.Windows.Forms.StatusBar();
            this.MainMenu = new System.Windows.Forms.MainMenu(this.components);
            this.MenuItem_File = new System.Windows.Forms.MenuItem();
            this.MenuItem_FileNew = new System.Windows.Forms.MenuItem();
            this.MenuItem_FileOpen = new System.Windows.Forms.MenuItem();
            this.FileSeparator1 = new System.Windows.Forms.MenuItem();
            this.MenuItem_FileSave = new System.Windows.Forms.MenuItem();
            this.FileSeparator2 = new System.Windows.Forms.MenuItem();
            this.MenuItem_FileProject = new System.Windows.Forms.MenuItem();
            this.MenuItem_FileProjectAddExistingSession = new System.Windows.Forms.MenuItem();
            this.MenuItem_FileProjectAddNewSession = new System.Windows.Forms.MenuItem();
            this.MenuItem_Seperator1 = new System.Windows.Forms.MenuItem();
            this.MenuItem_FileProjectSave = new System.Windows.Forms.MenuItem();
            this.MenuItem_FileProjectSaveAs = new System.Windows.Forms.MenuItem();
            this.MenuItem_FileSession = new System.Windows.Forms.MenuItem();
            this.MenuItem_FileSessionRemove = new System.Windows.Forms.MenuItem();
            this.MenuItem_Seperator8 = new System.Windows.Forms.MenuItem();
            this.MenuItem_FileSessionSave = new System.Windows.Forms.MenuItem();
            this.MenuItem_FileSessionSaveAs = new System.Windows.Forms.MenuItem();
            this.FileSeparator3 = new System.Windows.Forms.MenuItem();
            this.menuItemRecentProjs = new System.Windows.Forms.MenuItem();
            this.menuItemProj1 = new System.Windows.Forms.MenuItem();
            this.menuItemProj2 = new System.Windows.Forms.MenuItem();
            this.menuItemProj3 = new System.Windows.Forms.MenuItem();
            this.menuItemProj4 = new System.Windows.Forms.MenuItem();
            this.FileSeparator4 = new System.Windows.Forms.MenuItem();
            this.MenuItem_FileExit = new System.Windows.Forms.MenuItem();
            this.MenuEdit = new System.Windows.Forms.MenuItem();
            this.EditCopy = new System.Windows.Forms.MenuItem();
            this.EditSeparator1 = new System.Windows.Forms.MenuItem();
            this.EditSelectAll = new System.Windows.Forms.MenuItem();
            this.EditFind = new System.Windows.Forms.MenuItem();
            this.EditFindAgain = new System.Windows.Forms.MenuItem();
            this.EditSeparator2 = new System.Windows.Forms.MenuItem();
            this.EditEditScript = new System.Windows.Forms.MenuItem();
            this.MenuView = new System.Windows.Forms.MenuItem();
            this.ViewAskForBackupResultsFile = new System.Windows.Forms.MenuItem();
            this.ViewShowEmptySessions = new System.Windows.Forms.MenuItem();
            this.ViewShowDicomScripts = new System.Windows.Forms.MenuItem();
            this.ViewShowDicomSuperScripts = new System.Windows.Forms.MenuItem();
            this.ViewShowVisualBasicScripts = new System.Windows.Forms.MenuItem();
            this.ViewExpandVisualBasicScript = new System.Windows.Forms.MenuItem();
            this.MenuItem_Seperator7 = new System.Windows.Forms.MenuItem();
            this.ViewRefreshSessionTree = new System.Windows.Forms.MenuItem();
            this.MenuEmulator = new System.Windows.Forms.MenuItem();
            this.EmulatorStatusPrintSCPEmulator = new System.Windows.Forms.MenuItem();
            this.MenuReport = new System.Windows.Forms.MenuItem();
            this.GenerateReport = new System.Windows.Forms.MenuItem();
            this.MenuWindow = new System.Windows.Forms.MenuItem();
            this.WindowNewProjectView = new System.Windows.Forms.MenuItem();
            this.WindowNewProjectViewAndTile = new System.Windows.Forms.MenuItem();
            this.WindowTileViews = new System.Windows.Forms.MenuItem();
            this.WindowCascadeViews = new System.Windows.Forms.MenuItem();
            this.MenuHelp = new System.Windows.Forms.MenuItem();
            this.HelpDicomScript = new System.Windows.Forms.MenuItem();
            this.HelpUserGuide = new System.Windows.Forms.MenuItem();
            this.MenuItem_Seperator3 = new System.Windows.Forms.MenuItem();
            this.HelpAboutDVT = new System.Windows.Forms.MenuItem();
            this.MainToolBar = new System.Windows.Forms.ToolBar();
            this.ToolBarNew = new System.Windows.Forms.ToolBarButton();
            this.tb_sep1 = new System.Windows.Forms.ToolBarButton();
            this.ToolBarOpen = new System.Windows.Forms.ToolBarButton();
            this.ToolBarSave = new System.Windows.Forms.ToolBarButton();
            this.tb_sep2 = new System.Windows.Forms.ToolBarButton();
            this.ToolBarCopy = new System.Windows.Forms.ToolBarButton();
            this.tb_sep3 = new System.Windows.Forms.ToolBarButton();
            this.ToolBarEdit = new System.Windows.Forms.ToolBarButton();
            this.tb_sep4 = new System.Windows.Forms.ToolBarButton();
            this.ToolBarFind = new System.Windows.Forms.ToolBarButton();
            this.ToolBarNextWarning = new System.Windows.Forms.ToolBarButton();
            this.ToolBarNextError = new System.Windows.Forms.ToolBarButton();
            this.tb_sep5 = new System.Windows.Forms.ToolBarButton();
            this.ToolBarStop = new System.Windows.Forms.ToolBarButton();
            this.tb_sep6 = new System.Windows.Forms.ToolBarButton();
            this.ToolBarBack = new System.Windows.Forms.ToolBarButton();
            this.ToolBarForward = new System.Windows.Forms.ToolBarButton();
            this.ToolBarFilterResults = new System.Windows.Forms.ToolBarButton();
            this.ToolBarRefresh = new System.Windows.Forms.ToolBarButton();
            this.ImageListFunctions = new System.Windows.Forms.ImageList(this.components);
            this.DialogOpenProject = new System.Windows.Forms.OpenFileDialog();
            this.ProjectSaveAsForm = new System.Windows.Forms.SaveFileDialog();
            this.SuspendLayout();
            // 
            // MainStatusBar
            // 
            this.MainStatusBar.Location = new System.Drawing.Point(0, 431);
            this.MainStatusBar.Name = "MainStatusBar";
            this.MainStatusBar.Size = new System.Drawing.Size(656, 34);
            this.MainStatusBar.TabIndex = 0;
            // 
            // MainMenu
            // 
            this.MainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.MenuItem_File,
            this.MenuEdit,
            this.MenuView,
            this.MenuEmulator,
            this.MenuReport,
            this.MenuWindow,
            this.MenuHelp});
            // 
            // MenuItem_File
            // 
            this.MenuItem_File.Index = 0;
            this.MenuItem_File.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.MenuItem_FileNew,
            this.MenuItem_FileOpen,
            this.FileSeparator1,
            this.MenuItem_FileSave,
            this.FileSeparator2,
            this.MenuItem_FileProject,
            this.MenuItem_FileSession,
            this.FileSeparator3,
            this.menuItemRecentProjs,
            this.FileSeparator4,
            this.MenuItem_FileExit});
            this.MenuItem_File.Text = "File";
            // 
            // MenuItem_FileNew
            // 
            this.MenuItem_FileNew.Index = 0;
            this.MenuItem_FileNew.Text = "&New...";
            this.MenuItem_FileNew.Click += new System.EventHandler(this.FileNew_Click);
            // 
            // MenuItem_FileOpen
            // 
            this.MenuItem_FileOpen.Index = 1;
            this.MenuItem_FileOpen.Text = "&Open...";
            this.MenuItem_FileOpen.Click += new System.EventHandler(this.FileOpen_Click);
            // 
            // FileSeparator1
            // 
            this.FileSeparator1.Index = 2;
            this.FileSeparator1.Text = "-";
            // 
            // MenuItem_FileSave
            // 
            this.MenuItem_FileSave.Index = 3;
            this.MenuItem_FileSave.Text = "Save...";
            this.MenuItem_FileSave.Click += new System.EventHandler(this.MenuItem_FileSave_Click);
            // 
            // FileSeparator2
            // 
            this.FileSeparator2.Index = 4;
            this.FileSeparator2.Text = "-";
            // 
            // MenuItem_FileProject
            // 
            this.MenuItem_FileProject.Index = 5;
            this.MenuItem_FileProject.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.MenuItem_FileProjectAddExistingSession,
            this.MenuItem_FileProjectAddNewSession,
            this.MenuItem_Seperator1,
            this.MenuItem_FileProjectSave,
            this.MenuItem_FileProjectSaveAs});
            this.MenuItem_FileProject.Text = "Project";
            // 
            // MenuItem_FileProjectAddExistingSession
            // 
            this.MenuItem_FileProjectAddExistingSession.Index = 0;
            this.MenuItem_FileProjectAddExistingSession.Text = "Add Existing Session(s)...";
            this.MenuItem_FileProjectAddExistingSession.Click += new System.EventHandler(this.MenuItem_FileProjectAddExistingSession_Click);
            // 
            // MenuItem_FileProjectAddNewSession
            // 
            this.MenuItem_FileProjectAddNewSession.Index = 1;
            this.MenuItem_FileProjectAddNewSession.Text = "Add New Session...";
            this.MenuItem_FileProjectAddNewSession.Click += new System.EventHandler(this.MenuItem_FileProjectAddNewSession_Click);
            // 
            // MenuItem_Seperator1
            // 
            this.MenuItem_Seperator1.Index = 2;
            this.MenuItem_Seperator1.Text = "-";
            // 
            // MenuItem_FileProjectSave
            // 
            this.MenuItem_FileProjectSave.Index = 3;
            this.MenuItem_FileProjectSave.Text = "Save (project file only)";
            this.MenuItem_FileProjectSave.Click += new System.EventHandler(this.MenuItem_FileProjectSave_Click_1);
            // 
            // MenuItem_FileProjectSaveAs
            // 
            this.MenuItem_FileProjectSaveAs.Index = 4;
            this.MenuItem_FileProjectSaveAs.Text = "Save As (project file only)...";
            this.MenuItem_FileProjectSaveAs.Click += new System.EventHandler(this.MenuItem_FileProjectSaveAs_Click);
            // 
            // MenuItem_FileSession
            // 
            this.MenuItem_FileSession.Index = 6;
            this.MenuItem_FileSession.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.MenuItem_FileSessionRemove,
            this.MenuItem_Seperator8,
            this.MenuItem_FileSessionSave,
            this.MenuItem_FileSessionSaveAs});
            this.MenuItem_FileSession.Text = "Session";
            // 
            // MenuItem_FileSessionRemove
            // 
            this.MenuItem_FileSessionRemove.Index = 0;
            this.MenuItem_FileSessionRemove.Text = "Remove from Project";
            this.MenuItem_FileSessionRemove.Click += new System.EventHandler(this.FileSessionRemove_Click);
            // 
            // MenuItem_Seperator8
            // 
            this.MenuItem_Seperator8.Index = 1;
            this.MenuItem_Seperator8.Text = "-";
            // 
            // MenuItem_FileSessionSave
            // 
            this.MenuItem_FileSessionSave.Index = 2;
            this.MenuItem_FileSessionSave.Text = "Save";
            this.MenuItem_FileSessionSave.Click += new System.EventHandler(this.FileSessionSave_Click);
            // 
            // MenuItem_FileSessionSaveAs
            // 
            this.MenuItem_FileSessionSaveAs.Index = 3;
            this.MenuItem_FileSessionSaveAs.Text = "Save As...";
            this.MenuItem_FileSessionSaveAs.Click += new System.EventHandler(this.MenuItem_FileSessionSaveAs_Click);
            // 
            // FileSeparator3
            // 
            this.FileSeparator3.Index = 7;
            this.FileSeparator3.Text = "-";
            // 
            // menuItemRecentProjs
            // 
            this.menuItemRecentProjs.Index = 8;
            this.menuItemRecentProjs.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemProj1,
            this.menuItemProj2,
            this.menuItemProj3,
            this.menuItemProj4});
            this.menuItemRecentProjs.Text = "Recent Projects";
            this.menuItemRecentProjs.Visible = false;
            // 
            // menuItemProj1
            // 
            this.menuItemProj1.Index = 0;
            this.menuItemProj1.Text = "";
            // 
            // menuItemProj2
            // 
            this.menuItemProj2.Index = 1;
            this.menuItemProj2.Text = "";
            // 
            // menuItemProj3
            // 
            this.menuItemProj3.Index = 2;
            this.menuItemProj3.Text = "";
            // 
            // menuItemProj4
            // 
            this.menuItemProj4.Index = 3;
            this.menuItemProj4.Text = "";
            // 
            // FileSeparator4
            // 
            this.FileSeparator4.Index = 9;
            this.FileSeparator4.Text = "-";
            this.FileSeparator4.Visible = false;
            // 
            // MenuItem_FileExit
            // 
            this.MenuItem_FileExit.Index = 10;
            this.MenuItem_FileExit.Text = "Close";
            this.MenuItem_FileExit.Click += new System.EventHandler(this.FileExit_Click);
            // 
            // MenuEdit
            // 
            this.MenuEdit.Index = 1;
            this.MenuEdit.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.EditCopy,
            this.EditSeparator1,
            this.EditSelectAll,
            this.EditFind,
            this.EditFindAgain,
            this.EditSeparator2,
            this.EditEditScript});
            this.MenuEdit.Text = "Edit";
            // 
            // EditCopy
            // 
            this.EditCopy.Enabled = false;
            this.EditCopy.Index = 0;
            this.EditCopy.Shortcut = System.Windows.Forms.Shortcut.CtrlC;
            this.EditCopy.Text = "Copy";
            this.EditCopy.Click += new System.EventHandler(this.EditCopy_Click);
            // 
            // EditSeparator1
            // 
            this.EditSeparator1.Index = 1;
            this.EditSeparator1.Text = "-";
            // 
            // EditSelectAll
            // 
            this.EditSelectAll.Index = 2;
            this.EditSelectAll.Text = "Select All";
            this.EditSelectAll.Visible = false;
            this.EditSelectAll.Click += new System.EventHandler(this.EditSelectAll_Click);
            // 
            // EditFind
            // 
            this.EditFind.Index = 3;
            this.EditFind.Shortcut = System.Windows.Forms.Shortcut.CtrlF;
            this.EditFind.Text = "Find";
            this.EditFind.Click += new System.EventHandler(this.EditFind_Click);
            // 
            // EditFindAgain
            // 
            this.EditFindAgain.Index = 4;
            this.EditFindAgain.Shortcut = System.Windows.Forms.Shortcut.F3;
            this.EditFindAgain.Text = "Find Again";
            this.EditFindAgain.Click += new System.EventHandler(this.EditFindAgain_Click);
            // 
            // EditSeparator2
            // 
            this.EditSeparator2.Index = 5;
            this.EditSeparator2.Text = "-";
            // 
            // EditEditScript
            // 
            this.EditEditScript.Index = 6;
            this.EditEditScript.Shortcut = System.Windows.Forms.Shortcut.F2;
            this.EditEditScript.Text = "Edit Script with Notepad...";
            this.EditEditScript.Click += new System.EventHandler(this.EditEditScript_Click);
            // 
            // MenuView
            // 
            this.MenuView.Index = 2;
            this.MenuView.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.ViewAskForBackupResultsFile,
            this.ViewShowEmptySessions,
            this.ViewShowDicomScripts,
            this.ViewShowDicomSuperScripts,
            this.ViewShowVisualBasicScripts,
            this.ViewExpandVisualBasicScript,
            this.MenuItem_Seperator7,
            this.ViewRefreshSessionTree});
            this.MenuView.Text = "&View";
            // 
            // ViewAskForBackupResultsFile
            // 
            this.ViewAskForBackupResultsFile.Index = 0;
            this.ViewAskForBackupResultsFile.Text = "Ask for Backup";
            this.ViewAskForBackupResultsFile.Click += new System.EventHandler(this.ViewAskForBackupResultsFile_Click);
            // 
            // ViewShowEmptySessions
            // 
            this.ViewShowEmptySessions.Index = 1;
            this.ViewShowEmptySessions.Text = "Show Empty Script Sessions";
            this.ViewShowEmptySessions.Click += new System.EventHandler(this.ViewShowEmptySessions_Click);
            // 
            // ViewShowDicomScripts
            // 
            this.ViewShowDicomScripts.Index = 2;
            this.ViewShowDicomScripts.Text = "Show Dicom Scripts";
            this.ViewShowDicomScripts.Click += new System.EventHandler(this.ViewShowDsFiles_Click);
            // 
            // ViewShowDicomSuperScripts
            // 
            this.ViewShowDicomSuperScripts.Index = 3;
            this.ViewShowDicomSuperScripts.Text = "Show Dicom Super Scripts";
            this.ViewShowDicomSuperScripts.Click += new System.EventHandler(this.ViewShowDssFiles_Click);
            // 
            // ViewShowVisualBasicScripts
            // 
            this.ViewShowVisualBasicScripts.Index = 4;
            this.ViewShowVisualBasicScripts.Text = "Show Visual Basic Scripts";
            this.ViewShowVisualBasicScripts.Click += new System.EventHandler(this.ViewShowVbsFiles_Click);
            // 
            // ViewExpandVisualBasicScript
            // 
            this.ViewExpandVisualBasicScript.Index = 5;
            this.ViewExpandVisualBasicScript.Text = "Expand Visual Basic Script";
            this.ViewExpandVisualBasicScript.Click += new System.EventHandler(this.ViewExpandVisualBasicScript_Click);
            // 
            // MenuItem_Seperator7
            // 
            this.MenuItem_Seperator7.Index = 6;
            this.MenuItem_Seperator7.Text = "-";
            // 
            // ViewRefreshSessionTree
            // 
            this.ViewRefreshSessionTree.Index = 7;
            this.ViewRefreshSessionTree.Shortcut = System.Windows.Forms.Shortcut.F5;
            this.ViewRefreshSessionTree.Text = "Refresh Session Tree";
            this.ViewRefreshSessionTree.Click += new System.EventHandler(this.ViewRefreshSessionTree_Click);
            // 
            // MenuEmulator
            // 
            this.MenuEmulator.Index = 3;
            this.MenuEmulator.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.EmulatorStatusPrintSCPEmulator});
            this.MenuEmulator.Text = "Emulator Status";
            // 
            // EmulatorStatusPrintSCPEmulator
            // 
            this.EmulatorStatusPrintSCPEmulator.Index = 0;
            this.EmulatorStatusPrintSCPEmulator.Text = "Status Print SCP Emulator";
            this.EmulatorStatusPrintSCPEmulator.Click += new System.EventHandler(this.EmulatorStatusPrintSCPEmulator_Click);
            // 
            // MenuReport
            // 
            this.MenuReport.Index = 4;
            this.MenuReport.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.GenerateReport});
            this.MenuReport.Text = "Report";
            this.MenuReport.Click += new System.EventHandler(this.MenuReport_Click);
            // 
            // GenerateReport
            // 
            this.GenerateReport.Index = 0;
            this.GenerateReport.Text = "Generate Report";
            this.GenerateReport.Click += new System.EventHandler(this.GenerateReport_Click);
            // 
            // MenuWindow
            // 
            this.MenuWindow.Index = 5;
            this.MenuWindow.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.WindowNewProjectView,
            this.WindowNewProjectViewAndTile,
            this.WindowTileViews,
            this.WindowCascadeViews});
            this.MenuWindow.Text = "&Window";
            // 
            // WindowNewProjectView
            // 
            this.WindowNewProjectView.Index = 0;
            this.WindowNewProjectView.Text = "New Project View";
            this.WindowNewProjectView.Click += new System.EventHandler(this.WindowNewProjectView_Click);
            // 
            // WindowNewProjectViewAndTile
            // 
            this.WindowNewProjectViewAndTile.Index = 1;
            this.WindowNewProjectViewAndTile.Text = "New Project View and Tile";
            this.WindowNewProjectViewAndTile.Click += new System.EventHandler(this.WindowNewProjectViewAndTile_Click);
            // 
            // WindowTileViews
            // 
            this.WindowTileViews.Index = 2;
            this.WindowTileViews.Text = "Tile Project Views";
            this.WindowTileViews.Click += new System.EventHandler(this.WindowTileViews_Click);
            // 
            // WindowCascadeViews
            // 
            this.WindowCascadeViews.Index = 3;
            this.WindowCascadeViews.Text = "Cascade Project Views";
            this.WindowCascadeViews.Click += new System.EventHandler(this.WindowCascadeViews_Click);
            // 
            // MenuHelp
            // 
            this.MenuHelp.Index = 6;
            this.MenuHelp.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.HelpDicomScript,
            this.HelpUserGuide,
            this.MenuItem_Seperator3,
            this.HelpAboutDVT});
            this.MenuHelp.Text = "&Help";
            // 
            // HelpDicomScript
            // 
            this.HelpDicomScript.Index = 0;
            this.HelpDicomScript.Text = "Dicom Script";
            this.HelpDicomScript.Click += new System.EventHandler(this.HelpDicomScript_Click);
            // 
            // HelpUserGuide
            // 
            this.HelpUserGuide.Index = 1;
            this.HelpUserGuide.Text = "User Guide";
            this.HelpUserGuide.Click += new System.EventHandler(this.menuItem2_Click);
            // 
            // MenuItem_Seperator3
            // 
            this.MenuItem_Seperator3.Index = 2;
            this.MenuItem_Seperator3.Text = "-";
            // 
            // HelpAboutDVT
            // 
            this.HelpAboutDVT.Index = 3;
            this.HelpAboutDVT.Text = "About DVT";
            this.HelpAboutDVT.Click += new System.EventHandler(this.HelpAboutDVT_Click);
            // 
            // MainToolBar
            // 
            this.MainToolBar.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
            this.MainToolBar.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.ToolBarNew,
            this.tb_sep1,
            this.ToolBarOpen,
            this.ToolBarSave,
            this.tb_sep2,
            this.ToolBarCopy,
            this.tb_sep3,
            this.ToolBarEdit,
            this.tb_sep4,
            this.ToolBarFind,
            this.ToolBarNextWarning,
            this.ToolBarNextError,
            this.tb_sep5,
            this.ToolBarStop,
            this.tb_sep6,
            this.ToolBarBack,
            this.ToolBarForward,
            this.ToolBarFilterResults,
            this.ToolBarRefresh});
            this.MainToolBar.ButtonSize = new System.Drawing.Size(23, 22);
            this.MainToolBar.DropDownArrows = true;
            this.MainToolBar.ImageList = this.ImageListFunctions;
            this.MainToolBar.Location = new System.Drawing.Point(0, 0);
            this.MainToolBar.Name = "MainToolBar";
            this.MainToolBar.ShowToolTips = true;
            this.MainToolBar.Size = new System.Drawing.Size(656, 28);
            this.MainToolBar.TabIndex = 0;
            this.MainToolBar.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.MainToolBar_ButtonClick);
            // 
            // ToolBarNew
            // 
            this.ToolBarNew.ImageIndex = 0;
            this.ToolBarNew.Name = "ToolBarNew";
            this.ToolBarNew.ToolTipText = "New";
            // 
            // tb_sep1
            // 
            this.tb_sep1.Name = "tb_sep1";
            this.tb_sep1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // ToolBarOpen
            // 
            this.ToolBarOpen.ImageIndex = 1;
            this.ToolBarOpen.Name = "ToolBarOpen";
            this.ToolBarOpen.ToolTipText = "Open";
            // 
            // ToolBarSave
            // 
            this.ToolBarSave.ImageIndex = 2;
            this.ToolBarSave.Name = "ToolBarSave";
            this.ToolBarSave.ToolTipText = "Save";
            // 
            // tb_sep2
            // 
            this.tb_sep2.Name = "tb_sep2";
            this.tb_sep2.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // ToolBarCopy
            // 
            this.ToolBarCopy.ImageIndex = 6;
            this.ToolBarCopy.Name = "ToolBarCopy";
            this.ToolBarCopy.ToolTipText = "Copy";
            // 
            // tb_sep3
            // 
            this.tb_sep3.Name = "tb_sep3";
            this.tb_sep3.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // ToolBarEdit
            // 
            this.ToolBarEdit.Enabled = false;
            this.ToolBarEdit.ImageIndex = 10;
            this.ToolBarEdit.Name = "ToolBarEdit";
            this.ToolBarEdit.ToolTipText = "Edit Script with Notepad";
            // 
            // tb_sep4
            // 
            this.tb_sep4.Name = "tb_sep4";
            this.tb_sep4.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // ToolBarFind
            // 
            this.ToolBarFind.Enabled = false;
            this.ToolBarFind.ImageIndex = 7;
            this.ToolBarFind.Name = "ToolBarFind";
            this.ToolBarFind.ToolTipText = "Find";
            // 
            // ToolBarNextWarning
            // 
            this.ToolBarNextWarning.Enabled = false;
            this.ToolBarNextWarning.ImageIndex = 9;
            this.ToolBarNextWarning.Name = "ToolBarNextWarning";
            this.ToolBarNextWarning.ToolTipText = "Find Next Warning";
            // 
            // ToolBarNextError
            // 
            this.ToolBarNextError.Enabled = false;
            this.ToolBarNextError.ImageIndex = 8;
            this.ToolBarNextError.Name = "ToolBarNextError";
            this.ToolBarNextError.ToolTipText = "Find Next Error";
            // 
            // tb_sep5
            // 
            this.tb_sep5.Name = "tb_sep5";
            this.tb_sep5.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // ToolBarStop
            // 
            this.ToolBarStop.ImageIndex = 3;
            this.ToolBarStop.Name = "ToolBarStop";
            this.ToolBarStop.ToolTipText = "Stop";
            // 
            // tb_sep6
            // 
            this.tb_sep6.Name = "tb_sep6";
            this.tb_sep6.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // ToolBarBack
            // 
            this.ToolBarBack.Enabled = false;
            this.ToolBarBack.ImageIndex = 4;
            this.ToolBarBack.Name = "ToolBarBack";
            this.ToolBarBack.ToolTipText = "Navigate Back";
            // 
            // ToolBarForward
            // 
            this.ToolBarForward.Enabled = false;
            this.ToolBarForward.ImageIndex = 5;
            this.ToolBarForward.Name = "ToolBarForward";
            this.ToolBarForward.ToolTipText = "Navigate Forward";
            // 
            // ToolBarFilterResults
            // 
            this.ToolBarFilterResults.Enabled = false;
            this.ToolBarFilterResults.ImageIndex = 11;
            this.ToolBarFilterResults.Name = "ToolBarFilterResults";
            this.ToolBarFilterResults.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
            this.ToolBarFilterResults.ToolTipText = " Filter Results ";
            // 
            // ToolBarRefresh
            // 
            this.ToolBarRefresh.Enabled = false;
            this.ToolBarRefresh.ImageIndex = 12;
            this.ToolBarRefresh.Name = "ToolBarRefresh";
            this.ToolBarRefresh.ToolTipText = " Refresh Results ";
            // 
            // ImageListFunctions
            // 
            this.ImageListFunctions.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ImageListFunctions.ImageStream")));
            this.ImageListFunctions.TransparentColor = System.Drawing.Color.Transparent;
            this.ImageListFunctions.Images.SetKeyName(0, "");
            this.ImageListFunctions.Images.SetKeyName(1, "");
            this.ImageListFunctions.Images.SetKeyName(2, "");
            this.ImageListFunctions.Images.SetKeyName(3, "");
            this.ImageListFunctions.Images.SetKeyName(4, "");
            this.ImageListFunctions.Images.SetKeyName(5, "");
            this.ImageListFunctions.Images.SetKeyName(6, "");
            this.ImageListFunctions.Images.SetKeyName(7, "");
            this.ImageListFunctions.Images.SetKeyName(8, "");
            this.ImageListFunctions.Images.SetKeyName(9, "");
            this.ImageListFunctions.Images.SetKeyName(10, "");
            this.ImageListFunctions.Images.SetKeyName(11, "");
            this.ImageListFunctions.Images.SetKeyName(12, "");
            // 
            // DialogOpenProject
            // 
            this.DialogOpenProject.Filter = "DVT Project files (*.pdvt) |*.pdvt|Session files (*.ses)|*.ses";
            // 
            // ProjectSaveAsForm
            // 
            this.ProjectSaveAsForm.Filter = "DVT Project files (*.pdvt) |*.pdvt";
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.ClientSize = new System.Drawing.Size(656, 465);
            this.Controls.Add(this.MainToolBar);
            this.Controls.Add(this.MainStatusBar);
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.Menu = this.MainMenu;
            this.Name = "MainForm";
            this.Text = "Dicom Validation Tool";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Closing += new System.ComponentModel.CancelEventHandler(this.MainForm_Closing);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.MdiChildActivate += new System.EventHandler(this.MainForm_MdiChildActivate);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion


		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
        {
            try
            {
                DvtkApplicationLayer.DefinitionFilesChecker.CheckVersion("1.0.0", "2.0.0");
            }
            catch (Exception exception)
            {
                Exception customException = new Exception("PLEASE INSTALL DVTK DEFINITION FILES FROM www.dvtk.org website", exception);
                CustomExceptionHandler.ShowThreadExceptionDialog(customException);
                return;
            }
#if !DEBUG
			try
			{
#endif

            // Checks the version of both the application and the DVTk library.
            // If one or both are a non-official or Alpha version, a warning message box is displayed.
            //DvtkApplicationLayer.VersionChecker.CheckVersion();

			if (args.Length == 1)
			{
				if (args[0].ToLower().EndsWith(".pdvt"))
				{
					_StartWithProjectFile = args[0];
				}

				if (args[0].ToLower().EndsWith(".ses"))
				{
					_StartWithSessionFile = args[0];
				}
            }
            Application.Run(new MainForm());
#if !DEBUG
			}
			catch(Exception exception)
			{
				CustomExceptionHandler.ShowThreadExceptionDialog(exception);
			}
#endif			
		}


        #region ToolBar

        private void MainToolBar_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
        {
			if (e.Button == ToolBarNew)
			{
				this.ActionNew ();
			}
			else if (e.Button == ToolBarOpen)
			{
				this.ActionOpenProjectOrSession ();
			}
			else if (e.Button == ToolBarSave)
			{
				this.ActionSaveChangedFile ();
			}
			else if (e.Button == ToolBarCopy)
			{
				this.ActionEditCopy ();
			}
			else if (e.Button == ToolBarEdit)
			{
				if (GetActiveProjectForm() == null)
				{
					// Sanity check.
					Debug.Assert(false);
				}
				else
				{
					GetActiveProjectForm().GetSessionTreeViewManager().EditSelectedScriptFile();
				}
			}
			else if (e.Button == ToolBarStop)
			{
				this.ActionStopRunningProcess (); 
			}
			else if (e.Button == ToolBarBack)
			{
				this.ActionNavigateBack ();
			}
			else if (e.Button == ToolBarForward)
			{
				this.ActionNavigateForward ();
			}
			else if (e.Button == ToolBarFind)
			{
				this.ActionFind ();
			}
			else if (e.Button == ToolBarNextError)
			{
				this.ActionFindNextError ();
			}
			else if (e.Button == ToolBarNextWarning)
			{
				this.ActionFindNextWarning ();
			}
			else if (e.Button == ToolBarFilterResults )
			{
				if(this.ToolBarFilterResults.Pushed)
				GetActiveProjectForm().filterIndicator = true;
				else 
				GetActiveProjectForm().filterIndicator = false ;
				this.ActionFilteredResults ();
			}
			else if (e.Button == ToolBarRefresh )
			{
				this.ActionRefreshResults ();
			}
        }

        #endregion

        #region MenuFile
        private void FileNew_Click(object sender, System.EventArgs e)
        {
            this.ActionNew ();
        }

        private void FileOpen_Click(object sender, System.EventArgs e)
        {
            this.ActionOpenProjectOrSession ();
        }

        private void FileSave_Click(object sender, System.EventArgs e)
        {
            this.ActionSaveChangedFile ();
        }

        private void FileSaveAs_Click(object sender, System.EventArgs e)
        {
            this.ActionProjectSaveAs ();
        }

        private void FileSessionSave_Click(object sender, System.EventArgs e)
        {
			SaveSelectedSession();
        }

        private void FileSessionRemove_Click(object sender, System.EventArgs e)
        {
            RemoveSelectedSession();
        }

        private void FileSessionAddNew_Click(object sender, System.EventArgs e)
        {

        }

        private void FileSessionAddExisting_Click(object sender, System.EventArgs e)
        {
			AddExistingSessions();
        }

        private void FileExit_Click(object sender, System.EventArgs e)
        {
			this.Close();
        }
        #endregion

        #region MenuEdit

        private void EditCut_Click(object sender, System.EventArgs e)
        {
        
        }

        public void EditCopy_Click(object sender, System.EventArgs e)
        {
            this.ActionEditCopy ();
        }

        private void EditPaste_Click(object sender, System.EventArgs e)
        {
        
        }

        private void EditDelete_Click(object sender, System.EventArgs e)
        {
        
        }

        private void EditSelectAll_Click(object sender, System.EventArgs e)
        {
        
        }

        private void EditFind_Click(object sender, System.EventArgs e)
        {
            this.ActionFind ();
        }

        private void EditFindAgain_Click(object sender, System.EventArgs e)
        {
            this.ActionFindAgain ();
        }

        private void EditEditScript_Click(object sender, System.EventArgs e)
        {
			if (GetActiveProjectForm() == null)
			{
				// Sanity check.
				Debug.Assert(false);
			}
			else
			{
				GetActiveProjectForm().GetSessionTreeViewManager().EditSelectedScriptFile();
			}        
		}

        #endregion

        #region MenuView
        
        private void ViewRefreshSessionTree_Click(object sender, System.EventArgs e)
        {
			ActionRefreshSessionTree();
        }
        #endregion

        #region MenuEmulator

        private void EmulatorStatusPrintSCPEmulator_Click(object sender, System.EventArgs e)
        {
            //To Do after the printer status code is moved.
            PrintEmulatorStatusForm thePrintEmulatorStatusForm = new PrintEmulatorStatusForm((DvtkApplicationLayer.EmulatorSession)GetSelectedSessionNew());
            thePrintEmulatorStatusForm.ShowDialog(this);
        }

        #endregion

        #region MenuWindow
        private void WindowNewProjectView_Click(object sender, System.EventArgs e)
        {
            ProjectForm2 theChildProjectForm = new ProjectForm2(projectApp, this);

            theChildProjectForm.MdiParent = this;
			theChildProjectForm.Show ();
            theChildProjectForm.Activate();

			// Make the project form update itself completely.
			UpdateAll theUpdateAll = new UpdateAll();
			theChildProjectForm.Update(this, theUpdateAll);

			this.UpdateUIControls ();
        }

        private void WindowNewProjectViewAndTile_Click(object sender, System.EventArgs e)
        {
            ProjectForm2 theChildProjectForm = new ProjectForm2(projectApp, this);

            theChildProjectForm.MdiParent = this;

            // Note: Show is not robust enough for MDI childs with window state maximized.
            // Workaround: Forcing all existing child windows to window state normal before show.
            foreach (object theObject in this.MdiChildren)
            {
                ProjectForm2 theProjectForm2 = theObject as ProjectForm2;
                if (theProjectForm2 != null)
                {
                    theProjectForm2.WindowState = FormWindowState.Normal;
                }
            }

			theChildProjectForm.Show ();
            theChildProjectForm.Activate();

			this.LayoutMdi (MdiLayout.TileHorizontal);

			// Make the project form update itself completely.
			UpdateAll theUpdateAll = new UpdateAll();
			theChildProjectForm.Update(this, theUpdateAll);

            this.UpdateUIControls ();
        }

        private void WindowTileViews_Click(object sender, System.EventArgs e)
        {
            this.LayoutMdi (MdiLayout.TileHorizontal);
        }

        private void WindowCascadeViews_Click(object sender, System.EventArgs e)
        {
            this.LayoutMdi (MdiLayout.Cascade);
        }

        #endregion

		private DvtkApplicationLayer.Project projectApp = new DvtkApplicationLayer.Project();

        private void MainForm_MdiChildActivate(object sender, System.EventArgs e)
        {
            this.UpdateUIControls ();
        }

        #region CommonMenuToolBarActions
        private void ActionNew ()
        {
			bool hasUserCancelledOperation = false;
            WizardNew   wizard;
			// If unsaved changes exist for the project and/or sessions, ask the user
			// whether to save them.
			if (projectApp.AreProjectOrSessionsChanged())
			{
				// The user decides whether or what to save.
				Save(true);
				hasUserCancelledOperation = projectApp.HasUserCancelledLastOperation();
			}

			if (!hasUserCancelledOperation)
			{
				wizard = new WizardNew (WizardNew.StartPage.project);
				wizard.Text = "New Project Wizard";

				wizard.ShowDialog(this);

				if (wizard.has_created_project)
				{
					ProjectForm2 child_project;					

					// If a project is loaded, close the project and close all project forms
					if (projectApp.IsProjectConstructed)
					{
						// The user already has indicated which unsaved changes needed to be saved,
						// so don't ask a second time.
						projectApp.Close(false);

						CloseAllProjectForms();
					}

					wizard.ConstructAndSaveProject(projectApp);

					child_project = new ProjectForm2 (projectApp, this);
					child_project.MdiParent = this;

					child_project.Show ();
					child_project.Activate();

					// Make the project form update itself completely.
					child_project.Update(this, new UpdateAll());
				}
				else
				{
					// No project is created using the wizard, but
					// changed project and/or changes sessions may have been saved.
					Notify(this, new Saved());
				}

				UpdateUIControls ();
			}
        }

        private void CallBackMessageDisplay(string message)
        {
            MessageBox.Show (this, message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void ActionOpenProjectOrSession ()
        {
			bool hasUserCancelledOperation = false;
            isUserSelectedNo = false;

			// If unsaved changes exist for the project and/or sessions, ask the user
			// whether to save them.
			if ((projectApp.IsProjectConstructed) && (projectApp.AreProjectOrSessionsChanged()))
			{
				Save(true);
				hasUserCancelledOperation = projectApp.HasUserCancelledLastOperation();
			}

			if (!hasUserCancelledOperation)
			{
				if (this.DialogOpenProject.ShowDialog (this) == DialogResult.OK)
				{
					Cursor.Current = Cursors.WaitCursor;
					ProjectForm2 child_project;

					if (projectApp.IsProjectConstructed)
					{
						// User already decided what to save.
						//if(projectApp.tempButtonNo)
						CloseAllProjectForms();
					}

					FileInfo file = new FileInfo(this.DialogOpenProject.FileName);

					try
					{
						if (file.Extension == ".pdvt")
						{
							projectApp.display_message = new DvtkApplicationLayer.Project.CallBackMessageDisplay (this.CallBackMessageDisplay);
                            if (projectApp.Load(this.DialogOpenProject.FileName))
                            {
                                if (menuItemProj1.Text == "")
                                    menuItemProj1.Text = this.DialogOpenProject.FileName;

                                foreach (Session session in projectApp.Sessions)
                                {
                                    if (session is MediaSession)
                                    {
                                        MediaSession mediaSession = (MediaSession)session;
                                        mediaSession.CreateMediaFiles();
                                    }
                                    else if (session is EmulatorSession)
                                    {
                                        EmulatorSession emulatorSession = (EmulatorSession)session;
                                        emulatorSession.CreateEmulatorFiles();
                                    }
                                    else
                                    {
                                        ScriptSession scriptSession = (ScriptSession)session;
                                        scriptSession.CreateScriptFiles();
                                    }
                                }
                            }
						}

						if (file.Extension == ".ses")
						{
							projectApp.display_message = new DvtkApplicationLayer.Project.CallBackMessageDisplay (this.CallBackMessageDisplay);
							projectApp.New(this.DialogOpenProject.FileName.Replace (".ses", ".pdvt"));
							projectApp.AddSession (this.DialogOpenProject.FileName);
						}

                        if (projectApp.IsProjectConstructed)
                        {
                            child_project = new ProjectForm2(projectApp, this);
                            child_project.MdiParent = this;

                            child_project.Show();
                            child_project.Activate();

                            // Make the project form update itself completely.
                            child_project.Update(this, new UpdateAll());
                        }
					}
					catch( Exception ex)
					{
						MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return;
					}
				}
				else
				{
					// No project or session is opened, but
					// changed project and/or changes sessions may have been saved.
					Notify(this, new Saved());
				}

				this.UpdateUIControls ();
				Cursor.Current = Cursors.Default;
			}
        }

        private void ActionSaveChangedFile ()
        {
			// The user decides what to save.
            Save(false);

			Notify(this, new Saved());

            this.UpdateUIControls ();
        }

        private void ActionProjectSaveAs ()
        {
            FileInfo project_file = new FileInfo (projectApp.ProjectFileName);
            this.ProjectSaveAsForm.InitialDirectory = project_file.DirectoryName;
            this.ProjectSaveAsForm.FileName = project_file.Name;
            if (this.ProjectSaveAsForm.ShowDialog () == DialogResult.OK)
            {
                projectApp.SaveProject(this.ProjectSaveAsForm.FileName);

                this.UpdateUIControls ();
            }
        }

        private void ActionStopRunningProcess ()
        {
			Notify(this, new StopExecution(GetActiveProjectForm()));
        }

		public void ActionNavigateBack ()
		{	
			ProjectForm2 theActiveProjectForm = (ProjectForm2) this.ActiveMdiChild;
			if(theActiveProjectForm.GetActiveTab()== ProjectForm2.ProjectFormActiveTab.SCRIPT_TAB)
			{
				GetActiveProjectForm().getWebBrowserScript().GoBack();
			}
			else if (theActiveProjectForm.GetActiveTab()== ProjectForm2.ProjectFormActiveTab.RESULTS_MANAGER_TAB)
			{
				GetActiveProjectForm().getValidationResultsManagerBrowser().GoBack();
			}
			else 
			{
                GetActiveProjectForm().TCM_GetValidationResultsManager().Back();
			}
		}

		public void ActionNavigateForward ()
		{	
			ProjectForm2 theActiveProjectForm = (ProjectForm2) this.ActiveMdiChild;
			if(theActiveProjectForm.GetActiveTab()== ProjectForm2.ProjectFormActiveTab.SCRIPT_TAB)
			{
				GetActiveProjectForm().getWebBrowserScript().GoForward();
			}
			else if (theActiveProjectForm.GetActiveTab()== ProjectForm2.ProjectFormActiveTab.RESULTS_MANAGER_TAB)
			{
				GetActiveProjectForm().getValidationResultsManagerBrowser().GoForward();
			}
			else
			{
                GetActiveProjectForm().TCM_GetValidationResultsManager().Forward();
			}
		}

        public void ActionFind ()
        {
            _FindForm.ShowDialog(this);

			UpdateUIControls();
        }

        public void ActionFindAgain ()
        {
            GetActiveProjectForm().TCM_GetValidationResultsManager().FindNextText(_FindForm.search_string, _FindForm.search_match_whole_word, _FindForm.search_match_case);
        }

        private void ActionFindNextError ()
        {
			GetActiveProjectForm().TCM_GetValidationResultsManager().FindNextError();
        }

        private void ActionFindNextWarning ()
        {
			GetActiveProjectForm().TCM_GetValidationResultsManager().FindNextWarning();
        }

        public void ActionEditCopy ()
        {
            GetActiveProjectForm().TCM_CopySelectedTextToClipboard();
        }

		public void ActionFilteredResults ()
		{
			GetActiveProjectForm().FilteringResults();
		}
		
        public void ActionRefreshSessionTree()
        {
			Cursor.Current = Cursors.WaitCursor;

			ClearAll theClearAllEvent = new ClearAll();
			theClearAllEvent._StoreSessionTreeState = true;
			Notify(this, theClearAllEvent);

			UpdateAll theUpdateAllEvent = new UpdateAll();
			theUpdateAllEvent._RestoreSessionTreeState = true;
			Notify(this, theUpdateAllEvent);

			Cursor.Current = Cursors.Default;
			ActionRefreshResults ();
        }

		public void ActionRefreshResults()
		{
			if(GetActiveProjectForm().TabControl.SelectedTab == GetActiveProjectForm().TabResultsManager)
			{
				GetActiveProjectForm().TCM_UpdateTabResultsManager();
			}
			if (GetActiveProjectForm().TabControl.SelectedTab == GetActiveProjectForm().TabValidationResults)
			{
				GetActiveProjectForm().FilteringResults();
			}
			if (this.ToolBarFilterResults.Pushed)
			{
				this.MainStatusBar.Text = "Html Content is up-to-date" ;
			}
			else if (!this.ToolBarFilterResults.Pushed)
			{
				this.MainStatusBar.Text = "";
			}
			else 
			{
			}
		}

		private void RefreshResults ()
		{
			GetActiveProjectForm().TCM_UpdateTabResultsManager();
			GetActiveProjectForm().FilteringResults();
			if (this.ToolBarFilterResults.Pushed)
			{
				this.MainStatusBar.Text = "Html Content is up-to-date" ;
			}
			else if (!this.ToolBarFilterResults.Pushed)
			{
				this.MainStatusBar.Text = "";
			}
			else 
			{
			}
		}

        #endregion

        /// <summary>
        /// Update the title bar of the main form
        /// </summary>
        private void UpdateTitleBarText ()
        {			
			string theNewText = "Dicom Validation Tool - ";

			if (!projectApp.IsProjectConstructed)
			{
				theNewText+= "<No project loaded>";
			}
			else
			{
				theNewText+= projectApp.ProjectFileName;

				if (projectApp.AreProjectOrSessionsChanged())
				{
					theNewText += "*";
				}
			}

			Text = theNewText;			
         }

		private void MainForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			//e.Cancel = false;
			projectApp.tempButtonCancel = false ;
			projectApp.tempButtonNo = false ;
			if (projectApp.IsProjectConstructed)
			{
				if (IsExecuting())
				{
					DialogResult theDialogResult;

					// Ask the user if execution needs to be stopped.
					theDialogResult = MessageBox.Show("Executing is still going on.\nStop execution?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

					if (theDialogResult == DialogResult.Yes)
					{
						StopAllExecution();

						// Find out if execution really has stopped.
						if (IsExecuting())
						{
							theDialogResult = MessageBox.Show("Failed to stop all execution.\nGo ahead and close the application(may result in incomplete written results files)?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);	

							if (theDialogResult == DialogResult.No)
							{
								// Cancel the closing of the main form.
								e.Cancel = true;
							}
						}
					}
					else
					{
						// Cancel the closing of the main form.
						e.Cancel = true;
					}
				}

				if (!e.Cancel)
				{
					// Close the project.
					// If unsaved changes exist, give the user the possibility to save them.
                    if (projectApp.AreProjectOrSessionsChanged()) {
                        Save(true);
                    }
					projectApp.Close(true);

					if (projectApp.HasUserCancelledLastOperation())
						// The user has cancelled the close operation.
					{
						// Cancel the closing of the main form.
						e.Cancel = true;
					}
					else
						// The user has not cancelled the close operation.
					{
						Notify(this, new ProjectClosed());
					}
				}
			}

			if (e.Cancel)
			{
				_IsClosing = false;
			}
			else
			{
				if (_UserSettings.DoUnsavedChangesExist)
				{
					_UserSettings.Save();
				}
                _IsClosing = true;
			}
		}

        private void HelpAboutDVT_Click(object sender, System.EventArgs e)
        {
            DvtkApplicationLayer.UserInterfaces.AboutForm   about = new DvtkApplicationLayer.UserInterfaces.AboutForm("Dicom Validation Tool");
			about.InfoToDisplay = "DVT uses OpenSLL for secure communication\n"+
									"- see DVT User Guide for license details.\n";
            about.ShowDialog ();
        }

		/// <summary>
		/// Get the active project form.
		/// </summary>
		/// <returns>The active project form. If not existing, null is returned.</returns>
		public ProjectForm2 GetActiveProjectForm()
		{
			ProjectForm2 theActiveProjectForm = null;

			if (this.ActiveMdiChild is ProjectForm2)
			{
				theActiveProjectForm = (ProjectForm2) this.ActiveMdiChild;
			}
			
			return theActiveProjectForm;
		}

        /// <summary>
        /// Get the selected session of the active project form.
        /// </summary>
        /// <returns>The selected session. If not existing, null is returned.</returns>
        private DvtkApplicationLayer.Session GetSelectedSessionNew() 
        {
            DvtkApplicationLayer.Session theSelectedSession = null;

            if (GetActiveProjectForm() != null) 
            {
                if (GetActiveProjectForm().GetSelectedSessionNew() != null) 
                {
                    theSelectedSession = GetActiveProjectForm().GetSelectedSessionNew();
                }
            }

            return theSelectedSession;
        }

		/// <summary>
		/// Get the selected tree node tag of the active project form.
		/// </summary>
		/// <returns>Selected tree node tag. If not existing, null is returned.</returns>
		public object GetSelectedTag()
		{
			Object theSelectedTreeNodeTag = null;

			if (GetActiveProjectForm() != null)
			{
				if (GetActiveProjectForm().GetSelectedTag() != null)
				{
					theSelectedTreeNodeTag = GetActiveProjectForm().GetSelectedTag();
				}
			}

			return theSelectedTreeNodeTag;
		}

		private void SaveSelectedSession()
		{
			if (GetSelectedSessionNew() == null)
			{
				// Sanity check.
				System.Diagnostics.Debug.Assert(false, "No selected session.");
			}
			else
			{   Session tempSession = GetSelectedSessionNew();
				tempSession.SaveSession(tempSession.SessionFileName);
				Notify(this, new Saved());
				UpdateUIControls();
			}
		}

        public void RemoveSelectedSession() 
        {
            if (GetSelectedSessionNew() == null) 
            {
                // Sanity check.
                System.Diagnostics.Debug.Assert(false, "No selected session.");
            }
            else 
            {
                string theWarningText = string.Format("Are you sure you want to remove the session\n\n{0}\n\nfrom the project?", GetSelectedSessionNew().SessionFileName);

                if (MessageBox.Show (this,
                    theWarningText,
                    "Remove session from project?",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2) == DialogResult.Yes) 
                {
                    bool hasUserCancelledOperation = false;

                    if ((GetSelectedSessionNew()).GetSessionChanged (GetSelectedSessionNew())) 
                    {
                        DialogResult theDialogResult = MessageBox.Show("The session has has unsaved changes.\n\nDo you want to save the changes?", "Unsaved changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);

                        if (theDialogResult == DialogResult.Yes) 
                        {
                            (GetSelectedSessionNew()).SaveSession(GetSelectedSessionNew().SessionFileName);
                        }
                        else if (theDialogResult == DialogResult.No) 
                        {
                            // Do nothing, go ahead with removing it.
                        }
                        else 
                        {
                            hasUserCancelledOperation = true;
                        }
                    }
					
                    if (!hasUserCancelledOperation) 
                    {
                        DvtkApplicationLayer.Session tempSession = GetSelectedSessionNew();
                        tempSession.RemoveSession(tempSession);

                        Notify(this, new SessionRemovedEvent(tempSession));
                        UpdateUIControls ();
                    }
                }
            }
        }

		public void AddNewSession()
		{
			WizardNew theWizardNew;
        
			theWizardNew = new WizardNew(WizardNew.StartPage.session);

			theWizardNew.ShowDialog(this);
			if (theWizardNew.has_created_session)
			{
				projectApp.AddSession (theWizardNew.GetSession());
				Notify(this, new UpdateAll());
				UpdateUIControls ();
			}
		}

		public void AddExistingSessions()
		{
			// TODO: Set initial directory.
			// this.DialogOpenSessionFile.InitialDirectory;
			try 
			{
				System.Windows.Forms.OpenFileDialog theOpenFileDialog = new System.Windows.Forms.OpenFileDialog();

				theOpenFileDialog.Filter = "Session files (*.ses) |*.ses";
				theOpenFileDialog.Multiselect = true;
				theOpenFileDialog.Title = "Select Session File(s)";

				if (theOpenFileDialog.ShowDialog () == DialogResult.OK)
				{
					ArrayList skipped_files = new ArrayList ();
					foreach (object file in theOpenFileDialog.FileNames)
					{
						if (projectApp.ContainsSession (file.ToString()))
							skipped_files.Add (file);
						else
						{
							projectApp.AddSession (file.ToString());
						}
					}

					if (skipped_files.Count > 0)
					{
						string text = "Skipped the following session files because they already exist:";
						foreach (object file in skipped_files)
							text += "\n" + file.ToString();
						MessageBox.Show (this, text, "Skipped files", MessageBoxButtons.OK, MessageBoxIcon.Information);
					}

					Notify(this, new UpdateAll());
					UpdateUIControls ();
				}
			}
			catch(Exception e)
			{
				MessageBox.Show (e.Message,
					"Error",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error);
			}
		}

		public bool IsExecuting()
		{
			bool isExecuting = false;

			foreach(ProjectForm2 theProjectForm in MdiChildren)
			{
				if (theProjectForm.IsExecuting())
				{
					isExecuting = true;
				}
			}

			return(isExecuting);
		}

		private void MenuItem_FileSessionSaveAs_Click(object sender, System.EventArgs e)
		{
			DvtkApplicationLayer.Session theCurrentSession = GetSelectedSessionNew();
			DvtkApplicationLayer.Session theNewSession = theCurrentSession.SaveSessionAs(theCurrentSession);

			if (theNewSession != null)
			{
				Notify(this, new SessionReplaced(theCurrentSession, theNewSession));
			}				
		}

		private void MenuItem_FileProjectSave_Click_1(object sender, System.EventArgs e)
		{
			projectApp.SaveProject();

			UpdateUIControls();
		}

		private void MenuItem_FileProjectAddNewSession_Click(object sender, System.EventArgs e)
		{
			AddNewSession();
		}

		private void MenuItem_FileProjectAddExistingSession_Click(object sender, System.EventArgs e)
		{
			AddExistingSessions();
		}

		private void MenuItem_FileSave_Click(object sender, System.EventArgs e)
		{
			this.ActionSaveChangedFile ();				
		}

		private void MenuItem_FileProjectSaveAs_Click(object sender, System.EventArgs e)
		{
			this.ActionProjectSaveAs ();		
		}

		private void MenuItem_FileProjectSave_Click(object sender, System.EventArgs e)
		{
			this.ActionSaveChangedFile ();		
		}

		/// <summary>
		/// Stop all execution.
		/// </summary>
		public void StopAllExecution()
		{	
			Notify(this, new StopAllExecution());
			TimerMessageForm theTimerMessageForm = new TimerMessageForm();

			theTimerMessageForm.ShowDialogSpecifiedTime("Stopping execution...", 3000);
		}

		/// <summary>
		/// Get the sessions that are currently executing.
		/// </summary>
		/// <returns>Sessions that are currently executing.</returns>
		public ArrayList GetExecutingSessions()
		{
			ArrayList theExecutingSessions = new ArrayList();

			// Update all project forms.
			for (int theIndex = MdiChildren.GetLowerBound(0); theIndex <= MdiChildren.GetUpperBound(0); theIndex++)
			{
				ProjectForm2 theProjectForm2 = MdiChildren.GetValue(theIndex) as ProjectForm2;

				if (theProjectForm2 != null)
				{
					DvtkApplicationLayer.Session theExecutingSession = theProjectForm2.GetExecutingSession();

					if (theExecutingSession != null)
					{
						theExecutingSessions.Add(theExecutingSession);
					}
				}
			}

			return(theExecutingSessions);
		}

		public bool IsExecuting(DvtkApplicationLayer.Session theSession)
		{
			ArrayList theExecutingSessions = GetExecutingSessions();

			return(theExecutingSessions.Contains(theSession));
		}

		private void CloseAllProjectForms()
		{
			while (this.MdiChildren.Length > 0)
			{
				ProjectForm2 theProjectForm = (ProjectForm2)MdiChildren[0];
				theProjectForm.Close();
                isUserSelectedNo = false;
			}
		}

		private void ViewAskForBackupResultsFile_Click(object sender, System.EventArgs e)
		{
			ViewAskForBackupResultsFile.Checked = !ViewAskForBackupResultsFile.Checked;
			_UserSettings.AskForBackupResultsFile = ViewAskForBackupResultsFile.Checked;			
		}

		private void ViewShowEmptySessions_Click(object sender, System.EventArgs e)
		{
			ViewShowEmptySessions.Checked = !ViewShowEmptySessions.Checked;
			_UserSettings.ShowEmptySessions = ViewShowEmptySessions.Checked;

			ActionRefreshSessionTree();
		}

		private void ViewShowDsFiles_Click(object sender, System.EventArgs e)
		{
			ViewShowDicomScripts.Checked = !ViewShowDicomScripts.Checked;
			_UserSettings.ShowDicomScripts = ViewShowDicomScripts.Checked;

			ActionRefreshSessionTree();
		}

		private void ViewShowDssFiles_Click(object sender, System.EventArgs e)
		{
			ViewShowDicomSuperScripts.Checked = !ViewShowDicomSuperScripts.Checked;
			_UserSettings.ShowDicomSuperScripts = ViewShowDicomSuperScripts.Checked;

			ActionRefreshSessionTree();
		}

		private void ViewShowVbsFiles_Click(object sender, System.EventArgs e)
		{
			ViewShowVisualBasicScripts.Checked = !ViewShowVisualBasicScripts.Checked;
			_UserSettings.ShowVisualBasicScripts = ViewShowVisualBasicScripts.Checked;

			ActionRefreshSessionTree();
		}

		private void menuItem2_Click(object sender, System.EventArgs e)
		{
			string userGuideFullFileName = "";

			try
			{
				userGuideFullFileName = Path.Combine(Application.StartupPath, @"..\docs\DVT_userguide.pdf");

				System.Diagnostics.Process theProcess  = new System.Diagnostics.Process();

				theProcess.StartInfo.FileName= userGuideFullFileName;

				theProcess.Start();
			}
			catch
			{
				MessageBox.Show("Unable to show " + userGuideFullFileName, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		private void HelpDicomScript_Click(object sender, System.EventArgs e)
		{
			string dicomScriptHelpFullFileName = "";

			try
			{
				dicomScriptHelpFullFileName = Path.Combine(Application.StartupPath, @"..\docs\DVTDICOMSCRIPT.HLP");

				System.Diagnostics.Process theProcess  = new System.Diagnostics.Process();

				theProcess.StartInfo.FileName= dicomScriptHelpFullFileName;

				theProcess.Start();
			}
			catch
			{
				MessageBox.Show("Unable to show " + dicomScriptHelpFullFileName, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}		
		}

		private void ViewExpandVisualBasicScript_Click(object sender, System.EventArgs e)
		{
			ViewExpandVisualBasicScript.Checked = !ViewExpandVisualBasicScript.Checked;
			_UserSettings.ExpandVisualBasicScript = ViewExpandVisualBasicScript.Checked;

			ActionRefreshSessionTree();				
		}

        public void Save(bool showSaveUnsavedChangesQuestion) 
		{
            Session tempSession = null ;
            if (!projectApp.AreProjectOrSessionsChanged()) 
			{
                // Sanity check.
                // The UI should not call this method when nothing has changed.
                Debug.Assert (false);
            }
            else 
			{
                if(!isUserSelectedNo)
                {
                    projectApp.HasCancelledLastOperation = false;

                    if (showSaveUnsavedChangesQuestion) 
				    {
                        DialogResult theDialogResult = ShowSaveUnsavedChangesQuestion();

					    if (theDialogResult == DialogResult.Yes) 
					    {
                            isUserSelectedNo = false;
					    }
					    else if (theDialogResult == DialogResult.No) 
					    {
                            isUserSelectedNo = true;
						    projectApp.tempButtonNo = true ;
						    projectApp.tempButtonCancel = false ;
					    }
					    else 
					    {
						    projectApp.HasCancelledLastOperation = true;
						    projectApp.tempButtonCancel = true ;
						    projectApp.tempButtonNo = false ;
                            return;
					    }
                    }

                    if ((!showSaveUnsavedChangesQuestion) ||
                        (showSaveUnsavedChangesQuestion && (!isUserSelectedNo)))
                    {
                        SaveForm theSaveForm = new SaveForm();

                        // Fill the save form with all files that have been changed.
                        if (projectApp.HasProjectChanged)
                        {
                            theSaveForm.AddChangedItem(projectApp.ProjectFileName);
                        }

                        for (int i = 0; i < projectApp.Sessions.Count; i++)
                        {
                            tempSession = (Session)projectApp.Sessions[i];
                            if (tempSession.GetSessionChanged(tempSession.GetSession(i)))
                            {
                                theSaveForm.AddChangedItem(tempSession.GetSession(i).SessionFileName);
                            }
                        }

                        // Show the save form to the user.
                        DialogResult theDialogResult = theSaveForm.ShowDialog();

                        if (theDialogResult == DialogResult.Cancel)
                        {
                            // User pressed cancel.
                            projectApp.HasCancelledLastOperation = true;
                            return;
                        }
                        else
                        {
                            // User did not press cancel.
                            foreach (string theFileName in theSaveForm.ItemsToSave)
                            {
                                if (theFileName == projectApp.ProjectFileName)
                                {
                                    projectApp.SaveProject();
                                }
                                else
                                {
                                    tempSession.SaveSession(theFileName);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Show a dialog in which the user can determine to save (some) changes, save no changes or cancel
        /// the operation.
        /// 
        /// Precondition: unsaved changes exist.
        /// </summary>
        /// <returns>Boolean indicating if the operation should continue.</returns>
        private DialogResult ShowSaveUnsavedChangesQuestion() 
		{
            DialogResult theDialogResult = DialogResult.OK;
            string whatHasChangedText = "";
            int theNumberOfChangesSessions = projectApp.NosOfSessionsChanged;

            if (projectApp.HasProjectChanged) 
			{
                if (theNumberOfChangesSessions > 0) 
				{
                    if (theNumberOfChangesSessions == 1) 
					{
                        whatHasChangedText = "The project and one session have unsaved changes.";
                    }
                    else {
                        whatHasChangedText = string.Format("The project and {0} sessions have unsaved changes.", theNumberOfChangesSessions);
                    }
                }
                else 
				{
                    whatHasChangedText = "The Project has unsaved changes.";
                }
            }
            else 
			{
                if (theNumberOfChangesSessions > 0) 
				{
                    if (theNumberOfChangesSessions == 1) 
					{
                        whatHasChangedText = "One session has unsaved changes.";
                    }
                    else 
					{
                        whatHasChangedText = string.Format("{0} sessions have unsaved changes.", theNumberOfChangesSessions);
                    }
                }
                else 
				{
                    // Sanity check.
                    theDialogResult = DialogResult.None;
                    Debug.Assert(false);
                    return(theDialogResult);
                }
            }

            theDialogResult = MessageBox.Show(whatHasChangedText + "\n\n" + "Do you want to save the changes?", "Unsaved changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);

            return(theDialogResult);
        }
	
		private void GenerateReport_Click(object sender, System.EventArgs e)
		{
			ProjectForm2 theChildProjectForm = new ProjectForm2(projectApp, this);
			theChildProjectForm.GenerateFilterReport();
		}

		private void MenuReport_Click(object sender, System.EventArgs e)
		{
			this.GenerateReport.Enabled = true;		
		}

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_IsClosing)
            {
                e.Cancel = true;
            }
            else
            {
                CloseAllProjectForms();
                e.Cancel = false;
            }
        }

              
	}
}
