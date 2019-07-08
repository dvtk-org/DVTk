
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using DvtkHighLevelInterface.Dicom.Other;
using System.Net.NetworkInformation;
using DvtkHighLevelInterface.Dicom.Threads;
using DvtkData.Dimse;
using DvtkHighLevelInterface.Common.Threads;

namespace DvtkHighLevelInterface.Dicom.UserInterfaces
{
    /// <summary>
    /// Generic AE title registration control. This will give u a list of AE titles and It will store it in a xml file in the given location.
    /// </summary>
    public partial class AERegistrationControl : UserControl
    {
        private ThreadManager threadManager = null;
        public delegate void ShowEchoResults(string results,bool Success);
        public ShowEchoResults OnShowEchoResults;
        public AERegistrationControl()
        {
            InitializeComponent();
        }
        public List<DICOMPeer> RegisteredPeers
        {
            get { return peers; }
        }
        public DicomThreadOptions options = null;

        public void Initialize(string FilePath, DicomThreadOptions Options)
        {
            options = Options;
            Initialize(FilePath);
        }


        /// <summary>
        /// Initialize the Control with settings file path
        /// </summary>
        /// <param name="FilePath"></param>
        
        public void Initialize(string FilePath)
        {
            fileLocation = new FileInfo(FilePath);
            if (fileLocation.Exists)
            {
                peers = DICOMPeer.ReadFromFile(fileLocation);
                LoadDataInListview();
            }
            else
            {
                if (!fileLocation.Directory.Exists)
                    fileLocation.Directory.Create();

            }
            threadManager = new ThreadManager();
            
        }

        void LoadDataInListview()
        {
            registeredPeersGrid.Items.Clear();
            for (int i = 0; i < peers.Count; i++)
            {
                registeredPeersGrid.Items.Add(new ListViewItem(new string[] { peers[i].Name, peers[i].AE, peers[i].IP, peers[i].Port.ToString() }));
            }
        }
        
        List<DICOMPeer> peers = new List<DICOMPeer>();
        private FileInfo fileLocation;

        private void addButton_Click(object sender, EventArgs e)
        {
            if (addButton.Text == "Add")
            {
                if (peerName.Text.Trim() != "" && peerAE.Text.Trim() != "" && peerIp.Text.Trim() != "" && peerPort.Text.Trim() != "")
                {
                    for (int i = 0; i < peers.Count; i++)
                    {
                        if (peers[i].AE == peerAE.Text.Trim())
                        {
                            MessageBox.Show("AE Title already exists");
                            return;
                        }
                    }
                    DICOMPeer peer = new DICOMPeer();
                    peer.Name = peerName.Text.Trim();
                    peer.AE = peerAE.Text.Trim();
                    peer.IP = peerIp.Text.Trim();
                    ushort.TryParse(peerPort.Text.Trim(), out peer.Port);
                    peers.Add(peer);
                    LoadDataInListview();
                    ClearAllFields();
                    DICOMPeer.WriteIntoFile(fileLocation.FullName, peers);
                }
                else
                {
                    MessageBox.Show("All the fields are mandatory");
                }
            }
            else
            {
                if (peerName.Text.Trim() != "" && peerAE.Text.Trim() != "" && peerIp.Text.Trim() != "" && peerPort.Text.Trim() != "")
                {
                    for (int i = 0; i < peers.Count; i++)
                    {
                        if (i!=editIndex&&peers[i].AE == peerAE.Text.Trim())
                        {
                            MessageBox.Show("AE Title already exists");
                            return;
                        }
                    }
                    peers.RemoveAt(editIndex);
                    DICOMPeer peer = new DICOMPeer();
                    peer.Name = peerName.Text.Trim();
                    peer.AE = peerAE.Text.Trim();
                    peer.IP = peerIp.Text.Trim();
                    ushort.TryParse(peerPort.Text.Trim(), out peer.Port);
                    peers.Insert(editIndex,peer);
                    LoadDataInListview();
                    ClearAllFields();
                    addButton.Text = "Add";
                    newPeerGroup.Text = "New Peer";
                    DICOMPeer.WriteIntoFile(fileLocation.FullName, peers);
                }
                else
                {
                    MessageBox.Show("All the fields are mandatory");
                }
            }
        }
        private void ClearAllFields()
        {
            peerName.Text = "";
            peerAE.Text = "";
            peerIp.Text = "";
            peerPort.Text = "";
        }

        private void OnRemove(object sender, EventArgs e)
        {
            if (registeredPeersGrid.SelectedIndices.Count > 0)
            {
                peers.RemoveAt(registeredPeersGrid.SelectedIndices[0]);
                DICOMPeer.WriteIntoFile(fileLocation.FullName, peers);
                LoadDataInListview();
            }
        }

        private void OnEditClick(object sender, EventArgs e)
        {
            if(registeredPeersGrid.SelectedIndices.Count>0)
            {
                if (peerName.Text.Trim() == "" && peerAE.Text.Trim() == "" && peerIp.Text.Trim() == "" && peerPort.Text.Trim() == "")
                {
                    editIndex = registeredPeersGrid.SelectedIndices[0];
                    peerName.Text = peers[editIndex].Name;
                    peerAE.Text = peers[editIndex].AE;
                    peerIp.Text = peers[editIndex].IP;
                    peerPort.Text = peers[editIndex].Port.ToString();
                    addButton.Text = "Save";
                    newPeerGroup.Text = "Edit Peer";
                }
                else
                    MessageBox.Show("Please Save or Cancel the information entered"); 

            }
        }
        int editIndex = -1;

        private void OnCancel(object sender, EventArgs e)
        {
            if (addButton.Text == "Add")
            {
                ClearAllFields();
            }
            else
            {
                ClearAllFields();
                addButton.Text = "Add";
                newPeerGroup.Text = "New Peer";
            }
        }

        private void pingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (registeredPeersGrid.SelectedIndices.Count > 0)
            {
                int selectedIndex = registeredPeersGrid.SelectedIndices[0];

                #region Commented
                string msg = "";
                PingReply reply = null;
                bool ok = false;
                if ((peers[selectedIndex].IP != null) && (peers[selectedIndex].IP.Length != 0))
                {
                    string ipAddr = "";
                    try
                    {
                        ipAddr = peers[selectedIndex].IP.Trim();

                        Ping pingSender = new Ping();
                        reply = pingSender.Send(ipAddr, 4);
                    }
                    catch (PingException exp)
                    {
                        msg = string.Format("Error in pinging to {0} due to exception:{1}", ipAddr, exp.Message);
                    }
                }
                else
                    msg = "Pl Specify valid IP Address.";

                if (reply != null)
                {
                    switch (reply.Status)
                    {
                        case IPStatus.Success:
                            msg = "Ping successful.";
                            ok = true;
                            break;
                        case IPStatus.TimedOut:
                            msg = "Ping Timeout.";
                            break;
                        case IPStatus.IcmpError:
                            msg = "The ICMP echo request failed because of an ICMP protocol error.";
                            break;
                        case IPStatus.BadRoute:
                            msg = "The ICMP echo request failed because there is no valid route between the source and destination computers.";
                            break;
                        case IPStatus.DestinationProhibited:
                            msg = "The ICMP echo request failed because contact with the destination computer is administratively prohibited.";
                            break;
                        case IPStatus.DestinationNetworkUnreachable:
                        case IPStatus.DestinationHostUnreachable:
                        case IPStatus.DestinationPortUnreachable:
                        case IPStatus.DestinationUnreachable:
                            msg = "Destination host Unreachable.";
                            break;
                        case IPStatus.Unknown:
                            msg = "The ICMP echo request failed for an unknown reason.";
                            break;
                    }
                }

                if (ok)
                    MessageBox.Show(msg, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                {
                    MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                #endregion

            }
            else
                Console.Beep();

            
        }

        private void dICOMEchoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string detailedEchoFileName = "";
            if (registeredPeersGrid.SelectedIndices.Count > 0)
            {
                int selectedIndex = registeredPeersGrid.SelectedIndices[0];

                //isStorageEcho = true;
                allThreadsFinished = false;
                if ((peers[selectedIndex].IP == null) || (peers[selectedIndex].IP.Length == 0))
                {
                    MessageBox.Show("Pl Specify SCP IP Address.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                //Update DVT & SUT settings
                OverviewThread overviewThread = new OverviewThread();
                overviewThread.Initialize(threadManager);
                overviewThread.Options.DeepCopyFrom(options);
                overviewThread.Options.Identifier = "Move_Destinations";
                overviewThread.Options.AttachChildsToUserInterfaces = true;
                overviewThread.Options.LogThreadStartingAndStoppingInParent = false;
                overviewThread.Options.LogWaitingForCompletionChildThreads = false;
                String resultsFileName = "MoveDestinations_" + System.DateTime.Now.ToString("yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
                overviewThread.Options.ResultsFileNameOnlyWithoutExtension = resultsFileName;
                

                SCU echoScu = new SCU();
                echoScu.Initialize(overviewThread);
                echoScu.Options.DeepCopyFrom(options);

                String resultsFileBaseName = "MoveDestinationsEcho_" + System.DateTime.Now.ToString("yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
                echoScu.Options.ResultsFileNameOnlyWithoutExtension = resultsFileBaseName;
                echoScu.Options.Identifier = resultsFileBaseName;

                echoScu.Options.LogThreadStartingAndStoppingInParent = false;
                echoScu.Options.LogWaitingForCompletionChildThreads = false;
                echoScu.Options.AutoValidate = false;
                echoScu.Options.ResultsDirectory = options.ResultsDirectory;

                //echoScu.Options.LocalAeTitle ="SCU";
                echoScu.Options.RemoteAeTitle = peers[selectedIndex].AE;
                echoScu.Options.RemotePort = peers[selectedIndex].Port;
                echoScu.Options.RemoteHostName = peers[selectedIndex].IP;
                //this.userControlActivityLogging.Attach(echoScu);

                detailedEchoFileName = echoScu.Options.DetailResultsFullFileName;
                PresentationContext presentationContext = new PresentationContext("1.2.840.10008.1.1", // Abstract Syntax Name
                                                                                "1.2.840.10008.1.2"); // Transfer Syntax Name(s)
                PresentationContext[] presentationContexts = new PresentationContext[1];
                presentationContexts[0] = presentationContext;

                DvtkHighLevelInterface.Dicom.Messages.DicomMessage echoMessage = new DvtkHighLevelInterface.Dicom.Messages.DicomMessage(DvtkData.Dimse.DimseCommand.CECHORQ);
                
                echoScu.Start();

                bool sendResult = echoScu.TriggerSendAssociationAndWait(echoMessage, presentationContexts);
                
                if (!sendResult)
                {
                    MessageBox.Show("DICOM Echo failed ","Failed",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
                else
                    MessageBox.Show("DICOM Echo successful", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information); 
               
                echoScu.Stop();
                if (OnShowEchoResults != null)
                {
                    OnShowEchoResults.Invoke(detailedEchoFileName,sendResult);
                }
                allThreadsFinished=false;

            }
        }
        static bool allThreadsFinished = true;
        private class OverviewThread : DicomThread
        {
            public OverviewThread() { }

            protected override void Execute()
            {
                //
                // Keep looping until the last child thread has been stopped.
                // 
                bool endLoop = false;
                Object lockObject = new Object();
                while (!endLoop)
                {
                    Sleep(500);

                    lock (lockObject)
                    {
                        endLoop = allThreadsFinished;
                    }
                }
            }
        }               
    }



}
