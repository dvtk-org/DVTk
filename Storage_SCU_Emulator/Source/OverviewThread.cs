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

using DvtkHighLevelInterface.Common.UserInterfaces;
using DvtkHighLevelInterface.Dicom.Messages;
using DvtkHighLevelInterface.Dicom.Other;
using DvtkHighLevelInterface.Dicom.Threads;
using DvtkHighLevelInterface.InformationModel;
using DvtkHighLevelInterface.Dicom.Files;
using Attribute = DvtkHighLevelInterface.Dicom.Other.Attribute;
using Dvtk.DvtkDicomEmulators.StorageMessageHandlers;
using Dvtk.DvtkDicomEmulators.StorageCommitMessageHandlers;
using Dvtk.DvtkDicomEmulators.StorageClientServers;
using Dvtk.DvtkDicomEmulators.Bases;
using DvtkHighLevelInterface.Common.Threads;
using System.Collections.Generic;

namespace StorageSCUEmulator
{
	/// <summary>
	/// Descendent Class for handling specific messages
	/// </summary>
    class StoreCommitScp : MessageIterator 
	{
        /// <summary>
        /// Class constructor.
        /// </summary>
        public StoreCommitScp() : base()
        {}
        
		/// <summary>
		/// Handle Abort Rq
		/// </summary>
		/// <param name="abort"></param>
		public override void AfterHandlingAbort(Abort abort)
		{
			if (!IsMessageHandled)
			{
				StopResultsGathering();
				StartResultsGathering();

				IsMessageHandled = true;
			}			
		}

        /// <summary>
        /// Method to handle an Associate Request.
        /// </summary>
        /// <param name="associateRq">Associate Request message.</param>
        public override void AfterHandlingAssociateRequest(AssociateRq associateRq)
        {
            if (IsMessageHandled == false)
            {
                this.Options.LocalAeTitle = associateRq.CalledAETitle;
                this.Options.RemoteAeTitle = associateRq.CallingAETitle;

                // send an associate accept/reject with the supported transfer syntaxes.
                DulMessage dulMsg = SendAssociateRp(new SopClasses("1.2.840.10008.1.20.1"), new TransferSyntaxes(tsList));

                if (dulMsg is AssociateRj)
                    this.receiveMessages = false;

                // message has now been handled
                IsMessageHandled = true;
            }
        }

        /// <summary>
        /// Method to handle the workflow after receiving a Release Request.
        /// </summary>
        /// <param name="releaseRq">Release Request message.</param>
        public override void AfterHandlingReleaseRequest(ReleaseRq releaseRq)
        {
            if (IsMessageHandled == false)
            {
                SendReleaseRp();                        

                // message has now been handled
                IsMessageHandled = true;

                this.receiveMessages = false;
            }
        }

        protected override void AfterHandlingNEventReportRequest(DicomMessage dicomMessage)
        {
            base.AfterHandlingNEventReportRequest(dicomMessage);
            IsMessageHandled = true;

            // set up the default N-EVENT-REPORT-RSP with a successful status
            DicomMessage responseMessage = new DicomMessage(DvtkData.Dimse.DimseCommand.NEVENTREPORTRSP);
            responseMessage.Set("0x00000900", DvtkData.Dimse.VR.US, 0);

            // send the response
            this.Send(responseMessage);
        }

        /// <summary>
        /// Method to handle the workflow after receiving a C-EHO-RQ.
        /// </summary>
        /// <param name="dicomMessage">C-ECHO-RQ message.</param>
        protected override void AfterHandlingCEchoRequest(DicomMessage dicomMessage)
        {
            if (IsMessageHandled == false)
            {
                DicomMessage dicomMessageToSend = new DicomMessage(DvtkData.Dimse.DimseCommand.CECHORSP);

                dicomMessageToSend.Set("0x00000002", DvtkData.Dimse.VR.UI, "1.2.840.10008.1.1");
                dicomMessageToSend.Set("0x00000900", DvtkData.Dimse.VR.US, 0);

                Send(dicomMessageToSend);

                // message has now been handled
                IsMessageHandled = true;
            }
        }

        protected override void DisplayException(System.Exception exception)
		{
			string error = "Error in DICOM message handling due to exception:" + exception.Message;
			WriteError(error);
		}

        /// <summary>
        /// Selected Transfer Syntaxes
        /// </summary>
        public void setSupportedTS(ArrayList list)
        {
            tsList = (System.String[])list.ToArray(typeof(System.String));
        }
        public static String[] tsList = null;
	}

    class ConversionThread : DicomThread
    {
        public ConversionThread()
            : base()
        {

        }

        protected override void Execute()
        {

        }
    }
    /// <summary>
    /// Descendent Class for handling specific messages
    /// </summary>
    class StoreScu : DicomThread
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public StoreScu()
            : base()
        { }

        public DicomMessageCollection StoreMessages
        {
            set
            {
                _storeMessages = value;
            }
        }
        private DicomMessageCollection _storeMessages = null;

        private List<DicomFile> _dicomFileCollection = null;

        public List<DicomFile> DICOMFileCollection
        {
            set
            {
                _dicomFileCollection = value;
            }
        }

        public PresentationContext [] PresentationContexts
        {
            set
            {
                _presentationContexts = value;
            }
        }
        private PresentationContext [] _presentationContexts = null;

        protected override void Execute()
        {
            if (_presentationContexts.Length != 0)
                SendAssociateRq(_presentationContexts);
            else
            {
                WriteError("There is no presentation context available for proposing.");
                return;
            }
            
            DulMessage dulMsg = ReceiveAssociateRp();

            if(dulMsg is AssociateAc)
            {
                AssociateAc assocAc = (AssociateAc)dulMsg;
                foreach(PresentationContext acceptedPC in assocAc.PresentationContexts)
                {
                    if (acceptedPC.Result == 0)
                    {
                        string msg = string.Format("Accepted Presentation Context: Abstract Syntax - {0}, Transfer Syntax - {1}", acceptedPC.AbstractSyntax, acceptedPC.TransferSyntax);
                        WriteInformation(msg);

                        string sopClassUid = "";
                        string sopInstUid = "";
                        string transferSyntax = "";
                        foreach (DicomFile dcmFile in _dicomFileCollection)
                        {
                            

                            // Get the SOP Class UID
                            Values values = dcmFile.DataSet["0x00080016"].Values;
                            sopClassUid = values[0];

                            // Get the SOP Instance UID
                            Values sopClassUidvalues = dcmFile.DataSet["0x00080018"].Values;
                            sopInstUid = sopClassUidvalues[0];

                            Values transferSyntaxes = dcmFile.FileMetaInformation["0x00020010"].Values;
                            transferSyntax = transferSyntaxes[0];


                            // try for a match
                            if ((acceptedPC.Result == 0) &&
                                (acceptedPC.AbstractSyntax == sopClassUid)&&acceptedPC.TransferSyntax==transferSyntax)
                            {
                                DicomMessage storeMsg = new DicomMessage(DvtkData.Dimse.DimseCommand.CSTORERQ, dcmFile.DataSet);
                                

                                string message = string.Format("Sending DICOM object with PC ID - {0}", acceptedPC.ID);
                                WriteInformation(message);
                                System.Diagnostics.Debug.WriteLine(storeMsg.DataSet.Dump("set-"));
                                
                                Send(storeMsg,acceptedPC.ID);

                                DicomMessage rspMsg = ReceiveDicomMessage();
                                Int32 statusVal = Int32.Parse(rspMsg.CommandSet.GetValues("0x00000900")[0]);
                                if (statusVal == 0)
                                {
                                    string infoMsg = string.Format("Image with SOP Instance UID{0} stored successfully.", sopInstUid);
                                    WriteInformation(infoMsg);
                                }
                                else
                                {
                                    string warnMsg = string.Format("Non-zero status returned. Image with SOP Instance UID{0} storage failed.", sopInstUid);
                                    WriteWarning(warnMsg);
                                }
                            }
                        }
                    }
                    else
                    {
                        string resultStr = convertAccResult(acceptedPC.Result);
                        string message = string.Format("Can't store DICOM object with Rejected Abstract Syntax - {0}, PC ID - {1}, Reason - {2}", acceptedPC.AbstractSyntax, acceptedPC.ID, resultStr);
                        WriteWarning(message);
                    }                    
                }

                SendReleaseRq();

                ReceiveReleaseRp();
            }
            else if (dulMsg is AssociateRj)
            {
                AssociateRj assocRj = (AssociateRj)dulMsg;
                string msg = string.Format("Association Rejected for proposed presentation contexts:\nResult - {0}({1})\nSource - {2}({3})\nReason - {4}({5})", assocRj.Result, 
                                                                                                                                                convertResult(assocRj.Result),
                                                                                                                                                assocRj.Source,
                                                                                                                                                convertSource(assocRj.Source),
                                                                                                                                                assocRj.Reason,
                                                                                                                                                convertReason(assocRj.Source,assocRj.Reason));
                WriteInformation(msg);
            }
            else
            {
                WriteInformation("Unknown message is received from SCP.");
            }            
        }

        protected override void DisplayException(System.Exception exception)
        {
            string error = "Error in DICOM message handling due to exception:" + exception.Message;
            WriteError(error);
        }

        string convertAccResult(int result)
        {
            string resultStr;
            switch (result)
            {
                case 1:
                    resultStr = "User Rejection";
                    break;
                case 2:
                    resultStr = "No Reason (Provider Rejection)";
                    break;
                case 3:
                    resultStr = "Abstract Syntax Not Supported (Provider Rejection)";
                    break;
                case 4:
                    resultStr = "Transfer Syntaxes Not Supported (Provider Rejection)";
                    break;
                default:
                    resultStr = "Not Defined";
                    break;
            }
            return resultStr;
        }

        string convertResult(byte result)
        {
            string resultStr;
            switch (result)
            {
                case 1:
                    resultStr = "Rejected Permanently";
                    break;
                case 2:
                    resultStr = "Rejected Transiently";
                    break;
                default:
                    resultStr = "No Result";
                    break;
            }
            return resultStr;
        }

        string convertSource(byte source)
        {
            string sourceStr;
            switch (source)
            {
                case 1:
                    sourceStr = "DICOM UL Service User";
                    break;
                case 2:
                    sourceStr = "DICOM UL Service Provider (ACSE related function)";
                    break;
                case 3:
                    sourceStr = "DICOM UL Service Provider (Presentation related function)";
                    break;
                default:
                    sourceStr = "No Source";
                    break;
            }
            return sourceStr;
        }

        string convertReason(byte source, byte reason)
        {
            string reasonStr;
            switch (source)
            {
            case 1:
                switch (reason)
                {
                    case 1:
                        reasonStr = "No reason given";
                        break;
                    case 2:
                        reasonStr = "Application context name not supported";
                        break;
                    case 3:
                        reasonStr = "Calling AE title not recognized";
                        break;
                    case 7:
                        reasonStr = "Called AE title not recognized";
                        break;
                    default:
                        reasonStr = "Reserved";
                        break;
                }
                break;
            case 2:
                switch (reason)
                {
                    case 1:
                        reasonStr = "No reason given";
                        break;
                    case 2:
                        reasonStr = "Protocol version not supported";
                        break;
                    default:
                        reasonStr = "";
                        break;
                }
                break;
            case 3:
                switch (reason)
                {
                    case 1:
                        reasonStr = "Temporary congestion";
                        break;
                    case 2:
                        reasonStr = "Local limit exceeded";
                        break;
                    default:
                        reasonStr = "Reserved";
                        break;
                }
                break;
                default:
                    reasonStr = "No reason";
                    break;
            }
            return reasonStr;
        }
    }    

    /// <summary>
    /// Descendent Class for handling specific messages
    /// </summary>
    class CommitScu : DicomThread
    {
        private bool autoValidate;

        /// <summary>
        /// Constructor.
        /// </summary>
        public CommitScu(StorageSCUEmulator Obj, bool autoValidate)
            : base() 
        {
            emulatorObj = Obj;
            this.autoValidate = autoValidate;
        }

        StorageSCUEmulator emulatorObj = null;

        public DicomThreadOptions ThreadSettings
        {
            set
            {
                _threadOptions = value;
            }
        }
        private DicomThreadOptions _threadOptions = null;

        public DicomMessage NActionMessage
        {
            set
            {
                _nActionMessage = value;
            }
        }
        private DicomMessage _nActionMessage = null;

        public int Timeout
        {
            set
            {
                _Delay = value;
            }
        }
        private int _Delay = 5;

        ArrayList tslist = new ArrayList();

        protected override void Execute()
        {
            PresentationContext presentationContext = new PresentationContext("1.2.840.10008.1.20.1", // Abstract Syntax Name
                                                                            "1.2.840.10008.1.2"); // Transfer Syntax Name(s)
            PresentationContext[] presentationContexts = new PresentationContext[1];
            presentationContexts[0] = presentationContext;
            
            SendAssociateRq(presentationContexts);

            ReceiveAssociateAc();

            if (_nActionMessage != null)
            {
                WriteInformation("N-Action Request Information"+"\n"+_nActionMessage.DataSet.Dump(""));
                Send(_nActionMessage);
            }

            ReceiveDicomMessage();

            if (_Delay < 0)
            {
                // Async storage commitment
                SendReleaseRq();

                ReceiveReleaseRp();

                // Start the Storage commit SCP for receiving N-EVENTREPORT
                EmulateStorageCommitSCP();
            }
            else
            {
                string info;
                if (_Delay == 0)
                {
                    //Wait for 24 hrs(infinite)
                    int waitingTime = 24 * 60 * 60;
                    _Delay = (short)waitingTime;
                    info = "Waiting forever for N-Event-Report.";
                }
                else
                {
                    info = string.Format("Waiting for N-Event-Report for {0} secs", _Delay);
                }
                WriteInformation(info);

                int waitedTime = 0;
                if (WaitForPendingDataInNetworkInputBuffer(_Delay * 1000, ref waitedTime))
                {
                    DicomMessage nEventReportResponse = ReceiveDicomMessage();

                    if (nEventReportResponse.CommandSet.DimseCommand == DvtkData.Dimse.DimseCommand.NEVENTREPORTRQ)
                    {
                        // set up the default N-EVENT-REPORT-RSP with a successful status
                        DicomMessage responseMessage = new DicomMessage(DvtkData.Dimse.DimseCommand.NEVENTREPORTRSP);
                        responseMessage.Set("0x00000900", DvtkData.Dimse.VR.US, 0);

                        // send the response
                        this.Send(responseMessage);

                        string msg = "N-Event-Report is received from PACS.";
                        WriteInformation(msg);
                        WriteInformation("N-Event-Report Information\n"+nEventReportResponse.DataSet.Dump(""));

                        SendReleaseRq();

                        ReceiveReleaseRp();

                        return;
                    }
                }
                else
                {
                    SendReleaseRq();

                    ReceiveReleaseRp();

                    // Start the Storage commit SCP for receiving N-EVENTREPORT
                    EmulateStorageCommitSCP();
                }
            }
        }        

        private void EmulateStorageCommitSCP()
        {
            StoreCommitScp storageCommitScp = new StoreCommitScp();

            storageCommitScp.Initialize(this.Parent);
            storageCommitScp.Options.DeepCopyFrom(_threadOptions);

            storageCommitScp.Options.LocalAeTitle = _threadOptions.LocalAeTitle;
            storageCommitScp.Options.LocalPort = _threadOptions.LocalPort;
            //storageCommitScp.Options.RemoteAeTitle = _threadOptions.RemoteAeTitle;

            String resultsFileBaseName = "StorageCommitOperationAsSCP_" + System.DateTime.Now.ToString("yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
            storageCommitScp.Options.ResultsFileNameOnlyWithoutExtension = resultsFileBaseName;
            storageCommitScp.Options.Identifier = resultsFileBaseName;

            storageCommitScp.Options.ResultsDirectory = _threadOptions.ResultsDirectory;
            storageCommitScp.Options.DataDirectory = _threadOptions.DataDirectory;
            storageCommitScp.Options.StorageMode = Dvtk.Sessions.StorageMode.AsDataSet;
            storageCommitScp.Options.LogThreadStartingAndStoppingInParent = false;
            storageCommitScp.Options.LogWaitingForCompletionChildThreads = false;
            storageCommitScp.Options.AutoValidate = autoValidate;

            //Enable Stop button in UI
            emulatorObj.Invoke(emulatorObj.UpdateUIControlsHandler);

            if (tslist.Count == 0)
                tslist.Add("1.2.840.10008.1.2");
            storageCommitScp.setSupportedTS(tslist);

            storageCommitScp.Start();            
        }

        protected override void DisplayException(System.Exception exception)
        {
            string error = "Error in DICOM message handling due to exception:" + exception.Message;
            WriteError(error);
        }

        /// <summary>
        /// Selected Transfer Syntaxes
        /// </summary>
        public void setSupportedTS(ArrayList list)
        {
            tslist.Clear();
            foreach (string ts in list)
                tslist.Add(ts);
        }
    }    
}
