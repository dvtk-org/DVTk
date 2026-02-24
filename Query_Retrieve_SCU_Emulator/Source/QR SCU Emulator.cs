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
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.PropertyGridInternal;
using System.IO;
using System.Xml;
using System.Net;

using DvtkApplicationLayer.UserInterfaces;
using DvtkHighLevelInterface.Common.UserInterfaces;
using DvtkThreads = DvtkHighLevelInterface.Common.Threads;
using DvtkApplicationLayer;


namespace QR_SCU_Emulator
{
    public partial class QRSCUEmulator : Form
    {        
        private ArrayList patientList;
        private ArrayList studyList;

        DvtkThreads.ThreadManager threadManager;
        SCUDicomThread scuDicomThread;
        SCUStoreDicomThread scpStoreDicomThread;
        SCURetrieveDicomThread retrieveThread;

        string sesFile = Path.Combine(Application.StartupPath, "Query_SCU.ses");
        string styleSheet = Path.Combine(Application.StartupPath, "DVT_RESULTS.xslt");
        string configFile = Path.Combine(Application.StartupPath, "Network_Config.xml");

        const string patientRootQRFindSOP = "1.2.840.10008.5.1.4.1.2.1.1";
        const string studyRootQRFindSOP = "1.2.840.10008.5.1.4.1.2.2.1";
        const string patientRootQRMoveSOP = "1.2.840.10008.5.1.4.1.2.1.2";
        const string studyRootQRMoveSOP = "1.2.840.10008.5.1.4.1.2.2.2";
        string selectedQueryRootSop = patientRootQRFindSOP;

        //bool isCFindQuery = true;
        string patientId = string.Empty;
        string studyInstId = string.Empty;
        string seriesInstId = string.Empty;
        string imageInstId = string.Empty;
        string moveDest = string.Empty;
        string listenPort = string.Empty;
        string queryLevel = string.Empty;
        string modalityQueryAtt = string.Empty;
        string date = string.Empty;
        bool isStoreThreadRunning = false;

        //
        // - Entry point -
        //
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
#if !DEBUG
			try
			{
#endif
            // Checks the version of both the application and the DVTk library.
            // If one or both are a non-official or Alpha version, a warning message box is displayed.
            DvtkApplicationLayer.VersionChecker.CheckVersion();

            Application.Run(new QRSCUEmulator());

#if !DEBUG
			}
			catch(Exception exception)
			{
				CustomExceptionHandler.ShowThreadExceptionDialog(exception);
			}
#endif
        }

        public QRSCUEmulator()
        {
            InitializeComponent();

            studyDate.Enabled = false;
            studyDateCheck.CheckState = CheckState.Unchecked;
            studyDateCheck.CheckStateChanged += new EventHandler(studyDateCheck_CheckStateChanged);

            configurationTablePanel.Controls.Clear();
            splitContainer1.Panel1.Controls.Remove(configurationTablePanel);
            splitContainer1.Panel1.Controls.Remove(queryResultPanel);
            splitContainer1.Panel1.Controls.Remove(panelPatGrid);

            this.queryLevelComboBox.SelectedIndex = 0;

            //Create Dicom Thread
            threadManager = new DvtkThreads.ThreadManager();

            LoadSessionData();
        }


        /// <summary>
        /// Create SCUDicomThread just to load data on to the configuration UI
        /// </summary>
        private void LoadSessionData()
        {
            SCUDicomThread thread = new SCUDicomThread();
            thread.Initialize(threadManager);
            thread.Options.LoadFromFile(sesFile);

            aeTitScuText.Text = thread.Options.LocalAeTitle;
            aeTitScpText.Text = thread.Options.RemoteAeTitle;
            sutIPAddText.Text = thread.Options.RemoteHostName;
            configPortNoText.Text = thread.Options.RemotePort.ToString();

            //Get the IP Address of the current machine
            string strHostName = Dns.GetHostName();
            IPHostEntry ipEntry = Dns.GetHostEntry(strHostName);
            IPAddress[] addr = ipEntry.AddressList;

            ipAddLocalText.Text = addr[0].ToString();
            ipAddLocalText.ReadOnly = true;            
        }

        private void toolStripButtonConfig_Click(object sender, EventArgs e)
        {
            if (splitContainer1.Panel1.Controls.Contains(configurationTablePanel))
            {
                if (configurationTablePanel.Controls.Contains(systemConfigGroupBox))
                {
                    configurationTablePanel.Controls.Remove(systemConfigGroupBox);
                }
                else
                {
                    configurationTablePanel.Controls.Add(systemConfigGroupBox);
                }
            }
            else
            {
                if (splitContainer1.Panel1.Controls.Contains(userControlActivityLogging)) {
                    splitContainer1.Panel1.Controls.Remove(userControlActivityLogging);
                    splitContainer1.Panel1.Controls.Remove(clearLogButton);
                    buttonLog.Text = "Show Query Result";
                }
                else if (splitContainer1.Panel1.Controls.Contains(queryResultPanel))
                {
                    if (splitContainer1.Panel1.Controls.Contains(panelPatGrid))
                        splitContainer1.Panel1.Controls.Remove(panelPatGrid);
                    splitContainer1.Panel1.Controls.Remove(queryResultPanel);
                    buttonLog.Text = "Show Query Result";
                }                
                splitContainer1.Panel1.Controls.Add(configurationTablePanel);
                configurationTablePanel.Controls.Add(systemConfigGroupBox);   
            }
        }

        private void additionalKeysBut_Click(object sender, EventArgs e)
        {
            if (splitContainer1.Panel1.Controls.Contains(configurationTablePanel))
            {
                if (configurationTablePanel.Controls.Contains(queryFiltersGroupBox))
                {
                    configurationTablePanel.Controls.Remove(queryFiltersGroupBox);
                }
                else
                {
                    configurationTablePanel.Controls.Add(queryFiltersGroupBox);
                }
            }
            else
            {
                if (splitContainer1.Panel1.Controls.Contains(userControlActivityLogging))
                {
                    splitContainer1.Panel1.Controls.Remove(userControlActivityLogging);
                    splitContainer1.Panel1.Controls.Remove(clearLogButton);
                    buttonLog.Text = "Show Query Result";
                }
                else if (splitContainer1.Panel1.Controls.Contains(queryResultPanel))
                {
                    if (splitContainer1.Panel1.Controls.Contains(panelPatGrid))
                        splitContainer1.Panel1.Controls.Remove(panelPatGrid);
                    splitContainer1.Panel1.Controls.Remove(queryResultPanel);
                    buttonLog.Text = "Show Query Result";
                }                
                splitContainer1.Panel1.Controls.Add(configurationTablePanel);
                configurationTablePanel.Controls.Add(queryFiltersGroupBox);
            }
        }

        private void saveConfigBut_Click(object sender, EventArgs e)
        {
            XmlDocument doc = new XmlDocument();

            if (File.Exists(configFile))
            {
                doc.Load(configFile);

                XmlNode emulAETit = doc.DocumentElement.SelectSingleNode("EmulatorAETitle");
                emulAETit.InnerText = aeTitScuText.Text;

                XmlNode remoteSystems = doc.DocumentElement.SelectSingleNode("RemoteSystems");

                if (doc.DocumentElement.SelectNodes("//" + aeTitScpText.Text).Count == 0)
                {
                    XmlNode newRemoteSystem = CreateNewSystemNode(doc);
                    remoteSystems.AppendChild(newRemoteSystem);
                }
                else
                {
                    XmlNode remoteSystem = doc.DocumentElement.SelectSingleNode("//" + aeTitScpText.Text);

                    XmlNodeList nodes = remoteSystem.ChildNodes;

                    nodes[0].InnerText = sutIPAddText.Text;
                    nodes[1].InnerText = configPortNoText.Text;
                }
            }
            else
            {
                XmlElement root = doc.CreateElement("NetworkConfiguration");
                doc.AppendChild(root);

                XmlElement emulatorAetit = doc.CreateElement("EmulatorAETitle");
                emulatorAetit.InnerText = aeTitScuText.Text;
                root.AppendChild(emulatorAetit);


                XmlElement remoteSystems = doc.CreateElement("RemoteSystems");
                root.AppendChild(remoteSystems);

                XmlNode newRemoteSystem = CreateNewSystemNode(doc);
                remoteSystems.AppendChild(newRemoteSystem);
            }

            doc.Save(configFile);
        }

        private XmlNode CreateNewSystemNode(XmlDocument doc)
        {

            XmlElement remoteSystem = doc.CreateElement(aeTitScpText.Text);

            XmlElement ipAdd = doc.CreateElement("RemoteIPAddress");
            ipAdd.InnerText = sutIPAddText.Text;
            remoteSystem.AppendChild(ipAdd);

            XmlElement portNo = doc.CreateElement("RemotePortNumber");
            portNo.InnerText = configPortNoText.Text;
            remoteSystem.AppendChild(portNo);

            return remoteSystem;
        }

        private void loadConfigBut_Click(object sender, EventArgs e)
        {
            string selectedNode = string.Empty;
            if (File.Exists(configFile))
            {
                selectedNode = LoadConfiguration.showDialog();
            }

            if (!string.IsNullOrEmpty(selectedNode))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(configFile);

                XmlNode node = doc.DocumentElement.SelectSingleNode("//" + selectedNode);

                XmlNodeList nodes = node.ChildNodes;

                aeTitScpText.Text = selectedNode;
                sutIPAddText.Text = nodes[0].InnerText;
                configPortNoText.Text = nodes[1].InnerText;
            }
        }

        /// <summary>
        /// This method starts background thread which in turn starts scuDicomThread
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButtonDoQuery_Click(object sender, EventArgs e)
        {
            //If Do/Abort query button is clicked while query is in progress, we are stopping 
            //background thread and scuDicomThread
            if (backgroundWorker1.IsBusy)
            {
                if (backgroundWorker1.CancellationPending)
                {
                    MessageBox.Show("Aborting Query...Please Wait...", "Busy", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                else
                {
                    backgroundWorker1.CancelAsync();
                    threadManager.Stop();
                    while (scuDicomThread.ThreadState != DvtkThreads.ThreadState.Stopped)
                    {
                        this.Cursor = Cursors.WaitCursor;
                        scuDicomThread.Stop();
                    }
                    this.Cursor = Cursors.Default;
                    userControlActivityLogging.AddWriteActionToQueue("Stopped : ", "User aborted query", Color.Red);
                    toolStripButtonDoQuery.Text = "Do Query";
                }
            }
            else
            {
                if ((string)queryLevelComboBox.SelectedItem == "Patient_Root")
                {
                    selectedQueryRootSop = patientRootQRFindSOP;
                }
                else if ((string)queryLevelComboBox.SelectedItem == "Study_Root")
                {
                    selectedQueryRootSop = studyRootQRFindSOP;
                }
                
                if (splitContainer1.Panel1.Controls.Contains(configurationTablePanel)) 
                {
                    splitContainer1.Panel1.Controls.Remove(configurationTablePanel);
                }
                if (splitContainer1.Panel1.Controls.Contains(queryResultPanel))
                {
                    splitContainer1.Panel1.Controls.Remove(queryResultPanel);
                }
                if (splitContainer1.Panel1.Controls.Contains(panelPatGrid))
                    splitContainer1.Panel1.Controls.Remove(panelPatGrid);

                splitContainer1.Panel1.Controls.Add(clearLogButton);
                splitContainer1.Panel1.Controls.Add(userControlActivityLogging);
                userControlActivityLogging.Dock = DockStyle.Fill;
                buttonLog.Text = "Show Logfile";

                modalityQueryAtt = modalityComboBox.Text;
                if (studyDateCheck.CheckState == CheckState.Checked) { 
                    date = studyDate.Text;
                }
                if (!string.IsNullOrEmpty(date))
                {
                    StringBuilder dicomFormatDate = new StringBuilder(8);
                    string[] dateComp = date.Split('/');
                    dicomFormatDate.Append(dateComp[2]);
                    dicomFormatDate.Append(dateComp[0]);
                    dicomFormatDate.Append(dateComp[1]);
                    date = dicomFormatDate.ToString();
                }
                toolStripButtonDoQuery.Text = "Abort Query";
                toolStripButtonDoQuery.Image = global::QR_SCU_Emulator.Properties.Resources.delete_x_16;

                //Start background thread
                backgroundWorker1.RunWorkerAsync();

                statusLabel.Text = "Wait for Query results...";
                queryLevelComboBox.Enabled = false;
                patientIdTextBox.Enabled = false;
                patientNameTextBox.Enabled = false;
                toolStripButtonReSet.Enabled = false;
                buttonLog.Enabled = false;
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            String resultsFileBaseName = "QR_SCU_" + System.DateTime.Now.ToString("yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);

            scuDicomThread = new SCUDicomThread();

            SetQueryAttributes();
            
            scuDicomThread.Initialize(threadManager);
            userControlActivityLogging.Attach(scuDicomThread);

            scuDicomThread.Options.Identifier = resultsFileBaseName;

            //Load Session file
            scuDicomThread.Options.LoadFromFile(sesFile);

            scuDicomThread.Options.DataDirectory = scuDicomThread.Options.ResultsDirectory;

            //Set UI Value
            SetConfiguration();
            scuDicomThread.Options.SaveToFile(sesFile);

            scuDicomThread.Start();

            scuDicomThread.WaitForCompletion();
        }

        private void SetConfiguration()
        {
            scuDicomThread.Options.LocalAeTitle = aeTitScuText.Text;
            scuDicomThread.Options.RemoteAeTitle = aeTitScpText.Text;
            scuDicomThread.Options.RemoteHostName = sutIPAddText.Text;
            scuDicomThread.Options.RemotePort = ushort.Parse(configPortNoText.Text);
        }

        private void SetQueryAttributes() 
        {
            //Set query attributes
            scuDicomThread.PatientName = patientNameTextBox.Text;
            scuDicomThread.PatientId = patientIdTextBox.Text;
            scuDicomThread.QueryRoot = selectedQueryRootSop;
            scuDicomThread.Modality = modalityQueryAtt;
            scuDicomThread.AccessionNumber = accNoText.Text;
            scuDicomThread.StudyId = studyIdText.Text;
            
            scuDicomThread.StudyDate = date;
        }

        private void backgroundWorker1_RunWorkerCompleted_1(object sender, RunWorkerCompletedEventArgs e)
        {
            //Clear status message
            statusLabel.Text = "";

            ClearDataGrids();

            string statusResponseMsg = "Errors (" + scuDicomThread.NrOfErrors + ")\n" +
                "Warnings (" + scuDicomThread.NrOfWarnings + ")";

            richTextBox1.Text = statusResponseMsg;
            if (scuDicomThread.NrOfErrors > 0)
            {
                richTextBox1.ForeColor = Color.Red;
            }
            else
            {
                richTextBox1.ForeColor = Color.Black;
            }

            this.toolStripButtonDoQuery.Image = global::QR_SCU_Emulator.Properties.Resources.search_16;
            toolStripButtonDoQuery.Text = "Do Query";
            queryLevelComboBox.Enabled = true;
            patientIdTextBox.Enabled = true;
            patientNameTextBox.Enabled = true;
            toolStripButtonReSet.Enabled = true;
            buttonLog.Enabled = true;
            panelPatGrid.Visible = true;

            if (selectedQueryRootSop == patientRootQRFindSOP)
            {
                patientList = scuDicomThread.PatientList;

                //fill the patient datagridview (table of patients)
                dataGridViewPatient.DataBindings.Clear();
                dataGridViewPatient.DataSource = patientList;

                if (patientList.Count != 0)
                {
                    splitContainer1.Panel1.Controls.Remove(userControlActivityLogging);
                    splitContainer1.Panel1.Controls.Remove(clearLogButton);

                    if (!splitContainer1.Panel1.Controls.Contains(panelPatGrid))
                        splitContainer1.Panel1.Controls.Add(panelPatGrid);

                    if (!splitContainer1.Panel1.Controls.Contains(queryResultPanel))
                        splitContainer1.Panel1.Controls.Add(queryResultPanel);

                    panelPatGrid.Dock = DockStyle.Top;
                    queryResultPanel.Dock = DockStyle.Fill;
                    queryResultPanel.BringToFront();
                }                
            }
            else
            {
                //fill the study datagridview 
                studyList = scuDicomThread.StudyList;

                if (studyList != null)
                {
                    dataGridViewStudy.DataSource = scuDicomThread.StudyList;

                    if (scuDicomThread.StudyList.Count != 0)
                    {
                        splitContainer1.Panel1.Controls.Remove(userControlActivityLogging);
                        splitContainer1.Panel1.Controls.Remove(clearLogButton);
                        splitContainer1.Panel1.Controls.Remove(panelPatGrid);

                        if (!splitContainer1.Panel1.Controls.Contains(queryResultPanel))
                            splitContainer1.Panel1.Controls.Add(queryResultPanel);
                    }
                }
            }

            if ((scuDicomThread.PatientList.Count == 0) || (scuDicomThread.StudyList.Count == 0))
            {
                richTextBox1.AppendText("\n" + "\n" + "NO MATCH FOUND");
            }            
        }

        private void dataGridViewPatient_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                Patient _tempP = (Patient)patientList[e.RowIndex];
                dataGridViewStudy.DataBindings.Clear();
                dataGridViewStudy.DataSource = _tempP.studyList;
                propertyGrid1.SelectedObject = _tempP;

                dataGridViewSeries.DataSource = null;
                dataGridViewImage.DataSource = null;
            }
        }

        private void dataGridViewStudy_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                ArrayList _aList = (ArrayList)dataGridViewStudy.DataSource;
                Study _temp_study = (Study)_aList[e.RowIndex];
                dataGridViewSeries.DataSource = _temp_study.seriesList;

                propertyGrid1.SelectedObject = _temp_study;

                dataGridViewImage.DataSource = null;
            }
        }

        private void dataGridViewSeries_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                ArrayList _aList = (ArrayList)dataGridViewSeries.DataSource;
                Series _temp_series = (Series)_aList[e.RowIndex];
                dataGridViewImage.DataSource = _temp_series.imageList;

                propertyGrid1.SelectedObject = _temp_series;
            }
        }

        private void dataGridViewImage_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                ArrayList _aList = (ArrayList)dataGridViewImage.DataSource;
                Image _temp_image = (Image)_aList[e.RowIndex];

                propertyGrid1.SelectedObject = _temp_image;
            }
        }

        private void clearLogButton_Click(object sender, EventArgs e)
        {
            DialogResult msgBoxRes = MessageBox.Show("Do you want to save the current logging to a file?", "Save Logs", MessageBoxButtons.YesNoCancel);

            if (msgBoxRes == DialogResult.Yes)
            {
                SaveFileDialog sfd = new SaveFileDialog();

                sfd.DefaultExt = ".txt";
                sfd.Filter = "Text file (*.txt)|*.txt";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    string logs = userControlActivityLogging.LoggingText;
                    try
                    {
                        StreamWriter sw = new StreamWriter(sfd.FileName);
                        sw.Write(logs);
                        sw.Close();
                    }
                    catch (IOException ioe)
                    {
                        MessageBox.Show("Can't save.." + ioe.ToString(), "Error", MessageBoxButtons.OK);
                    }

                    userControlActivityLogging.Clear();
                    //clearLogButton.Enabled = false;
                }                
            }
            else if (msgBoxRes == DialogResult.No)
            {
                userControlActivityLogging.Clear();
                //clearLogButton.Enabled = false;
            }
            else if (msgBoxRes == DialogResult.Cancel)
            {
                return;
            }
        }

        private void buttonLog_Click(object sender, EventArgs e)
        {
            if (splitContainer1.Panel1.Controls.Contains(queryResultPanel)) 
            {
                if (splitContainer1.Panel1.Controls.Contains(panelPatGrid))
                    splitContainer1.Panel1.Controls.Remove(panelPatGrid);
                splitContainer1.Panel1.Controls.Remove(queryResultPanel);
                splitContainer1.Panel1.Controls.Add(clearLogButton);
                splitContainer1.Panel1.Controls.Add(userControlActivityLogging);
                userControlActivityLogging.Dock = DockStyle.Fill;
                buttonLog.Text = "Show Query Result";
            }
            else if (splitContainer1.Panel1.Controls.Contains(userControlActivityLogging) &&
                    buttonLog.Text != "Show Logfile"
                ) 
            {
                splitContainer1.Panel1.Controls.Remove(clearLogButton);
                splitContainer1.Panel1.Controls.Remove(userControlActivityLogging);
                splitContainer1.Panel1.Controls.Add(queryResultPanel);
                splitContainer1.Panel1.Controls.Add(panelPatGrid);
                buttonLog.Text = "Show Logfile";
            }
            else if (splitContainer1.Panel1.Controls.Contains(configurationTablePanel))
            {
                splitContainer1.Panel1.Controls.Remove(configurationTablePanel);
                splitContainer1.Panel1.Controls.Add(queryResultPanel);
                splitContainer1.Panel1.Controls.Add(panelPatGrid);
                buttonLog.Text = "Show Logfile";
            }
            else 
            {
                splitContainer1.Panel1.Controls.Add(clearLogButton);
                splitContainer1.Panel1.Controls.Add(userControlActivityLogging);
                userControlActivityLogging.Dock = DockStyle.Fill;
            }
        }

        private void ClearDataGrids()
        {
            dataGridViewPatient.DataSource = null;
            dataGridViewStudy.DataSource = null;
            dataGridViewSeries.DataSource = null;
            dataGridViewImage.DataSource = null;
        }

        private void studyDateCheck_CheckStateChanged(object sender, EventArgs e)
        {
            if (studyDateCheck.CheckState == CheckState.Checked)
            {
                studyDate.Enabled = true;
            }
            else 
            {
                studyDate.Enabled = false;
            }
        }

        private void ClearFilterButton_Click(object sender, EventArgs e)
        {
            studyDateCheck.CheckState = CheckState.Unchecked;
            studyIdText.Text = "";
            accNoText.Text = "";
        }

        private void toolStripButtonReSet_Click(object sender, EventArgs e)
        {
            ClearDataGrids();
            richTextBox1.Text = "";
            ClearFilterButton_Click(null, null);
            propertyGrid1.SelectedObject = null;
            if (splitContainer1.Panel1.Controls.Contains(queryResultPanel))
            {
                if (splitContainer1.Panel1.Controls.Contains(panelPatGrid))
                    splitContainer1.Panel1.Controls.Remove(panelPatGrid);
                splitContainer1.Panel1.Controls.Remove(queryResultPanel);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm about = new AboutForm("DVTk QR SCU Emulator");
            about.ShowDialog();
        }

        private void sendCMOVERQMenuItem_Click(object sender, EventArgs e)
        {
            if ((string)queryLevelComboBox.SelectedItem == "Patient_Root")
            {
                selectedQueryRootSop = patientRootQRMoveSOP;
            }
            else if ((string)queryLevelComboBox.SelectedItem == "Study_Root")
            {
                selectedQueryRootSop = studyRootQRMoveSOP;
            }

            if (splitContainer1.Panel1.Controls.Contains(configurationTablePanel))
            {
                splitContainer1.Panel1.Controls.Remove(configurationTablePanel);
            }
            if (splitContainer1.Panel1.Controls.Contains(queryResultPanel))
            {
                splitContainer1.Panel1.Controls.Remove(queryResultPanel);
            }
            if (splitContainer1.Panel1.Controls.Contains(panelPatGrid))
                splitContainer1.Panel1.Controls.Remove(panelPatGrid);

            splitContainer1.Panel1.Controls.Add(clearLogButton);
            splitContainer1.Panel1.Controls.Add(userControlActivityLogging);
            userControlActivityLogging.Dock = DockStyle.Fill;
            buttonLog.Text = "Show Query Result";

            SelectMode mode = new SelectMode();
            mode.MoveDestination = aeTitScuText.Text;
            if (mode.ShowDialog() == DialogResult.OK)
            {
                moveDest = mode.MoveDestination;
                listenPort = mode.LocalPortCMove;
            }

            //Check for Move as Storage SCP
            if (moveDest.Trim() == aeTitScuText.Text.Trim())
            {
                scpStoreDicomThread = new SCUStoreDicomThread("QR_SCU_STORE_SUBOPERATION");

                scpStoreDicomThread.Initialize(threadManager);
                userControlActivityLogging.Attach(scpStoreDicomThread);

                //String resultsBaseName = "QR_SCU_STORE_SUBOPERATION_" + System.DateTime.Now.ToString("yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
                scpStoreDicomThread.Options.Identifier = "QR_SCU_STORE_SUBOPERATION";
                scpStoreDicomThread.Options.AutoValidate = false;
                //scpStoreDicomThread.Options.LogThreadStartingAndStoppingInParent = false;
                //scpStoreDicomThread.Options.LogWaitingForCompletionChildThreads = false;

                // Load the Emulator session
                //this.scpStoreDicomThread.Options.LoadFromFile(Path.Combine(Application.StartupPath, "Store_SCP.ses"));

                scpStoreDicomThread.Options.LocalAeTitle = aeTitScuText.Text;
                scpStoreDicomThread.Options.LocalPort = ushort.Parse(listenPort);
                scpStoreDicomThread.Options.ResultsDirectory = scuDicomThread.Options.ResultsDirectory;
                scpStoreDicomThread.Options.DataDirectory = scuDicomThread.Options.ResultsDirectory;

                scpStoreDicomThread.Start();

                isStoreThreadRunning = true;
            }

            Thread.Sleep(500);

            //Run background thread
            backgroundWorkerRetreive.RunWorkerAsync();

            statusLabel.Text = "Wait for Move results...";
            queryLevelComboBox.Enabled = false;
            patientIdTextBox.Enabled = false;
            patientNameTextBox.Enabled = false;
            toolStripButtonReSet.Enabled = false;
            buttonLog.Enabled = false;
        }

        private void backgroundWorkerRetreive_DoWork(object sender, DoWorkEventArgs e)
        {
            Hashtable keys = new Hashtable();
            keys.Add("PatientId", patientId);
            keys.Add("StudyInstanceUID", studyInstId);
            keys.Add("SeriesUID", seriesInstId);
            keys.Add("SopInstanceUID", imageInstId);
            keys.Add("QueryRoot", selectedQueryRootSop);
            keys.Add("QueryLevel", queryLevel);
            keys.Add("MoveDestination", moveDest);

            retrieveThread = new SCURetrieveDicomThread(keys);

            String resultsBaseName = "QR_SCU_MOVE_OPERATION_" + System.DateTime.Now.ToString("yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
            retrieveThread.Initialize(threadManager);
            userControlActivityLogging.Attach(retrieveThread);

            retrieveThread.Options.Identifier = resultsBaseName;

            retrieveThread.Options.DataDirectory = scuDicomThread.Options.ResultsDirectory;

            retrieveThread.Options.LocalAeTitle = aeTitScuText.Text;
            retrieveThread.Options.RemoteAeTitle = aeTitScpText.Text;
            retrieveThread.Options.RemoteHostName = sutIPAddText.Text;
            retrieveThread.Options.RemotePort = ushort.Parse(configPortNoText.Text);

            retrieveThread.Start();

            retrieveThread.WaitForCompletion();            
        }

        private void backgroundWorkerRetreive_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //Clear status message
            statusLabel.Text = "";

            queryLevelComboBox.Enabled = true;
            patientIdTextBox.Enabled = true;
            patientNameTextBox.Enabled = true;
            toolStripButtonReSet.Enabled = true;
            buttonLog.Enabled = true;
            panelPatGrid.Visible = true;

            string statusResponseMsg = "Completed Operations (" + retrieveThread.CompleteSubOperations + ")\n" +
                "Failed Operations (" + retrieveThread.FailedSubOperations + ")\n" +
                "Remaining Operations (" + retrieveThread.RemainingSubOperations + ")\n" +
                "Warning Operations (" + retrieveThread.WarningSubOperations + ")";

            richTextBox1.Text = statusResponseMsg;

            if ((isStoreThreadRunning) && (scpStoreDicomThread.ThreadState != DvtkThreads.ThreadState.Stopped))
            {
                scpStoreDicomThread.Stop();
                isStoreThreadRunning = false;
            }
        }

        private void dataGridViewPatient_MouseDown(object sender, MouseEventArgs e)
        {
            DataGridView.HitTestInfo theHitTestInfo;

            if (e.Button == MouseButtons.Right)
            {
                // Find out what part of the data grid is below the mouse pointer.
                theHitTestInfo = dataGridViewPatient.HitTest(e.X, e.Y);

                switch (theHitTestInfo.Type)
                {
                    case DataGridViewHitTestType.Cell:
                        {
                            Patient pat = (Patient)patientList[theHitTestInfo.RowIndex];
                            patientId = pat.PatientId;
                            queryLevel = "PATIENT";
                            break;
                        }
                }
            }
        }

        private void dataGridViewStudy_MouseDown(object sender, MouseEventArgs e)
        {
            DataGridView.HitTestInfo theHitTestInfo;

            if (e.Button == MouseButtons.Right)
            {
                // Find out what part of the data grid is below the mouse pointer.
                theHitTestInfo = dataGridViewStudy.HitTest(e.X, e.Y);

                switch (theHitTestInfo.Type)
                {
                    case DataGridViewHitTestType.Cell:
                        {
                            if ((string)queryLevelComboBox.SelectedItem == "Patient_Root")
                            {
                                //Study study = (Study)scuDicomThread.StudyList[theHitTestInfo.RowIndex];
                                ArrayList _aList = (ArrayList)dataGridViewStudy.DataSource;
                                Study study = (Study)_aList[theHitTestInfo.RowIndex];
                                for (int i = 0; i < patientList.Count; i++)
                                {
                                    Patient patient = (Patient)patientList[i];
                                    ArrayList stdyList = patient.studyList;
                                    for (int j = 0; j < stdyList.Count; j++)
                                    {
                                        Study stdy = (Study)stdyList[j];
                                        string stdInsUid = stdy.StudyInstanceUID;
                                        string stdId = stdy.StudyID;
                                        if ((stdInsUid == study.StudyInstanceUID) || (stdId == study.StudyID))
                                        {
                                            patientId = patient.PatientId;
                                            studyInstId = stdy.StudyInstanceUID;
                                            break;
                                        }
                                    }
                                }
                            }
                            else if ((string)queryLevelComboBox.SelectedItem == "Study_Root")
                            {
                                Study study = (Study)studyList[theHitTestInfo.RowIndex];
                                studyInstId = study.StudyInstanceUID;
                            }

                            queryLevel = "STUDY";
                            break;
                        }
                }
            }
        }

        private void dataGridViewSeries_MouseDown(object sender, MouseEventArgs e)
        {
            DataGridView.HitTestInfo theHitTestInfo;

            if (e.Button == MouseButtons.Right)
            {
                // Find out what part of the data grid is below the mouse pointer.
                theHitTestInfo = dataGridViewSeries.HitTest(e.X, e.Y);

                switch (theHitTestInfo.Type)
                {
                    case DataGridViewHitTestType.Cell:
                        {
                            //Series series = (Series)scuDicomThread.SeriesList[theHitTestInfo.RowIndex];
                            ArrayList _aList = (ArrayList)dataGridViewSeries.DataSource;
                            Series series = (Series)_aList[theHitTestInfo.RowIndex];
                            if ((string)queryLevelComboBox.SelectedItem == "Patient_Root")
                            {
                                for (int i = 0; i < patientList.Count; i++)
                                {
                                    Patient patient = (Patient)patientList[i];
                                    ArrayList stdyList = patient.studyList;
                                    for (int j = 0; j < stdyList.Count; j++)
                                    {
                                        Study stdy = (Study)stdyList[j];
                                        ArrayList serieslist = stdy.seriesList;
                                        for (int k = 0; k < serieslist.Count; k++)
                                        {
                                            Series seri = (Series)serieslist[k];
                                            if (seri.SeriesUID == series.SeriesUID)
                                            {
                                                patientId = patient.PatientId;
                                                studyInstId = stdy.StudyInstanceUID;
                                                seriesInstId = seri.SeriesUID;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            else if ((string)queryLevelComboBox.SelectedItem == "Study_Root")
                            {
                                for (int i = 0; i < studyList.Count; i++)
                                {
                                    Study stdy = (Study)studyList[i];
                                    ArrayList serieslist = stdy.seriesList;
                                    for (int j = 0; j < serieslist.Count; j++)
                                    {
                                        Series seri = (Series)serieslist[j];
                                        if (seri.SeriesUID == series.SeriesUID)
                                        {
                                            studyInstId = stdy.StudyInstanceUID;
                                            seriesInstId = seri.SeriesUID;
                                            break;
                                        }
                                    }
                                }
                            }

                            queryLevel = "SERIES";
                            break;
                        }
                }
            }
        }

        private void dataGridViewImage_MouseDown(object sender, MouseEventArgs e)
        {
            DataGridView.HitTestInfo theHitTestInfo;

            if (e.Button == MouseButtons.Right)
            {
                // Find out what part of the data grid is below the mouse pointer.
                theHitTestInfo = dataGridViewImage.HitTest(e.X, e.Y);

                switch (theHitTestInfo.Type)
                {
                    case DataGridViewHitTestType.Cell:
                        {
                            //Image image = (Image)scuDicomThread.ImageList[theHitTestInfo.RowIndex];
                            ArrayList _aList = (ArrayList)dataGridViewImage.DataSource;
                            Image image = (Image)_aList[theHitTestInfo.RowIndex];
                            if ((string)queryLevelComboBox.SelectedItem == "Patient_Root")
                            {
                                for (int i = 0; i < patientList.Count; i++)
                                {
                                    Patient patient = (Patient)patientList[i];
                                    ArrayList stdyList = patient.studyList;
                                    for (int j = 0; j < stdyList.Count; j++)
                                    {
                                        Study stdy = (Study)stdyList[j];
                                        ArrayList serieslist = stdy.seriesList;
                                        for (int k = 0; k < serieslist.Count; k++)
                                        {
                                            Series seri = (Series)serieslist[k];
                                            ArrayList imglist = seri.imageList;
                                            for (int l = 0; l < imglist.Count; l++)
                                            {
                                                Image img = (Image)imglist[l];
                                                if (image.SOPInstanceUID == img.SOPInstanceUID)
                                                {
                                                    patientId = patient.PatientId;
                                                    studyInstId = stdy.StudyInstanceUID;
                                                    seriesInstId = seri.SeriesUID;
                                                    imageInstId = img.SOPInstanceUID;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else if ((string)queryLevelComboBox.SelectedItem == "Study_Root")
                            {
                                for (int i = 0; i < studyList.Count; i++)
                                {
                                    Study stdy = (Study)studyList[i];
                                    ArrayList serieslist = stdy.seriesList;
                                    for (int j = 0; j < serieslist.Count; j++)
                                    {
                                        Series seri = (Series)serieslist[j];
                                        ArrayList imglist = seri.imageList;
                                        for (int l = 0; l < imglist.Count; l++)
                                        {
                                            Image img = (Image)imglist[l];
                                            if (image.SOPInstanceUID == img.SOPInstanceUID)
                                            {
                                                studyInstId = stdy.StudyInstanceUID;
                                                seriesInstId = seri.SeriesUID;
                                                imageInstId = img.SOPInstanceUID;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }

                            queryLevel = "IMAGE";
                            break;
                        }
                }
            }
        }        
    }
}