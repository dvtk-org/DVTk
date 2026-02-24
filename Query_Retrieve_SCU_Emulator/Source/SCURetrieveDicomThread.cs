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
using DIMSE = DvtkHighLevelInterface.Dicom.Messages;
using DvtkHighLevelInterface.Dicom.Other;
using DvtkHighLevelInterface.Dicom.Threads;
using DvtkHighLevelInterface.InformationModel;
using DvtkHighLevelInterface.Common.Threads;
using DvtkData.Dimse;
using System.Collections;
using System.Collections.Generic;

namespace QR_SCU_Emulator
{

    using HLI = DvtkHighLevelInterface.Dicom.Other;

    public class SCURetrieveDicomThread : DicomThread
    {        
        string patientId = string.Empty;
        string studyInstanceUID = string.Empty;
        string seriesUID = string.Empty;
        string sopInstanceUID = string.Empty;
        string queryRoot = string.Empty;
        string queryLevel = string.Empty;
        string moveDestination = string.Empty;
        UInt16 remainingSubOperations = 0;
        UInt16 completeSubOperations = 0;
        UInt16 failedSubOperations = 0;
        UInt16 warningSubOperations = 0;

        const string explicitVRLittleEndian = "1.2.840.10008.1.2.1";
        const string patientRootQRMoveSOP = "1.2.840.10008.5.1.4.1.2.1.2";
        const string studyRootQRMoveSOP = "1.2.840.10008.5.1.4.1.2.2.2";

        public SCURetrieveDicomThread(Hashtable queryKeys)
        {
            patientId = (string)queryKeys["PatientId"];
            studyInstanceUID = (string)queryKeys["StudyInstanceUID"];
            seriesUID = (string)queryKeys["SeriesUID"];
            sopInstanceUID = (string)queryKeys["SopInstanceUID"];
            queryRoot = (string)queryKeys["QueryRoot"];
            queryLevel = (string)queryKeys["QueryLevel"];
            moveDestination = (string)queryKeys["MoveDestination"];
        }

        public UInt16 RemainingSubOperations
        {
            get
            {
                return remainingSubOperations;
            }
        }

        public UInt16 CompleteSubOperations
        {
            get
            {
                return completeSubOperations;
            }
        }

        public UInt16 FailedSubOperations
        {
            get
            {
                return failedSubOperations;
            }
        }

        public UInt16 WarningSubOperations
        {
            get
            {
                return warningSubOperations;
            }
        }

        protected override void Execute()
        {
            SendAssociateRq(
                new PresentationContext(queryRoot, explicitVRLittleEndian)
            );

            ReceiveAssociateAc();

            SendCMoveRq();

            SendReleaseRq();

            ReceiveReleaseRp();               
        }

        private void SendCMoveRq() {

            DIMSE.DicomMessage cMoveRq = PrepareCMoveCommand();
            Send(cMoveRq);
            
            while (true) {
                DIMSE.DicomMessage cMoveResponse = ReceiveDicomMessage();

                Int32 statusVal = Int32.Parse(cMoveResponse.CommandSet.GetValues("0x00000900")[0]);
                if ((statusVal == 0xff00) || (statusVal == 0xff01)) 
                {
                    continue;
                }
                else if (statusVal == 0x0000)
                {
                    remainingSubOperations = UInt16.Parse(cMoveResponse.CommandSet.GetValues("0x00001020")[0]);
                    completeSubOperations = UInt16.Parse(cMoveResponse.CommandSet.GetValues("0x00001021")[0]);
                    failedSubOperations = UInt16.Parse(cMoveResponse.CommandSet.GetValues("0x00001022")[0]);
                    warningSubOperations = UInt16.Parse(cMoveResponse.CommandSet.GetValues("0x00001023")[0]);
                    break;
                }
                else
                {
                    break;
                }
            }
        }

        private DIMSE.DicomMessage PrepareCMoveCommand() {

            DIMSE.DicomMessage cMoveRq = new DIMSE.DicomMessage(DimseCommand.CMOVERQ);

            if (queryRoot == patientRootQRMoveSOP)
            {
                cMoveRq.CommandSet.Set("0x00000002", VR.UI, patientRootQRMoveSOP);
            }
            else if (queryRoot == studyRootQRMoveSOP) 
            {
                cMoveRq.CommandSet.Set("0x00000002", VR.UI, studyRootQRMoveSOP);
            }
            
            cMoveRq.CommandSet.Set("0x00000800", VR.US, 0);
            cMoveRq.CommandSet.Set("0x00000600", VR.AE, moveDestination);

            cMoveRq.DataSet.Set("0x00080052", VR.CS, queryLevel);

            if (!string.IsNullOrEmpty(patientId)) 
                cMoveRq.DataSet.Set("0x00100020", VR.LO, patientId);
            
            if (!string.IsNullOrEmpty(studyInstanceUID)) 
                cMoveRq.DataSet.Set("0x0020000D", VR.UI, studyInstanceUID);

            if (!string.IsNullOrEmpty(seriesUID))
                cMoveRq.DataSet.Set("0x0020000E", VR.UI, seriesUID);

            if (!string.IsNullOrEmpty(sopInstanceUID)) 
                cMoveRq.DataSet.Set("0x00080018", VR.UI, sopInstanceUID);            

            return cMoveRq;
        }
       
    }
}