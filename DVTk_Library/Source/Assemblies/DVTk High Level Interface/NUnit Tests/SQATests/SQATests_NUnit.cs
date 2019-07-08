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
using System.Reflection;
using System.Windows.Forms;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using NUnit.Framework.SyntaxHelpers;

using VR=DvtkData.Dimse.VR;
using DvtkHighLevelInterface.NUnit;
using DvtkHighLevelInterface.Common.Threads;
using DvtkHighLevelInterface.Dicom.Messages;
using DvtkHighLevelInterface.Dicom.Other;
using DvtkHighLevelInterface.Dicom.Threads;
using DvtkHighLevelInterface.Common.Other;
using DvtkHighLevelInterface.Common.UserInterfaces;



namespace DvtkHighLevelInterface.SqaTests
{
    /// <summary>
    /// Contains NUnit Test Cases.
    /// </summary>
    [TestFixture]
    public class SqaTests_NUnit
    {
        //
        // - Fields -
        //

        DicomMainThread dicomThread = null;

        int SESSION_INDEX  = 1;

        int TEST_CASE_INDEX  = 2;

        int PASSED_FAILED_INDEX  = 3;

        int COMMENT_INDEX  = 4;

        public static bool testFinished = false;

        /// <summary>
        /// Used to lock the testFinished field.
        /// </summary>
        public static Object lockObject = new Object();
        
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

            ThreadManager threadManager = new ThreadManager();
            dicomThread = new DicomMainThread();
            dicomThread.Initialize(threadManager);

            dicomThread.Options.ResultsDirectory = Paths.ResultsDirectoryFullPath;
            dicomThread.Options.Identifier = "Overview";
            dicomThread.Options.LogChildThreadsOverview = false;

            HliForm theHliForm = new HliForm();
            theHliForm.Attach(dicomThread);

            theHliForm.AutoExit = true;

            dicomThread.Start();            
        }

        /// <summary>
        /// This method is performed once after all tests are completed in this
        /// class.
        /// </summary>
        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            lock (lockObject)
            {
                testFinished = true;
            }

            dicomThread.WaitForCompletion();

            Dvtk.Setup.Terminate();
        }

        //
        // - Private methods -
        //

        private DicomThreadScriptExecution CreateDicomScriptDicomThread(string sessionFile, string scriptFile)
        {
            DicomThreadScriptExecution dicomScriptThread = new DicomThreadScriptExecution(scriptFile);
            dicomScriptThread.Initialize(dicomThread);

            dicomScriptThread.Options.LoadFromFile(sessionFile);
            dicomScriptThread.Options.LogWaitingForCompletionChildThreads = false;
            dicomScriptThread.Options.LogThreadStartingAndStoppingInParent = false;
            dicomScriptThread.Options.ResultsDirectory = Paths.ResultsDirectoryFullPath;
            dicomScriptThread.Options.Identifier = (Path.GetFileName(sessionFile) + "_" + Path.GetFileName(scriptFile)).Replace(".", "_");

            return (dicomScriptThread);
        }

        private bool ValidateDicomScript(string sessionFile, 
                                         string scriptFile,
                                         int expectedNrOfGeneralErrors,
                                         int expectedNrOfGeneralWarnings,
                                         int expectedNrOfUserErrors,
                                         int expectedNrOfUserWarnings,
                                         int expectedNrOfValidationErrors,
                                         int expectedNrOfValidationWarnings)
        {
            bool expectedEqualsActuals = true;
            
            DicomThreadScriptExecution dicomScriptThread = CreateDicomScriptDicomThread(sessionFile, scriptFile);

            dicomScriptThread.Start();
            dicomScriptThread.WaitForCompletion();

            CheckErrorsAndWarnings(dicomScriptThread, scriptFile, expectedNrOfGeneralErrors, expectedNrOfGeneralWarnings, expectedNrOfUserErrors, expectedNrOfUserWarnings, expectedNrOfValidationErrors, expectedNrOfValidationWarnings);

            dicomScriptThread.Options.DvtkScriptSession.DefinitionManagement.UnLoadDefinitionFiles();

            expectedEqualsActuals = expectedEqualsActuals && (dicomScriptThread.NrOfGeneralErrors == expectedNrOfGeneralErrors);
            expectedEqualsActuals = expectedEqualsActuals && (dicomScriptThread.NrOfGeneralWarnings == expectedNrOfGeneralWarnings);
            expectedEqualsActuals = expectedEqualsActuals && (dicomScriptThread.NrOfUserErrors == expectedNrOfUserErrors);
            expectedEqualsActuals = expectedEqualsActuals && (dicomScriptThread.NrOfUserWarnings == expectedNrOfUserWarnings);
            expectedEqualsActuals = expectedEqualsActuals && (dicomScriptThread.NrOfValidationErrors == expectedNrOfValidationErrors);
            expectedEqualsActuals = expectedEqualsActuals && (dicomScriptThread.NrOfValidationWarnings == expectedNrOfValidationWarnings);

            dicomThread.Table.NewRow();
            dicomThread.Table.AddBlackItem(SESSION_INDEX, "-");
            dicomThread.Table.AddBlackItem(TEST_CASE_INDEX, "-");
            dicomThread.Table.AddBlackItem(PASSED_FAILED_INDEX, "-");
            dicomThread.Table.AddBlackItem(COMMENT_INDEX, "-");

            return (expectedEqualsActuals);
        }

        private bool ValidateDicomScriptPair(string scpScriptFile,
                                         string scpSessionFile,
                                         string scuScriptFile,
                                         string scuSessionFile,
                                         int expectedNrOfGeneralErrors1,
                                         int expectedNrOfGeneralWarnings1,
                                         int expectedNrOfUserErrors1,
                                         int expectedNrOfUserWarnings1,
                                         int expectedNrOfValidationErrors1,
                                         int expectedNrOfValidationWarnings1,
                                         int expectedNrOfGeneralErrors2,
                                         int expectedNrOfGeneralWarnings2,
                                         int expectedNrOfUserErrors2,
                                         int expectedNrOfUserWarnings2,
                                         int expectedNrOfValidationErrors2,
                                         int expectedNrOfValidationWarnings2)
        {
            bool expectedEqualsActuals = true;

            DicomThreadScriptExecution scpDicomThread = CreateDicomScriptDicomThread(scpSessionFile, scpScriptFile);
            DicomThreadScriptExecution scuDicomThread = CreateDicomScriptDicomThread(scuSessionFile, scuScriptFile);

            scpDicomThread.Start();
            scuDicomThread.Start(500);

            scpDicomThread.WaitForCompletion();
            scuDicomThread.WaitForCompletion();

            CheckErrorsAndWarnings(scpDicomThread, scpScriptFile, expectedNrOfGeneralErrors1, expectedNrOfGeneralWarnings1, expectedNrOfUserErrors1, expectedNrOfUserWarnings1, expectedNrOfValidationErrors1, expectedNrOfValidationWarnings1);
            CheckErrorsAndWarnings(scuDicomThread, scuScriptFile, expectedNrOfGeneralErrors2, expectedNrOfGeneralWarnings2, expectedNrOfUserErrors2, expectedNrOfUserWarnings2, expectedNrOfValidationErrors2, expectedNrOfValidationWarnings2);

            scpDicomThread.Options.DvtkScriptSession.DefinitionManagement.UnLoadDefinitionFiles();
            scuDicomThread.Options.DvtkScriptSession.DefinitionManagement.UnLoadDefinitionFiles();

            expectedEqualsActuals = expectedEqualsActuals && (scpDicomThread.NrOfGeneralErrors == expectedNrOfGeneralErrors1);
            expectedEqualsActuals = expectedEqualsActuals && (scpDicomThread.NrOfGeneralWarnings == expectedNrOfGeneralWarnings1);
            expectedEqualsActuals = expectedEqualsActuals && (scpDicomThread.NrOfUserErrors == expectedNrOfUserErrors1);
            expectedEqualsActuals = expectedEqualsActuals && (scpDicomThread.NrOfUserWarnings == expectedNrOfUserWarnings1);
            expectedEqualsActuals = expectedEqualsActuals && (scpDicomThread.NrOfValidationErrors == expectedNrOfValidationErrors1);
            expectedEqualsActuals = expectedEqualsActuals && (scpDicomThread.NrOfValidationWarnings == expectedNrOfValidationWarnings1);

            expectedEqualsActuals = expectedEqualsActuals && (scuDicomThread.NrOfGeneralErrors == expectedNrOfGeneralErrors2);
            expectedEqualsActuals = expectedEqualsActuals && (scuDicomThread.NrOfGeneralWarnings == expectedNrOfGeneralWarnings2);
            expectedEqualsActuals = expectedEqualsActuals && (scuDicomThread.NrOfUserErrors == expectedNrOfUserErrors2);
            expectedEqualsActuals = expectedEqualsActuals && (scuDicomThread.NrOfUserWarnings == expectedNrOfUserWarnings2);
            expectedEqualsActuals = expectedEqualsActuals && (scuDicomThread.NrOfValidationErrors == expectedNrOfValidationErrors2);
            expectedEqualsActuals = expectedEqualsActuals && (scuDicomThread.NrOfValidationWarnings == expectedNrOfValidationWarnings2);

            dicomThread.Table.NewRow();
            dicomThread.Table.AddBlackItem(SESSION_INDEX, "-");
            dicomThread.Table.AddBlackItem(TEST_CASE_INDEX, "-");
            dicomThread.Table.AddBlackItem(PASSED_FAILED_INDEX, "-");
            dicomThread.Table.AddBlackItem(COMMENT_INDEX, "-");

            return (expectedEqualsActuals);
        }

        private void CheckErrorsAndWarnings(DicomThread dicomScriptThread,
                                         string scriptFile,
                                         int expectedNrOfGeneralErrors,
                                         int expectedNrOfGeneralWarnings,
                                         int expectedNrOfUserErrors,
                                         int expectedNrOfUserWarnings,
                                         int expectedNrOfValidationErrors,
                                         int expectedNrOfValidationWarnings)
        {
            bool different = false;

            // Start with new row
            dicomThread.Table.NewRow();

            // Comment column
            if (expectedNrOfGeneralErrors != dicomScriptThread.NrOfGeneralErrors)
            {
                dicomThread.Table.AddRedItem(COMMENT_INDEX, String.Format("Number of general errors differs (expected {0}, actual {1}).", expectedNrOfGeneralErrors, dicomScriptThread.NrOfGeneralErrors));
                different = true;
            }

            // Comment column
            if (expectedNrOfGeneralWarnings != dicomScriptThread.NrOfGeneralWarnings)
            {
                dicomThread.Table.AddRedItem(COMMENT_INDEX, String.Format("Number of general warnings differs (expected {0}, actual {1}).", expectedNrOfGeneralWarnings, dicomScriptThread.NrOfGeneralWarnings));
                different = true;
            }

            // Comment column
            if (expectedNrOfUserErrors != dicomScriptThread.NrOfUserErrors)
            {
                dicomThread.Table.AddRedItem(COMMENT_INDEX, String.Format("Number of user errors differs (expected {0}, actual {1}).", expectedNrOfUserErrors, dicomScriptThread.NrOfUserErrors));
                different = true;
            }

            // Comment column
            if (expectedNrOfUserWarnings != dicomScriptThread.NrOfUserWarnings)
            {
                dicomThread.Table.AddRedItem(COMMENT_INDEX, String.Format("Number of user warnings differs (expected {0}, actual {1}).", expectedNrOfUserWarnings, dicomScriptThread.NrOfUserWarnings));
                different = true;
            }

            // Comment column
            if (expectedNrOfValidationErrors != dicomScriptThread.NrOfValidationErrors)
            {
                dicomThread.Table.AddRedItem(COMMENT_INDEX, String.Format("Number of validation errors differs (expected {0}, actual {1}).", expectedNrOfValidationErrors, dicomScriptThread.NrOfValidationErrors));
                different = true;
            }

            // Comment column
            if (expectedNrOfValidationWarnings != dicomScriptThread.NrOfValidationWarnings)
            {
                dicomThread.Table.AddRedItem(COMMENT_INDEX, String.Format("Number of validation warnings differs (expected {0}, actual {1}).", expectedNrOfValidationWarnings, dicomScriptThread.NrOfValidationWarnings));
                different = true;
            }

            dicomThread.Table.AddBlackItem(COMMENT_INDEX, "<A HREF='" + dicomScriptThread.Options.DetailResultsFileNameOnly + "'>See detail" + "</A>" + " " + "<A HREF='" + dicomScriptThread.Options.SummaryResultsFileNameOnly + "'>See summary" + "</A>");

            // Session column.
            dicomThread.Table.AddBlackItem(SESSION_INDEX, dicomScriptThread.Options.DvtkScriptSession.SessionFileName);


            // Test Case column.
            dicomThread.Table.AddBlackItem(TEST_CASE_INDEX, scriptFile);


            // PASSED or FAILED column.
            if (different)
                dicomThread.Table.AddRedItem(PASSED_FAILED_INDEX, "F");
            else
                dicomThread.Table.AddBlackItem(PASSED_FAILED_INDEX, "P");
        }

        //
        // - Test methods -
        //

        /// <summary>
        /// Test the usage of a Values instance in the Set method.
        /// </summary>
        [Test]
        public void val_test_vr_fail()
        {
            string scriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"Validation\val_test_vr_fail.ds");
            string sessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"Validation\test_validation.ses");

            Assert.That(ValidateDicomScript(sessionFile, scriptFile,0,0,0,0,102,2));            
        }

        /// <summary>
        /// Test the usage of a Values instance in the Set method.
        /// </summary>
        [Test]
        public void val_test_vr_pass()
        {
            string scriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"Validation\val_test_vr_pass.ds");
            string sessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"Validation\test_validation.ses");

            Assert.That(ValidateDicomScript(sessionFile, scriptFile, 0, 0, 0, 0, 2, 0));
        }

        /// <summary>
        /// Test the usage of a Values instance in the Set method.
        /// </summary>
        [Test]
        public void val_test_vm_fail()
        {
            string scriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"Validation\val_test_vm_fail.ds");
            string sessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"Validation\test_validation.ses");

            Assert.That(ValidateDicomScript(sessionFile, scriptFile, 0, 0, 0, 0, 12, 0));
        }

        /// <summary>
        /// Test the usage of a Values instance in the Set method.
        /// </summary>
        [Test]
        public void val_test_vm_pass()
        {
            string scriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"Validation\val_test_vm_pass.ds");
            string sessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"Validation\test_validation.ses");

            Assert.That(ValidateDicomScript(sessionFile, scriptFile, 0, 0, 0, 0, 0, 0));
        }

        /// <summary>
        /// Test the usage of a Values instance in the Set method.
        /// </summary>
        [Test]
        public void val_test_sq_nesting()
        {
            string scriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"Validation\val_test_sq_nesting.ds");
            string sessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"Validation\test_validation.ses");

            Assert.That(ValidateDicomScript(sessionFile, scriptFile, 0, 0, 0, 0, 14, 0));
        }

        /// <summary>
        /// Test the usage of a Values instance in the Set method.
        /// </summary>
        [Test]
        public void val_test_ref()
        {
            string scriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"Validation\val_test_ref.ds");
            string sessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"Validation\test_validation.ses");

            Assert.That(ValidateDicomScript(sessionFile, scriptFile, 0, 0, 0, 0, 74, 0));
        }

        /// <summary>
        /// Test the usage of a Values instance in the Set method.
        /// </summary>
        [Test]
        public void val_test_ranges()
        {
            string scriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"Validation\val_test_ranges.ds");
            string sessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"Validation\test_validation.ses");

            Assert.That(ValidateDicomScript(sessionFile, scriptFile, 0, 0, 0, 0, 22, 23));
        }

        /// <summary>
        /// Test the usage of a Values instance in the Set method.
        /// </summary>
        [Test]
        public void val_test_presence()
        {
            string scriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath,@"Validation\val_test_presence.ds");
            string sessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"Validation\test_validation.ses");

            Assert.That(ValidateDicomScript(sessionFile, scriptFile, 0, 0, 0, 0, 39, 0));
        }

        /// <summary>
        /// Test the usage of a Values instance in the Set method.
        /// </summary>
        [Test]
        public void val_test_conditions()
        {
            string scriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"Validation\val_test_conditions.ds");
            string sessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"Validation\test_validation.ses");

            Assert.That(ValidateDicomScript(sessionFile, scriptFile, 0, 0, 0, 0, 25, 0));
        }

        /// <summary>
        /// Test the usage of a Values instance in the Set method.
        /// </summary>
        [Test]
        public void val_test_ts_1()
        {
            string scpScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_1_scp.ds");
            string scpSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_SCP.ses");

            string scuScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_1_scu.ds");
            string scuSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_SCU.ses");

            Assert.That(ValidateDicomScriptPair(scpScriptFile, scpSessionFile, scuScriptFile, scuSessionFile,0,0,0,0,0,0,0,0,0,0,0,0));
        }

        /// <summary>
        /// Test the usage of a Values instance in the Set method.
        /// </summary>
        [Test]
        public void val_test_ts_2()
        {
            string scpScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_2_scp.ds");
            string scpSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_SCP.ses");

            string scuScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_2_scu.ds");
            string scuSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_SCU.ses");

            Assert.That(ValidateDicomScriptPair(scpScriptFile, scpSessionFile, scuScriptFile, scuSessionFile, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0));
        }

        /// <summary>
        /// Test the usage of a Values instance in the Set method.
        /// </summary>
        [Test]
        public void val_test_ts_3()
        {
            string scpScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_3_scp.ds");
            string scpSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_SCP.ses");

            string scuScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_3_scu.ds");
            string scuSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_SCU.ses");

            Assert.That(ValidateDicomScriptPair(scpScriptFile, scpSessionFile, scuScriptFile, scuSessionFile, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0));
        }

        /// <summary>
        /// Test the usage of a Values instance in the Set method.
        /// </summary>
        [Test]
        public void val_test_ts_4()
        {
            string scpScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_4_scp.ds");
            string scpSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_SCP.ses");

            string scuScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_4_scu.ds");
            string scuSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_SCU.ses");

            Assert.That(ValidateDicomScriptPair(scpScriptFile, scpSessionFile, scuScriptFile, scuSessionFile, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0));
        }

        /// <summary>
        /// Test the usage of a Values instance in the Set method.
        /// </summary>
        [Test]
        public void val_test_ts_5()
        {
            string scpScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_5_scp.ds");
            string scpSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_SCP.ses");

            string scuScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_5_scu.ds");
            string scuSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_SCU.ses");

            Assert.That(ValidateDicomScriptPair(scpScriptFile, scpSessionFile, scuScriptFile, scuSessionFile, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0));
        }

        /// <summary>
        /// Test the usage of a Values instance in the Set method.
        /// </summary>
        [Test]
        public void val_test_ts_6()
        {
            string scpScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_6_scp.ds");
            string scpSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_SCP.ses");

            string scuScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_6_scu.ds");
            string scuSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_SCU.ses");

            Assert.That(ValidateDicomScriptPair(scpScriptFile, scpSessionFile, scuScriptFile, scuSessionFile, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0));
        }

        /// <summary>
        /// Test the usage of a Values instance in the Set method.
        /// </summary>
        [Test]
        public void val_test_ts_7()
        {
            string scpScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_7_scp.ds");
            string scpSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_SCP.ses");

            string scuScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_7_scu.ds");
            string scuSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_SCU.ses");

            Assert.That(ValidateDicomScriptPair(scpScriptFile, scpSessionFile, scuScriptFile, scuSessionFile, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0));
        }

        /// <summary>
        /// Test the usage of a Values instance in the Set method.
        /// </summary>
        [Test]
        public void val_test_ts_8()
        {
            string scpScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_8_scp.ds");
            string scpSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_SCP.ses");

            string scuScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_8_scu.ds");
            string scuSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_SCU.ses");

            Assert.That(ValidateDicomScriptPair(scpScriptFile, scpSessionFile, scuScriptFile, scuSessionFile, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0));
        }

        /// <summary>
        /// Test the usage of a Values instance in the Set method.
        /// </summary>
        [Test]
        public void val_test_ts_9()
        {
            string scpScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_9_scp.ds");
            string scpSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_SCP.ses");

            string scuScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_9_scu.ds");
            string scuSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_SCU.ses");

            Assert.That(ValidateDicomScriptPair(scpScriptFile, scpSessionFile, scuScriptFile, scuSessionFile, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0));
        }

        /// <summary>
        /// Test the usage of a Values instance in the Set method.
        /// </summary>
        [Test]
        public void val_test_ts_10()
        {
            string scpScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_10_scp.ds");
            string scpSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_SCP.ses");

            string scuScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_10_scu.ds");
            string scuSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_SCU.ses");

            Assert.That(ValidateDicomScriptPair(scpScriptFile, scpSessionFile, scuScriptFile, scuSessionFile, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0));
        }

        /// <summary>
        /// Test the usage of a Values instance in the Set method.
        /// </summary>
        [Test]
        public void val_test_ts_11()
        {
            string scpScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_11_scp.ds");
            string scpSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_SCP.ses");

            string scuScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_11_scu.ds");
            string scuSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_SCU.ses");

            Assert.That(ValidateDicomScriptPair(scpScriptFile, scpSessionFile, scuScriptFile, scuSessionFile, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0));
        }

        /// <summary>
        /// Test the usage of a Values instance in the Set method.
        /// </summary>
        [Test]
        public void val_test_ts_12()
        {
            string scpScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_12_scp.ds");
            string scpSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_SCP.ses");

            string scuScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_12_scu.ds");
            string scuSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_SCU.ses");

            Assert.That(ValidateDicomScriptPair(scpScriptFile, scpSessionFile, scuScriptFile, scuSessionFile, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0));
        }

        /// <summary>
        /// Test the usage of a Values instance in the Set method.
        /// </summary>
        [Test]
        public void val_test_ts_13()
        {
            string scpScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_13_scp.ds");
            string scpSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_SCP.ses");

            string scuScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_13_scu.ds");
            string scuSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_SCU.ses");

            Assert.That(ValidateDicomScriptPair(scpScriptFile, scpSessionFile, scuScriptFile, scuSessionFile, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0));
        }

        /// <summary>
        /// Test the usage of a Values instance in the Set method.
        /// </summary>
        [Test]
        public void val_test_ts_14()
        {
            string scpScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_14_scp.ds");
            string scpSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_SCP.ses");

            string scuScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_14_scu.ds");
            string scuSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_SCU.ses");

            Assert.That(ValidateDicomScriptPair(scpScriptFile, scpSessionFile, scuScriptFile, scuSessionFile, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0));
        }

        /// <summary>
        /// Test the usage of a Values instance in the Set method.
        /// </summary>
        [Test]
        public void val_test_ts_15()
        {
            string scpScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_15_scp.ds");
            string scpSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_SCP.ses");

            string scuScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_15_scu.ds");
            string scuSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_SCU.ses");

            Assert.That(ValidateDicomScriptPair(scpScriptFile, scpSessionFile, scuScriptFile, scuSessionFile, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0));
        }

        /// <summary>
        /// Test the usage of a Values instance in the Set method.
        /// </summary>
        [Test]
        public void val_test_ts_16()
        {
            string scpScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_16_scp.ds");
            string scpSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_SCP.ses");

            string scuScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_16_scu.ds");
            string scuSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_SCU.ses");

            Assert.That(ValidateDicomScriptPair(scpScriptFile, scpSessionFile, scuScriptFile, scuSessionFile, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0));
        }

        /// <summary>
        /// Test the usage of a Values instance in the Set method.
        /// </summary>
        [Test]
        public void val_test_ts_17()
        {
            string scpScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_17_scp.ds");
            string scpSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_SCP.ses");

            string scuScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_17_scu.ds");
            string scuSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_SCU.ses");

            Assert.That(ValidateDicomScriptPair(scpScriptFile, scpSessionFile, scuScriptFile, scuSessionFile, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0));
        }

        /// <summary>
        /// Test the usage of a Values instance in the Set method.
        /// </summary>
        [Test]
        public void val_test_ts_18()
        {
            string scpScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_18_scp.ds");
            string scpSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_SCP.ses");

            string scuScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_18_scu.ds");
            string scuSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_SCU.ses");

            Assert.That(ValidateDicomScriptPair(scpScriptFile, scpSessionFile, scuScriptFile, scuSessionFile, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0));
        }

        /// <summary>
        /// Test the usage of a Values instance in the Set method.
        /// </summary>
        [Test]
        public void val_test_ts_19()
        {
            string scpScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_19_scp.ds");
            string scpSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_SCP.ses");

            string scuScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_19_scu.ds");
            string scuSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_SCU.ses");

            Assert.That(ValidateDicomScriptPair(scpScriptFile, scpSessionFile, scuScriptFile, scuSessionFile, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0));
        }

        /// <summary>
        /// Test the usage of a Values instance in the Set method.
        /// </summary>
        [Test]
        public void val_test_ts_20()
        {
            string scpScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_20_scp.ds");
            string scpSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_SCP.ses");

            string scuScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_20_scu.ds");
            string scuSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_SCU.ses");

            Assert.That(ValidateDicomScriptPair(scpScriptFile, scpSessionFile, scuScriptFile, scuSessionFile, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0));
        }

        /// <summary>
        /// Test the usage of a Values instance in the Set method.
        /// </summary>
        [Test]
        public void val_test_ts_21()
        {
            string scpScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_21_scp.ds");
            string scpSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_SCP.ses");

            string scuScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_21_scu.ds");
            string scuSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_SCU.ses");

            Assert.That(ValidateDicomScriptPair(scpScriptFile, scpSessionFile, scuScriptFile, scuSessionFile, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0));
        }

        /// <summary>
        /// Test the usage of a Values instance in the Set method.
        /// </summary>
        [Test]
        public void val_test_ts_22()
        {
            string scpScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_22_scp.ds");
            string scpSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_SCP.ses");

            string scuScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_22_scu.ds");
            string scuSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_SCU.ses");

            Assert.That(ValidateDicomScriptPair(scpScriptFile, scpSessionFile, scuScriptFile, scuSessionFile, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0));
        }

        /// <summary>
        /// Test the usage of a Values instance in the Set method.
        /// </summary>
        [Test]
        public void val_test_ts_23()
        {
            string scpScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_23_scp.ds");
            string scpSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_SCP.ses");

            string scuScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_23_scu.ds");
            string scuSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_SCU.ses");

            Assert.That(ValidateDicomScriptPair(scpScriptFile, scpSessionFile, scuScriptFile, scuSessionFile, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0));
        }

        /// <summary>
        /// Test the usage of a Values instance in the Set method.
        /// </summary>
        [Test]
        public void val_test_ts_24()
        {
            string scpScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_24_scp.ds");
            string scpSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_SCP.ses");

            string scuScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_24_scu.ds");
            string scuSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"TransferSyntax\ts_SCU.ses");

            Assert.That(ValidateDicomScriptPair(scpScriptFile, scpSessionFile, scuScriptFile, scuSessionFile, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0));
        }

        /// <summary>
        /// Test the usage of a Values instance in the Set method.
        /// </summary>
        [Test]
        public void script_test_warehouse_commands4()
        {
            string scriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"scripting\script_test_warehouse_commands4.ds");
            string sessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"scripting\test_DICOMScriptLanguage.ses");

            Assert.That(ValidateDicomScript(sessionFile, scriptFile, 0, 0, 0, 0, 0, 0));
        }

        /// <summary>
        /// Test the usage of a Values instance in the Set method.
        /// </summary>
        [Test]
        public void script_test_warehouse_commands3()
        {
            string scriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"scripting\script_test_warehouse_commands3.ds");
            string sessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"scripting\test_DICOMScriptLanguage.ses");

            Assert.That(ValidateDicomScript(sessionFile, scriptFile, 1, 0, 0, 0, 0, 0));
        }

        /// <summary>
        /// Test the usage of a Values instance in the Set method.
        /// </summary>
        [Test]
        public void script_test_warehouse_commands2()
        {
            string scriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"scripting\script_test_warehouse_commands2.ds");
            string sessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"scripting\test_DICOMScriptLanguage.ses");

            Assert.That(ValidateDicomScript(sessionFile, scriptFile, 2, 0, 0, 0, 0, 0));
        }

        /// <summary>
        /// Test the usage of a Values instance in the Set method.
        /// </summary>
        [Test]
        public void script_test_warehouse_commands1()
        {
            string scriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"scripting\script_test_warehouse_commands1.ds");
            string sessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"scripting\test_DICOMScriptLanguage.ses");

            Assert.That(ValidateDicomScript(sessionFile, scriptFile, 1, 0, 0, 0, 0, 0));
        }

        /// <summary>
        /// Test the usage of a Values instance in the Set method.
        /// </summary>
        [Test]
        public void script_test_delay()
        {
            string scpScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"scripting\script_test_delay_scp.ds");
            string scpSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"scripting\test_DICOMScriptLanguage_scp.ses");

            string scuScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"scripting\script_test_delay_scu.ds");
            string scuSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"scripting\test_DICOMScriptLanguage_scu.ses");

            Assert.That(ValidateDicomScriptPair(scpScriptFile, scpSessionFile, scuScriptFile, scuSessionFile, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0));
        }

        /// <summary>
        /// Test the usage of a Values instance in the Set method.
        /// </summary>
        [Test]
        public void script_test_import()
        {
            string scpScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"scripting\script_test_import.ds");
            string scpSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"scripting\test_DICOMScriptLanguage_scp.ses");

            string scuScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"scripting\script_test_export.ds");
            string scuSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"scripting\test_DICOMScriptLanguage_scu.ses");

            Assert.That(ValidateDicomScriptPair(scpScriptFile, scpSessionFile, scuScriptFile, scuSessionFile, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0));
        }

        /// <summary>
        /// Test the usage of a Values instance in the Set method.
        /// </summary>
        //[Test]
        //public void script_test_receive()
        //{
        //    string scpScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"scripting\script_test_receive.ds");
        //    string scpSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"scripting\test_DICOMScriptLanguage_scp.ses");

        //    string scuScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"scripting\script_test_send.ds");
        //    string scuSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"scripting\test_DICOMScriptLanguage_scu.ses");

        //    Assert.That(ValidateDicomScriptPair(scpScriptFile, scpSessionFile, scuScriptFile, scuSessionFile, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0));
        //}

        /// <summary>
        /// Test the usage of a Values instance in the Set method.
        /// </summary>
        [Test]
        public void script_test_populate()
        {
            string scpScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"scripting\script_test_populate_scp.ds");
            string scpSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"scripting\test_DICOMScriptLanguage_scp.ses");

            string scuScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"scripting\script_test_populate_scu.ds");
            string scuSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"scripting\test_DICOMScriptLanguage_scu.ses");

            Assert.That(ValidateDicomScriptPair(scpScriptFile, scpSessionFile, scuScriptFile, scuSessionFile, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0));
        }

        /// <summary>
        /// Test the usage of a Values instance in the Set method.
        /// </summary>
        [Test]
        public void script_test_system()
        {
            string scpScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"scripting\script_test_system_scp.dss");
            string scpSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"scripting\test_DICOMScriptLanguage_scp.ses");

            string scuScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"scripting\script_test_system_scu.ds");
            string scuSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"scripting\test_DICOMScriptLanguage_scu.ses");

            Assert.That(ValidateDicomScriptPair(scpScriptFile, scpSessionFile, scuScriptFile, scuSessionFile, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0));
        }

        /// <summary>
        /// Test the usage of a Values instance in the Set method.
        /// </summary>
        [Test]
        public void media_create_dicomdir()
        {
            string scriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"media\media_create_dicomdir.ds");
            string sessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"media\media.ses");

            Assert.That(ValidateDicomScript(sessionFile, scriptFile, 0, 0, 0, 0, 0, 0));
        }

        /// <summary>
        /// Test the usage of a Values instance in the Set method.
        /// </summary>
        [Test]
        public void media_test_read()
        {
            string scriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"media\media_test_read.ds");
            string sessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"media\media.ses");

            Assert.That(ValidateDicomScript(sessionFile, scriptFile, 0, 0, 0, 0, 0, 0));
        }

        /// <summary>
        /// Test the usage of a Values instance in the Set method.
        /// </summary>
        [Test]
        public void mwl_extendedchar()
        {
            string scuScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"extendedchar\worklist\SCU.ds");
            string scuSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"extendedchar\worklist\SCU_MWL.ses");

            string scpScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"extendedchar\worklist\SCP_MWL.dss");
            string scpSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"extendedchar\worklist\SCP_MWL.ses");

            Assert.That(ValidateDicomScriptPair(scuScriptFile, scuSessionFile, scpScriptFile, scpSessionFile, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0, 0));
        }

        /// <summary>
        /// Test the usage of a Values instance in the Set method.
        /// </summary>
        [Test]
        public void val_test_acse_1()
        {
            string scpScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_1_scp.ds");
            string scpSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_scp.ses");

            string scuScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_1_scu.ds");
            string scuSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_scu.ses");

            Assert.That(ValidateDicomScriptPair(scpScriptFile, scpSessionFile, scuScriptFile, scuSessionFile, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0));
        }

        /// <summary>
        /// Test the usage of a Values instance in the Set method.
        /// </summary>
        [Test]
        public void val_test_acse_2()
        {
            string scpScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_2_scp.ds");
            string scpSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_scp.ses");

            string scuScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_2_scu.ds");
            string scuSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_scu.ses");

            Assert.That(ValidateDicomScriptPair(scpScriptFile, scpSessionFile, scuScriptFile, scuSessionFile, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0));
        }

        /// <summary>
        /// Test the usage of a Values instance in the Set method.
        /// </summary>
        [Test]
        public void val_test_acse_3()
        {
            string scpScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_3_scp.ds");
            string scpSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_scp.ses");

            string scuScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_3_scu.ds");
            string scuSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_scu.ses");

            Assert.That(ValidateDicomScriptPair(scpScriptFile, scpSessionFile, scuScriptFile, scuSessionFile, 0, 0, 0, 0, 13, 10, 0, 0, 0, 0, 18, 0));
        }

        /// <summary>
        /// Test the usage of a Values instance in the Set method.
        /// </summary>
        [Test]
        public void val_test_acse_4()
        {
            string scpScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_4_scp.ds");
            string scpSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_scp.ses");

            string scuScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_4_scu.ds");
            string scuSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_scu.ses");

            Assert.That(ValidateDicomScriptPair(scpScriptFile, scpSessionFile, scuScriptFile, scuSessionFile, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 2, 0));
        }

        /// <summary>
        /// Test the usage of a Values instance in the Set method.
        /// </summary>
        [Test]
        public void val_test_acse_5()
        {
            string scpScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_5_scp.ds");
            string scpSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_scp.ses");

            string scuScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_5_scu.ds");
            string scuSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_scu.ses");

            Assert.That(ValidateDicomScriptPair(scpScriptFile, scpSessionFile, scuScriptFile, scuSessionFile, 1, 0, 0, 0, 25, 1, 0, 0, 0, 0, 3, 3));
        }

        /// <summary>
        /// Test the usage of a Values instance in the Set method.
        /// </summary>
        [Test]
        public void val_test_acse_6()
        {
            string scpScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_6_scp.ds");
            string scpSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_scp.ses");

            string scuScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_6_scu.ds");
            string scuSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_scu.ses");

            Assert.That(ValidateDicomScriptPair(scpScriptFile, scpSessionFile, scuScriptFile, scuSessionFile, 1, 0, 0, 0, 10, 0, 0, 0, 0, 0, 3, 1));
        }

        /// <summary>
        /// Test the usage of a Values instance in the Set method.
        /// </summary>
        [Test]
        public void val_test_acse_7()
        {
            string scpScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_7_scp.ds");
            string scpSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_scp.ses");

            string scuScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_7_scu.ds");
            string scuSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_scu.ses");

            Assert.That(ValidateDicomScriptPair(scpScriptFile, scpSessionFile, scuScriptFile, scuSessionFile, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 21, 1));
        }

        /// <summary>
        /// Test the usage of a Values instance in the Set method.
        /// </summary>
        [Test]
        public void val_test_acse_8()
        {
            string scpScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_8_scp.ds");
            string scpSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_scp.ses");

            string scuScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_8_scu.ds");
            string scuSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_scu.ses");

            Assert.That(ValidateDicomScriptPair(scpScriptFile, scpSessionFile, scuScriptFile, scuSessionFile, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 9, 0));
        }

        /// <summary>
        /// Test the usage of a Values instance in the Set method.
        /// </summary>
        [Test]
        public void val_test_acse_9()
        {
            string scpScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_9_scp.ds");
            string scpSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_scp.ses");

            string scuScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_9_scu.ds");
            string scuSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_scu.ses");

            Assert.That(ValidateDicomScriptPair(scpScriptFile, scpSessionFile, scuScriptFile, scuSessionFile, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0));
        }

        /// <summary>
        /// Test the usage of a Values instance in the Set method.
        /// </summary>
        [Test]
        public void val_test_acse_10()
        {
            string scpScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_10_scp.ds");
            string scpSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_scp.ses");

            string scuScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_10_scu.ds");
            string scuSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_scu.ses");

            Assert.That(ValidateDicomScriptPair(scpScriptFile, scpSessionFile, scuScriptFile, scuSessionFile, 0, 0, 0, 0, 13, 10, 0, 0, 0, 0, 3, 0));
        }

        /// <summary>
        /// Test the usage of a Values instance in the Set method.
        /// </summary>
        [Test]
        public void val_test_acse_11()
        {
            string scpScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_11_scp.ds");
            string scpSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_scp.ses");

            string scuScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_11_scu.ds");
            string scuSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_scu.ses");

            Assert.That(ValidateDicomScriptPair(scpScriptFile, scpSessionFile, scuScriptFile, scuSessionFile, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0));
        }

        /// <summary>
        /// Test the usage of a Values instance in the Set method.
        /// </summary>
        [Test]
        public void val_test_acse_12()
        {
            string scpScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_12_scp.ds");
            string scpSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_scp.ses");

            string scuScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_12_scu.ds");
            string scuSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_scu.ses");

            Assert.That(ValidateDicomScriptPair(scpScriptFile, scpSessionFile, scuScriptFile, scuSessionFile, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 6, 0));
        }

        /// <summary>
        /// Test the usage of a Values instance in the Set method.
        /// </summary>
        [Test]
        public void val_test_acse_13()
        {
            string scpScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_13_scp.ds");
            string scpSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_scp.ses");

            string scuScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_13_scu.ds");
            string scuSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_scu.ses");

            Assert.That(ValidateDicomScriptPair(scpScriptFile, scpSessionFile, scuScriptFile, scuSessionFile, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0));
        }

        /// <summary>
        /// Test the usage of a Values instance in the Set method.
        /// </summary>
        [Test]
        public void val_test_acse_14()
        {
            string scpScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_14_scp.ds");
            string scpSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_scp.ses");

            string scuScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_14_scu.ds");
            string scuSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_scu.ses");

            Assert.That(ValidateDicomScriptPair(scpScriptFile, scpSessionFile, scuScriptFile, scuSessionFile, 0, 0, 0, 0, 13, 10, 0, 0, 0, 0, 2, 0));
        }

        /// <summary>
        /// Test the usage of a Values instance in the Set method.
        /// </summary>
        [Test]
        public void val_test_acse_15()
        {
            string scpScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_15_scp.ds");
            string scpSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_scp.ses");

            string scuScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_15_scu.ds");
            string scuSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_scu.ses");

            Assert.That(ValidateDicomScriptPair(scpScriptFile, scpSessionFile, scuScriptFile, scuSessionFile, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0));
        }

        /// <summary>
        /// Test the usage of a Values instance in the Set method.
        /// </summary>
        [Test]
        public void val_test_acse_16()
        {
            string scpScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_16_scp.ds");
            string scpSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_scp.ses");

            string scuScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_16_scu.ds");
            string scuSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_scu.ses");

            Assert.That(ValidateDicomScriptPair(scpScriptFile, scpSessionFile, scuScriptFile, scuSessionFile, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0));
        }

        /// <summary>
        /// Test the usage of a Values instance in the Set method.
        /// </summary>
        [Test]
        public void val_test_acse_17()
        {
            string scpScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_17_scp.ds");
            string scpSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_scp.ses");

            string scuScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_17_scu.ds");
            string scuSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_scu.ses");

            Assert.That(ValidateDicomScriptPair(scpScriptFile, scpSessionFile, scuScriptFile, scuSessionFile, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0));
        }

        /// <summary>
        /// Test the usage of a Values instance in the Set method.
        /// </summary>
        [Test]
        public void val_test_acse_18()
        {
            string scpScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_18_scp.ds");
            string scpSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_scp.ses");

            string scuScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_18_scu.ds");
            string scuSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_scu.ses");

            Assert.That(ValidateDicomScriptPair(scpScriptFile, scpSessionFile, scuScriptFile, scuSessionFile, 0, 0, 0, 0, 15, 10, 0, 0, 0, 0, 18, 0));
        }

        /// <summary>
        /// Test the usage of a Values instance in the Set method.
        /// </summary>
        [Test]
        public void val_test_acse_19()
        {
            string scpScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_19_scp.ds");
            string scpSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_scp.ses");

            string scuScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_19_scu.ds");
            string scuSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_scu.ses");

            Assert.That(ValidateDicomScriptPair(scpScriptFile, scpSessionFile, scuScriptFile, scuSessionFile, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 2, 0));
        }

        /// <summary>
        /// Test the usage of a Values instance in the Set method.
        /// </summary>
        [Test]
        public void val_test_acse_20()
        {
            string scpScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_20_scp.ds");
            string scpSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_scp.ses");

            string scuScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_20_scu.ds");
            string scuSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"acse\acse_scu.ses");

            Assert.That(ValidateDicomScriptPair(scpScriptFile, scpSessionFile, scuScriptFile, scuSessionFile, 0, 0, 0, 0, 29, 1, 0, 0, 0, 0, 22, 0));
        }

        /// <summary>
        /// Test the usage of a Values instance in the Set method.
        /// </summary>
        [Test]
        public void val_test_grayscale_print()
        {
            string scpScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"abstractprint\grayscale_scu.ds");
            string scpSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"abstractprint\grayscale_scu.ses");

            string scuScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"abstractprint\abstract_scp.ds");
            string scuSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"abstractprint\grayscale_scp.ses");

            Assert.That(ValidateDicomScriptPair(scpScriptFile, scpSessionFile, scuScriptFile, scuSessionFile, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0));
        }

        /// <summary>
        /// Test the usage of a Values instance in the Set method.
        /// </summary>
        //[Test]
        //public void val_test_color_print()
        //{
        //    string scpScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"abstractprint\color_scu.ds");
        //    string scpSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"abstractprint\color_scu.ses");

        //    string scuScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"abstractprint\abstract_scp.ds");
        //    string scuSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"abstractprint\color_scp.ses");

        //    Assert.That(ValidateDicomScriptPair(scpScriptFile, scpSessionFile, scuScriptFile, scuSessionFile, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0));
        //}

        /// <summary>
        /// Test the usage of a Values instance in the Set method.
        /// </summary>
        [Test]
        public void val_test_sc_store()
        {
            string scpScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"abstractstorage\sc_scu.ds");
            string scpSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"abstractstorage\sc_and_cr_scu.ses");

            string scuScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"abstractstorage\abstract_scp.ds");
            string scuSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"abstractstorage\sc_scp.ses");

            Assert.That(ValidateDicomScriptPair(scpScriptFile, scpSessionFile, scuScriptFile, scuSessionFile, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0));
        }

        /// <summary>
        /// Test the usage of a Values instance in the Set method.
        /// </summary>
        //[Test]
        //public void val_test_cr_store()
        //{
        //    string scpScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"abstractstorage\cr_scu.ds");
        //    string scpSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"abstractstorage\sc_and_cr_scu.ses");

        //    string scuScriptFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"abstractstorage\abstract_scp.ds");
        //    string scuSessionFile = Path.Combine(Paths.SQATestsResourcesDirectoryFullPath, @"abstractstorage\cr_scp.ses");

        //    Assert.That(ValidateDicomScriptPair(scpScriptFile, scpSessionFile, scuScriptFile, scuSessionFile, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0));
        //}

        private class DicomThreadScriptExecution : DicomThread
        {
            private string scriptFileName = null;

            public DicomThreadScriptExecution(string script)
            {
                this.scriptFileName = script;
            }

            protected override void Execute()
            {
                this.Options.DvtkScriptSession.ExecuteScript(scriptFileName, false);
            }
        }

        private class DicomMainThread : DicomThread
        {
            //
            // - Fields -
            //
            /// <summary>
            /// See property Table.
            /// </summary>
            Table table = new Table(4);

            //
            // - Constructors -
            //
            /// <summary>
            /// Default constructor.
            /// </summary>
            public DicomMainThread()
            {
                this.table.CellItemSeperator = "<br>";
                this.table.AddHeader("Session", "Test case", "P/F", "Comment");
            }            

            //
            // - Properties -
            //
            /// <summary>
            /// Table in which the overview of all executed SQA tests is stored.
            /// </summary>
            public Table Table
            {
                get
                {
                    return (this.table);
                }
            }

            //
            // - Methods -
            //
            /// <summary>
            /// Waits until all SQA tests have been executed and writes overview
            /// of SQA tests in results files.
            /// </summary>
            protected override void Execute()
            {
                //
                // Keep looping until the last method has been called.
                // 
                
                bool endLoop = false;

                while (!endLoop)
                {
                    Sleep(500);

                    lock (lockObject)
                    {
                        endLoop = testFinished;
                    }
                }

                //
                // All SQA tests have now been executed.
                //
                WriteHtmlInformation(this.table.ConvertToHtml());
            }
        }
    }
}