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
using System.Collections.Generic;
using System.IO;
using System.Text;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using NUnit.Framework.SyntaxHelpers;

using DimseCommand = DvtkData.Dimse.DimseCommand;
using VR = DvtkData.Dimse.VR;
using DvtkHighLevelInterface.Common.Threads;
using DvtkHighLevelInterface.Dicom.Messages;
using DvtkHighLevelInterface.Dicom.Other;
using DvtkHighLevelInterface.NUnit;
using DvtkHighLevelInterface.Common.UserInterfaces;



namespace DvtkHighLevelInterface.Dicom.Threads
{
    /// <summary>
    /// Contains NUnit Test Cases.
    /// </summary>
    [TestFixture]
    public class ConcurrentMessageIterator_NUnit
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
        /// Test for ticket 1232.
        /// </summary>
        [Test]
        public void Ticket1232_1_1()
        {
            ThreadManager threadManager1 = new ThreadManager();
            ThreadManager threadManager2 = new ThreadManager();

            HliForm hliForm = new HliForm();

            EchoSCP echoSCP = new EchoSCP("SCP_Association");
            echoSCP.Initialize(threadManager1);

            Config.SetOptions(echoSCP, "Ticket1232_1_1", "SCP");
            echoSCP.Options.AutoValidate = false;

            hliForm.Attach(echoSCP);

            echoSCP.Start();

            System.Threading.Thread.Sleep(2000);

            for (int index = 0; index < 10; index++)
            {
                DicomThread_NUnit.EchoSCU echoSCU = new DicomThread_NUnit.EchoSCU();

                echoSCU.Initialize(threadManager2);

                Config.SetOptions(echoSCU, "Ticket1232_1_1", "SCU_" + index.ToString());
                echoSCU.Options.AutoValidate = false;

                hliForm.Attach(echoSCU);

                echoSCU.Start();

                System.Threading.Thread.Sleep(500);
            }

            threadManager2.WaitForCompletionThreads();

            echoSCP.Stop();
            echoSCP.WaitForCompletion();

            Assert.That(echoSCP.NrOfErrors + echoSCP.NrOfWarnings, Is.EqualTo(0));
        }

        public class EchoSCP : ConcurrentMessageIterator
        {
            public EchoSCP(String identifierBasisChildThreads)
                : base(identifierBasisChildThreads)
            {

            }

            public override void AfterHandlingAssociateRequest(AssociateRq associateRq)
            {
                if (!IsMessageHandled)
                {
                    SendAssociateRp();
                    IsMessageHandled = true;
                }
            }

            public override void AfterHandlingReleaseRequest(ReleaseRq releaseRq)
            {
                if (!IsMessageHandled)
                {
                    SendReleaseRp();
                    IsMessageHandled = true;
                }
            }

            protected override void AfterHandlingCEchoRequest(DicomMessage dicomMessage)
            {
                if (!IsMessageHandled)
                {
                    DicomMessage CEchoRsp = new DicomMessage(DimseCommand.CECHORSP);

                    CEchoRsp.Set("0x00000002", VR.UI, "1.2.840.10008.1.1");
                    CEchoRsp.Set("0x00000900", VR.US, 0);

                    Send(CEchoRsp);

                    IsMessageHandled = true;
                }
            }
        }
    }
}