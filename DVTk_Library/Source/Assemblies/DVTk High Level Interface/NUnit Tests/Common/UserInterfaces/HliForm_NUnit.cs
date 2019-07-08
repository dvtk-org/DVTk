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
using System.Windows.Forms;

using VR=DvtkData.Dimse.VR;
using DvtkHighLevelInterface.Common.Threads;
using DvtkHighLevelInterface.Dicom.Threads;



namespace DvtkHighLevelInterface.Common.UserInterfaces
{
    /// <summary>
    /// Contains NUnit Test Cases.
    /// </summary>
    [TestFixture]
    public class HliForm_NUnit
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
        /// Tests the stability of the HliForm when using the text property.
        /// </summary>
        [Test]
        public void TestTextProperty()
        {
            ThreadManager threadManager = new ThreadManager();

            HliForm hliForm = new HliForm();

            DicomThread1 dicomThread1 = new DicomThread1(hliForm);
            dicomThread1.Initialize(threadManager);
            // No logging needed this time.
            dicomThread1.Options.StartAndStopResultsGatheringEnabled = false;

            hliForm.Attach(dicomThread1);
            dicomThread1.Start();
            dicomThread1.WaitForCompletion();
        }

        [Test]
        public void Ticket429_1_1()
        {
            ThreadManager threadManager = new ThreadManager();

            LotsOfLoggingDicomThread lotsOfLoggingDicomThread1 = new LotsOfLoggingDicomThread();
            lotsOfLoggingDicomThread1.Initialize(threadManager);

            HliForm hliForm = new HliForm();
            // hliForm.AutoExit = false;
            hliForm.Attach(lotsOfLoggingDicomThread1);

            lotsOfLoggingDicomThread1.Start();
            lotsOfLoggingDicomThread1.WaitForCompletion();

            hliForm.SetNumberOfLinesActivityLogging(500, 250);

            LotsOfLoggingDicomThread lotsOfLoggingDicomThread2 = new LotsOfLoggingDicomThread();
            lotsOfLoggingDicomThread2.Initialize(threadManager);
            hliForm.Attach(lotsOfLoggingDicomThread2);
            lotsOfLoggingDicomThread2.Start();

            hliForm.WaitUntilClosed();
        }

        [Test]
        public void Ticket429_2_1()
        {
            ThreadManager threadManager = new ThreadManager();

            LotsOfLoggingDicomThread lotsOfLoggingDicomThread1 = new LotsOfLoggingDicomThread();
            lotsOfLoggingDicomThread1.Initialize(threadManager);

            HliForm hliForm = new HliForm();
            // hliForm.AutoExit = false;
            hliForm.Attach(lotsOfLoggingDicomThread1);

            lotsOfLoggingDicomThread1.Start();
            lotsOfLoggingDicomThread1.WaitForCompletion();

            hliForm.ClearActivityLogging();

            LotsOfLoggingDicomThread lotsOfLoggingDicomThread2 = new LotsOfLoggingDicomThread();
            lotsOfLoggingDicomThread2.Initialize(threadManager);
            hliForm.Attach(lotsOfLoggingDicomThread2);
            lotsOfLoggingDicomThread2.numberOfLinesToLog = 10;
            lotsOfLoggingDicomThread2.Start();

            hliForm.WaitUntilClosed();
        }

        private class DicomThread1 : DicomThread
        {
            private HliForm hliForm = null;

            public DicomThread1(HliForm hliForm)
            {
                this.hliForm = hliForm;
            }

            protected override void Execute()
            {
                Sleep(500);
                this.hliForm.Text = "Text 1";
                Sleep(500);
                this.hliForm.Text = "";
                Sleep(500);
                this.hliForm.Text = "Text 2";
                Sleep(500);
                this.hliForm.Text = null;
                Sleep(500);
                this.hliForm.Text = " ";
                Sleep(500);
                this.hliForm.Text = null;
                Sleep(500);
            }
        }

        private class LotsOfLoggingDicomThread : DicomThread
        {
            public int numberOfLinesToLog = 2000;

            protected override void Execute()
            {
                for (int counter = 0; counter < numberOfLinesToLog; counter++)
                {
                    WriteInformation("Text to log: " + counter.ToString() + " of " + numberOfLinesToLog.ToString());
                }

                Sleep(1000);
            }
        }
    }
}