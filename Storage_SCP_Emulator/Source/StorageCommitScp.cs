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
using System.Collections.Specialized;

using DvtkHighLevelInterface.Common.UserInterfaces;
using DvtkHighLevelInterface.Dicom.Messages;
using DvtkHighLevelInterface.Dicom.Other;
using DvtkHighLevelInterface.Dicom.Threads;
using DvtkHighLevelInterface.InformationModel;
using DvtkHighLevelInterface.Dicom.Files;
using Attribute = DvtkHighLevelInterface.Dicom.Other.Attribute;
using Dvtk.DvtkDicomEmulators.StorageMessageHandlers;
using Dvtk.DvtkDicomEmulators.StorageClientServers;
using Dvtk.DvtkDicomEmulators.Bases;
using DvtkHighLevelInterface.Common.Threads;

namespace StorageSCPEmulator
{
    /// <summary>
	/// Summary description for HliScp.
	/// </summary>
    public class StorageScp : ConcurrentMessageIterator
    {
        /// <summary>
        /// 
        /// </summary>
        public enum ScpRespondToAssociateRequestEnum
        {
            WithAssociateAccept,
            WithAssociateReject,
            WithAbort
        }

        /// <summary>
        /// 
        /// </summary>
        public enum ScpRespondToReleaseRequestEnum
        {
            WithReleaseResponse,
            WithAbort
        }

        private ScpRespondToAssociateRequestEnum _scpRespondToAssociateRequest = ScpRespondToAssociateRequestEnum.WithAssociateAccept;

        private Byte _rejectResult = 0;
        private Byte _rejectSource = 0;
        private Byte _rejectReason = 0;

        private ScpRespondToReleaseRequestEnum _scpRespondToReleaseRequest = ScpRespondToReleaseRequestEnum.WithReleaseResponse;

        private Byte _abortSource = 0;
        private Byte _abortReason = 0;

        private StorageScp()
            : base("")
        {

        }

        private StorageScp(String identifierBasisChildThreads)
            : base("")
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identifierBasisChildThreads"></param>
        /// <param name="resultsFileNameOnlyWithoutExtensionBasisChildThreads"></param>
        public StorageScp(String identifierBasisChildThreads, String resultsFileNameOnlyWithoutExtensionBasisChildThreads)
            : base(identifierBasisChildThreads, resultsFileNameOnlyWithoutExtensionBasisChildThreads)
        {
            // Do nothing.
        }

        #region DULP request overrides
        /// <summary>
        /// Method to handle the workflow after receiving an Associate Request.
        /// </summary>
        /// <param name="associateRq">Associate Request message.</param>
        public override void AfterHandlingAssociateRequest(AssociateRq associateRq)
        {
            if (IsMessageHandled == false)
            {
                // determine which workflow to follow
                switch (_scpRespondToAssociateRequest)
                {
                    case ScpRespondToAssociateRequestEnum.WithAssociateAccept:
                        {
                            // send an associate accept with the supported transfer syntaxes.
                            this.Options.LocalAeTitle = associateRq.CalledAETitle;
                            this.Options.RemoteAeTitle = associateRq.CallingAETitle;
                            AssociateAc associateAc = SendAssociateAc(new SopClasses(sopList), new TransferSyntaxes(tsStoreList));
                            break;
                        }
                    case ScpRespondToAssociateRequestEnum.WithAssociateReject:
                        // send an associate reject with the given parameters
                        SendAssociateRj(_rejectResult, _rejectSource, _rejectReason);
                        break;
                    case ScpRespondToAssociateRequestEnum.WithAbort:
                    default:
                        // send an abort request with the given parameters
                        SendAbort(_abortSource, _abortReason);
                        break;
                }

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
                // determine which workflow to follow
                switch (_scpRespondToReleaseRequest)
                {
                    case ScpRespondToReleaseRequestEnum.WithReleaseResponse:
                        // send a release response
                        SendReleaseRp();
                        break;
                    case ScpRespondToReleaseRequestEnum.WithAbort:
                    default:
                        // send an abort request with the given parameters
                        SendAbort(_abortSource, _abortReason);
                        break;
                }

                // message has now been handled
                IsMessageHandled = true;
            }
        }

        /// <summary>
        /// Method to handle the workflow after receiving an Abort Request.
        /// </summary>
        /// <param name="abort">Abort Request message.</param>
        public override void AfterHandlingAbort(Abort abort)
        {
            // message has now been handled
            if (!IsMessageHandled)
            {
                StopResultsGathering();
                StartResultsGathering();

                IsMessageHandled = true;
            }
        }
        #endregion DULP request overrides

        #region DIMSE request overrides
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
        #endregion DIMSE request overrides

        #region workflow settings
        /// <summary>
        /// Property - define how to RespondToAssociateRequest.
        /// </summary>
        public ScpRespondToAssociateRequestEnum RespondToAssociateRequest
        {
            get
            {
                return _scpRespondToAssociateRequest;
            }
            set
            {
                _scpRespondToAssociateRequest = value;
            }
        }

        /// <summary>
        /// Property - define how to RespondToReleaseRequest
        /// </summary>
        public ScpRespondToReleaseRequestEnum RespondToReleaseRequest
        {
            get
            {
                return _scpRespondToReleaseRequest;
            }
            set
            {
                _scpRespondToReleaseRequest = value;
            }
        }

        /// <summary>
        ///  Set Associate Reject parameters.
        /// </summary>
        /// <param name="result">DULP reject result.</param>
        /// <param name="source">DULP reject source.</param>
        /// <param name="reason">DULP reject reason.</param>
        public void SetRejectParameters(Byte result, Byte source, Byte reason)
        {
            _rejectResult = result;
            _rejectSource = source;
            _rejectReason = reason;
        }

        /// <summary>
        /// Set the Abort Request parameters.
        /// </summary>
        /// <param name="source">DULP abort source.</param>
        /// <param name="reason">DULP abort reason.</param>
        public void SetAbortParameters(Byte source, Byte reason)
        {
            _abortSource = source;
            _abortReason = reason;
        }

        /// <summary>
        /// Selected Store Transfer Syntaxes
        /// </summary>
        public void setSupportedTSForStore(ArrayList list)
        {
            tsStoreList = (System.String[])list.ToArray(typeof(System.String));
        }
        public static String[] tsStoreList = null;

        /// <summary>
        /// Selected SOP Classes
        /// </summary>
        public void setSupportedSops(ArrayList list)
        {
            sopList = (System.String[])list.ToArray(typeof(System.String));
        }
        public static String[] sopList = null;
        #endregion workflow settings
    }

	/// <summary>
	/// Summary description for HliScp.
	/// </summary>
    public class StoreCommitScp : ConcurrentMessageIterator
	{
        public static bool isEventSent = false;
        public static bool isAssociated = false;

        /// <summary>
        /// 
        /// </summary>
		public enum ScpRespondToAssociateRequestEnum
		{
			WithAssociateAccept,
			WithAssociateReject,
			WithAbort
		}

        /// <summary>
        /// 
        /// </summary>
		public enum ScpRespondToReleaseRequestEnum
		{
			WithReleaseResponse,
			WithAbort
		}

        private ScpRespondToAssociateRequestEnum _scpRespondToAssociateRequest = ScpRespondToAssociateRequestEnum.WithAssociateAccept;

		private Byte _rejectResult = 0;
		private Byte _rejectSource = 0;
		private Byte _rejectReason = 0;

		private ScpRespondToReleaseRequestEnum _scpRespondToReleaseRequest = ScpRespondToReleaseRequestEnum.WithReleaseResponse;

		private Byte _abortSource = 0;
		private Byte _abortReason = 0;

        private String startDateTime = String.Empty;

        /// <summary>
        /// 
        /// </summary>
        public String StartDateTime
        {
            set
            {
                this.startDateTime = value;
            }
        }

        private StoreCommitScp()
            : base("")
        {

        }

        private StoreCommitScp(String identifierBasisChildThreads)
            : base("")
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identifierBasisChildThreads"></param>
        /// <param name="resultsFileNameOnlyWithoutExtensionBasisChildThreads"></param>
        public StoreCommitScp(String identifierBasisChildThreads, String resultsFileNameOnlyWithoutExtensionBasisChildThreads)
            : base(identifierBasisChildThreads, resultsFileNameOnlyWithoutExtensionBasisChildThreads)
        {
            // Do nothing.
        }

        /// <summary>
        /// 
        /// </summary>
        public string DataDirectory
        {
            set
            {
                _dataDirectory = value;
            }
        }
        public static string _dataDirectory = null;

        /// <summary>
        /// 
        /// </summary>
        public string RemoteAETitleForCommitSCU
        {
            set
            {
                _remoteAETitle = value;
            }
        }
        public static string _remoteAETitle = null;

        /// <summary>
        /// 
        /// </summary>
        public string RemoteHostNameForCommitSCU
        {
            set
            {
                _remoteHostName = value;
            }
        }
        public static string _remoteHostName = null;

        /// <summary>
        /// 
        /// </summary>
        public ushort RemotePortForCommitSCU
        {
            set
            {
                _port = value;
            }
        }
        public static ushort _port = 0;

        /// <summary>
        /// 
        /// </summary>
        public int Delay
        {
            set
            {
                _eventDelay = value;
            }
        }
        public static int _eventDelay = 5000;

        #region DULP request overrides
		/// <summary>
		/// Method to handle the workflow after receiving an Associate Request.
		/// </summary>
		/// <param name="associateRq">Associate Request message.</param>
		public override void AfterHandlingAssociateRequest(AssociateRq associateRq)
		{
			if (IsMessageHandled == false)
			{
				// determine which workflow to follow
				switch(_scpRespondToAssociateRequest)
				{
                    case ScpRespondToAssociateRequestEnum.WithAssociateAccept:
                        {
                            // send an associate accept with the supported transfer syntaxes.
                            PresentationContextCollection presentationContextsForAssociateAc = new PresentationContextCollection();
                            foreach (PresentationContext reqtedPC in associateRq.PresentationContexts)
                            {
                                String abstractSyntaxInAssociateRq = reqtedPC.AbstractSyntax;

                                PresentationContext presentationContextForAssociateAc = null;

                                if (sopList.Contains(abstractSyntaxInAssociateRq))
                                {
                                    String transferSyntaxForAssociateAc = null;
                                    this.Options.LocalAeTitle = associateRq.CalledAETitle;
                                    this.Options.RemoteAeTitle = associateRq.CallingAETitle;
                                    if (reqtedPC.AbstractSyntax == "1.2.840.10008.1.20.1")
                                    {
                                        transferSyntaxForAssociateAc = DetermineTransferSyntaxToAccept
                                            (reqtedPC.TransferSyntaxes, tsCommitList);                                        
                                    }
                                    else
                                    {
                                        transferSyntaxForAssociateAc = DetermineTransferSyntaxToAccept
                                            (reqtedPC.TransferSyntaxes, tsStoreList);
                                    }

                                    if (transferSyntaxForAssociateAc.Length == 0)
                                    {
                                        presentationContextForAssociateAc = new PresentationContext
                                            (reqtedPC.AbstractSyntax,
                                            4,
                                            "");
                                    }
                                    else
                                    {
                                        presentationContextForAssociateAc = new PresentationContext
                                            (reqtedPC.AbstractSyntax,
                                            0,
                                            transferSyntaxForAssociateAc);
                                    }
                                }
                                else
                                {
                                    presentationContextForAssociateAc = new PresentationContext
                                        (reqtedPC.AbstractSyntax,
                                        3,
                                        "");
                                }

                                presentationContextsForAssociateAc.Add(presentationContextForAssociateAc);                                
                            }

                            SendAssociateAc(presentationContextsForAssociateAc);
                            isAssociated = true;
                            break;
                        }
					case ScpRespondToAssociateRequestEnum.WithAssociateReject:
						// send an associate reject with the given parameters
						SendAssociateRj(_rejectResult, _rejectSource, _rejectReason);						
						break;
					case ScpRespondToAssociateRequestEnum.WithAbort:
					default:
						// send an abort request with the given parameters
						SendAbort(_abortSource, _abortReason);						
						break;
				}

				// message has now been handled
				IsMessageHandled = true;                
			}
		}

        private String DetermineTransferSyntaxToAccept(StringCollection transferSyntaxesFromAssociateRq, ArrayList transferSyntaxesList)
        {
            String transferSyntaxForAssociateAc = "";

            foreach (String transferSyntaxFromAssociateRq in transferSyntaxesFromAssociateRq)
            {
                if (transferSyntaxesList.Contains(transferSyntaxFromAssociateRq))
                {
                    transferSyntaxForAssociateAc = transferSyntaxFromAssociateRq;
                    break;
                }
            }            

            return (transferSyntaxForAssociateAc);
        }

		/// <summary>
		/// Method to handle the workflow after receiving a Release Request.
		/// </summary>
		/// <param name="releaseRq">Release Request message.</param>
		public override void AfterHandlingReleaseRequest(ReleaseRq releaseRq)
		{
			if (IsMessageHandled == false)
			{
				// determine which workflow to follow
				switch(_scpRespondToReleaseRequest)
				{
					case ScpRespondToReleaseRequestEnum.WithReleaseResponse:
						// send a release response
						SendReleaseRp();
						break;
					case ScpRespondToReleaseRequestEnum.WithAbort:
					default:
						// send an abort request with the given parameters
						SendAbort(_abortSource, _abortReason);
						break;
				}				

				// message has now been handled
				IsMessageHandled = true;
			}			
		}

		/// <summary>
		/// Method to handle the workflow after receiving an Abort Request.
		/// </summary>
		/// <param name="abort">Abort Request message.</param>
		public override void AfterHandlingAbort(Abort abort)
		{
			// message has now been handled
            if (!IsMessageHandled)
            {
                StopResultsGathering();
                StartResultsGathering();

                IsMessageHandled = true;
                isAssociated = false;
            }
		}
		#endregion DULP request overrides

		#region DIMSE request overrides
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

        private static int storageCommitmentScuIndex = 1;

        /// <summary>
        /// Overridden N-ACTION-RQ message handler. Return an N-EVENT-REPORT-RQ
        /// after the N-ACTION-RSP.
        /// </summary>
        /// <param name="queryMessage">N-ACTION-RQ and Dataset.</param>
        /// <returns>Boolean - true if dicomMessage handled here.</returns>
        protected override void AfterHandlingNActionRequest(DicomMessage dicomMessage)
        {
            isEventSent = false;

            // set up the default N-ACTION-RSP with a successful status
            DicomMessage responseMessage = new DicomMessage(DvtkData.Dimse.DimseCommand.NACTIONRSP);
            responseMessage.Set("0x00000900", DvtkData.Dimse.VR.US, 0);

            // send the response
            this.Send(responseMessage);

            IsMessageHandled = true;

            // creating information model
            QueryRetrieveInformationModels qrInfoModels = CreateQueryRetrieveInformationModels(true, _dataDirectory);

            // delay before generating the N-EVENT-REPORT-RQ
            WriteInformation(string.Format("Delaying the N-EVENT-REPORT by {0} secs", _eventDelay / 1000));

            isAssociated = true;
            int waitedTime = 0;

            if (WaitForPendingDataInNetworkInputBuffer(_eventDelay, ref waitedTime))
            {
                ReleaseRq releaseRq = ReceiveReleaseRq();

                if (releaseRq != null)
                {
                    SendReleaseRp();

                    isAssociated = false;
                }
            }

            if (!isAssociated)
            {
                string info = "Sending the N-Event-Report asynchronously.";
                WriteInformation(info);              

                SCU commitScu = new SCU();

                commitScu.Initialize(this.Parent);

                // Set the correct settings for the overview DicomThread.
                //
                commitScu.Options.Identifier = "Storage_Commitment_SCU_association_" + storageCommitmentScuIndex.ToString();
                commitScu.Options.ResultsFileNameOnlyWithoutExtension = (this.overviewThread as StoreCommitScp).startDateTime + "_Storage_Commitment_SCU_association_" + storageCommitmentScuIndex.ToString();
                storageCommitmentScuIndex++;
                commitScu.Options.LocalAeTitle = this.Options.LocalAeTitle;
                commitScu.Options.RemoteAeTitle = _remoteAETitle;
                commitScu.Options.RemotePort = _port;
                commitScu.Options.RemoteHostName = _remoteHostName;
                commitScu.Options.RemoteRole = Dvtk.Sessions.SutRole.Requestor;
                commitScu.Options.ResultsDirectory = this.Options.ResultsDirectory;
                commitScu.Options.DataDirectory = this.Options.DataDirectory;
                commitScu.Options.StorageMode = Dvtk.Sessions.StorageMode.NoStorage;
                commitScu.Options.LogThreadStartingAndStoppingInParent = false;
                commitScu.Options.LogWaitingForCompletionChildThreads = false;
                commitScu.Options.AutoValidate = false;

                PresentationContext presentationContext = new PresentationContext("1.2.840.10008.1.20.1", // Abstract Syntax Name
                                                                                "1.2.840.10008.1.2"); // Transfer Syntax Name(s)
                PresentationContext[] presentationContexts = new PresentationContext[1];
                presentationContexts[0] = presentationContext;

                // create the N-EVENT-REPORT-RQ based in the contents of the N-ACTION-RQ
                DicomMessage requestMessage = GenerateTriggers.MakeStorageCommitEvent(qrInfoModels, dicomMessage);

                if (waitedTime < _eventDelay)
                    Sleep(_eventDelay - waitedTime);

                commitScu.Start();

                isEventSent = commitScu.TriggerSendAssociationAndWait(requestMessage, presentationContexts);
            }
            else
            {
                string info = "Sending the N-Event-Report synchronously.";
                WriteInformation(info);

                if (!isEventSent)
                    SendNEventReport(qrInfoModels, dicomMessage);
            }
        }

        private bool SendNEventReport(QueryRetrieveInformationModels qrInfoModels, DicomMessage dicomMessage)
        {
            // create the N-EVENT-REPORT-RQ based in the contents of the N-ACTION-RQ
            DicomMessage requestMessage = GenerateTriggers.MakeStorageCommitEvent(qrInfoModels, dicomMessage);

            // send the request
            this.Send(requestMessage);

            ReceiveDicomMessage();

            isEventSent = true;

            return isEventSent;
        }

        QueryRetrieveInformationModels CreateQueryRetrieveInformationModels(bool randomizeFirst, string directory)
        {
            WriteInformation(string.Format("Creating the QR information model based on data directory : {0}", directory));
            QueryRetrieveInformationModels queryRetrieveInformationModels = new QueryRetrieveInformationModels();

            //Specify directory for temp DCM files
            //String tempDir = Path.Combine(Path.GetTempPath(), "DVTkStorageSCP");
            //if (!Directory.Exists(tempDir))
            //{
            //    Directory.CreateDirectory(tempDir);
            //}

            queryRetrieveInformationModels.DataDirectory = directory;

            DirectoryInfo directoryInfo = new DirectoryInfo(directory);

            FileInfo[] fileInfos = directoryInfo.GetFiles();

            foreach (FileInfo fileInfo in fileInfos)
            {
                if ((fileInfo.Extension.ToLower() == ".dcm") || 
                    (fileInfo.Extension == "") || (fileInfo.Extension == null))
                {
                    try
                    {
                        DvtkHighLevelInterface.Dicom.Files.DicomFile dicomFile = new DvtkHighLevelInterface.Dicom.Files.DicomFile();

                        dicomFile.Read(fileInfo.FullName, this);

                        if (randomizeFirst)
                            dicomFile.DataSet.Randomize("@");

                        queryRetrieveInformationModels.Add(dicomFile, false);
                    }

                    catch (Exception)
                    {
                        string theErrorText = string.Format("Invalid DICOM File - {0} will be skiped from QR information model.", fileInfo.FullName);
                        WriteInformation(theErrorText);
                    }
                }
            }

            return (queryRetrieveInformationModels);
        }

        protected override void DisplayException(System.Exception exception)
        {
            string error = "Error in DICOM message handling due to exception:" + exception.Message;
            WriteError(error);
        }
		#endregion DIMSE request overrides

		#region workflow settings
		/// <summary>
		/// Property - define how to RespondToAssociateRequest.
		/// </summary>
		public ScpRespondToAssociateRequestEnum RespondToAssociateRequest
		{
			get
			{
				return _scpRespondToAssociateRequest;
			}
			set
			{
				_scpRespondToAssociateRequest = value;
			}
		}

		/// <summary>
		/// Property - define how to RespondToReleaseRequest
		/// </summary>
		public ScpRespondToReleaseRequestEnum RespondToReleaseRequest
		{
			get
			{
				return _scpRespondToReleaseRequest;
			}
			set
			{
				_scpRespondToReleaseRequest = value;
			}
		}

		/// <summary>
		///  Set Associate Reject parameters.
		/// </summary>
		/// <param name="result">DULP reject result.</param>
		/// <param name="source">DULP reject source.</param>
		/// <param name="reason">DULP reject reason.</param>
		public void SetRejectParameters(Byte result, Byte source, Byte reason)
		{
			_rejectResult = result;
			_rejectSource = source;
			_rejectReason = reason;
		}

		/// <summary>
		/// Set the Abort Request parameters.
		/// </summary>
		/// <param name="source">DULP abort source.</param>
		/// <param name="reason">DULP abort reason.</param>
		public void SetAbortParameters(Byte source, Byte reason)
		{
			_abortSource = source;
			_abortReason = reason;
		}

        /// <summary>
        /// Selected Storage Transfer Syntaxes
        /// </summary>
        public void setSupportedStoreTS(ArrayList list)
        {
            //tsStoreList = (System.String[])list.ToArray(typeof(System.String));
            tsStoreList = list;
        }
        //public String[] tsStoreList = null;
        public static ArrayList tsStoreList = new ArrayList();

        /// <summary>
        /// Selected Commit Transfer Syntaxes
        /// </summary>
        public void setSupportedCommitTS(ArrayList list)
        {
            //tsCommitList = (System.String[])list.ToArray(typeof(System.String));
            tsCommitList = list;
        }
        //public String[] tsCommitList = null;
        public static ArrayList tsCommitList = new ArrayList();

        /// <summary>
        /// Selected SOP Classes
        /// </summary>
        public void setSupportedSops(ArrayList list)
        {
            //sopList = (System.String[])list.ToArray(typeof(System.String));
            sopList = list;
        }
        //public String[] sopList = null;
        public static ArrayList sopList = new ArrayList();
		#endregion workflow settings
	}
}
