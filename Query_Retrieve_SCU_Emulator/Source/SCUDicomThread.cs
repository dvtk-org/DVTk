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
using DvtkHighLevelInterface.Dicom.Messages;
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

    public class SCUDicomThread : DicomThread
    {
        string patientName = string.Empty;
        string patientId = string.Empty;
        string modality = string.Empty;
        string studyId = string.Empty;
        string accessionNo = string.Empty;
        string studyDate = string.Empty;
        string moveDestination = string.Empty;
        string selectedQueryRootSop = string.Empty;

        ArrayList recdPatientList = null;
        ArrayList recdStudyList = null;
        ArrayList recdSeriesList = null;
        ArrayList recdImageList = null;
        
        DvtkHighLevelInterface.Dicom.Messages.DicomMessage requestMsg = null;
        const string explicitVRLittleEndian = "1.2.840.10008.1.2.1";
        const string patientRootQRFindSOP = "1.2.840.10008.5.1.4.1.2.1.1";
        const string studyRootQRFindSOP = "1.2.840.10008.5.1.4.1.2.2.1";
        
        public SCUDicomThread()
        {
            recdPatientList = new ArrayList();
        }

        public ArrayList PatientList
        {
            get
            {
                return recdPatientList;
            }
        }

        public ArrayList StudyList
        {
            get 
            {
                return recdStudyList;
            }
        }

        //public ArrayList SeriesList
        //{
        //    get
        //    {
        //        return recdSeriesList;
        //    }
        //}

        //public ArrayList ImageList
        //{
        //    get
        //    {
        //        return recdImageList;
        //    }
        //}

        public string QueryRoot
        {
            set 
            {
                selectedQueryRootSop = value;
            }
        }
        public string PatientName
        {
            set 
            {
                patientName = value;
            }
        }

        public string PatientId
        {
            set 
            {
                patientId = value;
            }
        }

        public string Modality
        {
            set
            {
                modality = value;
            }
        }

        public string AccessionNumber
        {
            set
            {
                accessionNo = value;    
            }
        }

        public string StudyId 
        {
            set
            {
                studyId = value;
            }
        }

        public string StudyDate
        {
            set 
            {
                studyDate = value;
            }
        }        

        protected override void Execute()
        {
            SendAssociateRq(
                new PresentationContext(selectedQueryRootSop, explicitVRLittleEndian)
            );

            DulMessage dulMsg = ReceiveAssociateRp();

            if (dulMsg is AssociateAc)
            {
                if (selectedQueryRootSop == patientRootQRFindSOP)
                {
                    DoPatientLevelQuery();
                }
                else if (selectedQueryRootSop == studyRootQRFindSOP)
                {
                    DoStudyLevelQuery(patientId);
                }

                if ((this.ThreadState != ThreadState.Stopping) && (this.ThreadState != ThreadState.Stopped))
                {
                    SendReleaseRq();

                    ReceiveReleaseRp();
                }

                FilterQueryData();
            }
            else if (dulMsg is AssociateRj)
            {
                AssociateRj assocRj = (AssociateRj)dulMsg;
                string msg = string.Format("Association Rejected for proposed presentation contexts:\nResult - {0}({1})\nSource - {2}({3})\nReason - {4}({5})", assocRj.Result,
                                                                                                                                                convertResult(assocRj.Result),
                                                                                                                                                assocRj.Source,
                                                                                                                                                convertSource(assocRj.Source),
                                                                                                                                                assocRj.Reason,
                                                                                                                                                convertReason(assocRj.Source, assocRj.Reason));
                WriteInformation(msg);
            }
            else
            {
                WriteInformation("Unknown message is received from SCP.");
            }
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

        private void FilterQueryData() 
        {
            for (int i = recdPatientList.Count-1 ; i >= 0; i--)
            {
                bool removePatient = false;
                if (((Patient)recdPatientList[i]).studyList.Count == 0) {
                    recdPatientList.RemoveAt(i);
                }
                else {
                    ArrayList studyList = ((Patient)recdPatientList[i]).studyList;
                    for (int j = 0; j < studyList.Count; j++)
                    {
                        if (((Study)studyList[j]).seriesList.Count == 0) {
                            recdPatientList.RemoveAt(i);
                            removePatient = true;
                            break;
                        }
                        else {
                            ArrayList seriesList = ((Study)studyList[j]).seriesList;
                            for (int k = 0; k < seriesList.Count; k++)
                            {
                                if (((Series)seriesList[k]).imageList.Count == 0) {
                                    recdPatientList.RemoveAt(i);
                                    removePatient = true;
                                    break;
                                }
                            }    
                        }
                        if (removePatient == true)
                            break;
                    }
                }                
            }
        }

        private void DoPatientLevelQuery()
        {
            try 
            {
                PrepareRequest("PATIENT", patientId, null, null);

                //set patient level keys
                requestMsg.DataSet.Set("0x00100030", VR.DA, "");
                Send(requestMsg);

                while (true) 
                {
                    DvtkHighLevelInterface.Dicom.Messages.DicomMessage response =
                        ReceiveDicomMessage();

                    Int32 statusVal = Int32.Parse(response.CommandSet.GetValues("0x00000900")[0]);
                    if ((statusVal == 0xff00) || (statusVal == 0xff01)) 
                    {
                        Patient newPatientInfo = CreateNewPatientInfo(response.DataSet);
                        recdPatientList.Add(newPatientInfo);
                        continue;
                    }
                    else 
                    {
                        break;
                    }
                }                               
                
                for (int i = 0; i < recdPatientList.Count; i++) 
                {
                    string patId = ((Patient)recdPatientList[i]).PatientId;
                    if ((this.ThreadState != ThreadState.Stopping) && (this.ThreadState != ThreadState.Stopped))
                        DoStudyLevelQuery(patId);
                }
            } 
            catch 
            {
                return;
            }
        }

        private void DoStudyLevelQuery(string patientId)
        {
            recdStudyList = new ArrayList();

            try 
            {
                PrepareRequest("STUDY", patientId, null, null);

                //Set study level keys
                if (!string.IsNullOrEmpty(accessionNo)){
                    requestMsg.DataSet.Set("0x00080050", VR.SH, accessionNo);
                }
                else{
                    requestMsg.DataSet.Set("0x00080050", VR.SH, "");
                }
                if (!string.IsNullOrEmpty(studyId)) {
                    requestMsg.DataSet.Set("0x00200010", VR.SH, studyId);
                }else {
                    requestMsg.DataSet.Set("0x00200010", VR.SH, "");
                }
                if (!string.IsNullOrEmpty(studyDate)) {
                    requestMsg.DataSet.Set("0x00080020", VR.DA, studyDate);
                }

                Send(requestMsg);

                while (true) 
                {
                    DvtkHighLevelInterface.Dicom.Messages.DicomMessage response =
                        ReceiveDicomMessage();

                    Int32 statusVal = Int32.Parse(response.CommandSet.GetValues("0x00000900")[0]);
                    if ((statusVal == 0xff00) || (statusVal == 0xff01)) {
                        Study newStudyInfo = CreateNewStudyInfo(response.DataSet);
                        recdStudyList.Add(newStudyInfo);
                        continue;
                    } 
                    else 
                    {
                        break;
                    }
                }

                AddStudyInfo(patientId, recdStudyList);

                for (int i = 0; i < recdStudyList.Count; i++) 
                {
                    string studyInstUid = ((Study)recdStudyList[i]).StudyInstanceUID;
                    if ((this.ThreadState != ThreadState.Stopping) && (this.ThreadState != ThreadState.Stopped))
                        DoSeriesLevelQuery(patientId, studyInstUid);
                }
            }  
            catch 
            {
                return;
            }
        }

        private void DoSeriesLevelQuery(string patientId, string studyInstUid)
        {
            recdSeriesList = new ArrayList();

            try 
            {
                PrepareRequest("SERIES", patientId, studyInstUid, null);

                if (!string.IsNullOrEmpty(modality)) 
                {
                    requestMsg.DataSet.Set("0x00080060", VR.CS, modality);
                }
                Send(requestMsg);

                while (true) 
                {
                    DvtkHighLevelInterface.Dicom.Messages.DicomMessage response =
                        ReceiveDicomMessage();

                    Int32 statusVal = Int32.Parse(response.CommandSet.GetValues("0x00000900")[0]);
                    if ((statusVal == 0xff00) || (statusVal == 0xff01)) 
                    {
                        Series newSeriesInfo = CreateNewSeriesInfo(response.DataSet);
                        recdSeriesList.Add(newSeriesInfo);
                        continue;
                    } 
                    else 
                    {
                        break;
                    }
                }

                if (selectedQueryRootSop == patientRootQRFindSOP) 
                {
                    AddSeriesInfo(patientId, studyInstUid, recdSeriesList);
                } 
                else 
                {
                    AddStudyRootSeriesInfo(studyInstUid, recdSeriesList);
                }

                for (int i = 0; i < recdSeriesList.Count; i++) 
                {
                    string seriesInstUid = ((Series)recdSeriesList[i]).SeriesUID;
                    if ((this.ThreadState != ThreadState.Stopping) && (this.ThreadState != ThreadState.Stopped))
                        DoImageLevelQuery(patientId, studyInstUid, seriesInstUid);
                }
            }  
            catch 
            {
                return;
            }
        }

        private void DoImageLevelQuery(string patientId, string studyInstUid, string seriesInstUid)
        {
            recdImageList = new ArrayList();

            try 
            {
                PrepareRequest("IMAGE", patientId, studyInstUid, seriesInstUid);

                Send(requestMsg);

                while (true) 
                {
                    DvtkHighLevelInterface.Dicom.Messages.DicomMessage response =
                        ReceiveDicomMessage();

                    Int32 statusVal = Int32.Parse(response.CommandSet.GetValues("0x00000900")[0]);
                    if ((statusVal == 0xff00) || (statusVal == 0xff01)) 
                    {
                        Image newImageInfo = CreateNewImageInfo(response.DataSet);
                        recdImageList.Add(newImageInfo);
                        continue;
                    } 
                    else 
                    {
                        break;
                    }
                }
            } 
            catch 
            {
                return;
            }

            if (selectedQueryRootSop == patientRootQRFindSOP) 
            {
                AddImageInfo(patientId, studyInstUid, seriesInstUid, recdImageList);
            } 
            else 
            {
                AddStudyRootImageInfo(studyInstUid, seriesInstUid, recdImageList);
            }
        }

        private void PrepareRequest(
            string queryLevel,
            string patientId,
            string studyInstUid,
            string seriesInstUid
            )
        {
            requestMsg = new DvtkHighLevelInterface.Dicom.Messages.DicomMessage(DimseCommand.CFINDRQ);
            
            requestMsg.CommandSet.Set("0x00000002", VR.UI, selectedQueryRootSop);
            requestMsg.CommandSet.Set("0x00000800", VR.US, 0);
            requestMsg.DataSet.Set("0x00080052", VR.CS, queryLevel); // Query/Retrieve Level

            if (queryLevel == "PATIENT")
            {
                if (!string.IsNullOrEmpty(patientName))
                {
                    requestMsg.DataSet.Set("0x00100010", VR.PN, patientName);// Patient's name.
                }
                else
                {
                    requestMsg.DataSet.Set("0x00100010", VR.PN);// Patient's name.
                }

                if (!string.IsNullOrEmpty(patientId))
                {
                    requestMsg.DataSet.Set("0x00100020", VR.LO, patientId);//Patient ID.
                }
                else
                {
                    requestMsg.DataSet.Set("0x00100020", VR.LO);//Patient ID.
                }
            }

            if (queryLevel == "STUDY")
            {
                if (!string.IsNullOrEmpty(patientId))
                {
                    requestMsg.DataSet.Set("0x00100020", VR.LO, patientId);//Patient ID.
                }
                else
                {
                    requestMsg.DataSet.Set("0x00100020", VR.LO);//Patient ID.
                }

                if (!string.IsNullOrEmpty(studyInstUid))
                {
                    requestMsg.DataSet.Set("0x0020000D", VR.UI, studyInstUid);
                }
                else
                {
                    requestMsg.DataSet.Set("0x0020000D", VR.UI);
                }
            }

            if (queryLevel == "SERIES")
            {
                if (!string.IsNullOrEmpty(patientId))
                {
                    requestMsg.DataSet.Set("0x00100020", VR.LO, patientId);//Patient ID.
                }
                else
                {
                    requestMsg.DataSet.Set("0x00100020", VR.LO);//Patient ID.
                }

                if (!string.IsNullOrEmpty(studyInstUid))
                {
                    requestMsg.DataSet.Set("0x0020000D", VR.UI, studyInstUid);
                }
                else
                {
                    requestMsg.DataSet.Set("0x0020000D", VR.UI);
                }

                if (!string.IsNullOrEmpty(seriesInstUid))
                {
                    requestMsg.DataSet.Set("0x0020000E", VR.UI, seriesInstUid);
                }
                else
                {
                    requestMsg.DataSet.Set("0x0020000E", VR.UI);
                }
            }

            if (queryLevel == "IMAGE")
            {
                if (!string.IsNullOrEmpty(patientId))
                {
                    requestMsg.DataSet.Set("0x00100020", VR.LO, patientId);//Patient ID.
                }
                else
                {
                    requestMsg.DataSet.Set("0x00100020", VR.LO);//Patient ID.
                }

                if (!string.IsNullOrEmpty(studyInstUid))
                {
                    requestMsg.DataSet.Set("0x0020000D", VR.UI, studyInstUid);
                }
                else
                {
                    requestMsg.DataSet.Set("0x0020000D", VR.UI);
                }

                if (!string.IsNullOrEmpty(seriesInstUid))
                {
                    requestMsg.DataSet.Set("0x0020000E", VR.UI, seriesInstUid);
                }
                else
                {
                    requestMsg.DataSet.Set("0x0020000E", VR.UI);
                }

                requestMsg.DataSet.Set("0x00080018", VR.UI);
            }
        }

        private void AddStudyInfo(string patientId, ArrayList studyList)
        {
            for (int i = 0; i < recdPatientList.Count; i++)
            {
                string patId = ((Patient)recdPatientList[i]).PatientId;
                if (patId == patientId)
                {
                    ((Patient)recdPatientList[i]).studyList = studyList;
                    return;
                }
            }
        }

        private void AddSeriesInfo(string patientId, string studyInstUid, ArrayList seriesList)
        {
            for (int i = 0; i < recdPatientList.Count; i++)
            {
                string patId = ((Patient)recdPatientList[i]).PatientId;
                if (patId == patientId)
                {
                    ArrayList studyList = ((Patient)recdPatientList[i]).studyList;
                    for (int j = 0; j < studyList.Count; j++)
                    {
                        string stdInsUid = ((Study)studyList[j]).StudyInstanceUID;
                        if (stdInsUid == studyInstUid)
                        {
                            ((Study)((Patient)recdPatientList[i]).studyList[j]).seriesList = seriesList;
                            return;
                        }
                    }
                }
            }
        }

        private void AddImageInfo(string patientId, string studyInstUid, string seriesInstUid, ArrayList imageList)
        {
            for (int i = 0; i < recdPatientList.Count; i++)
            {
                string patId = ((Patient)recdPatientList[i]).PatientId;
                if (patId == patientId)
                {
                    ArrayList studyList = ((Patient)recdPatientList[i]).studyList;
                    for (int j = 0; j < studyList.Count; j++)
                    {
                        string stdInsUid = ((Study)studyList[j]).StudyInstanceUID;
                        if (stdInsUid == studyInstUid)
                        {
                            ArrayList seriesList = ((Study)studyList[j]).seriesList;
                            for (int k = 0; k < seriesList.Count; k++)
                            {
                                string srsInUid = ((Series)seriesList[k]).SeriesUID;
                                if (srsInUid == seriesInstUid)
                                {
                                    ((Series)((Study)((Patient)recdPatientList[i]).studyList[j]).seriesList[k]).imageList = imageList;
                                    return;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void AddStudyRootSeriesInfo(string studyInstUid, ArrayList seriesList) 
        {
                                
            for (int i = 0; i < recdStudyList.Count; i++) 
            {
                string stdInsUid = ((Study)recdStudyList[i]).StudyInstanceUID;
                if (stdInsUid == studyInstUid) 
                {
                    ((Study)recdStudyList[i]).seriesList = seriesList;
                    return;
                }
            }     
        }

        private void AddStudyRootImageInfo(string studyInstUid, string seriesInstUid, ArrayList imageList) 
        {
            for (int i = 0; i < recdStudyList.Count; i++) 
            {
                string stdInsUid = ((Study)recdStudyList[i]).StudyInstanceUID;
                if (stdInsUid == studyInstUid) 
                {
                    ArrayList seriesList = ((Study)recdStudyList[i]).seriesList;
                    for (int j = 0; j < seriesList.Count; j++) 
                    {
                        string srsInUid = ((Series)seriesList[j]).SeriesUID;
                        if (srsInUid == seriesInstUid) 
                        {
                            ((Series)((Study)recdStudyList[i]).seriesList[j]).imageList = imageList;
                            return;
                        }
                    }
                }
            }
        }
        
        private Patient CreateNewPatientInfo(DvtkHighLevelInterface.Dicom.Other.DataSet dataSet)
        {
            string patientName = "";
            if (dataSet.Exists("0x00100010"))
            {
                HLI.Attribute patientNameAtt = dataSet["0x00100010"];
                patientName = patientNameAtt.Values[0];
            }

            string patientId = "";
            if (dataSet.Exists("0x00100020"))
            {
                HLI.Attribute patientIdAtt = dataSet["0x00100020"];
                patientId = patientIdAtt.Values[0];
            }

            string patientBd = "";
            if (dataSet.Exists("0x00100030"))
            {
                HLI.Attribute patientBdAtt = dataSet["0x00100030"];
                patientBd = patientBdAtt.Values[0];
            }

            Patient patient = new Patient();
            patient.PatientName = patientName;
            patient.PatientId = patientId;
            patient.PatientBirthDate = patientBd;

            return patient;
        }


        private Study CreateNewStudyInfo(DvtkHighLevelInterface.Dicom.Other.DataSet dataSet)
        {
            string studyId = "";
            if (dataSet.Exists("0x00200010"))
            {
                HLI.Attribute studyIdAtt = dataSet["0x00200010"];
                studyId = studyIdAtt.Values[0];
            }

            string accNr = "";
            if (dataSet.Exists("0x00080050"))
            {
                HLI.Attribute accNrAtt = dataSet["0x00080050"];
                accNr = accNrAtt.Values[0];
            }

            string studyInstUid = "";
            if (dataSet.Exists("0x0020000D"))
            {
                HLI.Attribute studyInstUidAtt = dataSet["0x0020000D"];
                studyInstUid = studyInstUidAtt.Values[0];
            }

            Study study = new Study();
            study.StudyID = studyId;
            study.AccessionNumber = accNr;
            study.StudyInstanceUID = studyInstUid;

            return study;
        }

        private Series CreateNewSeriesInfo(DvtkHighLevelInterface.Dicom.Other.DataSet dataSet)
        {
            string seriesInstUid = "";
            if (dataSet.Exists("0x0020000E"))
            {
                HLI.Attribute seriesInstUidAtt = dataSet["0x0020000E"];
                seriesInstUid = seriesInstUidAtt.Values[0];
            }

            Series series = new Series();
            series.SeriesUID = seriesInstUid;

            return series;
        }

        private Image CreateNewImageInfo(DvtkHighLevelInterface.Dicom.Other.DataSet dataSet)
        {
            string sopInstUid = "";
            if (dataSet.Exists("0x00080018"))
            {
                HLI.Attribute sopInstUidAtt = dataSet["0x00080018"];
                sopInstUid = sopInstUidAtt.Values[0];
            }
            Image image = new Image();
            image.SOPInstanceUID = sopInstUid;

            return image;
        }
    }
}