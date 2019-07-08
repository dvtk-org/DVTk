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
using NUnit.Framework;
using NUnit.Framework.Constraints;
using NUnit.Framework.SyntaxHelpers;
using System.IO;


using DimseCommand = DvtkData.Dimse.DimseCommand;
using VR = DvtkData.Dimse.VR;
using DvtkHighLevelInterface.Dicom.Messages;
using DvtkHighLevelInterface.NUnit;
using DataSet = DvtkHighLevelInterface.Dicom.Other.DataSet;
using DvtkHighLevelInterface.Common.Threads;
using DvtkHighLevelInterface.Common.UserInterfaces;
using DvtkHighLevelInterface.Dicom.Other;
using DvtkHighLevelInterface.Dicom.Threads;


namespace DvtkHighLevelInterface.InformationModel
{
    /// <summary>
    /// Contains NUnit Test Cases.
    /// </summary>
    [TestFixture]
    public class ModalityWorklistInformationModel_NUnit
    {
        //
        // - Methods containing common functionality for all test methods -
        //
        public static ModalityWorklistInformationModel modalityWorklistInformationModel = null;

        /// <summary>
        /// This method is performed just before each test method is called.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            
        }

        /// <summary>
        /// This method is performed after each test method is run.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
        }

        /// <summary>
        /// This method is performed once prior to executing any of the tests
        /// in this class.
        /// </summary>
        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            Dvtk.Setup.Initialize();

            modalityWorklistInformationModel = new ModalityWorklistInformationModel();
            modalityWorklistInformationModel.DataDirectory = Paths.ResultsDirectoryFullPath;

            String dataDirectoryFullName = Path.Combine(Paths.DataDirectoryFullPath, "Ticket1152");
            DirectoryInfo directoryInfo = new DirectoryInfo(dataDirectoryFullName);

            FileInfo[] fileInfos = directoryInfo.GetFiles();

            if (fileInfos.Length != 0)
            {
                foreach (FileInfo fileInfo in fileInfos)
                {
                    DataSet dataSet = new DataSet();

                    dataSet.Read(fileInfo.FullName);                        

                    modalityWorklistInformationModel.AddToInformationModel(dataSet);
                }
            }
        }

        /// <summary>
        /// This method is performed once after all tests are completed in this
        /// class.
        /// </summary>
        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            if (modalityWorklistInformationModel != null)
                modalityWorklistInformationModel = null;

            Dvtk.Setup.Terminate();
        }



        //
        // - Test methods -
        //
        
        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void Ticket1152_1_1()
        {
            //
            // Create and fill the worklist request.
            //
            DicomMessage worklistQueryRequest = new DicomMessage(DimseCommand.CFINDRQ);

            worklistQueryRequest.CommandSet.Set("0x00000002", VR.UI, "1.2.840.10008.5.1.4.31");
            worklistQueryRequest.DataSet.Set("0x00100010", VR.PN, "*");
            worklistQueryRequest.DataSet.Set("0x00100020", VR.LO, "");

            ThreadManager threadManager = new ThreadManager();

            Ticket1152DicomThread thread = new Ticket1152DicomThread(worklistQueryRequest,1);

            thread.Initialize(threadManager);

            Config.SetOptions(thread, "Ticket1152", "Ticket1152_1_1");

            thread.Start();

            thread.WaitForCompletion();

            Assert.That(thread.RspMessageCount, Is.EqualTo(4));
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void Ticket1152_2_1()
        {
            //
            // Create and fill the worklist request.
            //
            DicomMessage worklistQueryRequest = new DicomMessage(DimseCommand.CFINDRQ);

            worklistQueryRequest.CommandSet.Set("0x00000002", VR.UI, "1.2.840.10008.5.1.4.31");
            worklistQueryRequest.DataSet.Set("0x00100010", VR.PN, "?^Secondary Capture Image");
            worklistQueryRequest.DataSet.Set("0x00100020", VR.LO, "");

            ThreadManager threadManager = new ThreadManager();

            Ticket1152DicomThread thread = new Ticket1152DicomThread(worklistQueryRequest, 2);

            thread.Initialize(threadManager);

            Config.SetOptions(thread, "Ticket1152", "Ticket1152_2_1");

            thread.Start();

            thread.WaitForCompletion();

            Assert.That(thread.RspMessageCount, Is.EqualTo(4));
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void Ticket1152_3_1()
        {
            //
            // Create and fill the worklist request.
            //
            DicomMessage worklistQueryRequest = new DicomMessage(DimseCommand.CFINDRQ);

            worklistQueryRequest.CommandSet.Set("0x00000002", VR.UI, "1.2.840.10008.5.1.4.31");
            worklistQueryRequest.DataSet.Set("0x00100010", VR.PN, "One^*");
            worklistQueryRequest.DataSet.Set("0x00100020", VR.LO, "");

            ThreadManager threadManager = new ThreadManager();

            Ticket1152DicomThread thread = new Ticket1152DicomThread(worklistQueryRequest, 3);

            thread.Initialize(threadManager);

            Config.SetOptions(thread, "Ticket1152", "Ticket1152_3_1");

            thread.Start();

            thread.WaitForCompletion();

            Assert.That(thread.RspMessageCount, Is.EqualTo(2));
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void Ticket1152_4_1()
        {
            //
            // Create and fill the worklist request.
            //
            DicomMessage worklistQueryRequest = new DicomMessage(DimseCommand.CFINDRQ);

            worklistQueryRequest.CommandSet.Set("0x00000002", VR.UI, "1.2.840.10008.5.1.4.31");
            worklistQueryRequest.DataSet.Set("0x00100020", VR.LO, "");
            worklistQueryRequest.DataSet.Set("0x00100010", VR.PN, "");
            worklistQueryRequest.DataSet.Set("0x00400100[1]/0x00400001", VR.AE, "");
            worklistQueryRequest.DataSet.Set("0x00400100[1]/0x00400002", VR.DA, "");
            worklistQueryRequest.DataSet.Set("0x00400100[1]/0x00080060", VR.CS, "CT");

            ThreadManager threadManager = new ThreadManager();

            Ticket1152DicomThread thread = new Ticket1152DicomThread(worklistQueryRequest, 4);

            thread.Initialize(threadManager);

            Config.SetOptions(thread, "Ticket1152", "Ticket1152_4_1");

            thread.Start();

            thread.WaitForCompletion();

            Assert.That(thread.RspMessageCount, Is.EqualTo(3));
        }

        private class Ticket1152DicomThread : DicomThread
        {
            DicomMessage cFindRequest = null;
            int i = 0;
            CFINDSCP scp = null;

            public Ticket1152DicomThread(DicomMessage reqMsg, int index)
            {
                cFindRequest = reqMsg;
                i = index;
            }

            public int RspMessageCount
            {
                get
                {
                    return scp.RspMessageCount;
                }
            }

            protected override void Execute()
            {
                //HliForm hliForm = new HliForm();
                //hliForm.Attach(this);
                //hliForm.AutoExit = false;

                CFINDSCU scu = new CFINDSCU(cFindRequest);

                scu.Initialize(this);

                Config.SetOptions(scu, "Ticket1152", "SCU " + i.ToString());

                scp = new CFINDSCP();

                scp.Initialize(this);
                scp.Options.LoadDefinitionFile(Path.Combine(Paths.DefinitionsDirectoryFullPath, "ModalityWorklist-FIND.def"));

                Config.SetOptions(scp, "Ticket1152", "SCP " + i.ToString());

                scp.Start();

                // Wait 1 seconds before starting the SCU.
                Sleep(1000);

                scu.Start();

                scp.WaitForCompletion();                
            }
        }

        public class CFINDSCU : DicomThread
        {
            private DicomMessage cFindMessage = null;

            public CFINDSCU(DicomMessage cFindMsg)
            {
                cFindMessage = cFindMsg;
            }

            protected override void Execute()
            {
                PresentationContext pc = new PresentationContext("1.2.840.10008.5.1.4.31", "1.2.840.10008.1.2");
                
                SendAssociateRq(pc);

                ReceiveAssociateAc();

                Send(cFindMessage);

                while (true)
                {
                    DicomMessage response = ReceiveDicomMessage();

                    Int32 statusVal = Int32.Parse(response.CommandSet.GetValues("0x00000900")[0]);
                    if ((statusVal == 0xff00) || (statusVal == 0xff01))
                    {
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }

                SendReleaseRq();

                ReceiveReleaseRp();
            }
        }

        public class CFINDSCP : DicomThread
        {
            public CFINDSCP(){}

            int count = 0;

            public int RspMessageCount
            {
                get
                {
                    return count;
                }
            }

            protected override void Execute()
            {
                ReceiveAssociateRq();

                SendAssociateAc();

                DicomMessage receivedCFindRequest = ReceiveDicomMessage();

                //
                // Query the information model.
                //
                DicomMessageCollection worklistQueryResponses = modalityWorklistInformationModel.QueryInformationModel(receivedCFindRequest);

                count = worklistQueryResponses.Count;

                if (worklistQueryResponses.Count > 1)
			    {
                    WriteInformation(string.Format("Sending {0} C-FIND responses after performing query.\r\n", worklistQueryResponses.Count));
			    }
			    else
			    {
				    WriteWarning("No response from MWL information model after performing query.\r\n");
			    }

			    // send responses
                foreach (DicomMessage responseMessage in worklistQueryResponses)
                {
                    this.Send(responseMessage);
                }

                ReceiveReleaseRq();

                SendReleaseRp();
            }
        }
    }
}