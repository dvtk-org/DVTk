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
using System.IO;
using System.Windows.Forms;

using NUnit.Framework;
using NUnit.Framework.Constraints;
using NUnit.Framework.SyntaxHelpers;

using VR=DvtkData.Dimse.VR;
using DimseCommand = DvtkData.Dimse.DimseCommand;
using DvtkHighLevelInterface.Common.Threads;
using DvtkHighLevelInterface.Common.UserInterfaces;
using DvtkHighLevelInterface.Dicom.Other;
using DvtkHighLevelInterface.Dicom.Threads;
using DvtkHighLevelInterface.NUnit;




namespace DvtkHighLevelInterface.Dicom.Messages
{
    /// <summary>
    /// Contains NUnit Test Cases.
    /// </summary>
    [TestFixture]
    public class DicomMessage_NUnit
    {
        //
        // - Methods containing common functionality for all test methods -
        //

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
        }

        /// <summary>
        /// This method is performed once after all tests are completed in this
        /// class.
        /// </summary>
        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            Dvtk.Setup.Terminate();
        }



        //
        // - Test methods -
        //

        /// <summary>
        /// Test for ticket 411.
        /// </summary>
        [Test]
        public void Ticket411_1_1()
        {
            ThreadManager threadManager = new ThreadManager();

            Ticket411DicomThread ticket411DicomThread = new Ticket411DicomThread();
            ticket411DicomThread.Initialize(threadManager);

            ticket411DicomThread.Options.ResultsDirectory = Path.GetTempPath();
            ticket411DicomThread.Options.ResultsFileNameOnlyWithoutExtension = "Ticket411_1_1";
            ticket411DicomThread.Options.LoadDefinitionFile(Path.Combine(Paths.DefinitionsDirectoryFullPath, "SecondaryCaptureImageStorage.def"));

            // Ticket411: enable temporarily below to see where the results file is written.
            // MessageBox.Show(ticket411DicomThread.Options.ResultsFullFileName);
            
            ticket411DicomThread.Start();
            ticket411DicomThread.WaitForCompletion();
        }


        public static DicomMessage CreateValidCEchoRq()
        {
            DicomMessage dicomMessage = new DicomMessage(DimseCommand.CECHORQ);

            dicomMessage.CommandSet.Set("0x00000000", VR.UL, 10); // "Group length", set to incorrect value.
            dicomMessage.CommandSet.Set("0x00000002", VR.UI, "1.2.840.10008.1.1"); // "Affected SOP Class UID"
            dicomMessage.CommandSet.Set("0x00000110", VR.US, 100); // "Message ID"
            dicomMessage.CommandSet.Set("0x00000800", VR.US, 0x101); // "Data Set Type"

            return (dicomMessage);
        }

        public static DicomMessage CreateValidPatientRootQueryRetrieveCFindRq()
        {
            DicomMessage dicomMessage = new DicomMessage(DimseCommand.CFINDRQ);

            dicomMessage.CommandSet.Set("0x00000000", VR.UL, 10); // "Group length", set to incorrect value.
            dicomMessage.CommandSet.Set("0x00000002", VR.UI, "1.2.840.10008.5.1.4.1.2.1.1"); // "Affected SOP Class UID"
            dicomMessage.CommandSet.Set("0x00000110", VR.US, 100); // "Message ID"
            dicomMessage.CommandSet.Set("0x00000700", VR.US, 0); // "Priority"
            dicomMessage.CommandSet.Set("0x00000800", VR.US, 0); // "Data Set Type"

            dicomMessage.DataSet.Set("0x00080052", VR.CS, "PATIENT"); // "Query Retrieve Level"
            dicomMessage.DataSet.Set("0x00100020", VR.LO); // "Patient ID."

            return (dicomMessage);
        }

        private class Ticket411DicomThread : DicomThread
        {
            protected override void Execute()
            {
                DicomMessage dicomMessage = new DicomMessage(DimseCommand.CSTORERQ);

                dicomMessage.CommandSet.Set("0x00000002", VR.UI, "1.2.840.10008.5.1.4.1.1.7"); // "Affected SOP Class UID"
                dicomMessage.CommandSet.Set("0x00001000", VR.UI, "5.6.7.8.9"); // "Affected SOP Instance UID"
                dicomMessage.CommandSet.Set("0x00000110", VR.US, 100); // "Message ID"
                dicomMessage.CommandSet.Set("0x00000700", VR.US, 0); // "Priority"

                // dicomMessage.CommandSet.Set("0x00100010", VR.PN, "Patient name in Command Set");

                dicomMessage.DataSet.Set("0x00100010", VR.PN, "Patient name in Data Set");

                Validate(dicomMessage);
            }
        }

        /// <summary>
        /// Test for ticket 1293.
        /// </summary>
        [Test]
        public void Ticket1293_1_1()
        {
            ThreadManager threadManager = new ThreadManager();

            Ticket1293MainDicomThread ticket1293MainDicomThread = new Ticket1293MainDicomThread("1.2.840.10008.1.2");

            ticket1293MainDicomThread.Initialize(threadManager);

            Config.SetOptions(ticket1293MainDicomThread, "Ticket1293", "Main 1.2.840.10008.1.2");

            HliForm hliForm = new HliForm();
            hliForm.Attach(ticket1293MainDicomThread);

            ticket1293MainDicomThread.Start();

            ticket1293MainDicomThread.WaitForCompletion();

            Assert.That(ticket1293MainDicomThread.NrOfErrors + ticket1293MainDicomThread.NrOfWarnings, Is.EqualTo(0));

        }

        /// <summary>
        /// Test for ticket 1293.
        /// </summary>
        [Test]
        public void Ticket1293_2_1()
        {
            ThreadManager threadManager = new ThreadManager();

            Ticket1293MainDicomThread ticket1293MainDicomThread = new Ticket1293MainDicomThread("1.2.840.10008.1.2.1");

            ticket1293MainDicomThread.Initialize(threadManager);

            Config.SetOptions(ticket1293MainDicomThread, "Ticket1293", "Main 1.2.840.10008.1.2.1");

            HliForm hliForm = new HliForm();
            hliForm.Attach(ticket1293MainDicomThread);

            ticket1293MainDicomThread.Start();

            ticket1293MainDicomThread.WaitForCompletion();

            Assert.That(ticket1293MainDicomThread.NrOfErrors + ticket1293MainDicomThread.NrOfWarnings, Is.EqualTo(0));
        }


        /// <summary>
        /// Test for ticket 1293.
        /// </summary>
        [Test]
        public void Ticket1293_3_1()
        {
            ThreadManager threadManager = new ThreadManager();

            Ticket1293MainDicomThread ticket1293MainDicomThread = new Ticket1293MainDicomThread("1.2.840.10008.1.2.2");

            ticket1293MainDicomThread.Initialize(threadManager);

            Config.SetOptions(ticket1293MainDicomThread, "Ticket1293", "Main 1.2.840.10008.1.2.2");
            ticket1293MainDicomThread.Options.LoadDefinitionFile(Path.Combine(Paths.DefinitionsDirectoryFullPath, "PatientRootQueryRetrieve-FIND.def"));

            HliForm hliForm = new HliForm();
            hliForm.Attach(ticket1293MainDicomThread);

            ticket1293MainDicomThread.Start();

            ticket1293MainDicomThread.WaitForCompletion();

            Assert.That(ticket1293MainDicomThread.NrOfErrors + ticket1293MainDicomThread.NrOfWarnings, Is.EqualTo(0));
        }

        /// <summary>
        /// Test for ticket 1293.
        /// </summary>
        [Test]
        public void Ticket1293_4_1()
        {
            ThreadManager threadManager = new ThreadManager();

            Ticket1293MainDicomThread ticket1293MainDicomThread = new Ticket1293MainDicomThread("1.2.840.10008.5.1.4.1.1.7");

            ticket1293MainDicomThread.Initialize(threadManager);

            Config.SetOptions(ticket1293MainDicomThread, "Ticket1293", "Main 1.2.840.10008.5.1.4.1.1.7");
            ticket1293MainDicomThread.Options.LoadDefinitionFile(Path.Combine(Paths.DefinitionsDirectoryFullPath, "PatientRootQueryRetrieve-FIND.def"));

            HliForm hliForm = new HliForm();
            hliForm.Attach(ticket1293MainDicomThread);

            ticket1293MainDicomThread.Start();

            ticket1293MainDicomThread.WaitForCompletion();

            Assert.That(ticket1293MainDicomThread.NrOfErrors + ticket1293MainDicomThread.NrOfWarnings, Is.EqualTo(0));
        }

        private class Ticket1293MainDicomThread: DicomThread
        {
            private String transferSyntaxUid = null;

            public DicomThread_NUnit.CFINDSCUThread scu = null;

            public DicomThread_NUnit.CFINDSCPThread scp = null;

            public Ticket1293MainDicomThread(String transferSyntaxUid)
            {
                this.transferSyntaxUid = transferSyntaxUid;
            }

            /// <summary>
            /// Determines if the first received DicomMessage in a DicomThread instance has an explicit transfer syntax.
            /// </summary>
            /// <param name="dicomThread">The DicomThread.</param>
            /// <returns>Boolean indicating if the first received DicomMessage has an explicit transfer syntax.</returns>
            private bool IsDataTransferExplicit(DicomThread dicomThread)
            {
                bool isDataTransferExplicit = true;

                if ((dicomThread.Messages.ReceivedMessages.DicomMessages.Count > 0))
                {
                    DicomMessage firstReceivedDicomMessage = dicomThread.Messages.ReceivedMessages.DicomMessages[0];

                    byte presentationContextIdFirstReceivedDicomMessage = firstReceivedDicomMessage.EncodedPresentationContextID;

                    AssociateAc firstReceivedAssociateAc = null;

                    foreach (DulMessage dulMessage in dicomThread.Messages.SendMessages.DulMessages)
                    {
                        if (dulMessage is AssociateAc)
                        {
                            firstReceivedAssociateAc = dulMessage as AssociateAc;

                            break;
                        }
                    }

                    if (firstReceivedAssociateAc != null)
                    {
                        foreach (PresentationContext presentationContext in firstReceivedAssociateAc.PresentationContexts)
                        {
                            if (presentationContext.ID == presentationContextIdFirstReceivedDicomMessage)
                            {
                                String transferSyntax = presentationContext.TransferSyntax;

                                if (transferSyntax == "1.2.840.10008.1.2")
                                {
                                    isDataTransferExplicit = false;
                                }
                                else
                                {
                                    isDataTransferExplicit = true;
                                }
                            }
                        }
                    }
                }

                return (isDataTransferExplicit);
            }

            protected override void Execute()
            {
                DicomMessage cFindRequest = new DicomMessage(DimseCommand.CFINDRQ);
                cFindRequest.CommandSet.Set("0x00000002", VR.UI, "1.2.840.10008.5.1.4.1.2.1.1"); // Set Affected SOP class to Patient Root Query/Retrieve Information Model – FIND
                cFindRequest.DataSet.Set("0x00080052", VR.CS, "PATIENT"); // "Query Retrieve Level"
                cFindRequest.DataSet.Set("0x00100020", VR.LO, "ABCD"); // Patient ID.

                scu = new DicomThread_NUnit.CFINDSCUThread(cFindRequest, this.transferSyntaxUid);

                scu.Initialize(this);

                Config.SetOptions(scu, "Ticket1293", "SCU " + this.transferSyntaxUid);
                scu.Options.LoadDefinitionFile(Path.Combine(Paths.DefinitionsDirectoryFullPath, "PatientRootQueryRetrieve-FIND.def"));

                scp = new DicomThread_NUnit.CFINDSCPThread();

                scp.Initialize(this);
                scp.Options.LoadDefinitionFile(Path.Combine(Paths.DefinitionsDirectoryFullPath, "PatientRootQueryRetrieve-FIND.def"));

                Config.SetOptions(scp, "Ticket1293", "SCP " + this.transferSyntaxUid);

                scp.Start();

                // Wait 1 seconds before starting the SCU.
                Sleep(1000);

                scu.Start();

                scp.WaitForCompletion();

                DicomMessage receivedCFindRequest = scp.Messages.DicomMessages.CFindRequests[0];

                Options.DvtkScriptSession.IsDataTransferExplicit = IsDataTransferExplicit(this.scp);

                Validate(receivedCFindRequest);

                WriteInformation(receivedCFindRequest.DataSet.DumpUsingVisualBasicNotation("dataSet"));
            }
        }
    }
}