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
using DvtkHighLevelInterface.Common.Threads;
using DvtkHighLevelInterface.Dicom.Threads;
using DvtkHighLevelInterface.InformationModel;
using DvtkHighLevelInterface.Dicom.Files;
using Attribute = DvtkHighLevelInterface.Dicom.Other.Attribute;

namespace RIS_Emulator
{
	/// <summary>
	/// Summary description for OverviewThread.
	/// </summary>
	public class OverviewThread: DicomThread
	{
		public OverviewThread(DicomThreadOptions mppsOptions, DicomThread thread, 
			ArrayList selectedTS, bool isSendRandomRsps, string dcmFile, int nrOfRsps)
		{
			this.mppsOptions = mppsOptions;
            this.worklistThread = thread;
            this.worklistOptions = thread.Options;
			this.selectedTSList = selectedTS;
			this.sendRandomizeRsps = isSendRandomRsps;
			this.selectedDCMFile = dcmFile;
			this.nrOfRandomRsps = nrOfRsps;
		}

		private DicomThreadOptions mppsOptions = null;
		private DicomThreadOptions worklistOptions = null;
        private DicomThread worklistThread = null;
		private ArrayList selectedTSList = null;
		private bool sendRandomizeRsps = true;
		private string selectedDCMFile = "";
		private int nrOfRandomRsps;
		ArrayList randomizedDatasets = new ArrayList();

		protected override void Execute()
		{
			// 
			// The Worklist message handler.
			//
			MessageHandlerSendRandomCFindRsp wlHandler = null;
			Dvtk.DvtkDicomEmulators.WorklistMessageHandlers.CFindHandler worklistCFindHandler = null;

            if (sendRandomizeRsps)
            {
                worklistThread.WriteInformation(string.Format("Test true info model"));
                ModalityWorklistInformationModel modalityWorklistInformationModel = RisEmulator.CreateMWLInformationModel(false, worklistThread);
                worklistCFindHandler = new Dvtk.DvtkDicomEmulators.WorklistMessageHandlers.CFindHandler(modalityWorklistInformationModel);
            }
            else
            {
                worklistThread.WriteInformation(string.Format("sending randomized responses"));
                wlHandler = new MessageHandlerSendRandomCFindRsp();
                RandomizeDatasets();
                wlHandler.setRandomDatasets(randomizedDatasets);
            }

			//
			// The MPPS message handlers.
			//			
			Dvtk.DvtkDicomEmulators.MppsMessageHandlers.NCreateHandler mppsNCreateHandler = new Dvtk.DvtkDicomEmulators.MppsMessageHandlers.NCreateHandler();
			Dvtk.DvtkDicomEmulators.MppsMessageHandlers.NSetHandler mppsNSetHandler = new Dvtk.DvtkDicomEmulators.MppsMessageHandlers.NSetHandler();
			MessageHandlerShowMPPSStatus messageHandlerShowMPPSStatus = new MessageHandlerShowMPPSStatus();

			//
			// Create the actual SCP's
			//
            if (this.mppsOptions.LocalPort == this.worklistOptions.LocalPort)
			{
				WriteWarning("Worklist SCP and MPPS are configured to listen to the same port. Only one DicomThread is started using the settings from the Worklist SCP");

                RISScp worklistMppsScp = new RISScp("Worklist_MPPS_SCP");
				worklistMppsScp.Initialize(this);
				worklistMppsScp.Options.DeepCopyFrom(this.worklistOptions);
				worklistMppsScp.Options.Identifier = "Worklist_MPPS_SCP";
				worklistMppsScp.setSupportedTS(selectedTSList);

                if (sendRandomizeRsps)
                {
                    worklistThread.WriteInformation(string.Format("Test true"));
                    worklistMppsScp.AddToFront(worklistCFindHandler);
                }
                else
                    worklistMppsScp.AddToFront(wlHandler);

				worklistMppsScp.AddToFront(messageHandlerShowMPPSStatus);				
				worklistMppsScp.AddToFront(mppsNCreateHandler);
				worklistMppsScp.AddToFront(mppsNSetHandler);

				worklistMppsScp.Start();
			}
			else
			{
                RISScp mppsScp = new RISScp("MPPS_SCP");
				mppsScp.Initialize(this);
				mppsScp.Options.DeepCopyFrom(this.mppsOptions);
				mppsScp.Options.Identifier = "MPPS_SCP";
				mppsScp.setSupportedTS(selectedTSList);

                RISScp worklistScp = new RISScp("Worklist_SCP");
				worklistScp.Initialize(this);
				worklistScp.Options.DeepCopyFrom(this.worklistOptions);
				worklistScp.Options.Identifier = "Worklist_SCP";
				//worklistScp.setSupportedTS(selectedTSList);

				// Add the message handlers.
                if (sendRandomizeRsps)
                {
                    worklistThread.WriteInformation(string.Format("Test true"));
                    worklistScp.AddToFront(worklistCFindHandler);
                }
                else
                    worklistScp.AddToFront(wlHandler);

				mppsScp.AddToFront(messageHandlerShowMPPSStatus);
				mppsScp.AddToFront(mppsNCreateHandler);
				mppsScp.AddToFront(mppsNSetHandler);

				worklistScp.Start();
				mppsScp.Start();			
			}
		}
		
		private void RandomizeDatasets()
		{
			// Read the DCM File
			DicomFile dcmFile = new DicomFile();				
			dcmFile.Read(selectedDCMFile, this);

			// Randomize the dataset
			Random random = new Random((int)System.DateTime.Now.Ticks);
			
			WriteInformation("adsadasd Sending randomized responses:\r\n");
			for (int i=0; i<nrOfRandomRsps; i++)
			{
				DataSet randomDataset = null;
				randomDataset = dcmFile.DataSet.Clone();

				randomDataset.Randomize("@",random);

				Attribute patNameAttr = randomDataset["0x00100010"];
				string patName = patNameAttr.Values[0];

				Attribute patIDAttr = randomDataset["0x00100020"];
				string patID = patIDAttr.Values[0];

				WriteInformation(string.Format("Response:{0}\r\nPatient Name: {1}, Patient ID: {2}\r\n", i+1,patName,patID));

				randomizedDatasets.Add(randomDataset);
			}
		}
	}

	/// <summary>
	/// Descendent Class for handling specific messages
	/// </summary>
	class RISScp: ConcurrentMessageIterator 
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		public RISScp(String identifierBasisChildThreads)
                : base(identifierBasisChildThreads)
        {}

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
				SendAssociateRp(new TransferSyntaxes(tsList));
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
		/// Selected Transfer Syntaxes
		/// </summary>
		public void setSupportedTS(ArrayList list)
		{
			tsList = (System.String[])list.ToArray(typeof(System.String));
		}
		public static String[] tsList = null;
	}

    //class Inb: InboundDicomMessageFilter 
    //{
    //    public override void Apply(DicomMessage dicomMessage)
    //    {
    //        dicomMessage.Set("0x00080005", DvtkData.Dimse.VR.CS, "");

    //        if (dicomMessage.Exists("0x00400100"))
    //        {
    //            Attribute attribute = dicomMessage.DataSet["0x00400100"];
    //            SequenceItem sequenceItem =attribute.GetItem(1);

    //            sequenceItem.Set("0x00080060", DvtkData.Dimse.VR.CS, ""); //Modality.
    //            sequenceItem.Set("0x00400002", DvtkData.Dimse.VR.DA, ""); // System.DateTime.Now.ToString("yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture)); // Scheduled Procedure Step Start Date.
    //        }

    //        /*if (dicomMessage.AttributeExists("0x00400100[1]/0x00080060"))
    //        {
    //            dicomMessage.DataSet.GetAttribute(new TagSequence("0x00400100")).GetItem(1).Set("0x00080060", DvtkData.Dimse.VR.CS, "*");

    //        }*/
    //    }
    //}

    //class Outb: OutboundDicomMessageFilter 
    //{
    //    private String modality = "";

    //    public Outb(String modality)
    //    {
    //        this.modality = modality;
    //    }

    //    public override void Apply(DicomMessage dicomMessage)
    //    {
    //        if (dicomMessage.Exists("0x00400100"))
    //        {
    //            Attribute attribute = dicomMessage.DataSet["0x00400100"];
    //            SequenceItem sequenceItem = attribute.GetItem(1);

    //            sequenceItem.Set("0x00080060", DvtkData.Dimse.VR.CS, this.modality); //Modality.
    //            sequenceItem.Set("0x0040002", DvtkData.Dimse.VR.DA, System.DateTime.Now.ToString("yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture)); // Scheduled Procedure Step Start Date.
    //        }
    //    }
    //}

	class MessageHandlerSendRandomCFindRsp: MessageHandler
	{
		/// <summary>
		/// Set Random datasets
		/// </summary>
		public void setRandomDatasets(ArrayList list)
		{
			randomizedDatasets = list;
		}
		private ArrayList randomizedDatasets = null;

		/// <summary>
		/// Overridden C-FIND-RQ message handler that makes use of the appropriate Information Model to handle the query.
		/// </summary>
		/// <param name="queryMessage">C-FIND-RQ Identifier (Dataset) containing query attributes.</param>
		/// <returns>Boolean - true if dicomMessage handled here.</returns>
		public override bool HandleCFindRequest(DicomMessage queryMessage)
		{
			// Query response messages
			DicomMessageCollection responseMessages = new DicomMessageCollection();

			DvtkHighLevelInterface.Dicom.Other.Values values = queryMessage.CommandSet["0x00000002"].Values;
			System.String sopClassUid = values[0];

			DicomMessage responseMessage = null;

			foreach (DataSet randomDataset in randomizedDatasets)
			{
				responseMessage = new DicomMessage(DvtkData.Dimse.DimseCommand.CFINDRSP);
				
				responseMessage.CommandSet.Set("0x00000002", DvtkData.Dimse.VR.UI, sopClassUid);
				responseMessage.CommandSet.Set("0x00000900", DvtkData.Dimse.VR.US, 0xFF00);
				responseMessage.DataSet.CloneFrom(randomDataset);
				
				responseMessages.Add(responseMessage);
			}

			responseMessage = new DicomMessage(DvtkData.Dimse.DimseCommand.CFINDRSP);
				
			responseMessage.CommandSet.Set("0x00000002", DvtkData.Dimse.VR.UI, sopClassUid);
			responseMessage.CommandSet.Set("0x00000900", DvtkData.Dimse.VR.US, 0x0000);

			responseMessages.Add(responseMessage);

			// handle responses
			foreach (DicomMessage rspMessage in responseMessages)
			{				
				try
				{
                    int waitedTime = 0;

                    // Check for cancel message from SCU
                    if (WaitForPendingDataInNetworkInputBuffer(100, ref waitedTime))
                    {
                        DicomMessage cancelRq = ReceiveDicomMessage();

                        if (cancelRq.CommandSet.DimseCommand == DvtkData.Dimse.DimseCommand.CCANCELRQ)
                        {
                            // set up the C-FIND-RSP with cancel status
                            DicomMessage respMessage = new DicomMessage(DvtkData.Dimse.DimseCommand.CFINDRSP);
                            respMessage.Set("0x00000900", DvtkData.Dimse.VR.US, 0xFE00);

                            // send the response
                            this.Send(respMessage);
                            break;
                        }
                    }

					this.Send(rspMessage);
				}
				catch(Exception)
				{
					string theErrorText = "DICOM Connection Error: RIS Emulator is failed to send the C-FIND-RSP.";
					WriteError(theErrorText);
				}
			}

			// message handled
			return true;
		}		
	}

	class MessageHandlerShowMPPSStatus: MessageHandler
	{
		public override bool HandleNCreateRequest(DicomMessage dicomMessage)
		{
			WriteInformation("N-CREATE-RQ received with Performed Procedure Step Status : " + dicomMessage.DataSet["0x00400252"].Values[0]);

			return false;
		}

		public override bool HandleNSetRequest(DicomMessage dicomMessage)
		{
			WriteInformation("N-SET-RQ received with Performed Procedure Step Status : " + dicomMessage.DataSet["0x00400252"].Values[0]);

			return false;
		}
	}
}
