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

using DvtkHighLevelInterface.Dicom.Messages;
using DvtkHighLevelInterface.Dicom.Other;
using DvtkHighLevelInterface.Dicom.Threads;
using DvtkHighLevelInterface.InformationModel;
using VR = DvtkData.Dimse.VR;
using System.Collections.Generic;

namespace QR_Emulator
{
	/// <summary>
	/// Summary description for OverviewThread.
	/// </summary>
	public class OverviewThread: DicomThread
	{
        public OverviewThread(DicomThread sourceQRDicomThread, ArrayList selectedTS, bool patientRoot, bool studyRoot, bool patientStudyRoot,List<DICOMPeer> dicomPeers)
		{
			this.sourceQRDicomThread = sourceQRDicomThread;
			this.selectedTSList = selectedTS;
			isPatientRoot = patientRoot;
			isStudyRoot = studyRoot;
			isPatientStudyRoot = patientStudyRoot;
            DicomPeers = dicomPeers;
		}
        private List<DICOMPeer> DicomPeers;
		private DicomThread sourceQRDicomThread = null;
		private ArrayList selectedSopClassesList = new ArrayList();
		private ArrayList selectedTSList = null;
		bool isPatientRoot = false;
		bool isStudyRoot = false;
		bool isPatientStudyRoot = false;

		protected override void Execute()
		{
			selectedSopClassesList.Clear();

			//
			// The Query/Retrieve message handler.
			//
            this.WriteInformation(string.Format("Creating the QR information model based on data directory : {0}",QREmulator.dataDirectory.ToString()));
            QueryRetrieveInformationModels queryRetrieveInformationModels = QREmulator.CreateQueryRetrieveInformationModels(true, false, isPatientRoot, isStudyRoot, isPatientStudyRoot, this.sourceQRDicomThread);

            Dvtk.DvtkDicomEmulators.QueryRetrieveMessageHandlers.CFindHandler cFindHandler = new Dvtk.DvtkDicomEmulators.QueryRetrieveMessageHandlers.CFindHandler();
			Dvtk.DvtkDicomEmulators.QueryRetrieveMessageHandlers.CMoveHandler cMoveHandler = new Dvtk.DvtkDicomEmulators.QueryRetrieveMessageHandlers.CMoveHandler();
            cMoveHandler.EnableMultipleMoveDestinations(DicomPeers);
			Dvtk.DvtkDicomEmulators.QueryRetrieveMessageHandlers.CGetHandler cGetHandler = new Dvtk.DvtkDicomEmulators.QueryRetrieveMessageHandlers.CGetHandler();

			//Set supported PCs for QR responses
			selectedSopClassesList.Add("1.2.840.10008.1.1");
			
			if(isPatientRoot)
			{
                // add any additional attribute values to the information models
                queryRetrieveInformationModels.PatientRoot.AddAdditionalAttributeToInformationModel(true, "0x00080054", VR.AE, sourceQRDicomThread.Options.RemoteAeTitle);

				cFindHandler.PatientRootInformationModel = queryRetrieveInformationModels.PatientRoot;
				cMoveHandler.PatientRootInformationModel = queryRetrieveInformationModels.PatientRoot;
				cGetHandler.PatientRootInformationModel = queryRetrieveInformationModels.PatientRoot;
				selectedSopClassesList.Add("1.2.840.10008.5.1.4.1.2.1.1");
				selectedSopClassesList.Add("1.2.840.10008.5.1.4.1.2.1.2");
				selectedSopClassesList.Add("1.2.840.10008.5.1.4.1.2.1.3");
			}

			if(isStudyRoot)
			{
                // add any additional attribute values to the information models
                queryRetrieveInformationModels.StudyRoot.AddAdditionalAttributeToInformationModel(true, "0x00080054", VR.AE, sourceQRDicomThread.Options.RemoteAeTitle);

				cFindHandler.StudyRootInformationModel = queryRetrieveInformationModels.StudyRoot;
				cMoveHandler.StudyRootInformationModel = queryRetrieveInformationModels.StudyRoot;
				cGetHandler.StudyRootInformationModel = queryRetrieveInformationModels.StudyRoot;
				selectedSopClassesList.Add("1.2.840.10008.5.1.4.1.2.2.1");
				selectedSopClassesList.Add("1.2.840.10008.5.1.4.1.2.2.2");
				selectedSopClassesList.Add("1.2.840.10008.5.1.4.1.2.2.3");
			}

			if(isPatientStudyRoot)
			{
                // add any additional attribute values to the information models
                queryRetrieveInformationModels.PatientStudyOnly.AddAdditionalAttributeToInformationModel(true, "0x00080054", VR.AE, sourceQRDicomThread.Options.RemoteAeTitle);

				cFindHandler.PatientStudyOnlyInformationModel = queryRetrieveInformationModels.PatientStudyOnly;			
				cMoveHandler.PatientStudyOnlyInformationModel = queryRetrieveInformationModels.PatientStudyOnly;
				cGetHandler.PatientStudyOnlyInformationModel = queryRetrieveInformationModels.PatientStudyOnly;
				selectedSopClassesList.Add("1.2.840.10008.5.1.4.1.2.3.1");
				selectedSopClassesList.Add("1.2.840.10008.5.1.4.1.2.3.2");
				selectedSopClassesList.Add("1.2.840.10008.5.1.4.1.2.3.3");
			}

			//
			// Create the actual SCP
			//
            QRScp qRScp = new QRScp("QR_SCP");
			qRScp.Initialize(this);
			qRScp.Options.DeepCopyFrom(sourceQRDicomThread.Options);
			qRScp.Options.Identifier = "QR_SCP";
			qRScp.setSupportedSopClasses(selectedSopClassesList);
			qRScp.setSupportedTS(selectedTSList);
			qRScp.Options.LogThreadStartingAndStoppingInParent = false;
			qRScp.Options.LogWaitingForCompletionChildThreads = false;

			// Add the message handlers.
			qRScp.AddToFront(cFindHandler);
			qRScp.AddToFront(cMoveHandler);
			qRScp.AddToFront(cGetHandler);

			qRScp.Start();
		}        
	}

	/// <summary>
	/// Descendent Class for handling specific messages
	/// </summary>
    class QRScp : ConcurrentMessageIterator 
	{
		/// <summary>
		/// Constructor.
		/// </summary>
        public QRScp(String identifierBasisChildThreads)
            : base(identifierBasisChildThreads) 
        { }

        void QRScp_MessageReceivedEvent(DicomProtocolMessage dicomProtocolMessage)
        {
            if (dicomProtocolMessage.IsDicomMessage)
            {
                if (DvtkData.Dimse.DimseCommand.CFINDRQ == dicomProtocolMessage.DicomMessage.CommandSet.DimseCommand)
                {
                    WriteInformation("******** C-Find Request Info********");
                    WriteInformation(dicomProtocolMessage.DicomMessage.DataSet.Dump(""));
                }
                else if(    DvtkData.Dimse.DimseCommand.CMOVERQ == dicomProtocolMessage.DicomMessage.CommandSet.DimseCommand)
                {
                    WriteInformation("******** C-Move Request Info********");
                    WriteInformation(dicomProtocolMessage.DicomMessage.DataSet.Dump(""));
                }
            }
            
        }

        void QRScp_SendingMessageEvent(DicomProtocolMessage dicomProtocolMessage)
        {
            
            if (dicomProtocolMessage.IsDicomMessage)
            {
                if (DvtkData.Dimse.DimseCommand.CFINDRSP == dicomProtocolMessage.DicomMessage.CommandSet.DimseCommand)
                {
                    
                    string value=dicomProtocolMessage.DicomMessage.CommandSet.GetValues("0x00000900")[0];
                    UInt32 val=0;
                    UInt32.TryParse(value, out val);
                    value = "0x" + val.ToString("X4");
                    if (value != "0xFF00" && value != "0xFF01")
                    {
                        int noOfMatches = 0;
                        for (int count = Messages.DicomMessages.CFindResponses.Count - 2; count >= 0; count--)
                        {
                            DicomProtocolMessage msg = Messages.DicomMessages.CFindResponses[count];
                            if (msg.DicomMessage.CommandSet.GetValues("0x00000900")[0] != "65280" && msg.DicomMessage.CommandSet.GetValues("0x00000900")[0] != "65281")
                            {
                                break;
                            }
                            noOfMatches++;
                        }
                        WriteInformation("Number of matches: " + noOfMatches.ToString());
                        return;
                    }
                    WriteInformation("******** C-Find Response Info********");
                    WriteInformation("C-Find Response Status:"+value);
                    WriteInformation(dicomProtocolMessage.DicomMessage.DataSet.Dump(""));
                }
            }
        }
        public override void BeforeHandlingAssociateRequest(AssociateRq associateRq)
        {
            this.SendingMessageEvent += new SendingMessageEventHandler(QRScp_SendingMessageEvent);
            this.MessageReceivedEvent += new MessageReceivedEventHandler(QRScp_MessageReceivedEvent);
            base.BeforeHandlingAssociateRequest(associateRq);
        }
        public override void BeforeHandlingAbort(Abort abort)
        {
            this.SendingMessageEvent -= new SendingMessageEventHandler(QRScp_SendingMessageEvent);
            this.MessageReceivedEvent -= new MessageReceivedEventHandler(QRScp_MessageReceivedEvent);
            base.BeforeHandlingAbort(abort);
        }
        public override void BeforeHandlingReleaseRequest(ReleaseRq releaseRq)
        {
            this.SendingMessageEvent -= new SendingMessageEventHandler(QRScp_SendingMessageEvent);
            this.MessageReceivedEvent -= new MessageReceivedEventHandler(QRScp_MessageReceivedEvent);
            base.BeforeHandlingReleaseRequest(releaseRq);
        }
        public override void BeforeHandlingAssociateReject(AssociateRj associateRj)
        {
            this.SendingMessageEvent -= new SendingMessageEventHandler(QRScp_SendingMessageEvent);
            this.MessageReceivedEvent -= new MessageReceivedEventHandler(QRScp_MessageReceivedEvent);
            base.BeforeHandlingAssociateReject(associateRj);
        }
		/// <summary>
		/// Handle Association Req
		/// </summary>
		/// <param name="associateRq"></param>
		public override void AfterHandlingAssociateRequest(AssociateRq associateRq)
		{
			if (!IsMessageHandled)
			{
                this.Options.LocalAeTitle = associateRq.CalledAETitle;
                this.Options.RemoteAeTitle = associateRq.CallingAETitle;
				SendAssociateRp(sopClasses, new TransferSyntaxes(tsList));
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
                // send a release response
                SendReleaseRp();                        

                // message has now been handled
                IsMessageHandled = true;
            }
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

		protected override void DisplayException(System.Exception exception)
		{
			string error = "Error in DICOM message handling due to exception:" + exception.Message;
			WriteError(error);
		}

		/// <summary>
		/// Selected SOP Classes
		/// </summary>
		public void setSupportedSopClasses(ArrayList list)
		{
			sopClasses = new SopClasses((System.String[])list.ToArray(typeof(System.String)));
		}
		private static SopClasses sopClasses = null;

		/// <summary>
		/// Selected Transfer Syntaxes
		/// </summary>
		public void setSupportedTS(ArrayList list)
		{
			tsList = (System.String[])list.ToArray(typeof(System.String));
		}
		private static String[] tsList = null;
	}	
}
