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

using DvtkApplicationLayer;
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Dvt
{
    /// <summary>
    /// Summary description for UserControlSessionTree.
    /// </summary>
    public class UserControlSessionTree : UserControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        private TreeView userControlTreeView;
        private string _FirstMediaFileToValidate = "";
        private bool reloadHtml = true;

        //Boolean indicating whether the Media Session is validating a Media Directory
        private bool _ISMediaDirectoryValidation = false;
        private ArrayList listOfFileNames = new ArrayList();
        public Project projectApp;
        private int _UpdateCount = 0;
        private object _TagThatIsBeingExecuted = null;
        private EndExecution _EndExecution = null;
        private MainForm _MainForm = null;
        private NotifyDelegate _NotifyDelegate;
        private NodesInformation nodeInformation = new NodesInformation();
        private StorageSCUEmulatorForm _StorageSCUEmulatorForm;
        private AsyncCallback _StorageSCUEmulatorFormAsyncCallback;
        private Thread _ScriptThread = null;

        private delegate void NotifyDelegate(object theEvent);
        private Queue mediaFilesToBeValidated = Queue.Synchronized(new Queue());


        public ProjectForm2 ProjectForm { get; set; }


        public UserControlSessionTree()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            _StorageSCUEmulatorFormAsyncCallback = new AsyncCallback(ResultsFromExecutingEmulatorStorageScuAsynchronously);
            _StorageSCUEmulatorForm = new StorageSCUEmulatorForm(_StorageSCUEmulatorFormAsyncCallback);
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


        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            userControlTreeView = new TreeView();
            SuspendLayout();
            // 
            // userControlTreeView
            // 
            userControlTreeView.Dock = DockStyle.Fill;
            userControlTreeView.HideSelection = false;
            userControlTreeView.ImageIndex = -1;
            userControlTreeView.Location = new System.Drawing.Point(0, 0);
            userControlTreeView.Name = "userControlTreeView";
            userControlTreeView.SelectedImageIndex = -1;
            userControlTreeView.Size = new System.Drawing.Size(150, 150);
            userControlTreeView.TabIndex = 0;
            userControlTreeView.MouseDown += new MouseEventHandler(userControlTreeView_MouseDown);
            userControlTreeView.DoubleClick += new EventHandler(userControlTreeView_DoubleClick);
            userControlTreeView.AfterSelect += new TreeViewEventHandler(userControlTreeView_AfterSelect);
            userControlTreeView.BeforeSelect += new TreeViewCancelEventHandler(userControlTreeView_BeforeSelect);
            // 
            // UserControlSessionTree
            // 
            Controls.Add(userControlTreeView);
            Name = "UserControlSessionTree";
            ResumeLayout(false);

        }
        #endregion


        public void UpdateEmulatorNode(TreeNode emulatorTreeNode, Emulator emulator)
        {
            emulatorTreeNode.Text = emulator.EmulatorName;
            if (emulator.ParentSession.IsExecute)
            {
                emulatorTreeNode.Text += " (executing)";
            }

            // Set the tag for this media file tree node.
            emulatorTreeNode.Tag = emulator;

            // Remove the old tree nodes that may be present under this script file tree node.
            emulatorTreeNode.Nodes.Clear();
            // For all results that belong to this media file, create a sub node.
            // The theResultsFiles object contains all results files that have not yet been added to
            // already processed script file nodes.
            // Set the text on this session tree node.
            foreach (Result results in emulator.Result)
            {
                foreach (string filename in results.ResultFiles)
                {
                    TreeNode resultsFileTreeNode = new TreeNode();
                    emulatorTreeNode.Nodes.Add(resultsFileTreeNode);
                    UpdateResultsFileNode(resultsFileTreeNode, results, filename);
                }
            }
        }


        public void UpdateEmulatorSessionNode(TreeNode emulatorSessionTreeNode, EmulatorSession emulatorSession)
        {
            bool isSessionExecuting = emulatorSession.IsExecute;
            bool isSessionExecutingInOtherSessionTreeView = (isSessionExecuting && (emulatorSession != GetExecutingSession()));
            // Set the tag for this session tree node.
            emulatorSessionTreeNode.Tag = emulatorSession;
            // Remove the old tree nodes that may be present under this session tree node.
            emulatorSessionTreeNode.Nodes.Clear();
            emulatorSession.CreateEmulatorFiles();
            if (isSessionExecutingInOtherSessionTreeView)
            {
                // Do nothing.
            }
            else if (!isSessionExecuting)
            {
                foreach (Emulator emulator in emulatorSession.Emulators)
                {
                    if (emulator.EmulatorName == "Pr_Scp_Em")
                    {
                        TreeNode printScpEmulatorTreeNode = new TreeNode();
                        emulatorSessionTreeNode.Nodes.Add(printScpEmulatorTreeNode);
                        UpdateEmulatorNode(printScpEmulatorTreeNode, emulator);
                    }
                    else if (emulator.EmulatorName == "St_Scp_Em")
                    {
                        // Add the Storage SCP emulator tree node.
                        TreeNode storageScpEmulatorTreeNode = new TreeNode();
                        emulatorSessionTreeNode.Nodes.Add(storageScpEmulatorTreeNode);
                        UpdateEmulatorNode(storageScpEmulatorTreeNode, emulator);

                    }
                    else
                    {
                        // Add the Storage SCU emulator tree node.
                        TreeNode storageScuEmulatorTreeNode = new TreeNode();
                        emulatorSessionTreeNode.Nodes.Add(storageScuEmulatorTreeNode);
                        UpdateEmulatorNode(storageScuEmulatorTreeNode, emulator);
                    }
                }
            }
            else
            {
                // Sanity check, pre-condition of this method is not fullfilled.
                Debug.Assert(false);
            }
        }


        public void UpdateMediaSessionNode(TreeNode mediaSessionTreeNode, MediaSession mediaSession)
        {
            bool isSessionExecuting = mediaSession.IsExecute;
            // Set the tag for this session tree node.
            mediaSessionTreeNode.Tag = mediaSession;
            // Remove the old tree nodes that may be present under this session tree node.
            mediaSessionTreeNode.Nodes.Clear();
            mediaSession.CreateMediaFiles();
            if (!isSessionExecuting)
            {
                if (mediaSession.MediaFiles != null)
                {
                    foreach (MediaFile mediaFile in mediaSession.MediaFiles)
                    {
                        TreeNode mediaTreeNode = new TreeNode();
                        mediaSessionTreeNode.Nodes.Add(mediaTreeNode);
                        UpdateMediaFileNode(mediaTreeNode, mediaFile);
                    }
                }
            }
        }


        public void UpdateMediaFileNode(TreeNode mediaFileTreeNode, MediaFile mediaFile)
        {
            mediaFileTreeNode.Text = mediaFile.MediaFileName;
            // Set the tag for this media file tree node.
            mediaFileTreeNode.Tag = mediaFile;

            // Remove the old tree nodes that may be present under this script file tree node.
            mediaFileTreeNode.Nodes.Clear();
            // For all results that belong to this media file, create a sub node.
            // The theResultsFiles object contains all results files that have not yet been added to
            // already processed script file nodes.

            foreach (Result results in mediaFile.Result)
            {
                foreach (string filename in results.ResultFiles)
                {
                    TreeNode resultsFileTreeNode = new TreeNode();
                    mediaFileTreeNode.Nodes.Add(resultsFileTreeNode);
                    UpdateResultsFileNode(resultsFileTreeNode, results, filename);
                }
            }
        }


        public void UpdateScriptFileNode(TreeNode scriptFileTreeNode, Script scriptFile)
        {
            // Set the text on this script file tree node.
            if (scriptFile.ParentSession.IsExecute)
            {
                scriptFileTreeNode.Text = scriptFile.ScriptFileName + " (executing)";
            }
            else
            {
                scriptFileTreeNode.Text = scriptFile.ScriptFileName;
            }

            // Set the tag for this script file tree node.
            scriptFileTreeNode.Tag = scriptFile;

            // Remove the old tree nodes that may be present under this script file tree node.
            scriptFileTreeNode.Nodes.Clear();

            // For all results that belong to this script file, create a sub node.
            // The theResultsFiles object contains all results files that have not yet been added to
            // already processed script file nodes.

            foreach (Result results in scriptFile.Result)
            {
                foreach (string filename in results.ResultFiles)
                {
                    TreeNode resultsFileTreeNode = new TreeNode();
                    scriptFileTreeNode.Nodes.Add(resultsFileTreeNode);
                    UpdateResultsFileNode(resultsFileTreeNode, results, filename);
                }
                foreach (string filename in results.SubDetailResultFiles)
                {
                    TreeNode resultsFileTreeNode = new TreeNode();
                    scriptFileTreeNode.Nodes.Add(resultsFileTreeNode);
                    UpdateResultsFileNode(resultsFileTreeNode, results, filename);
                }
                foreach (string filename in results.SubSummaryResultFiles)
                {
                    TreeNode resultsFileTreeNode = new TreeNode();
                    scriptFileTreeNode.Nodes.Add(resultsFileTreeNode);
                    UpdateResultsFileNode(resultsFileTreeNode, results, filename);
                }
            }
        }


        public void UpdateScriptSessionNode(TreeNode scriptSessionTreeNode, ScriptSession scriptSession, ref bool isEmpty)
        {
            bool isSessionExecuting = scriptSession.IsExecute;
            bool isSessionExecutingInOtherSessionTreeView = (isSessionExecuting && (scriptSession != GetExecutingSession()));
            IList scriptFilesTemp = new ArrayList();

            // Set the tag for this session tree node.			
            scriptSessionTreeNode.Tag = scriptSession;

            // Remove the old tree nodes that may be present under this session tree node.
            scriptSessionTreeNode.Nodes.Clear();
            scriptSession.CreateScriptFiles();

            // If this session is executing...
            if (isSessionExecutingInOtherSessionTreeView)
            {
                // Do nothing.
            }
            else if (!isSessionExecuting)
            {
                // Create a sub-node for each script file contained in this session.
                if (scriptSession.ScriptFiles != null)
                {
                    // Determine the visible scripts.
                    ArrayList visibleScripts = GetVisibleScripts(scriptSession);
                    isEmpty = (visibleScripts.Count == 0);
                    foreach (string scriptName in visibleScripts)
                    {
                        foreach (Script script in scriptSession.ScriptFiles)
                        {
                            if (scriptName == script.ScriptFileName)
                            {
                                scriptFilesTemp.Add(script);
                            }
                        }
                    }

                    foreach (Script script in scriptFilesTemp)
                    {
                        TreeNode scriptNode = new TreeNode();
                        scriptSessionTreeNode.Nodes.Add(scriptNode);
                        UpdateScriptFileNode(scriptNode, script);
                    }
                }
                else
                {
                    TreeNode scriptNode = new TreeNode();
                    scriptNode.Text = "Warning: Session doesn't contain any scripts";
                    scriptSessionTreeNode.Nodes.Add(scriptNode);
                }
            }
            else
            {
                // Sanity check, pre-condition of this method is not fullfilled.
                Debug.Assert(false);
            }
        }


        public void UpdateSessionNode(TreeNode sessionTreeNode, Session session, ref bool isEmpty)
        {
            isEmpty = false;
            UpdateSessionNodeTextMainNodeOnly(sessionTreeNode, session);

            if (session is ScriptSession)
            {
                UpdateScriptSessionNode(sessionTreeNode, session as ScriptSession, ref isEmpty);
            }

            if (session is MediaSession)
            {
                UpdateMediaSessionNode(sessionTreeNode, session as MediaSession);
            }

            if (session is EmulatorSession)
            {
                UpdateEmulatorSessionNode(sessionTreeNode, session as EmulatorSession);
            }
        }


        public void UpdateResultsFileNode(TreeNode resultsFileTreeNode, Result result, string filename)
        {
            // Set the text on this script file tree node.
            resultsFileTreeNode.Text = filename;
            // Set the tag for this result file tree node.
            resultsFileTreeNode.Tag = result;
        }


        public void Update(object theSource, object theEvent)
        {
            if (theEvent is UpdateAll)
            {
                OnUpdateAll(theSource, theEvent);
            }

            if (theEvent is ClearAll)
            {
                OnClearAll(theSource, theEvent);
            }

            if (theEvent is StartExecution)
            {
                OnStartExecution(theSource, theEvent);
            }

            if (theEvent is EndExecution)
            {
                OnEndExecution(theSource, theEvent);
            }

            if ((theEvent is StopExecution) ||
                (theEvent is StopAllExecution)
                )
            {
                OnStopExecution(theSource, theEvent);
            }

            if (theEvent is SessionChange)
            {
                OnSessionChanged(theSource, theEvent);
            }

            if (theEvent is SessionRemovedEvent)
            {
                OnSessionRemovedEvent(theSource, theEvent);
            }

            if (theEvent is SessionReplaced)
            {
                OnSessionReplaced(theSource, theEvent);
            }
            if (theEvent is Saved)
            {
                OnSaved(theSource, theEvent);
            }
        }


        public void UpdateProjectNodeTextMainNodeOnly(TreeNode theTreeNode, Project theProject)
        {
            theTreeNode.Text = Path.GetFileName(theProject.ProjectFileName);
            if (theProject.HasProjectChanged)
            {
                theTreeNode.Text += " *";
            }
        }


        public void UpdateSessionNodeTextMainNodeOnly(TreeNode theTreeNode, Session theSession)
        {
            _MainForm = (MainForm)ProjectForm._MainForm;
            bool isSessionExecuting = _MainForm.IsExecuting(theSession);
            bool isSessionExecutingInOtherSessionTreeView = (isSessionExecuting && (theSession != GetExecutingSession()));

            theTreeNode.Text = Path.GetFileName(theSession.SessionFileName);

            if ((theSession is ScriptSession) ||
                (theSession is EmulatorSession)
                )
            {
                if (isSessionExecutingInOtherSessionTreeView)
                {
                    theTreeNode.Text += " (disabled)";
                }
            }

            if (theSession is MediaSession)
            {
                // If this session is executing...
                if (isSessionExecuting)
                {
                    // If the executing session is executed by this session tree view...
                    if (theSession == GetExecutingSession())
                    {
                        theTreeNode.Text += " (executing)";
                    }
                    // If the executing session is not executed by this session tree view...
                    else
                    {
                        theTreeNode.Text += "(disabled)";
                    }
                }
            }

            if (theSession.GetSessionChanged(theSession))
            {
                theTreeNode.Text += " *";
            }
        }


        private void OnEndExecution(object theSource, object theEvent)
        {
            Session tempSession = null;
            object selectedTag = GetSelectedTag();
            // The tree node that is being executed.
            object theTreeNodeTag = ((EndExecution)theEvent)._Tag;
            TreeNode theSessionTreeNodeToRefresh = null;

            // Refresh the complete session node.
            if (theTreeNodeTag is PartOfSession)
            {
                PartOfSession partOfSession = theTreeNodeTag as PartOfSession;
                tempSession = partOfSession.ParentSession;
            }
            else
            {
                tempSession = (MediaSession)theTreeNodeTag;
            }
            tempSession.IsExecute = false;
            theSessionTreeNodeToRefresh = GetSessionNode(tempSession);

            if (theSessionTreeNodeToRefresh != null)
            {
                bool isEmpty = false;
                UpdateSessionNode(theSessionTreeNodeToRefresh, tempSession, ref isEmpty);
                // Expand back the nodes and sub nodes.
                nodeInformation.RestoreExpandInformation(theSessionTreeNodeToRefresh);
                nodeInformation.RemoveExpandInformationForSession(tempSession);

                if (theSource == ProjectForm)
                {
                    // After execution, select and expand a summary results file.
                    string theCurrentSummaryResultsFileName = "";

                    if (theTreeNodeTag is Script)
                    {
                        object theScriptFileTag = (Script)theTreeNodeTag;
                        theCurrentSummaryResultsFileName = Result.GetSummaryNameForScriptFile(((Script)theScriptFileTag).ParentSession, ((Script)theScriptFileTag).ScriptFileName);
                    }
                    else if (theTreeNodeTag is Emulator)
                    {
                        object theEmulatorTag = (Emulator)theTreeNodeTag;
                        theCurrentSummaryResultsFileName = Result.GetSummaryNameForEmulator(((Emulator)theEmulatorTag).ParentSession, ((Emulator)theEmulatorTag).EmulatorType);
                    }
                    else if (theTreeNodeTag is MediaSession)
                    {
                        object theMediaSessionTag = (MediaSession)theTreeNodeTag;
                        theCurrentSummaryResultsFileName = Result.GetSummaryNameForMediaFile((MediaSession)theMediaSessionTag, _FirstMediaFileToValidate);
                    }
                    else
                    {
                        // Not supposed to get here.
                        Debug.Assert(false);
                    }

                    ExpandAndSelectResultFile(theSessionTreeNodeToRefresh, theCurrentSummaryResultsFileName);

                    userControlTreeView.Enabled = true;
                }

                // If, by calling an Update...Node, the selected tag has changed, 
                // the control tab also needs updating.
                if (selectedTag != GetSelectedTag())
                {
                    NotifySessionTreeViewSelectionChange(GetSelectedUserNode());
                }
            }
            else
            {
                // Sanity check.
                Debug.Assert(false);
            }
        }


        private void OnStopExecution(object theSource, object theEvent)
        {
            if (((theEvent is StopExecution) && (((StopExecution)theEvent)._ProjectForm == ProjectForm)) ||
                (theEvent is StopAllExecution)
                )
            {
                // If execution is going on...
                if (_MainForm.IsExecuting(GetSelectedSessionNew()))
                {
                    ScriptSession theScriptSession = GetSelectedSessionNew() as ScriptSession;
                    EmulatorSession theEmulatorSession = GetSelectedSessionNew() as EmulatorSession;

                    // If the selected node is a script file...
                    if (theScriptSession != null)
                    {
                        theScriptSession.ScriptSessionImplementation.TerminateConnection();

                        theScriptSession.ScriptSessionImplementation.WriteError("stop button pressed.");
                    }

                    // If the selected node is an emulator...
                    if (theEmulatorSession != null)
                    {
                        theEmulatorSession.EmulatorSessionImplementation.TerminateConnection();
                    }
                }
            }
        }

        private void OnSessionRemovedEvent(object theSource, object theEvent)
        {
            SessionRemovedEvent theSessionRemovedEvent = (SessionRemovedEvent)theEvent;
            ClearAll theClearAllEvent = new ClearAll();
            theClearAllEvent._StoreSessionTreeState = true;
            Notify(theClearAllEvent);

            UpdateAll theUpdateAllEvent = new UpdateAll();
            theUpdateAllEvent._RestoreSessionTreeState = true;
            Notify(theUpdateAllEvent);
        }


        private void OnSessionReplaced(object theSource, object theEvent)
        {
            bool isEmpty = false;
            SessionReplaced theSessionReplaced = (SessionReplaced)theEvent;
            TreeNode theSessionNodeToUpdate = GetSessionNode(theSessionReplaced._OldSession);
            UpdateSessionNode(theSessionNodeToUpdate, theSessionReplaced._NewSession, ref isEmpty);
            NotifySessionTreeViewSelectionChange(theSessionNodeToUpdate);
        }


        private void OnSaved(object theSource, object theEvent)
        {
            foreach (TreeNode theProjectNode in userControlTreeView.Nodes)
            {
                if (theProjectNode.Tag is Project)
                {
                    object theTreeNodeProjectTag = (object)theProjectNode.Tag;
                    UpdateProjectNodeTextMainNodeOnly(theProjectNode, (Project)theTreeNodeProjectTag);
                    foreach (TreeNode theSessionNode in theProjectNode.Nodes)
                    {
                        object theTreeNodeTag = (object)theSessionNode.Tag;
                        UpdateSessionNodeTextMainNodeOnly(theSessionNode, (Session)theTreeNodeTag);
                    }
                }
            }
        }


        private void OnStartExecution(object theSource, object theEvent)
        {
            Session tempSession = null;
            object selectedTag = GetSelectedTag();
            string selectedText = GetSelectedUserNode().Text;
            // The tree node that is being executed.
            TreeNode theTreeNode = ((StartExecution)theEvent).TreeNode;

            // The tag of the tree node that is being executed.
            object theTreeNodeTag = theTreeNode.Tag;
            if (theTreeNodeTag is PartOfSession)
            {
                PartOfSession partOfSession = theTreeNodeTag as PartOfSession;
                tempSession = partOfSession.ParentSession;
            }
            else
            {
                tempSession = (MediaSession)theTreeNodeTag;
            }

            // If the session tree view managed by this class has initiated the execution,
            // disable the complete session tree view.
            if (theSource == ProjectForm)
            {
                TreeNode theSessionTreeNode = GetSessionNode(tempSession);

                if (theTreeNodeTag is Script)
                {
                    UpdateScriptFileNode(theTreeNode, (Script)theTreeNodeTag);
                }
                else if (theTreeNodeTag is Emulator)
                {
                    UpdateEmulatorNode(theTreeNode, (Emulator)theTreeNodeTag);
                }
                else if (theTreeNodeTag is MediaSession)
                {
                    UpdateSessionNodeTextMainNodeOnly(theTreeNode, (MediaSession)theTreeNodeTag);
                    UpdateMediaSessionNode(theTreeNode, (MediaSession)theTreeNodeTag);
                }
                else
                {
                    throw new ApplicationException("Unknown tag in OnStartExecution");
                }

                userControlTreeView.Enabled = false;
            }

            // If the session tree view managed by this class has NOT initiated the execution,
            // disable one session.
            else
            {
                TreeNode theSessionTreeNodeToDisable = GetSessionNode(tempSession);

                if (theSessionTreeNodeToDisable != null)
                {
                    nodeInformation.StoreExpandInformation(theSessionTreeNodeToDisable);

                    bool isEmpty = false;
                    UpdateSessionNode(theSessionTreeNodeToDisable, tempSession, ref isEmpty);
                    theSessionTreeNodeToDisable.Collapse();
                }
            }

            // If, by calling an Update...Node, the selected tag has changed, 
            // the control tab also needs updating.
            if ((selectedTag != GetSelectedTag()) || (GetSelectedUserNode().Text != selectedText))
            {
                NotifySessionTreeViewSelectionChange(GetSelectedUserNode());
            }
        }


        private void OnSessionChanged(object theSource, object theEvent)
        {
            SessionChange theSessionChange = (SessionChange)theEvent;

            TreeNode theTreeNode = GetSessionNode(theSessionChange.SessionApp);

            if (theTreeNode != null)
            {
                if ((theSessionChange.SessionChangeSubTyp == SessionChange.SessionChangeSubTypEnum.RESULTS_DIR) ||
                    (theSessionChange.SessionChangeSubTyp == SessionChange.SessionChangeSubTypEnum.SCRIPTS_DIR)
                    )
                {
                    bool isEmpty = false;
                    UpdateSessionNode(theTreeNode, theSessionChange.SessionApp, ref isEmpty);
                }
                else
                {
                    UpdateSessionNodeTextMainNodeOnly(theTreeNode, theSessionChange.SessionApp);
                }
            }
        }


        private void OnUpdateAll(object theSource, object theEvent)
        {
            // Disable drawing of the session tree.
            userControlTreeView.BeginUpdate();

            UpdateAll theUpdateAllEvent = (UpdateAll)theEvent;

            UpdateTreeView();

            if (theUpdateAllEvent._RestoreSessionTreeState)
            {
                foreach (TreeNode theTreeNode in userControlTreeView.Nodes)
                {
                    nodeInformation.RestoreExpandInformation(theTreeNode);
                }

                nodeInformation.RemoveAllExpandInformation();

                nodeInformation.RestoreSelectedNode(userControlTreeView);
            }
            else
            {
                if (userControlTreeView.Nodes.Count > 0)
                {
                    userControlTreeView.SelectedNode = userControlTreeView.Nodes[0];
                }
            }

            // Enable drawing of the session tree.
            userControlTreeView.EndUpdate();

            SessionTreeViewSelectionChange theSessionTreeViewSelectionChange = new SessionTreeViewSelectionChange(GetSelectedUserNode());
            Notify(theSessionTreeViewSelectionChange);
        }


        public void UpdateTreeView()
        {
            userControlTreeView.Nodes.Clear();
            TreeNode projectTreeNode = new TreeNode();
            projectTreeNode.Tag = projectApp;
            projectTreeNode.Text = Path.GetFileName(projectApp.ProjectFileName);
            userControlTreeView.Nodes.Add(projectTreeNode);
            UpdateProjectNodeTextMainNodeOnly(projectTreeNode, projectApp);

            // For each session present in the project, create a session node in the session tree view.
            for (int theIndex = 0; theIndex < projectApp.Sessions.Count; theIndex++)
            {
                bool isEmpty = false;
                Session session = (Session)projectApp.Sessions[theIndex];

                TreeNode theTreeNode = new TreeNode();
                UpdateSessionNode(theTreeNode, session, ref isEmpty);
                if ((_MainForm._UserSettings.ShowEmptySessions) || (!isEmpty))
                {
                    projectTreeNode.Nodes.Add(theTreeNode);
                }
            }
            projectTreeNode.Expand();
        }


        public TreeNode GetSelectedUserNode()
        {
            //TreeNode theprojectNode = userControlTreeView.SelectedNode;
            TreeNode tempNode = null;
            if (userControlTreeView.Nodes.Count > 0)
            {
                TreeNode theprojectNode = userControlTreeView.SelectedNode;
                if (theprojectNode != null)
                {
                    tempNode = theprojectNode;
                }
                else
                {
                    theprojectNode = userControlTreeView.Nodes[0];
                    foreach (TreeNode sessionNode in theprojectNode.Nodes)
                    {
                        if (sessionNode.IsSelected)
                        {
                            tempNode = sessionNode;
                        }
                        else
                        {
                            foreach (TreeNode subNode in sessionNode.Nodes)
                            {
                                if (subNode.IsSelected)
                                {
                                    tempNode = subNode;
                                }
                                else
                                {
                                    foreach (TreeNode resultNode in subNode.Nodes)
                                    {
                                        if (resultNode.IsSelected)
                                        {
                                            tempNode = resultNode;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return tempNode;
        }


        /// <summary>
        /// Get the selected tree node tag.
        /// </summary>
        /// <returns>Selected tree node tag.</returns>
        public object GetSelectedTag()
        {
            object tag = null;

            if (GetSelectedUserNode() != null)
            {
                tag = (object)GetSelectedUserNode().Tag;
            }

            return tag;
        }


        public Session GetSession()
        {
            Session theSelectedSession = null;

            object selectedTag = GetSelectedTag();

            if (selectedTag is Project)
            {
                theSelectedSession = null;
                //theSelectedSession = ((DvtkApplicationLayer.Project)selectedTag).Sessions[0] as DvtkApplicationLayer.Session;
            }
            else
            {
                if (selectedTag is Session)
                {
                    theSelectedSession = selectedTag as Session;
                }
                else if (selectedTag is PartOfSession)
                {
                    PartOfSession partOfSession = selectedTag as PartOfSession;

                    theSelectedSession = partOfSession.ParentSession;
                }
                else
                {
                    Debug.Assert(true, "Error");
                }
            }

            return theSelectedSession;
        }


        public void NotifySessionTreeViewSelectionChange(TreeNode theTreeNode)
        {
            SessionTreeViewSelectionChange theSessionTreeViewSelectionChange;
            theSessionTreeViewSelectionChange = new SessionTreeViewSelectionChange(theTreeNode);
            Notify(theSessionTreeViewSelectionChange);
        }


        public void Notify(object theEvent)
        {
            ProjectForm.Notify(theEvent);
        }


        private void userControlTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (projectApp.Sessions.Count == 0)
            {
                userControlTreeView.Focus();
            }
            else
            {
                if (reloadHtml)
                {
                    if (_UpdateCount == 0)
                    {
                        NotifySessionTreeViewSelectionChange(GetSelectedUserNode());

                        // Make sure the tree node regains the focus.
                        userControlTreeView.Focus();
                    }
                }
            }
        }


        public void Execute()
        {
            string errorTextFromIsFileInUse;
            Session session = GetSession();
            session.IsExecute = true;

            if (!Directory.Exists(session.ResultsRootDirectory))
                Directory.CreateDirectory(session.ResultsRootDirectory);
            //{
            //    MessageBox.Show("The results directory specified for this session is not valid.\nExecution is cancelled.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    session.IsExecute = false;
            //}
            if (IsFileInUse(GetSelectedTag(), out errorTextFromIsFileInUse))
            {
                string firstPartWarningText = "";

                if (GetSelectedTag() is Script)
                {
                    firstPartWarningText = "Unable to execute script.\n\n";
                }
                else if (GetSelectedTag() is Emulator)
                {
                    firstPartWarningText = "Unable to execute emulator.\n\n";
                }

                MessageBox.Show(firstPartWarningText + errorTextFromIsFileInUse + "\n\n(hint: change the session ID to obtain a different results file name)", "Warning");
            }
            else
            {
                _TagThatIsBeingExecuted = GetSelectedTag();

                /* if session is an emulator session or a script session . 
                 * Since media session do not require any connection to have 
                 * secured connection*/
                if ((_TagThatIsBeingExecuted is Emulator) || (_TagThatIsBeingExecuted is DicomScript))
                {
                    bool bDisplayPwdMsg = false;
                    bool bPwdMsg = true;
                    int certIndex = 0;
                    int credIndex = 0;

                    Dvtk.Sessions.ISecuritySettings theISecuritySettings = null;
                    theISecuritySettings = (GetSelectedSessionNew().Implementation as Dvtk.Sessions.ISecure).SecuritySettings;

                    /* If the session selected has security settings enabled  
                      */
                    if (theISecuritySettings.SecureSocketsEnabled == true)
                    {

                        certIndex = theISecuritySettings.CertificateFileName.LastIndexOf("\\");
                        credIndex = theISecuritySettings.CredentialsFileName.LastIndexOf("\\");

                        /* if Certificate file name not specified */
                        if ((certIndex + 1) == theISecuritySettings.CertificateFileName.Length)
                        {
                            MessageBox.Show("Session cannot be executed as the certificate name is not specified ");

                            _TagThatIsBeingExecuted = null;
                            bDisplayPwdMsg = true;
                        }
                        /* if Certificate file donot exist */
                        else if (!File.Exists(theISecuritySettings.CertificateFileName))
                        {
                            MessageBox.Show("Certificate File does not exist ");
                            _TagThatIsBeingExecuted = null;
                            bDisplayPwdMsg = true;
                        }
                        /* if Credential file name not specified */
                        else if ((credIndex + 1) == theISecuritySettings.CredentialsFileName.Length)
                        {
                            MessageBox.Show("Session can not be executed as the credential name is not specified ");
                            _TagThatIsBeingExecuted = null;
                            bDisplayPwdMsg = true;
                        }
                        /* If credential file donot exist */
                        else if (!File.Exists(theISecuritySettings.CredentialsFileName))
                        {
                            MessageBox.Show("Credential File do not exist ");
                            _TagThatIsBeingExecuted = null;
                            bDisplayPwdMsg = true;
                        }

                        (GetSelectedSessionNew().Implementation as Dvtk.Sessions.ISecure).SecuritySettings.TlsPassword = ":-DHE-RSA-AES256-SHA:-DHE-DSS-AES256-SHA:-AES256-SHA";

                        while (!bDisplayPwdMsg)
                        {
                            try
                            {
                                (GetSelectedSessionNew().Implementation as Dvtk.Sessions.ISecure).CreateSecurityCredentialHandler();
                                bDisplayPwdMsg = true;
                                (GetSelectedSessionNew().Implementation as Dvtk.Sessions.ISecure).DisposeSecurityCredentialHandler();
                            }
                            catch (Exception theException)
                            {
                                if (theException.GetType().FullName == "Wrappers.Exceptions.PasswordExpection")
                                {
                                    bDisplayPwdMsg = false;
                                    PassWordForm passWordForm = new PassWordForm(GetSelectedSessionNew(), bPwdMsg);
                                    bPwdMsg = false;
                                    if (passWordForm.ShowDialog() != DialogResult.OK)
                                    {
                                        bDisplayPwdMsg = true;
                                        bPwdMsg = false;

                                        _TagThatIsBeingExecuted = null;
                                    }
                                }
                                else
                                {
                                    MessageBox.Show(theException.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    bDisplayPwdMsg = true;

                                    _TagThatIsBeingExecuted = null;

                                }

                            }
                        }
                    }
                }

                if (_TagThatIsBeingExecuted != null)
                {
                    // Update the UI.
                    StartExecution theStartExecution = new StartExecution(GetSelectedUserNode());
                    Notify(theStartExecution);

                    // If this is a script file tag, start execution of the script.
                    if (_TagThatIsBeingExecuted is Script)
                    {
                        ExecuteSelectedScript();
                    }

                    // If this is a emulator tag, start execution of the correct emulator.
                    if (_TagThatIsBeingExecuted is Emulator)
                    {
                        ExecuteSelectedEmulator();
                    }

                    // If this is a media session tag, start execution of the media validator.
                    if (_TagThatIsBeingExecuted is MediaSession)
                    {
                        ExecuteSelectedMediaSession();
                    }
                }
                else
                {
                    session.IsExecute = false;
                    // Update the UI.
                    EndExecution theEndExecution = new EndExecution(GetSelectedTag());
                    Notify(theEndExecution);
                }
            }
        }


        /// <summary>
        /// This method performs the validation of a media direcetory
        /// </summary>
        /// <param name="MediaDirectoryInfo"></param>
        public void Execute(DirectoryInfo MediaDirectoryInfo)
        {
            _ISMediaDirectoryValidation = true;
            string errorTextFromIsFileInUse;
            Session session = GetSession();
            session.IsExecute = true;

            if (!Directory.Exists(session.ResultsRootDirectory))
                Directory.CreateDirectory(session.ResultsRootDirectory);
            //{
            //    MessageBox.Show("The results directory specified for this session is not valid.\nExecution is cancelled.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    session.IsExecute = false;
            //}
            if (IsFileInUse(GetSelectedTag(), out errorTextFromIsFileInUse))
            {
                string firstPartWarningText = "";

                if (GetSelectedTag() is Script)
                {
                    firstPartWarningText = "Unable to execute script.\n\n";
                }
                else if (GetSelectedTag() is Emulator)
                {
                    firstPartWarningText = "Unable to execute emulator.\n\n";
                }

                MessageBox.Show(firstPartWarningText + errorTextFromIsFileInUse + "\n\n(hint: change the session ID to obtain a different results file name)", "Warning");
            }
            else
            {
                _TagThatIsBeingExecuted = GetSelectedTag();

                /* if session is an emulator session or a script session . 
                 * Since media session do not require any connection to have 
                 * secured connection*/
                if ((_TagThatIsBeingExecuted is Emulator) || (_TagThatIsBeingExecuted is DicomScript))
                {
                    bool bDisplayPwdMsg = false;
                    bool bPwdMsg = true;
                    int certIndex = 0;
                    int credIndex = 0;

                    Dvtk.Sessions.ISecuritySettings theISecuritySettings = null;
                    theISecuritySettings = (GetSelectedSessionNew().Implementation as Dvtk.Sessions.ISecure).SecuritySettings;

                    /* If the session selected has security settings enabled  
                      */
                    if (theISecuritySettings.SecureSocketsEnabled == true)
                    {

                        certIndex = theISecuritySettings.CertificateFileName.LastIndexOf("\\");
                        credIndex = theISecuritySettings.CredentialsFileName.LastIndexOf("\\");

                        /* if Certificate file name not specified */
                        if ((certIndex + 1) == theISecuritySettings.CertificateFileName.Length)
                        {
                            MessageBox.Show("Session cannot be executed as the certificate name is not specified ");

                            _TagThatIsBeingExecuted = null;
                            bDisplayPwdMsg = true;
                        }
                        /* if Certificate file donot exist */
                        else if (!File.Exists(theISecuritySettings.CertificateFileName))
                        {
                            MessageBox.Show("Certificate File does not exist ");
                            _TagThatIsBeingExecuted = null;
                            bDisplayPwdMsg = true;
                        }
                        /* if Credential file name not specified */
                        else if ((credIndex + 1) == theISecuritySettings.CredentialsFileName.Length)
                        {
                            MessageBox.Show("Session can not be executed as the credential name is not specified ");
                            _TagThatIsBeingExecuted = null;
                            bDisplayPwdMsg = true;
                        }
                        /* If credential file donot exist */
                        else if (!File.Exists(theISecuritySettings.CredentialsFileName))
                        {
                            MessageBox.Show("Credential File do not exist ");
                            _TagThatIsBeingExecuted = null;
                            bDisplayPwdMsg = true;
                        }

                        (GetSelectedSessionNew().Implementation as Dvtk.Sessions.ISecure).SecuritySettings.TlsPassword = ":-DHE-RSA-AES256-SHA:-DHE-DSS-AES256-SHA:-AES256-SHA";

                        while (!bDisplayPwdMsg)
                        {
                            try
                            {
                                (GetSelectedSessionNew().Implementation as Dvtk.Sessions.ISecure).CreateSecurityCredentialHandler();
                                bDisplayPwdMsg = true;
                                (GetSelectedSessionNew().Implementation as Dvtk.Sessions.ISecure).DisposeSecurityCredentialHandler();
                            }
                            catch (Exception theException)
                            {
                                if (theException.GetType().FullName == "Wrappers.Exceptions.PasswordExpection")
                                {
                                    bDisplayPwdMsg = false;
                                    PassWordForm passWordForm = new PassWordForm(GetSelectedSessionNew(), bPwdMsg);
                                    bPwdMsg = false;
                                    if (passWordForm.ShowDialog() != DialogResult.OK)
                                    {
                                        bDisplayPwdMsg = true;
                                        bPwdMsg = false;

                                        _TagThatIsBeingExecuted = null;
                                    }
                                }
                                else
                                {
                                    MessageBox.Show(theException.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    bDisplayPwdMsg = true;

                                    _TagThatIsBeingExecuted = null;

                                }

                            }
                        }
                    }
                }

                if (_TagThatIsBeingExecuted != null)
                {
                    // Update the UI.
                    StartExecution theStartExecution = new StartExecution(GetSelectedUserNode());
                    Notify(theStartExecution);

                    // If this is a script file tag, start execution of the script.
                    if (_TagThatIsBeingExecuted is Script)
                    {
                        ExecuteSelectedScript();
                    }

                    // If this is a emulator tag, start execution of the correct emulator.
                    if (_TagThatIsBeingExecuted is Emulator)
                    {
                        ExecuteSelectedEmulator();
                    }

                    // If this is a media session tag, start execution of the media validator.
                    if (_TagThatIsBeingExecuted is MediaSession)
                    {

                        ExecuteMediaDirectoryValidation(MediaDirectoryInfo);

                    }
                    else
                    {
                        session.IsExecute = false;
                        // Update the UI.
                        EndExecution theEndExecution = new EndExecution(GetSelectedTag());
                        Notify(theEndExecution);
                    }
                }
            }
        }


        /// <summary>
        /// Is the main results file for the selected tree node tag in use when a script or emulator
        /// will be executed or media files will be validated?
        /// </summary>
        /// <param name="treeNodeTag">The selected tree node tag.</param>
        /// <returns>Indicates if the main results file for the selected tree node tag is in use.</returns>
        public bool IsFileInUse(object treeNodeTag, out string errorText)
        {
            bool isFileInUse = false;
            string resultsFileNameOnly = "";
            errorText = "";

            if (treeNodeTag is Script)
            {
                Script script = treeNodeTag as Script;

                // resultsFileNameOnly = ResultsFile.GetSummaryNameForScriptFile(scriptFileTag._Session, scriptFileTag._ScriptFileName);
            }
            else if (treeNodeTag is Emulator)
            {
                Emulator emulator = treeNodeTag as Emulator;

                //resultsFileNameOnly = ResultsFile.GetSummaryNameForEmulator(emulatorTag._Session, emulatorTag._EmulatorType);
            }

            if (resultsFileNameOnly != "")
            {
                string resultsFullFileName = " ";
                // resultsFullFileName = Path.Combine(treeNodeTag._Session.ResultsRootDirectory, resultsFileNameOnly);
                FileStream fileStream = null;

                try
                {
                    fileStream = File.OpenRead(resultsFullFileName);
                }
                catch (IOException exception)
                {
                    if (!(exception is FileNotFoundException))
                    {
                        isFileInUse = true;
                        errorText = exception.Message;
                    }
                }
                finally
                {
                    if (fileStream != null)
                    {
                        fileStream.Close();
                    }
                }
            }

            return isFileInUse;
        }


        private void userControlTreeView_DoubleClick(object sender, EventArgs e)
        {
            object theTreeNodeTag = GetSelectedTag();

            if ((theTreeNodeTag is Script) ||
                (theTreeNodeTag is Emulator) ||
                (theTreeNodeTag is MediaSession)
                )
            {
                Execute();
            }
        }


        private void userControlTreeView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                // Right button has been pressed.
                // Make the node that is below the mouse the selected one.
                userControlTreeView.SelectedNode = userControlTreeView.GetNodeAt(e.X, e.Y);
                Refresh();
            }
        }


        public TreeNode GetSessionNode(Session session)
        {
            TreeNode theSessionNodeToFind = null;

            foreach (TreeNode theNode in userControlTreeView.Nodes)
            {
                if (theNode.Tag is Project)
                {
                    foreach (TreeNode node in theNode.Nodes)
                    {
                        if (node.Tag is Session)
                        {
                            object theSessionTag = (Session)node.Tag;

                            if (theSessionTag == session)
                            {
                                theSessionNodeToFind = node;
                                break;
                            }
                        }
                    }
                }
            }

            return theSessionNodeToFind;
        }


        public void ClearSessionTreeView()
        {
            userControlTreeView.Nodes.Clear();
        }


        private void OnClearAll(object theSource, object theEvent)
        {
            ClearAll theClearAllEvent = (ClearAll)theEvent;

            if (theClearAllEvent._StoreSessionTreeState)
            {
                foreach (TreeNode theTreeNode in userControlTreeView.Nodes)
                {
                    nodeInformation.StoreExpandInformation(theTreeNode);
                }
                nodeInformation.StoreSelectedNode(userControlTreeView);
            }

            ClearSessionTreeView();

            SessionTreeViewSelectionChange theSessionTreeViewSelectionChange = new SessionTreeViewSelectionChange(GetSelectedUserNode());
            Notify(theSessionTreeViewSelectionChange);
        }


        public void ViewExpandedScriptFile()
        {
            Script selectedScriptFileTag = GetSelectedTag() as Script;

            if (selectedScriptFileTag != null)
            {
                if (selectedScriptFileTag.ScriptFileName.ToLower().EndsWith(".vbs"))
                {

                    VisualBasicScript applicationLayerVisualBasicScript =
                        new VisualBasicScript(((ScriptSession)selectedScriptFileTag.ParentSession).ScriptSessionImplementation, selectedScriptFileTag.ScriptFileName);

                    applicationLayerVisualBasicScript.ViewExpanded();
                }
            }
        }


        public void EditSelectedScriptFile()
        {
            object theScriptFileTag = GetSelectedTag() as Script;

            if (theScriptFileTag == null)
            {
                // Sanity check.
                Debug.Assert(false);
            }
            else
            {
                ScriptSession theScriptSession = ((Script)theScriptFileTag).ParentSession as ScriptSession;

                Process theProcess = new Process();

                theProcess.StartInfo.FileName = "Notepad.exe";
                theProcess.StartInfo.Arguments = Path.Combine(theScriptSession.DicomScriptRootDirectory, ((Script)theScriptFileTag).ScriptFileName);

                theProcess.Start();
            }
        }


        public Session GetExecutingSession()
        {
            Session theExecutingSession = null;

            if (_TagThatIsBeingExecuted != null)
            {
                if (_TagThatIsBeingExecuted is MediaSession)
                {
                    theExecutingSession = ((MediaSession)_TagThatIsBeingExecuted);
                }
                else if (_TagThatIsBeingExecuted is Script)
                {
                    theExecutingSession = ((Script)_TagThatIsBeingExecuted).ParentSession;
                }
                else
                {
                    theExecutingSession = ((Emulator)_TagThatIsBeingExecuted).ParentSession;
                }
            }
            return theExecutingSession;
        }


        private void ExecuteSelectedMediaSession()
        {

            _MainForm = (MainForm)ProjectForm._MainForm;
            ArrayList theMediaFilesToBeValidatedLocalList = new ArrayList();
            MediaSession theMediaSession = GetSelectedSessionNew() as MediaSession;

            if (theMediaSession == null)
            {
                // Sanity check.
                Debug.Assert(false);
            }
            else
            {
                theMediaSession.IsExecute = true;
                mediaFilesToBeValidated.Clear();

                OpenFileDialog theOpenFileDialog = new OpenFileDialog();

                theOpenFileDialog.Filter = "All files (*.*)|*.*";
                theOpenFileDialog.Multiselect = true;
                theOpenFileDialog.ReadOnlyChecked = true;
                theOpenFileDialog.Title = "Select media files to validate";

                // Show the file dialog.
                // If the user pressed the OK button...
                if (theOpenFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Validate all files selected.
                    foreach (string theFullFileName in theOpenFileDialog.FileNames)
                    {
                        mediaFilesToBeValidated.Enqueue(theFullFileName);
                        theMediaFilesToBeValidatedLocalList.Add(theFullFileName);
                    }
                }

                if (mediaFilesToBeValidated.Count == 0)
                {
                    // No files selected, so no media validation to perform.
                    // Update UI.
                    _TagThatIsBeingExecuted = null;

                    EndExecution theEndExecution = new EndExecution(GetSelectedTag());
                    Notify(theEndExecution);
                }
                else
                {
                    bool isExecutionCancelled = false;

                    // Remove the current results files for the selected media files.
                    // If results files exists that will be removed, ask the user what to do with them.
                    ArrayList theResultsFilesForSession = Result.GetAllNamesForSession(theMediaSession);
                    ArrayList theResultsFilesToRemove = new ArrayList();

                    foreach (string theMediaFullFileName in theMediaFilesToBeValidatedLocalList)
                    {
                        string theMediaFileBaseName = Result.GetBaseNameForMediaFile(theMediaFullFileName);

                        ArrayList theResultsFilesToRemoveForMediaFile = Result.GetNamesForBaseName(theMediaFileBaseName, theResultsFilesForSession);
                        theResultsFilesToRemoveForMediaFile = Result.GetNamesForCurrentSessionId(theMediaSession, theResultsFilesToRemoveForMediaFile);

                        theResultsFilesToRemove.AddRange(theResultsFilesToRemoveForMediaFile);
                    }

                    if (theResultsFilesToRemove.Count != 0)
                    {
                        string theWarningMessage = string.Format("Results files exist that will be removed before media validation.\nCopy these results files to backup files?");
                        DialogResult theDialogResult = DialogResult.No;

                        // Only ask to backup the results file if this is configured.
                        if (_MainForm._UserSettings.AskForBackupResultsFile)
                        {
                            theDialogResult = MessageBox.Show(theWarningMessage, "Warning", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                        }

                        if (theDialogResult == DialogResult.Yes)
                        {
                            Result.BackupFiles(theMediaSession, theResultsFilesToRemove);
                            Result.Remove(theMediaSession, theResultsFilesToRemove);
                        }
                        else if (theDialogResult == DialogResult.No)
                        {
                            Result.Remove(theMediaSession, theResultsFilesToRemove);
                        }
                        else
                        {
                            _TagThatIsBeingExecuted = null;

                            // Update the UI.
                            EndExecution theEndExecution = new EndExecution(GetSelectedTag());
                            Notify(theEndExecution);

                            isExecutionCancelled = true;
                        }
                    }

                    if (!isExecutionCancelled)
                    {
                        _FirstMediaFileToValidate = (string)mediaFilesToBeValidated.Peek();
                        ValidateMediaFiles();
                    }
                }
            }
        }


        //This method performs the validation of a media directory.
        private void ExecuteMediaDirectoryValidation(DirectoryInfo MediaDirectoryInfo)
        {
            _MainForm = (MainForm)ProjectForm._MainForm;
            ArrayList theMediaFilesToBeValidatedLocalList = null;
            MediaSession theMediaSession = GetSelectedSessionNew() as MediaSession;

            if (theMediaSession == null)
            {
                // Sanity check.
                Debug.Assert(false);
            }
            else
            {
                theMediaSession.IsExecute = true;
                mediaFilesToBeValidated.Clear();

                //Recursively get all the media files from all the sub directories
                theMediaFilesToBeValidatedLocalList = GetFilesRecursively(MediaDirectoryInfo);

                foreach (string theMediaFileName in theMediaFilesToBeValidatedLocalList)
                {
                    listOfFileNames.Add(Path.GetFileName(theMediaFileName));
                    mediaFilesToBeValidated.Enqueue(theMediaFileName);
                }

                if (mediaFilesToBeValidated.Count == 0)
                {
                    // No directory selected, so no media validation to perform.
                    // Update UI.
                    _TagThatIsBeingExecuted = null;

                    MessageBox.Show("The Selected Directory has no media files", "No Media Files present!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                    EndExecution theEndExecution = new EndExecution(GetSelectedTag());
                    Notify(theEndExecution);
                }
                else
                {
                    bool isExecutionCancelled = false;

                    if (!isExecutionCancelled)
                    {
                        _FirstMediaFileToValidate = (string)mediaFilesToBeValidated.Peek();
                        ValidateMediaDirectory();
                    }
                }
            }
        }


        private void ValidateMediaFiles()
        {
            lock (this)
            {
                MediaSession theMediaSession = (MediaSession)_TagThatIsBeingExecuted;

                _ISMediaDirectoryValidation = false;
                if (mediaFilesToBeValidated.Count > 0)
                {
                    string theFullFileName = (string)mediaFilesToBeValidated.Dequeue();
                    string baseName = "";
                    string fileName = Path.GetFileName(theFullFileName);

                    if (fileName.ToLower().IndexOf("dicomdir") != -1)
                    {
                        baseName = fileName;
                    }
                    else
                    {

                        baseName = fileName + "_DCM";
                        baseName = baseName.Replace(".", "_");
                    }
                    string theExpandedResultsFileName = theMediaSession.SessionId.ToString("000") + "_" + baseName + "_res.xml";

                    // Start the results gathering.
                    theMediaSession.MediaSessionImplementation.StartResultsGathering(theExpandedResultsFileName);

                    string[] mediaFilesToValidate = new string[] { theFullFileName };

                    // Perform the actual execution of the script.
                    AsyncCallback theValidateMediaFilesAsyncCallback = new AsyncCallback(ResultsFromValidateMediaFilesAsynchronously);
                    theMediaSession.MediaSessionImplementation.BeginValidateMediaFiles(mediaFilesToValidate, theValidateMediaFilesAsyncCallback);
                } // foreach: Validate all files selected.
            }
        }


        // This method validates every media file present in the selected Directory(Non-DICOMDIR)
        private void ValidateMediaDirectory()
        {
            lock (this)
            {
                MediaSession theMediaSession = (MediaSession)_TagThatIsBeingExecuted;

                if (mediaFilesToBeValidated.Count > 0)
                {
                    int index = listOfFileNames.Count - mediaFilesToBeValidated.Count;
                    string theFullFileName = (string)mediaFilesToBeValidated.Dequeue();
                    string baseName = "";
                    string fileName = Path.GetFileName(theFullFileName);

                    int count = 0;

                    //Find the number of media files with the same name as the current media file.
                    for (int j = index + 1; j < mediaFilesToBeValidated.Count; j++)
                    {
                        if ((string)listOfFileNames[index] == (string)listOfFileNames[j])
                        {
                            ++count;
                        }
                    }

                    if (fileName.ToLower().IndexOf("dicomdir") != -1)
                    {
                        baseName = fileName;
                    }
                    else
                    {
                        if (count != 0)
                        {
                            //Append the  count to the name of the result file.
                            baseName = fileName + "_DCM" + count.ToString();
                        }
                        else
                        {
                            baseName = fileName + "_DCM";
                        }

                        baseName = baseName.Replace(".", "_");
                    }
                    string theExpandedResultsFileName = theMediaSession.SessionId.ToString("000") + "_" + baseName + "_res.xml";

                    // Start the results gathering.
                    theMediaSession.MediaSessionImplementation.StartResultsGathering(theExpandedResultsFileName);

                    string[] mediaFilesToValidate = new string[] { theFullFileName };

                    // Perform the actual execution of the script.
                    AsyncCallback theValidateMediaFilesAsyncCallback = new AsyncCallback(ResultsFromValidateMediaFilesAsynchronously);
                    theMediaSession.MediaSessionImplementation.BeginValidateMediaFiles(mediaFilesToValidate, theValidateMediaFilesAsyncCallback);
                } // foreach: Validate all files selected.                          
            }
        }


        public void ResultsFromValidateMediaFilesAsynchronously(IAsyncResult theIAsyncResult)
        {
            MediaSession theMediaSession = (MediaSession)GetExecutingSession();

            try
            {
                // Obligated to call the following method according to the asynchronous design pattern.
                theMediaSession.MediaSessionImplementation.EndValidateMediaFiles(theIAsyncResult);
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
                //                CustomExceptionHandler eh = new CustomExceptionHandler();
                //                System.Threading.ThreadExceptionEventArgs args = new ThreadExceptionEventArgs(ex);
                //                eh.OnThreadException(this, args);
                //
                // Rethrow. This rethrow may work in the future .NET 2.x CLR.
                // Currently eaten.
                //
                throw ex;
            }

            theMediaSession.MediaSessionImplementation.EndResultsGathering();

            if (mediaFilesToBeValidated.Count > 0)
            {
                if (!_ISMediaDirectoryValidation)
                {
                    ValidateMediaFiles();
                }
                else
                {
                    ValidateMediaDirectory();
                }
            }
            else
            {
                MediaSession mediaSession = (MediaSession)_TagThatIsBeingExecuted;
                mediaSession.CreateMediaFiles();
                // Update the UI. Do this with an invoke, because the thread that is calling this
                // method is NOT the thread that created all controls used!
                _EndExecution = new EndExecution(_TagThatIsBeingExecuted);
                theMediaSession.IsExecute = false;

                _TagThatIsBeingExecuted = null;

                _NotifyDelegate = new NotifyDelegate(ProjectForm.Notify);
                ProjectForm.Invoke(_NotifyDelegate, new object[] { _EndExecution });
            }
        }


        public void GenerateDICOMDIR()
        {
            OpenFileDialog theOpenFileDialog = new OpenFileDialog();

            theOpenFileDialog.Filter = "DICOM media files (*.dcm)|*.dcm|All files (*.*)|*.*";
            theOpenFileDialog.Title = "Select DCM files to create DICOMDIR";
            theOpenFileDialog.Multiselect = true;
            theOpenFileDialog.ReadOnlyChecked = true;

            // Show the file dialog.
            // If the user pressed the OK button...
            if (theOpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Add all DCM files selected.
                string[] dcmFiles = new string[theOpenFileDialog.FileNames.Length];
                MediaSession theMediaSession = GetSelectedSessionNew() as MediaSession;
                if (theMediaSession == null)
                {
                    // Sanity check.
                    Debug.Assert(false);
                }
                theMediaSession.IsExecute = true;
                _TagThatIsBeingExecuted = GetSelectedTag();

                if (theOpenFileDialog.FileNames.Length == 0)
                {
                    // No files selected, so no media validation to perform.
                    // Update UI.
                    _TagThatIsBeingExecuted = null;

                    EndExecution theEndExecution = new EndExecution(GetSelectedTag());
                    Notify(theEndExecution);
                }

                // Move all selected DCM files to directory "DICOM" in result root directory.
                int i = 0;
                DirectoryInfo theDirectoryInfo = null;
                try
                {
                    string resultsDir = theMediaSession.ResultsRootDirectory;
                    if (!resultsDir.EndsWith("\\"))
                        resultsDir += "\\";
                    theDirectoryInfo = new DirectoryInfo(resultsDir + "DICOM\\");

                    // Create "DICOM" directory if it doesn't exist
                    if (!theDirectoryInfo.Exists)
                    {
                        theDirectoryInfo.Create();
                    }
                    else
                    { // Remove existing DCM files from "DICOM" directory
                        FileInfo[] files = theDirectoryInfo.GetFiles();
                        foreach (FileInfo file in files)
                        {
                            file.Delete();
                        }
                    }

                    foreach (string dcmFile in theOpenFileDialog.FileNames)
                    {
                        FileInfo theFileInfo = new FileInfo(dcmFile);
                        string newFileName = string.Format("I{0:00000}", i);
                        string destFileName = theDirectoryInfo.FullName + "\\" + newFileName;
                        //string destFileName = theDirectoryInfo.FullName + theFileInfo.Name;
                        theFileInfo.CopyTo(destFileName, true);
                        dcmFiles.SetValue(destFileName, i);
                        i++;
                    }
                }
                catch (IOException exception)
                {
                    MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                _MainForm = (MainForm)ProjectForm._MainForm;
                if (_MainForm != null)
                {
                    _MainForm.MainStatusBar.Text = "Please wait, DICOMDIR creation is in progress...";
                }

                string theExpandedResultsFileName = theMediaSession.SessionId.ToString("000") + "_" + "dicomdir_creation_logging" + "_res.xml";
                theMediaSession.Implementation.StartResultsGathering(theExpandedResultsFileName);
                AsyncCallback createDicomdirAsyncCallback = new AsyncCallback(ResultsFromCreationDicomdirAsynchronously);
                theMediaSession.MediaSessionImplementation.BeginGenerationDICOMDIR(dcmFiles, createDicomdirAsyncCallback);
            }
        }


        public void GenerateDICOMDIRWithDirectory()
        {
            FolderBrowserDialog mediaDirectoryBrowserDialog = new FolderBrowserDialog();
            mediaDirectoryBrowserDialog.Description = "Select the directory contains media files:";
            if (mediaDirectoryBrowserDialog.ShowDialog(this) == DialogResult.OK)
            {
                DirectoryInfo theDirectoryInfo = new DirectoryInfo(mediaDirectoryBrowserDialog.SelectedPath);
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
                            if (dcmFilesInSubDir.Length != 0)
                            {
                                foreach (FileInfo fileNext in dcmFilesInSubDir)
                                {
                                    allDCMFiles.Add(fileNext);
                                }
                            }
                        }
                        else if (f is FileInfo)
                        {
                            allDCMFiles.Add((FileInfo)f);
                        }
                    }

                    dcmFiles = (FileInfo[])allDCMFiles.ToArray(typeof(FileInfo));
                    allDCMFiles.Clear();
                }

                if (dcmFiles.Length != 0)
                {
                    MediaSession theMediaSession = GetSelectedSessionNew() as MediaSession;
                    if (theMediaSession == null)
                    {
                        // Sanity check.
                        Debug.Assert(false);
                    }
                    theMediaSession.IsExecute = true;
                    _TagThatIsBeingExecuted = GetSelectedTag();

                    // Move all selected DCM files to directory "DICOM" in result root directory.
                    int i = 0;
                    DirectoryInfo theDICOOMDirInfo = null;
                    string resultsDir = theMediaSession.ResultsRootDirectory;
                    if (!resultsDir.EndsWith("\\"))
                        resultsDir += "\\";
                    theDICOOMDirInfo = new DirectoryInfo(resultsDir + "DICOM\\");

                    // Create "DICOM" directory if it doesn't exist
                    if (!theDICOOMDirInfo.Exists)
                    {
                        theDICOOMDirInfo.Create();
                    }
                    else
                    {
                        // Remove existing DCM files from "DICOM" directory
                        FileInfo[] files = theDICOOMDirInfo.GetFiles();
                        foreach (FileInfo file in files)
                        {
                            file.Delete();
                        }
                    }

                    string[] filesToSend = new string[dcmFiles.Length];
                    foreach (FileInfo theFileInfo in dcmFiles)
                    {
                        string newFileName = string.Format("I{0:00000}", i);
                        string destFileName = theDICOOMDirInfo.FullName + "\\" + newFileName;
                        //string destFileName = theDICOOMDirInfo.FullName + theFileInfo.Name;
                        theFileInfo.CopyTo(destFileName, true);
                        filesToSend.SetValue(destFileName, i);
                        i++;
                    }

                    _MainForm = (MainForm)ProjectForm._MainForm;
                    if (_MainForm != null)
                    {
                        _MainForm.MainStatusBar.Text = "Please wait, DICOMDIR creation is in progress...";
                    }

                    string theExpandedResultsFileName = theMediaSession.SessionId.ToString("000") + "_" + "dicomdir_creation_logging" + "_res.xml";
                    theMediaSession.Implementation.StartResultsGathering(theExpandedResultsFileName);
                    AsyncCallback createDicomdirAsyncCallback = new AsyncCallback(ResultsFromCreationDicomdirAsynchronously);
                    theMediaSession.MediaSessionImplementation.BeginGenerationDICOMDIR(filesToSend, createDicomdirAsyncCallback);
                }
                else
                {
                    // No files selected, so no media validation to perform.
                    // Update UI.
                    _TagThatIsBeingExecuted = null;

                    EndExecution theEndExecution = new EndExecution(GetSelectedTag());
                    Notify(theEndExecution);
                }
            }
        }


        public void ResultsFromCreationDicomdirAsynchronously(IAsyncResult theIAsyncResult)
        {
            MediaSession theMediaSession = (MediaSession)GetExecutingSession();

            try
            {
                // Obligated to call the following method according to the asynchronous design pattern.
                theMediaSession.MediaSessionImplementation.EndGenerationDICOMDIR(theIAsyncResult);
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

            theMediaSession.MediaSessionImplementation.EndResultsGathering();

            MediaSession mediaSession = (MediaSession)_TagThatIsBeingExecuted;
            mediaSession.CreateMediaFiles();
            _FirstMediaFileToValidate = "dicomdir_creation_logging";

            // Update the UI. Do this with an invoke, because the thread that is calling this
            // method is NOT the thread that created all controls used!
            _EndExecution = new EndExecution(_TagThatIsBeingExecuted);
            theMediaSession.IsExecute = false;

            _TagThatIsBeingExecuted = null;

            _MainForm = (MainForm)ProjectForm._MainForm;
            if (_MainForm != null)
            {
                _MainForm.MainStatusBar.Text = "DICOMDIR creation completed.";
            }

            _NotifyDelegate = new NotifyDelegate(ProjectForm.Notify);
            ProjectForm.Invoke(_NotifyDelegate, new object[] { _EndExecution });
        }


        private void ExecuteSelectedEmulator()
        {
            _MainForm = (MainForm)ProjectForm._MainForm;
            EmulatorSession theEmulatorSession = GetSession() as EmulatorSession;
            /// To check added some code 
            if (theEmulatorSession == null)
            {
                // Sanity check.
                Debug.Assert(false);
            }
            else
            {
                theEmulatorSession.IsExecute = true;
                Emulator theEmulatorTag = (Emulator)GetSelectedTag();
                string theResultsFileName = null;
                bool isExecutionCancelled = false;

                // Remove the current results files for the emulator.
                // If results files exists that will be removed, ask the user what to do with them.
                ArrayList theResultsFilesToRemove = Result.GetAllNamesForSession(theEmulatorSession);
                string theEmulatorBaseName = Result.GetBaseNameForEmulator(theEmulatorTag.EmulatorType);
                theResultsFilesToRemove = Result.GetNamesForBaseName(theEmulatorBaseName, theResultsFilesToRemove);
                theResultsFilesToRemove = Result.GetNamesForCurrentSessionId(theEmulatorSession, theResultsFilesToRemove);

                if (theResultsFilesToRemove.Count != 0)
                {
                    string theWarningMessage = string.Format("Results files exist that will be removed before execution of the emulator.\nCopy these results files to backup files?");
                    DialogResult theDialogResult = DialogResult.No;

                    // Only ask to backup the results file if this is configured.
                    if (_MainForm._UserSettings.AskForBackupResultsFile)
                    {
                        theDialogResult = MessageBox.Show(theWarningMessage, "Warning", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    }

                    if (theDialogResult == DialogResult.Yes)
                    {
                        Result.BackupFiles(theEmulatorSession, theResultsFilesToRemove);
                        Result.Remove(theEmulatorSession, theResultsFilesToRemove);
                    }
                    else if (theDialogResult == DialogResult.No)
                    {
                        Result.Remove(theEmulatorSession, theResultsFilesToRemove);
                    }
                    else
                    {
                        _TagThatIsBeingExecuted = null;

                        // Update the UI.
                        EndExecution theEndExecution = new EndExecution(GetSelectedTag());
                        Notify(theEndExecution);

                        isExecutionCancelled = true;
                    }
                }
                if (!isExecutionCancelled)
                {
                    // Determine the results file name.
                    theResultsFileName = Result.GetExpandedNameForEmulator(theEmulatorSession, theEmulatorTag.EmulatorType);
                    // If this is the print SCP emulator or the storage SCP emulator...
                    if ((theEmulatorTag.EmulatorType == Emulator.EmulatorTypes.PRINT_SCP) ||
                        (theEmulatorTag.EmulatorType == Emulator.EmulatorTypes.STORAGE_SCP))
                    {
                        // Perform the actual execution of the emulator
                        if (theEmulatorTag.EmulatorType == Emulator.EmulatorTypes.STORAGE_SCP)
                            theEmulatorSession.EmulatorSessionImplementation.ScpEmulatorType = DvtkData.Results.ScpEmulatorType.Storage;
                        else
                            theEmulatorSession.EmulatorSessionImplementation.ScpEmulatorType = DvtkData.Results.ScpEmulatorType.Printing;
                        theEmulatorSession.EmulatorSessionImplementation.StartResultsGathering(theResultsFileName);
                        AsyncCallback theAsyncCallback = new AsyncCallback(ResultsFromExecutingEmulatorScpAsynchronously);
                        theEmulatorSession.EmulatorSessionImplementation.BeginEmulateSCP(theAsyncCallback);
                    }

                    // If this is the storage SCU emulator...
                    if (theEmulatorTag.EmulatorType == Emulator.EmulatorTypes.STORAGE_SCU)
                    {
                        theEmulatorSession.EmulatorSessionImplementation.ScuEmulatorType = DvtkData.Results.ScuEmulatorType.Storage;
                        theEmulatorSession.EmulatorSessionImplementation.StartResultsGathering(theResultsFileName);

                        DialogResult theDialogResult = _StorageSCUEmulatorForm.ShowDialog(ProjectForm, theEmulatorSession);

                        if (theDialogResult == DialogResult.Cancel)
                        {
                            // No sending of Dicom files is happening now.

                            // Save the results.
                            GetSession().Implementation.EndResultsGathering();
                            _TagThatIsBeingExecuted = null;

                            // Update the UI.
                            EndExecution theEndExecution = new EndExecution(GetSelectedTag());
                            Notify(theEndExecution);
                        }
                        else
                        {
                            // Dicom files are being send in another thread.
                            // Do nothing, let the call back method handle the enabling of the session in the UI.
                        }
                    }
                }
            }
        }


        public void ResultsFromExecutingEmulatorScpAsynchronously(IAsyncResult theIAsyncResult)
        {
            EmulatorSession theEmulatorSession = (EmulatorSession)GetExecutingSession();

            try
            {
                // Obligated to call the following method according to the asynchronous design pattern.
                theEmulatorSession.EmulatorSessionImplementation.EndEmulateSCP(theIAsyncResult);
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
                //                CustomExceptionHandler eh = new CustomExceptionHandler();
                //                System.Threading.ThreadExceptionEventArgs args = new ThreadExceptionEventArgs(ex);
                //                eh.OnThreadException(this, args);
                //
                // Rethrow. This rethrow may work in the future .NET 2.x CLR.
                // Currently eaten.
                //
                throw ex;
            }

            // Save the results.
            theEmulatorSession.EmulatorSessionImplementation.EndResultsGathering();
            theEmulatorSession.IsExecute = false;
            Emulator emulator = (Emulator)_TagThatIsBeingExecuted;
            ((EmulatorSession)emulator.ParentSession).CreateEmulatorFiles();

            // Update the UI. Do this with an invoke, because the thread that is calling this
            // method is NOT the thread that created all controls used!
            _EndExecution = new EndExecution(_TagThatIsBeingExecuted);

            _TagThatIsBeingExecuted = null;

            _NotifyDelegate = new NotifyDelegate(ProjectForm.Notify);
            ProjectForm.Invoke(_NotifyDelegate, new object[] { _EndExecution });
        }


        public void ResultsFromExecutingEmulatorStorageScuAsynchronously(IAsyncResult theIAsyncResult)
        {
            EmulatorSession theEmulatorSession = (EmulatorSession)GetExecutingSession();

            try
            {
                // Obligated to call the following method according to the asynchronous design pattern.
                theEmulatorSession.EmulatorSessionImplementation.EndEmulateStorageSCU(theIAsyncResult);
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
                //                CustomExceptionHandler eh = new CustomExceptionHandler();
                //                System.Threading.ThreadExceptionEventArgs args = new ThreadExceptionEventArgs(ex);
                //                eh.OnThreadException(this, args);
                //
                // Rethrow. This rethrow may work in the future .NET 2.x CLR.
                // Currently eaten.
                //
                throw ex;
            }

            // Save the results.
            theEmulatorSession.EmulatorSessionImplementation.EndResultsGathering();
            theEmulatorSession.IsExecute = false;
            Emulator emulator = (Emulator)_TagThatIsBeingExecuted;
            ((EmulatorSession)emulator.ParentSession).CreateEmulatorFiles();

            // Update the UI. Do this with an invoke, because the thread that is calling this
            // method is NOT the thread that created all controls used!
            _EndExecution = new EndExecution(_TagThatIsBeingExecuted);

            _TagThatIsBeingExecuted = null;

            _NotifyDelegate = new NotifyDelegate(ProjectForm.Notify);
            ProjectForm.Invoke(_NotifyDelegate, new object[] { _EndExecution });
        }


        public void ExecuteSelectedScript()
        {
            _MainForm = (MainForm)ProjectForm._MainForm;
            Script theScriptFileTag = GetSelectedTag() as Script;

            if (theScriptFileTag == null)
            {
                // Sanity check.
                Debug.Assert(false);
            }
            else
            {
                theScriptFileTag.ParentSession.IsExecute = true;
                bool isExecutionCancelled = false;

                // Remove the current results files for this script file.
                // If results files exists that will be removed, ask the user what to do with them.
                ArrayList theResultsFilesToRemove = Result.GetAllNamesForSession(theScriptFileTag.ParentSession);
                theResultsFilesToRemove = Result.GetNamesForScriptFile(theScriptFileTag.ScriptFileName, theResultsFilesToRemove);
                theResultsFilesToRemove = Result.GetNamesForCurrentSessionId(theScriptFileTag.ParentSession, theResultsFilesToRemove);

                if (theResultsFilesToRemove.Count != 0)
                {
                    string theWarningMessage = string.Format("Results files exist that will be removed before execution of script file {0}.\nCopy these results files to backup files?", theScriptFileTag.ScriptFileName);
                    DialogResult theDialogResult = DialogResult.No;

                    // Only ask to backup the results file if this is configured.
                    if (_MainForm._UserSettings.AskForBackupResultsFile)
                    {
                        theDialogResult = MessageBox.Show(theWarningMessage, "Warning", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    }

                    if (theDialogResult == DialogResult.Yes)
                    {
                        Result.BackupFiles(theScriptFileTag.ParentSession, theResultsFilesToRemove);
                        Result.Remove(theScriptFileTag.ParentSession, theResultsFilesToRemove);
                    }
                    else if (theDialogResult == DialogResult.No)
                    {
                        Result.Remove(theScriptFileTag.ParentSession, theResultsFilesToRemove);
                    }
                    else
                    {
                        _TagThatIsBeingExecuted = null;

                        // Update the UI.
                        EndExecution theEndExecution = new EndExecution(GetSelectedTag());
                        Notify(theEndExecution);

                        isExecutionCancelled = true;
                    }
                }

                if (!isExecutionCancelled)
                {
                    if ((Path.GetExtension(theScriptFileTag.ScriptFileName).ToLower() == ".dss") ||
                        (Path.GetExtension(theScriptFileTag.ScriptFileName).ToLower() == ".ds")
                        )
                    {
                        ExecuteDicomScriptInThread(theScriptFileTag);
                    }
                    else if (Path.GetExtension(theScriptFileTag.ScriptFileName).ToLower() == ".vbs")
                    {
                        ExecuteVisualBasicScriptInThread(theScriptFileTag);
                    }
                    else
                    {
                        string msg = string.Format("The script file format {0} not supported.", theScriptFileTag.ScriptFileName);
                        MessageBox.Show(msg);

                        _TagThatIsBeingExecuted = null;

                        // Update the UI.
                        EndExecution theEndExecution = new EndExecution(GetSelectedTag());
                        Notify(theEndExecution);
                    }
                }
            }
        }


        private void ExecuteDicomScriptInThread(Script theScriptFileTag)
        {
            string theResultsFileName = null;
            ScriptSession theScriptSession = (ScriptSession)theScriptFileTag.ParentSession;

            // Determine the results file name.
            theResultsFileName = Result.GetExpandedNameForScriptFile(theScriptSession, theScriptFileTag.ScriptFileName);

            // Start the results gathering.
            theScriptSession.ScriptSessionImplementation.StartResultsGathering(theResultsFileName);

            // Perform the actual execution of the script.
            AsyncCallback theExecuteScriptAsyncCallback = new AsyncCallback(ResultsFromExecutingScriptAsynchronously);
            theScriptSession.ScriptSessionImplementation.BeginExecuteScript(theScriptFileTag.ScriptFileName, false, theExecuteScriptAsyncCallback);
        }


        private void ResultsFromExecutingScriptAsynchronously(IAsyncResult theIAsyncResult)
        {
            ScriptSession theScriptSession = (ScriptSession)GetExecutingSession();

            try
            {
                // Obligated to call the following method according to the asynchronous design pattern.
                theScriptSession.ScriptSessionImplementation.EndExecuteScript(theIAsyncResult);
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
                //                CustomExceptionHandler eh = new CustomExceptionHandler();
                //                System.Threading.ThreadExceptionEventArgs args = new ThreadExceptionEventArgs(ex);
                //                eh.OnThreadException(this, args);
                //
                // Rethrow. This rethrow may work in the future .NET 2.x CLR.
                // Currently eaten.
                //
                throw ex;
            }

            theScriptSession.ScriptSessionImplementation.EndResultsGathering();
            theScriptSession.IsExecute = false;
            Script script = (Script)_TagThatIsBeingExecuted;
            ((ScriptSession)script.ParentSession).CreateScriptFiles();

            // Update the UI. Do this with an invoke, because the thread that is calling this
            // method is NOT the thread that created all controls used!

            _EndExecution = new EndExecution(_TagThatIsBeingExecuted);

            _TagThatIsBeingExecuted = null;

            _NotifyDelegate = new NotifyDelegate(ProjectForm.Notify);
            ProjectForm.Invoke(_NotifyDelegate, new object[] { _EndExecution });
        }


        public void ExecuteVisualBasicScriptInThread(Script theScriptFileTag)
        {
            _ScriptThread = new Thread(new ThreadStart(ExecuteVisualBasicScript));
            _ScriptThread.Start();
        }


        private void ExecuteVisualBasicScript()
        {
            try
            {
                Script theScriptFileTag = _TagThatIsBeingExecuted as Script;

                if (theScriptFileTag == null)
                {
                    // Sanity check.
                    Debug.Assert(false);
                }
                else
                {
                    VisualBasicScript applicationLayerVisualBasicScript =
                        new VisualBasicScript(((ScriptSession)theScriptFileTag.ParentSession).ScriptSessionImplementation, theScriptFileTag.ScriptFileName);

                    applicationLayerVisualBasicScript.Execute();
                }

                // Update the UI. Do this with an invoke, because the thread that is calling this
                // method is NOT the thread that created all controls used!
                theScriptFileTag.ParentSession.IsExecute = false;
                Script script = (Script)_TagThatIsBeingExecuted;
                ((ScriptSession)script.ParentSession).CreateScriptFiles();
                _EndExecution = new EndExecution(_TagThatIsBeingExecuted);

                _TagThatIsBeingExecuted = null;

                _NotifyDelegate = new NotifyDelegate(ProjectForm.Notify);
                ProjectForm.Invoke(_NotifyDelegate, new object[] { _EndExecution });
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
                //                CustomExceptionHandler eh = new CustomExceptionHandler();
                //                System.Threading.ThreadExceptionEventArgs args = new ThreadExceptionEventArgs(ex);
                //                eh.OnThreadException(this, args);
                //
                // Rethrow. This rethrow may work in the future .NET 2.x CLR.
                // Currently eaten.
                //
                throw ex;
            }
        }


        /// <summary>
        /// Get the selected session.
        /// </summary>
        /// <returns>Selected session.</returns>
        public Session GetSelectedSessionNew()
        {
            Session tempSession = null;
            object selectedTag = GetSelectedTag();
            if (selectedTag is Project)
            {
                tempSession = ((Project)selectedTag).Sessions[0] as Session;
            }
            else
            {
                if (selectedTag is PartOfSession)
                {
                    PartOfSession partOfSession = selectedTag as PartOfSession;
                    tempSession = partOfSession.ParentSession;
                }
                else
                {
                    tempSession = (Session)selectedTag;
                }
            }

            return tempSession;
        }


        public TreeNode ExpandAndSelectResultFile(TreeNode theTreeNode, string theResultsFileName)
        {
            TreeNode theResult = null;
            foreach (TreeNode theSubTreeNode in theTreeNode.Nodes)
            {
                if (theSubTreeNode.Tag is Result)
                {
                    Result theResultsFileTag = (Result)theSubTreeNode.Tag;
                    string tempResultName = theSubTreeNode.Text.Substring(0, theSubTreeNode.Text.IndexOf(".xml"));
                    if ((theSubTreeNode.Text == theResultsFileName) || (theResultsFileName.StartsWith(tempResultName)))
                    {
                        // This is the correct results file node.
                        // Stop further searching.
                        theResult = theSubTreeNode;
                        break;
                    }
                }
                else
                {
                    theResult = ExpandAndSelectResultFile(theSubTreeNode, theResultsFileName);

                    if (theResult != null)
                    {
                        // The correct results file name is present in this sub node.
                        // Stop further searching.
                        theSubTreeNode.Expand();
                        break;
                    }
                }
            }

            if (theResult != null)
            {
                if (theTreeNode.Tag is Session)
                {
                    reloadHtml = false;
                    userControlTreeView.SelectedNode = theResult;
                    reloadHtml = true;
                }
            }
            return theResult;
        }


        public void SearchAndSelectHTMLNode(string theDirectory, string theResultsFileName)
        {
            TreeNode theResult = null;

            foreach (TreeNode theProjectNode in userControlTreeView.Nodes)
            {
                Project theProjectTag = theProjectNode.Tag as Project;
                foreach (TreeNode theSessionNode in theProjectNode.Nodes)
                {
                    Session theSessionTag = theSessionNode.Tag as Session;
                    if (theSessionTag is ScriptSession)
                    {
                        ScriptSession scriptSes = (ScriptSession)theSessionTag;
                        if (string.Compare(scriptSes.DescriptionDirectory, theDirectory, true) == 0)
                        {
                            foreach (TreeNode theSubTreeNode in theSessionNode.Nodes)
                            {
                                if (string.Compare(theSubTreeNode.Text, theResultsFileName, true) == 0)
                                {
                                    theResult = theSubTreeNode;
                                    break;
                                }
                            }

                            if (theResult != null)
                            {
                                if (theSessionNode.Tag is Session)
                                {
                                    reloadHtml = false;
                                    userControlTreeView.SelectedNode = theResult;
                                    reloadHtml = true;
                                }
                                break;
                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Search the correct results file node.
        /// When this is done, select the results file node.
        /// </summary>
        /// <param name="theResultsFileName">The filetheTreeNode name only of the results file.</param>
		public void SearchAndSelectResultNode(string theDirectory, string theResultsFileName)
        {
            TreeNode theResult = null;

            foreach (TreeNode theProjectNode in userControlTreeView.Nodes)
            {
                Project theProjectTag = theProjectNode.Tag as Project;
                foreach (TreeNode theSessionNode in theProjectNode.Nodes)
                {
                    Session theSessionTag = theSessionNode.Tag as Session;
                    if (string.Compare(theSessionTag.ResultsRootDirectory, theDirectory, true) == 0)
                    {
                        theResult = ExpandAndSelectResultFile(theSessionNode, theResultsFileName);
                        if (theResult != null)
                        {
                            break;
                        }
                    }
                }
            }
        }


        private void userControlTreeView_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            // Workaround, to make sure that the NumericUpDown controls _Leave method is called
            // (when selected) before a new session is selected in the session tree.
            if (ProjectForm.TabControl.SelectedTab == ProjectForm.TabSessionInformation)
            {
                ProjectForm.TabControl.SelectedTab.Focus();
            }
        }


        private ArrayList GetVisibleScripts(Session session)
        {
            ArrayList visibleScripts = new ArrayList();
            ScriptSession scriptSession = null;
            string scriptRootDirectory = "";
            DirectoryInfo directoryInfo = null;
            FileInfo[] filesInfo;

            scriptSession = (ScriptSession)session;
            scriptRootDirectory = scriptSession.DicomScriptRootDirectory;
            directoryInfo = new DirectoryInfo(scriptRootDirectory);

            if (directoryInfo.Exists)
            {
                filesInfo = directoryInfo.GetFiles();

                foreach (FileInfo fileInfo in filesInfo)
                {
                    bool showScriptFile = false;
                    string fileExtension = fileInfo.Extension.ToLower();

                    if ((fileExtension == ".ds") && (_MainForm._UserSettings.ShowDicomScripts))
                    {
                        showScriptFile = true;
                    }
                    else if ((fileExtension == ".dss") && (_MainForm._UserSettings.ShowDicomSuperScripts))
                    {
                        showScriptFile = true;
                    }
                    else if ((fileExtension == ".vbs") && (_MainForm._UserSettings.ShowVisualBasicScripts))
                    {
                        showScriptFile = true;
                    }
                    else
                    {
                        showScriptFile = false;
                    }

                    if (showScriptFile)
                    {
                        visibleScripts.Add(fileInfo.Name);
                    }
                }
            }

            return visibleScripts;
        }


        private static ArrayList GetFilesRecursively(DirectoryInfo directory)
        {
            // Get all the subdirectories
            FileSystemInfo[] infos = directory.GetFileSystemInfos();

            ArrayList allDCMFilesTemp = new ArrayList();
            foreach (FileSystemInfo f in infos)
            {
                if (f is FileInfo)
                {
                    if ((f.Extension.ToLower() == ".dcm") || (f.Extension == null) || (f.Extension == ""))
                    {
                        allDCMFilesTemp.Add(f.FullName);
                    }
                }
                else
                {
                    allDCMFilesTemp.AddRange(GetFilesRecursively((DirectoryInfo)f));
                }
            }
            return allDCMFilesTemp;
        }
    }
}