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
using System.Collections;

using DvtkHighLevelInterface.Dicom.Messages;
using DvtkHighLevelInterface.Dicom.Other;
using DvtkHighLevelInterface.Dicom.Threads;
using DvtkHighLevelInterface.InformationModel;
using DvtkHighLevelInterface.Dicom.Files;
using Dvtk.Results;
using VR = DvtkData.Dimse.VR;
using System.Collections.Generic;

namespace Dvtk.DvtkDicomEmulators.QueryRetrieveMessageHandlers
{
	/// <summary>
	/// Summary description for CMoveHandler.
	/// </summary>
	public class CMoveHandler : QueryRetrieveHandler
	{
        private Int32 cStoreStatusVal = 0x0000;

		public CMoveHandler() 
        {
           
        }
       /// <summary>
       /// This function is to set the list of move destinations added form 
       /// </summary>
       /// <param name="peers"></param>
        public void EnableMultipleMoveDestinations(List<DICOMPeer> peers)
        {
            IsHaveMoveDestinations = true;
            MoveDestiantions = peers;
        }
        bool IsHaveMoveDestinations = false;
        List<DICOMPeer> MoveDestiantions;
        int MoveAEdetailsIndex = -1;
		public override bool HandleCMoveRequest(DicomMessage retrieveMessage)
		{
			// try to get the SOP Class Uid so that we know which Information Model to use.
			DvtkHighLevelInterface.Dicom.Other.Values values = retrieveMessage.CommandSet["0x00000002"].Values;
			System.String sopClassUid = values[0];
			DvtkData.Dul.AbstractSyntax abstractSyntax = new DvtkData.Dul.AbstractSyntax(sopClassUid);

			// try to get the Move Destination AE.
			values = retrieveMessage.CommandSet["0x00000600"].Values;
            string vr = retrieveMessage.CommandSet["0x00000600"].VR.ToString();
			System.String moveDestinationAE = values[0];
            string hexString = moveDestinationAE;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if (DicomThread.Options.LoadedDefinitionFileNames.Length < 10)
                WriteWarning("Some of the definition files is not loaded properly.");
            if (vr == "UN")
            {
                for (int i = 0; i <= hexString.Length - 2; i += 2)
                {
                    sb.Append(Convert.ToString(Convert.ToChar(Int32.Parse(hexString.Substring(i, 2), System.Globalization.NumberStyles.HexNumber))));
                }
            }
            else if (vr == "AE")
            {
                sb.Append(moveDestinationAE);
            }
            if (moveDestinationAE == null || moveDestinationAE=="")
            {
                DicomMessage responseMessage = new DicomMessage(DvtkData.Dimse.DimseCommand.CMOVERSP);
                responseMessage.Set("0x00000900", VR.US, 0xA801);
                responseMessage.Set("0x00000902", VR.LO, "Unknown Move Destination");
                this.Send(responseMessage);
                return(true);
            }
            MoveAEdetailsIndex=FindMoveAEDetails(sb.ToString());
            if (IsHaveMoveDestinations && MoveAEdetailsIndex == -1)
            {
                DicomMessage responseMessage = new DicomMessage(DvtkData.Dimse.DimseCommand.CMOVERSP);
                responseMessage.Set("0x00000900", VR.US, 0xA801);
                responseMessage.Set("0x00000902", VR.LO, "Move Destination not registered in SCP");
                this.Send(responseMessage);
                WriteWarning("Move destination is not registered in SCP");
                return (true);
            }
			DvtkData.Collections.StringCollection retrieveList = null;

			// check if we should use the Patient Root Information Model
			if ((abstractSyntax.UID == DvtkData.Dul.AbstractSyntax.Patient_Root_Query_Retrieve_Information_Model_MOVE.UID) &&
				(PatientRootInformationModel != null))
			{
				// check if the information model should be refreshed before retrieving
				if (RefreshInformationModelBeforeUse == true)
				{
					PatientRootInformationModel.RefreshInformationModel();
				}

				// perform retrieve
				retrieveList = PatientRootInformationModel.RetrieveInformationModel(retrieveMessage);
			}
				// check if we should use the Study Root Information Model 
			else if ((abstractSyntax.UID == DvtkData.Dul.AbstractSyntax.Study_Root_Query_Retrieve_Information_Model_MOVE.UID) &&
				(StudyRootInformationModel != null))
			{
				// check if the information model should be refreshed before retrieving
				if (RefreshInformationModelBeforeUse == true)
				{
					StudyRootInformationModel.RefreshInformationModel();
				}

				// perform retrieve
				retrieveList = StudyRootInformationModel.RetrieveInformationModel(retrieveMessage);
			}
				// check if we should use the Patient Study Only Information Model
			else if ((abstractSyntax.UID == DvtkData.Dul.AbstractSyntax.Patient_Study_Only_Query_Retrieve_Information_Model_MOVE.UID) &&
				(PatientStudyOnlyInformationModel != null))
			{
				// check if the information model should be refreshed before retrieving
				if (RefreshInformationModelBeforeUse == true)
				{
					PatientStudyOnlyInformationModel.RefreshInformationModel();
				}

				// perform retrieve
				retrieveList = PatientStudyOnlyInformationModel.RetrieveInformationModel(retrieveMessage);
			}

			// process the retrieve list
			return ProcessRetrieveList(moveDestinationAE, retrieveList);
		}

        private int FindMoveAEDetails(string AE)
        {
            for (int i = 0; i < MoveDestiantions.Count; i++)
            {
                if (MoveDestiantions[i].AE==AE.Trim())
                {
                    return i;
                }
            }
            return -1;
        }
        private bool ProcessRetrieveList(System.String moveDestinationAE, DvtkData.Collections.StringCollection retrieveList)
		{
			UInt16 status = 0x0000;
			UInt16 remainingSubOperations = (UInt16)retrieveList.Count;
			UInt16 completeSubOperations = 0;
			UInt16 failedSubOperations = 0;
			UInt16 warningSubOperations = 0;
			int subOperationIndex = 0;
            bool isCancelRecd = false;
            
            foreach (System.String dcmFilename in retrieveList)
            {
                status = 0xFF00;
                SendCMoveRsp(status,
                    remainingSubOperations,
                    completeSubOperations,
                    failedSubOperations,
                    warningSubOperations);

                if (HandleSubOperation(moveDestinationAE, dcmFilename, subOperationIndex) == true && cStoreStatusVal == 0x0000)
                {
                    completeSubOperations++;
                }

                else if (cStoreStatusVal == 0xB007 || cStoreStatusVal == 0xB000 || cStoreStatusVal == 0xB006)
                {
                    warningSubOperations += 1;
                }

                else
                {
                    failedSubOperations++;
                }

                remainingSubOperations--;
                subOperationIndex++;

                int waitedTime = 0;

                // Check for cancel message from SCU
                if (WaitForPendingDataInNetworkInputBuffer(100, ref waitedTime))
                {
                    DicomMessage cancelRq = ReceiveDicomMessage();

                    if (cancelRq.CommandSet.DimseCommand == DvtkData.Dimse.DimseCommand.CCANCELRQ)
                    {
                        // set up the C-FIND-RSP with cancel status
                        DicomMessage responseMessage = new DicomMessage(DvtkData.Dimse.DimseCommand.CMOVERSP);
                        responseMessage.Set("0x00000900", DvtkData.Dimse.VR.US, 0xFE00);

                        // send the response
                        this.Send(responseMessage);

                        isCancelRecd = true;
                        break;
                    }
                }
            }            

            if (!isCancelRecd)
            {
                if ((failedSubOperations > 0) || (warningSubOperations > 0))
                {
                    status = 0xB000;
                }
                //else if ((failedSubOperations == 0) && (completeSubOperations==0) && (remainingSubOperations==0) &&(warningSubOperations==0))
                //{
                //    status = 0xA702;
                //}
                else
                {
                    status = 0x0000;
                }
                SendCMoveRsp(status,
                    remainingSubOperations,
                    completeSubOperations,
                    failedSubOperations,
                    warningSubOperations);                
            }

			// message handled
			return true;
		}

		private void SendCMoveRsp(UInt16 status,
			UInt16 remainingSubOperations,
			UInt16 completeSubOperations,
			UInt16 failedSubOperations,
			UInt16 warningSubOperations)
		{
			DicomMessage responseMessage = new DicomMessage(DvtkData.Dimse.DimseCommand.CMOVERSP);
			responseMessage.Set("0x00000900", VR.US, status);
			responseMessage.Set("0x00001020", VR.US, remainingSubOperations);
			responseMessage.Set("0x00001021", VR.US, completeSubOperations);
			responseMessage.Set("0x00001022", VR.US, failedSubOperations);
			responseMessage.Set("0x00001023", VR.US, warningSubOperations);
			this.Send(responseMessage);
		}

		private bool HandleSubOperation(System.String moveDestinationAE, System.String dcmFilename, int subOperationIndex)
		{
			SCU storageScu = new SCU();

			storageScu.Initialize(DicomThread.ThreadManager);
            storageScu.Options.DeepCopyFrom(DicomThread.Options);

			storageScu.Options.Identifier = "StorageSubOperationAsScu"; 

            ////Check for Secure connection
            //if (DicomThread.Options.SecureConnection)
            //{
            //    storageScu.Options.SecureConnection = true;
            //    storageScu.Options.CertificateFilename = DicomThread.Options.CertificateFilename;
            //    storageScu.Options.CredentialsFilename = DicomThread.Options.CredentialsFilename;
            //}

			storageScu.Options.ResultsFileNameOnlyWithoutExtension = "StorageSubOperationAsScu" + subOperationIndex.ToString();
			storageScu.Options.ResultsDirectory = DicomThread.Options.ResultsDirectory;

			storageScu.Options.LocalAeTitle = DicomThread.Options.LocalAeTitle;
			storageScu.Options.LocalPort = DicomThread.Options.LocalPort;
            if (IsHaveMoveDestinations)
            {
                storageScu.Options.RemoteAeTitle = moveDestinationAE;
                storageScu.Options.RemotePort = MoveDestiantions[MoveAEdetailsIndex].Port;
                storageScu.Options.RemoteHostName = MoveDestiantions[MoveAEdetailsIndex].IP;
            }
            else
            {
                storageScu.Options.RemoteAeTitle = moveDestinationAE;
                storageScu.Options.RemotePort = DicomThread.Options.RemotePort;
                storageScu.Options.RemoteHostName = DicomThread.Options.RemoteHostName;
            }

			storageScu.Options.DataDirectory = DicomThread.Options.DataDirectory;
			storageScu.Options.StorageMode = Dvtk.Sessions.StorageMode.AsDataSet;

			// Read the DCM File
			DicomFile dcmFile = new DicomFile();
			dcmFile.Read(dcmFilename, storageScu);

			FileMetaInformation fMI = dcmFile.FileMetaInformation;

			// Get the transfer syntax and SOP class UID
			System.String transferSyntax = "1.2.840.10008.1.2";
			if((fMI != null) && fMI.Exists("0x00020010"))
			{
				// Get the Transfer syntax
				DvtkHighLevelInterface.Dicom.Other.Attribute tranferSyntaxAttr = fMI["0x00020010"];
				transferSyntax = tranferSyntaxAttr.Values[0];
			}

			Values values = dcmFile.DataSet["0x00080016"].Values;
			System.String sopClassUid = values[0];

			PresentationContext presentationContext = new PresentationContext(sopClassUid, // Abstract Syntax Name
																			transferSyntax); // Transfer Syntax Name(s)
			PresentationContext[] presentationContexts = new PresentationContext[1];
			presentationContexts[0] = presentationContext;

			DicomMessage storageMessage = new DicomMessage(DvtkData.Dimse.DimseCommand.CSTORERQ);
			storageMessage.DataSet.CloneFrom(dcmFile.DataSet);

			storageScu.Start();
		
			bool sendResult = storageScu.TriggerSendAssociationAndWait(storageMessage, presentationContexts);
			if (!sendResult)
			{
				WriteWarning("Association to move destination for Storage Sub-Operation is rejected.");
			}

			if (storageScu.HasExceptionOccured)
			{
				WriteError("Storage Sub-Operation As SCU Failed");
			}
			storageScu.Stop();

            DicomMessageCollection cStoreResponses = storageScu.Messages.DicomMessages.CStoreResponses;

            // Obtain the value of the C-STORE RSP.The value of this variable is used to determine the attributes of the C-MOVE RSP.
            foreach (DicomMessage cStoreRsp in cStoreResponses)
            {
                cStoreStatusVal = Int32.Parse(cStoreRsp.CommandSet.GetValues("0x00000900")[0]);
            }

			// Transform the sub results
            Xslt.StyleSheetFullFileName = DicomThread.Options.StyleSheetFullFileName;
			System.String htmlResultsFilename = Xslt.Transform(storageScu.Options.ResultsDirectory, storageScu.Options.ResultsFileNameOnly);

			// Make link to the sub-operation results file
			System.String message = System.String.Format("<a href=\"{0}\">Storage sub-operation {1} to AE Title \"{2}\"</a><br/>",
				htmlResultsFilename,
				subOperationIndex,
				moveDestinationAE);
			DicomThread.WriteHtmlInformation(message);

			return sendResult;
		}
	}
}
