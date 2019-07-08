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

using DimseCommand = DvtkData.Dimse.DimseCommand;
using VR = DvtkData.Dimse.VR;
using DvtkHighLevelInterface.Common.Threads;
using DvtkHighLevelInterface.Common.UserInterfaces;
using DvtkHighLevelInterface.Dicom.Messages;
using DvtkHighLevelInterface.Dicom.Other;
using DvtkHighLevelInterface.NUnit;
using DvtkHighLevelInterface.Dicom.Files;
using DvtkSession = Dvtk.Sessions;


namespace DvtkHighLevelInterface.Dicom.Threads
{
    /// <summary>
    /// Contains NUnit Test Cases.
    /// </summary>
    [TestFixture]
    public class DicomThread_NUnit
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
        /// Test for ticket 425.
        /// 
        /// This test will verify that the same C_ECHO_RQ can be verified multiple times
        /// without creating a warning in the logging.
        /// </summary>
        [Test]
        public void Ticket425_1_1()
        {
            String tempFileName = Path.GetTempFileName() + "_CruiseControlInfo.txt";
            StreamWriter streamWriter = new StreamWriter(tempFileName);
            streamWriter.Write("Writing results files for test Ticket425_1_1 to \"" + Paths.ResultsDirectoryFullPath + "\"");
            streamWriter.Close();

            ThreadManager threadManager = new ThreadManager();

            DicomMessage dicomMessage = DicomMessage_NUnit.CreateValidCEchoRq();

            DicomThreadValideDicomMessageTwiceWithoutIodNameParameter dicomThread = new DicomThreadValideDicomMessageTwiceWithoutIodNameParameter(dicomMessage);
            dicomThread.Initialize(threadManager);

            dicomThread.Options.ResultsDirectory = Paths.ResultsDirectoryFullPath;
            dicomThread.Options.ResultsFileNameOnlyWithoutExtension = "Ticket425_1_1";
            dicomThread.Options.LoadDefinitionFile(Path.Combine(Paths.DefinitionsDirectoryFullPath, "Verification.def"));

            dicomThread.Start();
            dicomThread.WaitForCompletion();

            Assert.That(dicomThread.NrOfErrors + dicomThread.NrOfWarnings, Is.EqualTo(0));
        }

        /// <summary>
        /// Test for ticket 425.
        /// 
        /// This test will verify validating a C_ECHO_RQ without the Affected SOP Class attribute generates generates at least one warning.
        /// </summary>
        [Test]
        public void Ticket425_2_1()
        {
            ThreadManager threadManager = new ThreadManager();

            DicomMessage dicomMessage = DicomMessage_NUnit.CreateValidCEchoRq();
            dicomMessage.CommandSet.Delete("0x00000002");

            DicomThreadValideDicomMessageOnceWithoutIodNameParameter dicomThread = new DicomThreadValideDicomMessageOnceWithoutIodNameParameter(dicomMessage);
            dicomThread.Initialize(threadManager);

            dicomThread.Options.ResultsDirectory = Paths.ResultsDirectoryFullPath;
            dicomThread.Options.ResultsFileNameOnlyWithoutExtension = "Ticket425_2_1";
            dicomThread.Options.LoadDefinitionFile(Path.Combine(Paths.DefinitionsDirectoryFullPath, "Verification.def"));

            dicomThread.Start();
            dicomThread.WaitForCompletion();

            Assert.That(dicomThread.NrOfWarnings, Is.GreaterThan(0));
        }

        /// <summary>
        /// Test for ticket 425.
        /// 
        /// This test will verify that validating a C_ECHO_RQ with a correct IOD name contains no errors and warnings.
        /// </summary>
        [Test]
        public void Ticket425_3_1()
        {
            ThreadManager threadManager = new ThreadManager();

            DicomMessage dicomMessage = DicomMessage_NUnit.CreateValidCEchoRq();

            DicomThreadValideDicomMessageOnceWithIodNameSupplied dicomThread = new DicomThreadValideDicomMessageOnceWithIodNameSupplied(dicomMessage, "Verification");
            dicomThread.Initialize(threadManager);

            dicomThread.Options.ResultsDirectory = Paths.ResultsDirectoryFullPath;
            dicomThread.Options.ResultsFileNameOnlyWithoutExtension = "Ticket425_3_1";
            dicomThread.Options.LoadDefinitionFile(Path.Combine(Paths.DefinitionsDirectoryFullPath, "Verification.def"));

            dicomThread.Start();
            dicomThread.WaitForCompletion();

            Assert.That(dicomThread.NrOfErrors + dicomThread.NrOfWarnings, Is.EqualTo(0));
        }

        /// <summary>
        /// Test for ticket 425.
        /// 
        /// This test will verify that the same Query Retrieve C_FIND_RQ can be verified multiple times
        /// without creating a warning in the logging.
        /// </summary>
        [Test]
        public void Ticket425_4_1()
        {
            ThreadManager threadManager = new ThreadManager();

            DicomMessage dicomMessage = DicomMessage_NUnit.CreateValidPatientRootQueryRetrieveCFindRq();

            DicomThreadValideDicomMessageTwiceWithoutIodNameParameter dicomThread = new DicomThreadValideDicomMessageTwiceWithoutIodNameParameter(dicomMessage);
            dicomThread.Initialize(threadManager);

            dicomThread.Options.ResultsDirectory = Paths.ResultsDirectoryFullPath;
            dicomThread.Options.ResultsFileNameOnlyWithoutExtension = "Ticket425_4_1";
            dicomThread.Options.LoadDefinitionFile(Path.Combine(Paths.DefinitionsDirectoryFullPath, "PatientRootQueryRetrieve-FIND.def"));

            dicomThread.Start();
            dicomThread.WaitForCompletion();

            Assert.That(dicomThread.NrOfErrors + dicomThread.NrOfWarnings, Is.EqualTo(0));
        }

        /// <summary>
        /// Test for ticket 425.
        /// 
        /// This test will verify validating a Query Retrieve C_FIND_RQ without the Affected SOP Class attribute generates at least one warning.
        /// </summary>
        [Test]
        public void Ticket425_5_1()
        {
            ThreadManager threadManager = new ThreadManager();

            DicomMessage dicomMessage = DicomMessage_NUnit.CreateValidPatientRootQueryRetrieveCFindRq();
            dicomMessage.CommandSet.Delete("0x00000002");

            DicomThreadValideDicomMessageOnceWithoutIodNameParameter dicomThread = new DicomThreadValideDicomMessageOnceWithoutIodNameParameter(dicomMessage);
            dicomThread.Initialize(threadManager);

            dicomThread.Options.ResultsDirectory = Paths.ResultsDirectoryFullPath;
            dicomThread.Options.ResultsFileNameOnlyWithoutExtension = "Ticket425_5_1";
            dicomThread.Options.LoadDefinitionFile(Path.Combine(Paths.DefinitionsDirectoryFullPath, "PatientRootQueryRetrieve-FIND.def"));

            dicomThread.Start();
            dicomThread.WaitForCompletion();

            Assert.That(dicomThread.NrOfWarnings, Is.GreaterThan(0));
        }

        /// <summary>
        /// Test for ticket 425.
        /// 
        /// This test will verify that validating a Query Retrieve C-FIND-RQ with a correct IOD name contains no errors and warnings.
        /// </summary>
        [Test]
        public void Ticket425_6_1()
        {
            ThreadManager threadManager = new ThreadManager();

            DicomMessage dicomMessage = DicomMessage_NUnit.CreateValidPatientRootQueryRetrieveCFindRq();

            DicomThreadValideDicomMessageOnceWithIodNameSupplied dicomThread = new DicomThreadValideDicomMessageOnceWithIodNameSupplied(dicomMessage, "Patient Root Query/Retrieve - FIND");
            dicomThread.Initialize(threadManager);

            dicomThread.Options.ResultsDirectory = Paths.ResultsDirectoryFullPath;
            dicomThread.Options.ResultsFileNameOnlyWithoutExtension = "Ticket425_6_1";
            dicomThread.Options.LoadDefinitionFile(Path.Combine(Paths.DefinitionsDirectoryFullPath, "PatientRootQueryRetrieve-FIND.def"));

            dicomThread.Start();
            dicomThread.WaitForCompletion();

            Assert.That(dicomThread.NrOfErrors + dicomThread.NrOfWarnings, Is.EqualTo(0));
        }

        /// <summary>
        /// Test for ticket 1181.
        /// 
        /// This test will verify that the Send dicom message with no presentation context ID supplied.
        /// </summary>
        [Test]
        public void Ticket1181_1_1()
        {
            String tempFileName = Path.GetTempFileName() + "_CruiseControlInfo.txt";
            StreamWriter streamWriter = new StreamWriter(tempFileName);
            streamWriter.Write("Writing results files for test Ticket1181_1_1 to \"" + Paths.ResultsDirectoryFullPath + "\"");
            streamWriter.Close();

            ThreadManager threadManager = new ThreadManager();

            MainDicomThread dicomThread = new MainDicomThread(1, -1, "1.2.840.10008.5.1.4.1.1.7");
            dicomThread.Initialize(threadManager);

            dicomThread.Options.ResultsDirectory = Paths.ResultsDirectoryFullPath;
            dicomThread.Options.ResultsFileNameOnlyWithoutExtension = "Ticket1181_1_1";
            
            dicomThread.Start();
            dicomThread.WaitForCompletion();

            Assert.That(dicomThread.NrOfErrors, Is.EqualTo(2));
        }

        /// <summary>
        /// Test for ticket 1181.
        /// 
        /// This test will verify that the Send dicom message with a valid context ID supplied.
        /// </summary>
        [Test]
        public void Ticket1181_2_1()
        {
            ThreadManager threadManager = new ThreadManager();

            MainDicomThread dicomThread = new MainDicomThread(2, 1, "1.2.840.10008.5.1.4.1.1.7");
            dicomThread.Initialize(threadManager);

            dicomThread.Options.ResultsDirectory = Paths.ResultsDirectoryFullPath;
            dicomThread.Options.ResultsFileNameOnlyWithoutExtension = "Ticket1181_2_1";

            dicomThread.Start();
            dicomThread.WaitForCompletion();

            Assert.That(dicomThread.NrOfErrors, Is.EqualTo(2));
        }

        /// <summary>
        /// Test for ticket 1181.
        /// 
        /// This test will verify that the Send dicom message with an invalid context ID supplied like 10. Also check if this logged correctly.
        /// </summary>
        [Test]
        public void Ticket1181_3_1()
        {
            ThreadManager threadManager = new ThreadManager();

            MainDicomThread dicomThread = new MainDicomThread(3, 10, "1.2.840.10008.5.1.4.1.1.7");
            dicomThread.Initialize(threadManager);

            dicomThread.Options.ResultsDirectory = Paths.ResultsDirectoryFullPath;
            dicomThread.Options.ResultsFileNameOnlyWithoutExtension = "Ticket1181_3_1";

            dicomThread.Start();
            dicomThread.WaitForCompletion();

            Assert.That(dicomThread.NrOfErrors, Is.EqualTo(7));
        }

        /// <summary>
        /// Test for ticket 1181.
        /// 
        /// This test will verify that the Send dicom message with a invalid context ID supplied for a non-accepted presentation context. Also check if this logged correctly.
        /// </summary>
        [Test]
        public void Ticket1181_4_1()
        {
            ThreadManager threadManager = new ThreadManager();

            MainDicomThread dicomThread = new MainDicomThread(4, 3, "1.2.840.10008.5.1.4.1.1.7");
            dicomThread.Initialize(threadManager);

            dicomThread.Options.ResultsDirectory = Paths.ResultsDirectoryFullPath;
            dicomThread.Options.ResultsFileNameOnlyWithoutExtension = "Ticket1181_4_1";

            dicomThread.Start();
            dicomThread.WaitForCompletion();

            Assert.That(dicomThread.NrOfErrors, Is.EqualTo(7));
        }

        /// <summary>
        /// Test for ticket 1181.
        /// 
        /// This test will verify that the Send dicom message with an affected SOP Class UID different from the accepted abstract syntax UID will work.
        /// </summary>
        [Test]
        public void Ticket1181_5_1()
        {
            ThreadManager threadManager = new ThreadManager();

            MainDicomThread dicomThread = new MainDicomThread(5, 1, "1.2.840.10008.5.1.4.1.1.2");
            dicomThread.Initialize(threadManager);

            dicomThread.Options.ResultsDirectory = Paths.ResultsDirectoryFullPath;
            dicomThread.Options.ResultsFileNameOnlyWithoutExtension = "Ticket1181_5_1";

            dicomThread.Start();
            dicomThread.WaitForCompletion();

            Assert.That(dicomThread.NrOfErrors, Is.EqualTo(2));
        }

        /// <summary>
        /// Test for ticket 1113.
        /// 
        /// This test will verify if %commonprogramfiles% in definition directory and definition file 
        /// would load the definition file.
        /// </summary>
        [Test]
        public void Ticket1113_1()
        {   
            ThreadManager threadManager = new ThreadManager();

            DicomThreadValidateSession dicomThread = new DicomThreadValidateSession();
            dicomThread.Initialize(threadManager);

            dicomThread.Options.LoadFromFile(Path.Combine(Paths.SessionDirectoryFullPath, "1113_1.ses"));

            dicomThread.Options.ResultsDirectory = Paths.ResultsDirectoryFullPath;
            dicomThread.Options.ResultsFileNameOnlyWithoutExtension = "Ticket1113_1";
                        
            dicomThread.Start();
            dicomThread.WaitForCompletion();

            Assert.That(dicomThread.NrOfErrors + dicomThread.NrOfWarnings, Is.GreaterThan(0));
        }

        /// <summary>
        /// Test for ticket 1113.
        /// 
        /// This test will verify if %commonprogramfiles% in definition directory and with no path 
        /// specified in definition file would load the definition file.
        /// </summary>
        [Test]
        public void Ticket1113_2()
        {
            ThreadManager threadManager = new ThreadManager();

            DicomThreadValidateSession dicomThread = new DicomThreadValidateSession();
            dicomThread.Initialize(threadManager);

            dicomThread.Options.LoadFromFile(Path.Combine(Paths.SessionDirectoryFullPath, "1113_2.ses"));

            dicomThread.Options.ResultsDirectory = Paths.ResultsDirectoryFullPath;
            dicomThread.Options.ResultsFileNameOnlyWithoutExtension = "Ticket1113_2";

            dicomThread.Start();
            dicomThread.WaitForCompletion();

            Assert.That(dicomThread.NrOfErrors + dicomThread.NrOfWarnings, Is.GreaterThan(0));
        }

        /// <summary>
        /// Test for ticket 1113.
        /// 
        /// This test will verify if %commonprogramfiles% in definition directory and full path 
        /// specified in definition file would load the definition file.
        /// </summary>

        [Test]
        public void Ticket1113_3()
        {
            ThreadManager threadManager = new ThreadManager();

            DicomThreadValidateSession dicomThread = new DicomThreadValidateSession();
            dicomThread.Initialize(threadManager);

            dicomThread.Options.LoadFromFile(Path.Combine(Paths.SessionDirectoryFullPath, "1113_3.ses"));

            dicomThread.Options.ResultsDirectory = Paths.ResultsDirectoryFullPath;
            dicomThread.Options.ResultsFileNameOnlyWithoutExtension = "Ticket1113_3";

            dicomThread.Start();
            dicomThread.WaitForCompletion();

            Assert.That(dicomThread.NrOfErrors + dicomThread.NrOfWarnings, Is.GreaterThan(0));
        }

        /// <summary>
        /// Test for ticket 1113.
        /// 
        /// This test will verify if full path specified in definition directory and 
        /// %commonprogramfiles% variable specified in definition file would 
        /// load the definition file.
        /// </summary>
        [Test]
        public void Ticket1113_4()
        {
            ThreadManager threadManager = new ThreadManager();

            DicomThreadValidateSession dicomThread = new DicomThreadValidateSession();
            dicomThread.Initialize(threadManager);

            dicomThread.Options.LoadFromFile(Path.Combine(Paths.SessionDirectoryFullPath, "1113_4.ses"));

            dicomThread.Options.ResultsDirectory = Paths.ResultsDirectoryFullPath;
            dicomThread.Options.ResultsFileNameOnlyWithoutExtension = "Ticket1113_4";
            
            dicomThread.Start();
            dicomThread.WaitForCompletion();

            Assert.That(dicomThread.NrOfErrors + dicomThread.NrOfWarnings, Is.GreaterThan(0));
        }
        /// <summary>
        /// Test for ticket 1113.
        /// 
        /// This test will verify if full path specified in definition directory and no path 
        /// specified in definition file would load the definition file.
        /// </summary>
        [Test]
        public void Ticket1113_5()
        {
            ThreadManager threadManager = new ThreadManager();

            DicomThreadValidateSession dicomThread = new DicomThreadValidateSession();
            dicomThread.Initialize(threadManager);

            dicomThread.Options.LoadFromFile(Path.Combine(Paths.SessionDirectoryFullPath, "1113_5.ses"));

            dicomThread.Options.ResultsDirectory = Paths.ResultsDirectoryFullPath;
            dicomThread.Options.ResultsFileNameOnlyWithoutExtension = "Ticket1113_5";

            dicomThread.Start();
            dicomThread.WaitForCompletion();

            Assert.That(dicomThread.NrOfErrors + dicomThread.NrOfWarnings, Is.GreaterThan(0));
        }
        /// <summary>
        /// Test for ticket 1113.
        /// 
        /// This test will verify if full path specified in definition directory and full path 
        /// specified in definition file would load the definition file.
        /// </summary>
        [Test]
        public void Ticket1113_6()
        {
            ThreadManager threadManager = new ThreadManager();

            DicomThreadValidateSession dicomThread = new DicomThreadValidateSession();
            dicomThread.Initialize(threadManager);

            dicomThread.Options.LoadFromFile(Path.Combine(Paths.SessionDirectoryFullPath, "1113_6.ses"));

            dicomThread.Options.ResultsDirectory = Paths.ResultsDirectoryFullPath;
            dicomThread.Options.ResultsFileNameOnlyWithoutExtension = "Ticket1113_6";

            dicomThread.Start();
            dicomThread.WaitForCompletion();

            Assert.That(dicomThread.NrOfErrors + dicomThread.NrOfWarnings, Is.GreaterThan(0));
        }

        /// <summary>
        /// Test for ticket 1266.
        /// 
        /// This test will verify DA VR range values.
        /// </summary>
        [Test]
        public void Ticket1266_1()
        {
            ThreadManager threadManager = new ThreadManager();

            DicomMessage cFindMessage = new DicomMessage(DimseCommand.CFINDRQ);

            cFindMessage.CommandSet.Set("0x00000000", VR.UL, 10); // "Group length", set to incorrect value.
            cFindMessage.CommandSet.Set("0x00000002", VR.UI, "1.2.840.10008.5.1.4.1.2.1.1"); // "Affected SOP Class UID"
            cFindMessage.CommandSet.Set("0x00000110", VR.US, 100); // "Message ID"
            cFindMessage.CommandSet.Set("0x00000700", VR.US, 0); // "Priority"
            cFindMessage.CommandSet.Set("0x00000800", VR.US, 0); // "Data Set Type"

            cFindMessage.DataSet.Set("0x00080052", VR.CS, "PATIENT"); // "Query Retrieve Level"
            cFindMessage.DataSet.Set("0x10080001", VR.DA, "1970.12-1970.12.312", "1970.12.02-1970.12.312");
            cFindMessage.DataSet.Set("0x10080002", VR.DA, "1970.12X-1970.12.312");
            cFindMessage.DataSet.Set("0x10080003", VR.DA, "1970p121-1970.12.312");
            cFindMessage.DataSet.Set("0x10080004", VR.DA, "19701201-19701231");

            CFINDDicomThread dicomThread = new CFINDDicomThread(1, cFindMessage, "1.2.840.10008.5.1.4.1.2.1.1", "1.2.840.10008.1.2");
            dicomThread.Initialize(threadManager);

            dicomThread.Options.ResultsDirectory = Paths.ResultsDirectoryFullPath;
            dicomThread.Options.ResultsFileNameOnlyWithoutExtension = "Ticket1266_1";

            dicomThread.Start();
            dicomThread.WaitForCompletion();

            Assert.That(dicomThread.NrOfErrors + dicomThread.NrOfWarnings, Is.EqualTo(2));
        }

        /// <summary>
        /// Test for ticket 1266.
        /// 
        /// This test will verify DT VR range values.
        /// </summary>
        [Test]
        public void Ticket1266_2()
        {
            ThreadManager threadManager = new ThreadManager();

            DicomMessage cFindMessage = new DicomMessage(DimseCommand.CFINDRQ);

            cFindMessage.CommandSet.Set("0x00000000", VR.UL, 10); // "Group length", set to incorrect value.
            cFindMessage.CommandSet.Set("0x00000002", VR.UI, "1.2.840.10008.5.1.4.1.2.1.1"); // "Affected SOP Class UID"
            cFindMessage.CommandSet.Set("0x00000110", VR.US, 100); // "Message ID"
            cFindMessage.CommandSet.Set("0x00000700", VR.US, 0); // "Priority"
            cFindMessage.CommandSet.Set("0x00000800", VR.US, 0); // "Data Set Type"

            cFindMessage.DataSet.Set("0x00080052", VR.CS, "PATIENT"); // "Query Retrieve Level"
            cFindMessage.DataSet.Set("0x100C0001", VR.DT, "20011213125930.123456+1212-20011213125930.123456+1212");
            cFindMessage.DataSet.Set("0x100C0002", VR.DT, "19.99.02.28-19990228");
            cFindMessage.DataSet.Set("0x100C0003", VR.DT, "1999.1 -1999.2");
            cFindMessage.DataSet.Set("0x100C0004", VR.DT, "1999.+1212-1999.+1213");
            cFindMessage.DataSet.Set("0x100C0005", VR.DT, "20011213125930.123456+121212-20011213125930.123456+121212");
            cFindMessage.DataSet.Set("0x100C0006", VR.DT, "1999.123x- 1999.124");

            CFINDDicomThread dicomThread = new CFINDDicomThread(2, cFindMessage, "1.2.840.10008.5.1.4.1.2.1.1", "1.2.840.10008.1.2");
            dicomThread.Initialize(threadManager);

            dicomThread.Options.ResultsDirectory = Paths.ResultsDirectoryFullPath;
            dicomThread.Options.ResultsFileNameOnlyWithoutExtension = "Ticket1266_2";

            dicomThread.Start();
            dicomThread.WaitForCompletion();

            Assert.That(dicomThread.NrOfErrors + dicomThread.NrOfWarnings, Is.EqualTo(2));
        }

        /// <summary>
        /// Test for ticket 1266.
        /// 
        /// This test will verify TM VR range values.
        /// </summary>
        [Test]
        public void Ticket1266_3()
        {
            ThreadManager threadManager = new ThreadManager();

            DicomMessage cFindMessage = new DicomMessage(DimseCommand.CFINDRQ);

            cFindMessage.CommandSet.Set("0x00000000", VR.UL, 10); // "Group length", set to incorrect value.
            cFindMessage.CommandSet.Set("0x00000002", VR.UI, "1.2.840.10008.5.1.4.1.2.1.1"); // "Affected SOP Class UID"
            cFindMessage.CommandSet.Set("0x00000110", VR.US, 100); // "Message ID"
            cFindMessage.CommandSet.Set("0x00000700", VR.US, 0); // "Priority"
            cFindMessage.CommandSet.Set("0x00000800", VR.US, 0); // "Data Set Type"

            cFindMessage.DataSet.Set("0x00080052", VR.CS, "PATIENT"); // "Query Retrieve Level"
            cFindMessage.DataSet.Set("0x10280001", VR.TM, "12345678901234567-12345678901234");
            cFindMessage.DataSet.Set("0x10280002", VR.TM, "070907.0705-010101 1");
            cFindMessage.DataSet.Set("0x10280003", VR.TM, "070907.0705-071007.0706");
            cFindMessage.DataSet.Set("0x10280004", VR.TM, "010160-010170");
            cFindMessage.DataSet.Set("0x10280005", VR.TM, "23:14:01-23:14:01");

            CFINDDicomThread dicomThread = new CFINDDicomThread(3, cFindMessage, "1.2.840.10008.5.1.4.1.2.1.1", "1.2.840.10008.1.2");
            dicomThread.Initialize(threadManager);

            dicomThread.Options.ResultsDirectory = Paths.ResultsDirectoryFullPath;
            dicomThread.Options.ResultsFileNameOnlyWithoutExtension = "Ticket1266_3";

            dicomThread.Start();
            dicomThread.WaitForCompletion();

            Assert.That(dicomThread.NrOfErrors + dicomThread.NrOfWarnings, Is.EqualTo(2));
        }

        
      

        private class DicomThreadValidateSession : DicomThread
        {
            private DicomFile dicomFile = null;

            public DicomThreadValidateSession()
            {
                
                dicomFile = new DicomFile();
            }

            protected override void Execute()
            {
                DicomMessage dicomMessage = new DicomMessage(DimseCommand.CSTORERQ);

                dicomFile.Read(Path.Combine(Paths.DataDirectoryFullPath, "Ticket1113.DCM"), this);

                dicomMessage.CommandSet.Set("0x00000002", VR.UI, "1.2.840.10008.5.1.4.1.1.7"); // "Affected SOP Class UID"
                dicomMessage.CommandSet.Set("0x00001000", VR.UI, "5.6.7.8.9"); // "Affected SOP Instance UID"
                dicomMessage.CommandSet.Set("0x00000110", VR.US, 100); // "Message ID"
                dicomMessage.CommandSet.Set("0x00000700", VR.US, 0); // "Priority"

                dicomMessage.DataSet.DvtkDataDataSet = dicomFile.DataSet.DvtkDataDataSet;

                Validate(dicomMessage);
                
            }
        }

        private class DicomThreadValideDicomMessageOnceWithoutIodNameParameter : DicomThread
        {
            private DicomMessage dicomMessage = null;

            public DicomThreadValideDicomMessageOnceWithoutIodNameParameter(DicomMessage dicomMessage)
            {
                this.dicomMessage = dicomMessage;
            }

            protected override void Execute()
            {
                Validate(this.dicomMessage);
            }
        }

        private class DicomThreadValideDicomMessageTwiceWithoutIodNameParameter : DicomThread
        {
            private DicomMessage dicomMessage = null;

            public DicomThreadValideDicomMessageTwiceWithoutIodNameParameter(DicomMessage dicomMessage)
            {
                this.dicomMessage = dicomMessage;
            }

            protected override void Execute()
            {
                Validate(this.dicomMessage);
                Validate(this.dicomMessage);
            }
        }

        private class DicomThreadValideDicomMessageOnceWithIodNameSupplied : DicomThread
        {
            private DicomMessage dicomMessage = null;

            private String iodName = "";

            public DicomThreadValideDicomMessageOnceWithIodNameSupplied(DicomMessage dicomMessage, String iodName)
            {
                this.dicomMessage = dicomMessage;
                this.iodName = iodName;
            }

            protected override void Execute()
            {
                Validate(this.dicomMessage, iodName);
            }
        }

        private class MainDicomThread : DicomThread
        {
            int testNr;
            int sendingPCId;

            private String abstractSyntax = null;

            public MainDicomThread(int index, int pcId, String abstractSyntax)
            {
                testNr = index;
                sendingPCId = pcId;

                this.abstractSyntax = abstractSyntax;
            }

            protected override void Execute()
            {
                StoreSCUThread scu = new StoreSCUThread(sendingPCId, abstractSyntax);

                scu.Initialize(this);

                scu.Options.CopyFrom(this.Options);
                scu.Options.LogThreadStartingAndStoppingInParent = false;
                string resultFilename = string.Format("Ticket1181_{0}_1_SCU", testNr);
                scu.Options.ResultsFileNameOnlyWithoutExtension = resultFilename;
                scu.Options.LoadFromFile(Path.Combine(Paths.SessionDirectoryFullPath, "1181_Storage_SCU.ses"));

                StoreSCPThread scp = new StoreSCPThread(sendingPCId, abstractSyntax);

                scp.Initialize(this);

                scp.Options.CopyFrom(this.Options);
                scp.Options.LogThreadStartingAndStoppingInParent = false;
                resultFilename = string.Format("Ticket1181_{0}_1_SCP", testNr);
                scp.Options.ResultsFileNameOnlyWithoutExtension = resultFilename;
                scp.Options.LoadFromFile(Path.Combine(Paths.SessionDirectoryFullPath, "1181_Storage_SCP.ses"));

                scp.Start();

                // Wait 1 seconds before starting the SCU.
                Sleep(1000);

                scu.Start();
            }
        }

        private class StoreSCUThread : DicomThread
        {
            private DicomFile dicomFile = null;
            int pcId;

            private String abstractSyntax = null;

            public StoreSCUThread(int id, String abstractSyntax)
            {
                dicomFile = new DicomFile();
                pcId = id;
                this.abstractSyntax = abstractSyntax;
            }

            protected override void Execute()
            {
                PresentationContext pc1 = new PresentationContext(this.abstractSyntax, "1.2.840.10008.1.2");
                PresentationContext pc2 = new PresentationContext(this.abstractSyntax, "1.2.840.10008.1.2.1");
                SendAssociateRq(pc1,pc2);

                ReceiveAssociateAc();

                DicomMessage dicomMessage = new DicomMessage(DimseCommand.CSTORERQ);

                dicomFile.Read(Path.Combine(Paths.DataDirectoryFullPath, "1181.dcm"), this);

                dicomMessage.CommandSet.Set("0x00000002", VR.UI, "1.2.840.10008.5.1.4.1.1.7"); // "Affected SOP Class UID"
                //dicomMessage.CommandSet.Set("0x00001000", VR.UI, "5.6.7.8.9"); // "Affected SOP Instance UID"
                dicomMessage.CommandSet.Set("0x00000110", VR.US, 100); // "Message ID"
                dicomMessage.CommandSet.Set("0x00000700", VR.US, 0); // "Priority"

                dicomMessage.DataSet.DvtkDataDataSet = dicomFile.DataSet.DvtkDataDataSet;

                if(pcId == -1)
                    Send(dicomMessage);
                else
                    Send(dicomMessage,pcId);

                ReceiveDicomMessage();

                SendReleaseRq();

                ReceiveReleaseRp();
            }
        }

        private class StoreSCPThread : DicomThread
        {
            private int pcId = 0;

            private String abstractSyntax = null;

            public StoreSCPThread(int pcId, String abstractSyntax)
            {
                this.pcId = pcId;
                this.abstractSyntax = abstractSyntax;
            }

            protected override void Execute()
            {
                ReceiveAssociateRq();

                PresentationContext pc1 = new PresentationContext(this.abstractSyntax, 0, "1.2.840.10008.1.2");
                PresentationContext pc2 = new PresentationContext(this.abstractSyntax, 4, "1.2.840.10008.1.2.1");
                SendAssociateAc(pc1, pc2);

                ReceiveDicomMessage();

                DicomMessage dicomMessage = new DicomMessage(DimseCommand.CSTORERSP);
                dicomMessage.Set("0x00000900", VR.US, 0);

                if (pcId == -1)
                {
                    Send(dicomMessage);
                }
                else
                {
                    Send(dicomMessage, pcId);
                }

                ReceiveReleaseRq();

                SendReleaseRp();
            }
        }

        public class CFINDDicomThread : DicomThread
        {
            int testNr;

            private DicomMessage cFindMessage = null;

            private String transferSyntaxUid = null;
            private String sopClassUid = null;

            public CFINDDicomThread(int index, DicomMessage cFindMsg, String sopUid, String transferSyntaxUid)
            {
                testNr = index;
                cFindMessage = cFindMsg;
                this.sopClassUid = sopUid;
                this.transferSyntaxUid = transferSyntaxUid;
            }

            protected override void Execute()
            {
                CFINDSCUThread scu = new CFINDSCUThread(cFindMessage, this.transferSyntaxUid);

                scu.Initialize(this);
                scu.SopClassUid = this.sopClassUid;
                scu.Options.CopyFrom(this.Options);
                scu.Options.LogThreadStartingAndStoppingInParent = false;
                string resultFilename = string.Format("Ticket1266_{0}_1_SCU", testNr);
                scu.Options.ResultsFileNameOnlyWithoutExtension = resultFilename;
                scu.Options.LoadFromFile(Path.Combine(Paths.SessionDirectoryFullPath, "1266_CFind.ses"));

                CFINDSCPThread scp = new CFINDSCPThread();

                scp.Initialize(this);

                scp.Options.CopyFrom(this.Options);
                scp.Options.LogThreadStartingAndStoppingInParent = false;
                resultFilename = string.Format("Ticket1266_{0}_1_SCP", testNr);
                scp.Options.ResultsFileNameOnlyWithoutExtension = resultFilename;
                scp.Options.LoadFromFile(Path.Combine(Paths.SessionDirectoryFullPath, "1266_CFind.ses"));

                scp.Start();

                // Wait 1 seconds before starting the SCU.
                Sleep(1000);

                scu.Start();
            }
        }

        public class CFINDSCUThread : DicomThread
        {
            private DicomMessage cFindMessage = null;

            private String transferSyntaxUid = "1.2.840.10008.1.2";
            private String sopClassUid = "1.2.840.10008.5.1.4.1.2.1.1";

            public String SopClassUid
            {
                set
                {
                    this.sopClassUid = value;
                }
            }

            public CFINDSCUThread(DicomMessage cFindMsg, String transferSyntaxUid)
            {
                cFindMessage = cFindMsg;
                this.transferSyntaxUid = transferSyntaxUid;
            }

            protected override void Execute()
            {
                PresentationContext pc = new PresentationContext(this.sopClassUid, this.transferSyntaxUid);
                SendAssociateRq(pc);

                ReceiveAssociateAc();

                Send(cFindMessage);

                ReceiveDicomMessage();

                SendReleaseRq();

                ReceiveReleaseRp();
            }
        }

        public class CFINDSCPThread : DicomThread
        {
            private String transferSyntaxUid = "1.2.840.10008.1.2";

            public String TransferSyntaxUid
            {
                set
                {
                    this.transferSyntaxUid = value;
                }
            }

            protected override void Execute()
            {
                ReceiveAssociateRq();

                SendAssociateAc();

                DicomMessage receivedCFindRequest = ReceiveDicomMessage();

                WriteInformation(receivedCFindRequest.DataSet.DumpUsingVisualBasicNotation("dataSet"));

                DicomMessage dicomMessage = new DicomMessage(DimseCommand.CFINDRSP);
                dicomMessage.Set("0x00000900", VR.US, 0);

                Send(dicomMessage);
                
                ReceiveReleaseRq();

                SendReleaseRp();
            }
        }

        private class Ticket1202Scu : DicomThread
        {
            private int milliSecondsToWaitForPendingDataInNetworkInputBuffer = 0;

            private bool hasPendingDataInNetworkInputBuffer = true;

            public Ticket1202Scu(int milliSecondsToWaitForPendingDataInNetworkInputBuffer)
            {
                this.milliSecondsToWaitForPendingDataInNetworkInputBuffer = milliSecondsToWaitForPendingDataInNetworkInputBuffer;
            }

            public bool HasPendingDataInNetworkInputBuffer
            {
                get
                {
                    return (this.hasPendingDataInNetworkInputBuffer);
                }
            }

            protected override void Execute()
            {
                SendAssociateRq(new PresentationContext("1.2.840.10008.1.1", "1.2.840.10008.1.2.1"));

                ReceiveAssociateAc();

                int waitedTime = 0;
                this.hasPendingDataInNetworkInputBuffer = WaitForPendingDataInNetworkInputBuffer(this.milliSecondsToWaitForPendingDataInNetworkInputBuffer, ref waitedTime);

                if (this.hasPendingDataInNetworkInputBuffer)
                {
                    ReceiveDicomMessage();

                    DicomMessage cEchoResponse = new DicomMessage(DimseCommand.CECHORSP);

                    Send(cEchoResponse);
                }

                SendReleaseRq();

                ReceiveReleaseRp();
            }
        }

        private class Ticket1202Scp : DicomThread
        {
            private int milliSecondsToWaitForPendingDataInNetworkInputBuffer = 0;

            private bool hasPendingDataInNetworkInputBuffer = true;

            public Ticket1202Scp(int milliSecondsToWaitForPendingDataInNetworkInputBuffer)
            {
                this.milliSecondsToWaitForPendingDataInNetworkInputBuffer = milliSecondsToWaitForPendingDataInNetworkInputBuffer;
            }

            public bool HasPendingDataInNetworkInputBuffer
            {
                get
                {
                    return (this.hasPendingDataInNetworkInputBuffer);
                }
            }

            protected override void Execute()
            {
                ReceiveAssociateRq();

                SendAssociateAc(new PresentationContext("1.2.840.10008.1.1", 0, "1.2.840.10008.1.2.1"));

                int waitedTime = 0;
                this.hasPendingDataInNetworkInputBuffer = WaitForPendingDataInNetworkInputBuffer(this.milliSecondsToWaitForPendingDataInNetworkInputBuffer, ref waitedTime);

                if (!this.hasPendingDataInNetworkInputBuffer)
                // The SCU has not yet sent a release request.
                {
                    Send(DicomMessage_NUnit.CreateValidCEchoRq());

                    ReceiveDicomMessage();
                }

                DicomProtocolMessage dicomProtocolMessage = ReceiveMessage();

                if (dicomProtocolMessage is ReleaseRq)
                {
                    SendReleaseRp();
                }
                else
                {
                    throw new Exception("Not supposed to get here");
                }
            }
        }

        /// <summary>
        /// Test for ticket 1202.
        /// </summary>
        [Test]
        public void Ticket1202_1_1()
        {
            Ticket1202_1();

            Assert.That(ticket1202Scu.HasPendingDataInNetworkInputBuffer, Is.EqualTo(false));
        }

        /// <summary>
        /// Test for ticket 1202.
        /// </summary>
        [Test]
        public void Ticket1202_1_2()
        {
            Ticket1202_1();

            Assert.That(ticket1202Scp.HasPendingDataInNetworkInputBuffer, Is.EqualTo(true));
        }

        /// <summary>
        /// Test for ticket 1202.
        /// </summary>
        [Test]
        public void Ticket1202_1_3()
        {
            Ticket1202_1();

            Assert.That(ticket1202Scu.HasExceptionOccured, Is.EqualTo(false));
        }

        /// <summary>
        /// Test for ticket 1202.
        /// </summary>
        [Test]
        public void Ticket1202_1_4()
        {
            Ticket1202_1();

            Assert.That(ticket1202Scp.HasExceptionOccured, Is.EqualTo(false));
        }

        /// <summary>
        /// Test for ticket 1202.
        /// </summary>
        [Test]
        public void Ticket1202_2_1()
        {
            Ticket1202_2();

            Assert.That(ticket1202Scu.HasPendingDataInNetworkInputBuffer, Is.EqualTo(true));
        }

        /// <summary>
        /// Test for ticket 1202.
        /// </summary>
        [Test]
        public void Ticket1202_2_2()
        {
            Ticket1202_2();

            Assert.That(ticket1202Scp.HasPendingDataInNetworkInputBuffer, Is.EqualTo(false));
        }

        /// <summary>
        /// Test for ticket 1202.
        /// </summary>
        [Test]
        public void Ticket1202_2_3()
        {
            Ticket1202_2();

            Assert.That(ticket1202Scu.HasExceptionOccured, Is.EqualTo(false));
        }

        /// <summary>
        /// Test for ticket 1202.
        /// </summary>
        [Test]
        public void Ticket1202_2_4()
        {
            Ticket1202_2();

            Assert.That(ticket1202Scp.HasExceptionOccured, Is.EqualTo(false));
        }

        private Ticket1202Scu ticket1202Scu = null;

        private Ticket1202Scp ticket1202Scp = null;

        private bool ticket1202_1_Executed = false;

        private bool ticket1202_2_Executed = false;

        private void Ticket1202_1()
        {
            if (!ticket1202_1_Executed)
            {
                Ticket1202(2000, 4000);
                ticket1202_1_Executed = true;
            }
        }

        private void Ticket1202_2()
        {
            if (!ticket1202_2_Executed)
            {
                Ticket1202(4000, 2000);
                ticket1202_2_Executed = true;
            }
        }

        private void Ticket1202(int timeOutScu, int timeOutScp)
        {
            ThreadManager threadManager = new ThreadManager();
                        
            this.ticket1202Scu = new Ticket1202Scu(timeOutScu);
            ticket1202Scu.Initialize(threadManager);

            ticket1202Scu.Options.ResultsDirectory = Paths.ResultsDirectoryFullPath;
            ticket1202Scu.Options.ResultsFileNameOnlyWithoutExtension = "Ticket1202_1_scu_" + timeOutScu.ToString();
            ticket1202Scu.Options.SutIpAddress = "localhost";
            ticket1202Scu.Options.SutPort = 104;

            this.ticket1202Scp = new Ticket1202Scp(timeOutScp);
            ticket1202Scp.Initialize(threadManager);

            ticket1202Scp.Options.ResultsDirectory = Paths.ResultsDirectoryFullPath;
            ticket1202Scp.Options.ResultsFileNameOnlyWithoutExtension = "Ticket1202_1_scp_" + timeOutScp.ToString();
            ticket1202Scp.Options.DvtPort = 104;

            HliForm hliForm = new HliForm();
            hliForm.Attach(ticket1202Scp);
            hliForm.Attach(ticket1202Scu);

            ticket1202Scp.Start();
            ticket1202Scu.Start(100);

            ticket1202Scp.WaitForCompletion();
            ticket1202Scu.WaitForCompletion();
        }

        /// <summary>
        /// Test for ticket 1358.
        /// 
        /// This test will set up a secure connection between a SCU-SCP pair.
        /// </summary>
        [Test]
        public void Ticket1358_1_1()
        {
            ThreadManager threadManager = new ThreadManager();

            Ticket1358DicomThread dicomThread = new Ticket1358DicomThread(1);
            dicomThread.Initialize(threadManager);
            dicomThread.Options.ResultsDirectory = Paths.ResultsDirectoryFullPath;
            dicomThread.Options.ResultsFileNameOnlyWithoutExtension = "Ticket1358_1_1";

            dicomThread.Start();
            dicomThread.WaitForCompletion();

            Assert.That(dicomThread.NrOfErrors + dicomThread.NrOfWarnings, Is.EqualTo(2));
        }

       /// <summary>
        /// This test sets up a secure connection between a SCU & SCP(SCU timeout less than SCP timeout) and checks whether the SCU has pending data in its buffer.
       /// </summary>        
        [Test]
        public void Ticket1358_2_1()
        {
            Ticket1358_2();
            Assert.That(scu1358.HasPendingDataInNetworkInputBuffer, Is.EqualTo(false));
        }

        /// <summary>
        /// This test sets up a secure connection between a SCU & SCP(SCU timeout less than SCP timeout) and checks whether the SCP has pending data in its buffer.
        /// </summary>
        [Test]
        public void Ticket1358_2_2()
        {
            Ticket1358_2();
            Assert.That(scp1358.HasPendingDataInNetworkInputBuffer, Is.EqualTo(true));
        }

        [Test]
        public void Ticket1358_2_3()
        {
            Ticket1358_2();
            Assert.That(scu1358.HasExceptionOccured, Is.EqualTo(false));
        }

        [Test]
        public void Ticket1358_2_4()
        {
            Ticket1358_2();
            Assert.That(scp1358.HasExceptionOccured, Is.EqualTo(false));
        }

        /// <summary>
        /// This test sets up a secure connection between a SCU & SCP(SCU timeout>SCP timeout) and checks whether the SCU has pending data in its buffer.
        /// </summary>
        [Test]
        public void Ticket1358_3_1()
        {
            Ticket1358_3();
            Assert.That(scu1358.HasPendingDataInNetworkInputBuffer, Is.EqualTo(true));
        }

        /// <summary>
        /// This test sets up a secure connection between a SCU & SCP(SCU timeout>SCP timeout) and checks whether the SCP has pending data in its buffer.
        /// </summary>
        [Test]
        public void Ticket1358_3_2()
        {
            Ticket1358_3();
            Assert.That(scp1358.HasPendingDataInNetworkInputBuffer, Is.EqualTo(false));
        }

        [Test]
        public void Ticket1358_3_3()
        {
            Ticket1358_3();
            Assert.That(scu1358.HasExceptionOccured, Is.EqualTo(false));
        }

        [Test]
        public void Ticket1358_3_4()
        {
            Ticket1358_3();
            Assert.That(scp1358.HasExceptionOccured, Is.EqualTo(false));
        }

        private Ticket1202Scu scu1358 = null;

        private Ticket1202Scp scp1358 = null;

        private bool ticket1358_2_Executed = false;

        private bool ticket1358_3_Executed = false;

        private void Ticket1358_2()
        {
            if (!ticket1358_2_Executed)
            {
                ticket1358(2000, 4000);
                ticket1358_2_Executed = true;
            }
        }

        private void Ticket1358_3()
        {
            if (!ticket1358_3_Executed)
            {
                ticket1358(4000, 2000);
                ticket1358_3_Executed = true;
            }
        }

        private void ticket1358(int timeoutScu, int timeoutScp)
        {
            ThreadManager threadManager = new ThreadManager();

            this.scu1358 = new Ticket1202Scu(timeoutScu);
            scu1358.Initialize(threadManager);

            this.scp1358 = new Ticket1202Scp(timeoutScp);
            scp1358.Initialize(threadManager);

            scu1358.Options.LoadFromFile(Path.Combine(Paths.SessionDirectoryFullPath, "1358_Storage_SCU.ses"));
            scp1358.Options.LoadFromFile(Path.Combine(Paths.SessionDirectoryFullPath, "1358_Storage_SCP.ses"));

            scu1358.Options.ResultsFileNameOnlyWithoutExtension = "Ticket1358_2_scu_" + timeoutScu.ToString();
            scu1358.Options.DvtkScriptSession.SecuritySettings.TlsVersionFlags = DvtkSession.TlsVersionFlags.TLS_VERSION_SSLv3;
            scu1358.Options.DvtkScriptSession.SecuritySettings.CipherFlags = DvtkSession.CipherFlags.TLS_AUTHENICATION_METHOD_RSA | DvtkSession.CipherFlags.TLS_KEY_EXCHANGE_METHOD_RSA | DvtkSession.CipherFlags.TLS_DATA_INTEGRITY_METHOD_SHA1 | DvtkSession.CipherFlags.TLS_ENCRYPTION_METHOD_3DES;
            
            scp1358.Options.ResultsFileNameOnlyWithoutExtension = "Ticket1358_2_scp_" + timeoutScp.ToString();
            scp1358.Options.DvtkScriptSession.SecuritySettings.TlsVersionFlags = DvtkSession.TlsVersionFlags.TLS_VERSION_SSLv3;
            scp1358.Options.DvtkScriptSession.SecuritySettings.CipherFlags = DvtkSession.CipherFlags.TLS_AUTHENICATION_METHOD_RSA | DvtkSession.CipherFlags.TLS_KEY_EXCHANGE_METHOD_RSA | DvtkSession.CipherFlags.TLS_DATA_INTEGRITY_METHOD_SHA1 | DvtkSession.CipherFlags.TLS_ENCRYPTION_METHOD_3DES;

            scp1358.Start();
            scu1358.Start(100);

            scp1358.WaitForCompletion();
            scu1358.WaitForCompletion();
        }

        /// <summary>
        /// Tests Ticket1358_4_1() to Ticket1358_4_4() test different combination of security settings for a secure connection between a SCU and a SCP.
        /// </summary>
        [Test]
        public void Ticket1358_4_1()
        {
            ThreadManager threadManager = new ThreadManager();

            Secure1358DicomThread dicomThread = new Secure1358DicomThread(1);
            dicomThread.Initialize(threadManager);
            dicomThread.Options.ResultsDirectory = Paths.ResultsDirectoryFullPath;
            dicomThread.Options.ResultsFileNameOnlyWithoutExtension = "Ticket1358_4_1";
            dicomThread.Start();
            dicomThread.WaitForCompletion();

            Assert.That(dicomThread.NrOfErrors + dicomThread.NrOfWarnings, Is.EqualTo(2));
        }

        [Test]
        public void Ticket1358_4_2()
        {
            ThreadManager threadManager = new ThreadManager();

            Secure1358DicomThread dicomThread = new Secure1358DicomThread(2);
            dicomThread.Initialize(threadManager);
            dicomThread.Options.ResultsDirectory = Paths.ResultsDirectoryFullPath;
            dicomThread.Options.ResultsFileNameOnlyWithoutExtension = "Ticket1358_4_2";
            dicomThread.Start();
            dicomThread.WaitForCompletion();

            Assert.That(dicomThread.NrOfErrors + dicomThread.NrOfWarnings, Is.EqualTo(2));
        }

        //[Test]
        //public void Ticket1358_4_3()
        //{
        //    ThreadManager threadManager = new ThreadManager();

        //    Secure1358DicomThread dicomThread = new Secure1358DicomThread(3);
        //    dicomThread.Initialize(threadManager);
        //    dicomThread.Options.ResultsDirectory = Paths.ResultsDirectoryFullPath;
        //    dicomThread.Options.ResultsFileNameOnlyWithoutExtension = "Ticket1358_4_3";
        //    dicomThread.Start();
        //    dicomThread.WaitForCompletion();

        //    Assert.That(dicomThread.NrOfErrors + dicomThread.NrOfWarnings, Is.EqualTo(1));
        //}

        [Test]
        public void Ticket1358_4_3()
        {
            ThreadManager threadManager = new ThreadManager();

            Secure1358DicomThread dicomThread = new Secure1358DicomThread(3);
            dicomThread.Initialize(threadManager);
            dicomThread.Options.ResultsDirectory = Paths.ResultsDirectoryFullPath;
            dicomThread.Options.ResultsFileNameOnlyWithoutExtension = "Ticket1358_4_3";
            dicomThread.Start();
            dicomThread.WaitForCompletion();

            Assert.That(dicomThread.NrOfErrors + dicomThread.NrOfWarnings, Is.EqualTo(2));
        }

        //[Test]
        //public void Ticket1358_4_5()
        //{
        //    ThreadManager threadManager = new ThreadManager();

        //    Secure1358DicomThread dicomThread = new Secure1358DicomThread(5);
        //    dicomThread.Initialize(threadManager);
        //    dicomThread.Options.ResultsDirectory = Paths.ResultsDirectoryFullPath;
        //    dicomThread.Options.ResultsFileNameOnlyWithoutExtension = "Ticket1358_4_5";
        //    dicomThread.Start();
        //    dicomThread.WaitForCompletion();

        //    Assert.That(dicomThread.NrOfErrors + dicomThread.NrOfWarnings, Is.EqualTo(1));
        //}

        [Test]
        public void Ticket1358_4_4()
        {
            ThreadManager threadManager = new ThreadManager();

            Secure1358DicomThread dicomThread = new Secure1358DicomThread(4);
            dicomThread.Initialize(threadManager);
            dicomThread.Options.ResultsDirectory = Paths.ResultsDirectoryFullPath;
            dicomThread.Options.ResultsFileNameOnlyWithoutExtension = "Ticket1358_4_4";
            dicomThread.Start();
            dicomThread.WaitForCompletion();

            Assert.That(dicomThread.NrOfErrors + dicomThread.NrOfWarnings, Is.EqualTo(2));
        }

        [Test]
        public void Ticket1358_4_5()
        {
            ThreadManager threadManager = new ThreadManager();

            Secure1358DicomThread dicomThread = new Secure1358DicomThread(5);
            dicomThread.Initialize(threadManager);
            dicomThread.Options.ResultsDirectory = Paths.ResultsDirectoryFullPath;
            dicomThread.Options.ResultsFileNameOnlyWithoutExtension = "Ticket1358_4_5";
            dicomThread.Start();
            dicomThread.WaitForCompletion();

            Assert.That(dicomThread.NrOfErrors + dicomThread.NrOfWarnings, Is.EqualTo(2));
        }

        //[Test]
        //public void Ticket1358_4_8()
        //{
        //    ThreadManager threadManager = new ThreadManager();

        //    Secure1358DicomThread dicomThread = new Secure1358DicomThread(8);
        //    dicomThread.Initialize(threadManager);
        //    dicomThread.Options.ResultsDirectory = Paths.ResultsDirectoryFullPath;
        //    dicomThread.Options.ResultsFileNameOnlyWithoutExtension = "Ticket1358_4_8";
        //    dicomThread.Start();
        //    dicomThread.WaitForCompletion();

        //    Assert.That(dicomThread.NrOfErrors + dicomThread.NrOfWarnings, Is.EqualTo(1));
        //}

        [Test]
        public void Ticket1358_4_6()
        {
            ThreadManager threadManager = new ThreadManager();

            Secure1358DicomThread dicomThread = new Secure1358DicomThread(6);
            dicomThread.Initialize(threadManager);
            dicomThread.Options.ResultsDirectory = Paths.ResultsDirectoryFullPath;
            dicomThread.Options.ResultsFileNameOnlyWithoutExtension = "Ticket1358_4_6";
            dicomThread.Start();
            dicomThread.WaitForCompletion();

            Assert.That(dicomThread.NrOfErrors + dicomThread.NrOfWarnings, Is.EqualTo(2));
        }

        [Test]
        public void Ticket1358_4_7()
        {
            ThreadManager threadManager = new ThreadManager();

            Secure1358DicomThread dicomThread = new Secure1358DicomThread(7);
            dicomThread.Initialize(threadManager);
            dicomThread.Options.ResultsDirectory = Paths.ResultsDirectoryFullPath;
            dicomThread.Options.ResultsFileNameOnlyWithoutExtension = "Ticket1358_4_7";
            dicomThread.Start();
            dicomThread.WaitForCompletion();

            Assert.That(dicomThread.NrOfErrors + dicomThread.NrOfWarnings, Is.EqualTo(2));
        }

        [Test]
        public void Ticket1358_4_8()
        {
            ThreadManager threadManager = new ThreadManager();

            Secure1358DicomThread dicomThread = new Secure1358DicomThread(8);
            dicomThread.Initialize(threadManager);
            dicomThread.Options.ResultsDirectory = Paths.ResultsDirectoryFullPath;
            dicomThread.Options.ResultsFileNameOnlyWithoutExtension = "Ticket1358_4_8";
            dicomThread.Start();
            dicomThread.WaitForCompletion();

            Assert.That(dicomThread.NrOfErrors + dicomThread.NrOfWarnings, Is.EqualTo(2));
        }


        private class Ticket1358DicomThread : DicomThread
        {
            int testNr;
            public Ticket1358DicomThread(int index)
            {
                testNr = index;
            }

            protected override void Execute()
            {
                SecureSCUThread scu = new SecureSCUThread();

                scu.Initialize(this);

                scu.Options.CopyFrom(this.Options);
                scu.Options.LogThreadStartingAndStoppingInParent = false;
                string resultFilename = string.Format("Ticket1358_{0}_1_SCU", testNr);
                scu.Options.ResultsFileNameOnlyWithoutExtension = resultFilename;
                scu.Options.LoadFromFile(Path.Combine(Paths.SessionDirectoryFullPath, "1358_Storage_SCU.ses"));

                SecureSCPThread scp = new SecureSCPThread();

                scp.Initialize(this);

                scp.Options.CopyFrom(this.Options);
                scp.Options.LogThreadStartingAndStoppingInParent = false;
                resultFilename = string.Format("Ticket1358_{0}_1_SCP", testNr);
                scp.Options.ResultsFileNameOnlyWithoutExtension = resultFilename;
                scp.Options.LoadFromFile(Path.Combine(Paths.SessionDirectoryFullPath, "1358_Storage_SCP.ses"));

                scp.Start();

                // Wait 1 seconds before starting the SCU.
                Sleep(1000);

                scu.Start();
            }
        }

        private class SecureSCUThread : DicomThread
        {
            private DicomFile dicomFile = null;

            public SecureSCUThread()
            {
                dicomFile = new DicomFile();
            }

            protected override void Execute()
            {
                PresentationContext pc = new PresentationContext("1.2.840.10008.5.1.4.1.1.7", "1.2.840.10008.1.2.1");

                SendAssociateRq(pc);

                ReceiveAssociateAc();

                DicomMessage dicomMessage = new DicomMessage(DimseCommand.CSTORERQ);

                dicomFile.Read(Path.Combine(Paths.DataDirectoryFullPath, "1181.dcm"), this);

                dicomMessage.CommandSet.Set("0x00000002", VR.UI, "1.2.840.10008.5.1.4.1.1.7"); // "Affected SOP Class UID"
                //dicomMessage.CommandSet.Set("0x00001000", VR.UI, "5.6.7.8.9"); // "Affected SOP Instance UID"
                dicomMessage.CommandSet.Set("0x00000110", VR.US, 100); // "Message ID"
                dicomMessage.CommandSet.Set("0x00000700", VR.US, 0); // "Priority"

                dicomMessage.DataSet.DvtkDataDataSet = dicomFile.DataSet.DvtkDataDataSet;

                Send(dicomMessage);

                ReceiveDicomMessage();

                SendReleaseRq();

                ReceiveReleaseRp();
            }
        }

        private class SecureSCPThread : DicomThread
        {
            public SecureSCPThread() { }

            protected override void Execute()
            {
                ReceiveAssociateRq();

                PresentationContext pc = new PresentationContext("1.2.840.10008.5.1.4.1.1.7", 0, "1.2.840.10008.1.2.1");
                SendAssociateAc(pc);

                ReceiveDicomMessage();

                DicomMessage dicomMessage = new DicomMessage(DimseCommand.CSTORERSP);

                dicomMessage.Set("0x00000900", VR.US, 0);

                Send(dicomMessage);

                ReceiveReleaseRq();

                SendReleaseRp();
            }
        }
        
        private class Secure1358DicomThread : DicomThread
        {
            int testNr;
            public Secure1358DicomThread(int index)
            {
                testNr = index;
            }

            protected override void Execute()
            {
                SecureSCUThread Ticket1358scu = new SecureSCUThread();
                SecureSCPThread Ticket1358scp = new SecureSCPThread();
                Ticket1358scu.Initialize(this);
                Ticket1358scp.Initialize(this);

                Ticket1358scu.Options.CopyFrom(this.Options);
                Ticket1358scu.Options.LogThreadStartingAndStoppingInParent = false;
                string resultFilename = string.Format("Ticket1358_4_{0}_SCU", testNr);
                Ticket1358scu.Options.ResultsFileNameOnlyWithoutExtension = resultFilename;
                Ticket1358scu.Options.LoadFromFile(Path.Combine(Paths.SessionDirectoryFullPath, "1358_Storage_SCU.ses"));

                Ticket1358scp.Options.CopyFrom(this.Options);
                Ticket1358scp.Options.LogThreadStartingAndStoppingInParent = false;
                resultFilename = string.Format("Ticket1358_4_{0}_SCP", testNr);
                Ticket1358scp.Options.ResultsFileNameOnlyWithoutExtension = resultFilename;
                Ticket1358scp.Options.LoadFromFile(Path.Combine(Paths.SessionDirectoryFullPath, "1358_Storage_SCP.ses"));

                switch (testNr)
                {
                    case 1:
                        Ticket1358scu.Options.DvtkScriptSession.SecuritySettings.CipherFlags = DvtkSession.CipherFlags.TLS_AUTHENICATION_METHOD_RSA | DvtkSession.CipherFlags.TLS_KEY_EXCHANGE_METHOD_RSA | DvtkSession.CipherFlags.TLS_DATA_INTEGRITY_METHOD_SHA1 | DvtkSession.CipherFlags.TLS_ENCRYPTION_METHOD_AES256;
                        Ticket1358scp.Options.DvtkScriptSession.SecuritySettings.CipherFlags = DvtkSession.CipherFlags.TLS_AUTHENICATION_METHOD_RSA | DvtkSession.CipherFlags.TLS_KEY_EXCHANGE_METHOD_RSA | DvtkSession.CipherFlags.TLS_DATA_INTEGRITY_METHOD_SHA1 | DvtkSession.CipherFlags.TLS_ENCRYPTION_METHOD_AES256;
                        break;

                    case 2:
                        Ticket1358scu.Options.DvtkScriptSession.SecuritySettings.CipherFlags = DvtkSession.CipherFlags.TLS_AUTHENICATION_METHOD_RSA | DvtkSession.CipherFlags.TLS_KEY_EXCHANGE_METHOD_DH | DvtkSession.CipherFlags.TLS_DATA_INTEGRITY_METHOD_SHA1 | DvtkSession.CipherFlags.TLS_ENCRYPTION_METHOD_AES256;
                        Ticket1358scp.Options.DvtkScriptSession.SecuritySettings.CipherFlags = DvtkSession.CipherFlags.TLS_AUTHENICATION_METHOD_RSA | DvtkSession.CipherFlags.TLS_KEY_EXCHANGE_METHOD_DH | DvtkSession.CipherFlags.TLS_DATA_INTEGRITY_METHOD_SHA1 | DvtkSession.CipherFlags.TLS_ENCRYPTION_METHOD_AES256;
                        break;

                    //case 3:
                    //    Ticket1358scu.Options.DvtkScriptSession.SecuritySettings.CipherFlags = DvtkSession.CipherFlags.TLS_AUTHENICATION_METHOD_DSA | DvtkSession.CipherFlags.TLS_KEY_EXCHANGE_METHOD_DH | DvtkSession.CipherFlags.TLS_DATA_INTEGRITY_METHOD_SHA1 | DvtkSession.CipherFlags.TLS_ENCRYPTION_METHOD_AES256;
                    //    Ticket1358scp.Options.DvtkScriptSession.SecuritySettings.CipherFlags = DvtkSession.CipherFlags.TLS_AUTHENICATION_METHOD_DSA | DvtkSession.CipherFlags.TLS_KEY_EXCHANGE_METHOD_DH | DvtkSession.CipherFlags.TLS_DATA_INTEGRITY_METHOD_SHA1 | DvtkSession.CipherFlags.TLS_ENCRYPTION_METHOD_AES256;
                    //    break;

                    case 3:
                        Ticket1358scu.Options.DvtkScriptSession.SecuritySettings.CipherFlags = DvtkSession.CipherFlags.TLS_AUTHENICATION_METHOD_RSA | DvtkSession.CipherFlags.TLS_KEY_EXCHANGE_METHOD_DH | DvtkSession.CipherFlags.TLS_DATA_INTEGRITY_METHOD_SHA1 | DvtkSession.CipherFlags.TLS_ENCRYPTION_METHOD_3DES;
                        Ticket1358scp.Options.DvtkScriptSession.SecuritySettings.CipherFlags = DvtkSession.CipherFlags.TLS_AUTHENICATION_METHOD_RSA | DvtkSession.CipherFlags.TLS_KEY_EXCHANGE_METHOD_DH | DvtkSession.CipherFlags.TLS_DATA_INTEGRITY_METHOD_SHA1 | DvtkSession.CipherFlags.TLS_ENCRYPTION_METHOD_3DES;
                        break;

                    //case 5:
                    //    Ticket1358scu.Options.DvtkScriptSession.SecuritySettings.CipherFlags = DvtkSession.CipherFlags.TLS_AUTHENICATION_METHOD_DSA | DvtkSession.CipherFlags.TLS_KEY_EXCHANGE_METHOD_DH | DvtkSession.CipherFlags.TLS_DATA_INTEGRITY_METHOD_SHA1 | DvtkSession.CipherFlags.TLS_ENCRYPTION_METHOD_3DES;
                    //    Ticket1358scp.Options.DvtkScriptSession.SecuritySettings.CipherFlags = DvtkSession.CipherFlags.TLS_AUTHENICATION_METHOD_DSA | DvtkSession.CipherFlags.TLS_KEY_EXCHANGE_METHOD_DH | DvtkSession.CipherFlags.TLS_DATA_INTEGRITY_METHOD_SHA1 | DvtkSession.CipherFlags.TLS_ENCRYPTION_METHOD_3DES;
                    //    break;

                    case 4:
                        Ticket1358scu.Options.DvtkScriptSession.SecuritySettings.CipherFlags = DvtkSession.CipherFlags.TLS_AUTHENICATION_METHOD_RSA | DvtkSession.CipherFlags.TLS_KEY_EXCHANGE_METHOD_RSA | DvtkSession.CipherFlags.TLS_DATA_INTEGRITY_METHOD_SHA1 | DvtkSession.CipherFlags.TLS_ENCRYPTION_METHOD_3DES;
                        Ticket1358scp.Options.DvtkScriptSession.SecuritySettings.CipherFlags = DvtkSession.CipherFlags.TLS_AUTHENICATION_METHOD_RSA | DvtkSession.CipherFlags.TLS_KEY_EXCHANGE_METHOD_RSA | DvtkSession.CipherFlags.TLS_DATA_INTEGRITY_METHOD_SHA1 | DvtkSession.CipherFlags.TLS_ENCRYPTION_METHOD_3DES;
                        break;

                    case 5:
                        Ticket1358scu.Options.DvtkScriptSession.SecuritySettings.CipherFlags = DvtkSession.CipherFlags.TLS_AUTHENICATION_METHOD_RSA | DvtkSession.CipherFlags.TLS_KEY_EXCHANGE_METHOD_DH | DvtkSession.CipherFlags.TLS_DATA_INTEGRITY_METHOD_SHA1 | DvtkSession.CipherFlags.TLS_ENCRYPTION_METHOD_AES128;
                        Ticket1358scp.Options.DvtkScriptSession.SecuritySettings.CipherFlags = DvtkSession.CipherFlags.TLS_AUTHENICATION_METHOD_RSA | DvtkSession.CipherFlags.TLS_KEY_EXCHANGE_METHOD_DH | DvtkSession.CipherFlags.TLS_DATA_INTEGRITY_METHOD_SHA1 | DvtkSession.CipherFlags.TLS_ENCRYPTION_METHOD_AES128;
                        break;

                    //case 8:
                    //    Ticket1358scu.Options.DvtkScriptSession.SecuritySettings.CipherFlags = DvtkSession.CipherFlags.TLS_AUTHENICATION_METHOD_DSA | DvtkSession.CipherFlags.TLS_KEY_EXCHANGE_METHOD_DH | DvtkSession.CipherFlags.TLS_DATA_INTEGRITY_METHOD_SHA1 | DvtkSession.CipherFlags.TLS_ENCRYPTION_METHOD_AES128;
                    //    Ticket1358scp.Options.DvtkScriptSession.SecuritySettings.CipherFlags = DvtkSession.CipherFlags.TLS_AUTHENICATION_METHOD_DSA | DvtkSession.CipherFlags.TLS_KEY_EXCHANGE_METHOD_DH | DvtkSession.CipherFlags.TLS_DATA_INTEGRITY_METHOD_SHA1 | DvtkSession.CipherFlags.TLS_ENCRYPTION_METHOD_AES128;
                    //    break;

                    case 6:
                        Ticket1358scu.Options.DvtkScriptSession.SecuritySettings.CipherFlags = DvtkSession.CipherFlags.TLS_AUTHENICATION_METHOD_RSA | DvtkSession.CipherFlags.TLS_KEY_EXCHANGE_METHOD_RSA | DvtkSession.CipherFlags.TLS_DATA_INTEGRITY_METHOD_SHA1 | DvtkSession.CipherFlags.TLS_ENCRYPTION_METHOD_AES128;
                        Ticket1358scp.Options.DvtkScriptSession.SecuritySettings.CipherFlags = DvtkSession.CipherFlags.TLS_AUTHENICATION_METHOD_RSA | DvtkSession.CipherFlags.TLS_KEY_EXCHANGE_METHOD_RSA | DvtkSession.CipherFlags.TLS_DATA_INTEGRITY_METHOD_SHA1 | DvtkSession.CipherFlags.TLS_ENCRYPTION_METHOD_AES128;
                        break;

                    case 7:
                        Ticket1358scu.Options.DvtkScriptSession.SecuritySettings.CipherFlags = DvtkSession.CipherFlags.TLS_AUTHENICATION_METHOD_RSA | DvtkSession.CipherFlags.TLS_KEY_EXCHANGE_METHOD_RSA | DvtkSession.CipherFlags.TLS_DATA_INTEGRITY_METHOD_SHA1 | DvtkSession.CipherFlags.TLS_ENCRYPTION_METHOD_NONE;
                        Ticket1358scp.Options.DvtkScriptSession.SecuritySettings.CipherFlags = DvtkSession.CipherFlags.TLS_AUTHENICATION_METHOD_RSA | DvtkSession.CipherFlags.TLS_KEY_EXCHANGE_METHOD_RSA | DvtkSession.CipherFlags.TLS_DATA_INTEGRITY_METHOD_SHA1 | DvtkSession.CipherFlags.TLS_ENCRYPTION_METHOD_NONE;
                        break;

                    case 8:
                        Ticket1358scu.Options.DvtkScriptSession.SecuritySettings.CipherFlags = DvtkSession.CipherFlags.TLS_AUTHENICATION_METHOD_RSA | DvtkSession.CipherFlags.TLS_KEY_EXCHANGE_METHOD_RSA | DvtkSession.CipherFlags.TLS_DATA_INTEGRITY_METHOD_MD5 | DvtkSession.CipherFlags.TLS_ENCRYPTION_METHOD_NONE;
                        Ticket1358scp.Options.DvtkScriptSession.SecuritySettings.CipherFlags = DvtkSession.CipherFlags.TLS_AUTHENICATION_METHOD_RSA | DvtkSession.CipherFlags.TLS_KEY_EXCHANGE_METHOD_RSA | DvtkSession.CipherFlags.TLS_DATA_INTEGRITY_METHOD_MD5 | DvtkSession.CipherFlags.TLS_ENCRYPTION_METHOD_NONE;
                        break;
                }

                Ticket1358scu.Options.DvtkScriptSession.SecuritySettings.TlsVersionFlags = DvtkSession.TlsVersionFlags.TLS_VERSION_SSLv3;
                Ticket1358scp.Options.DvtkScriptSession.SecuritySettings.TlsVersionFlags = DvtkSession.TlsVersionFlags.TLS_VERSION_SSLv3;
                
                Ticket1358scp.Start();
                Ticket1358scu.Start(100);

                Ticket1358scp.WaitForCompletion();
                Ticket1358scu.WaitForCompletion();
            }
        }

        public class EchoSCU : DicomThread
        {
            protected override void Execute()
            {
                SendAssociateRq(new PresentationContext("1.2.840.10008.1.1", "1.2.840.10008.1.2.1"));

                ReceiveAssociateAc();

                DicomMessage CEchoRq = new DicomMessage(DimseCommand.CECHORQ);

                CEchoRq.CommandSet.Set("0x00000002", VR.UI, "1.2.840.10008.1.1");

                Send(CEchoRq);

                ReceiveDicomMessage();

                SendReleaseRq();

                ReceiveReleaseRp();
            }
        }

        private class DicomThreadValideDicomMessage: DicomThread
        {
            private DicomMessage dicomMessage = null;


            public DicomThreadValideDicomMessage(DicomMessage dicomMessage)
            {
                this.dicomMessage = dicomMessage;
            }

            protected override void Execute()
            {
                Validate(this.dicomMessage);
            }
        }

        /// <summary>
        /// Test for ticket 1282.
        /// 
        /// This test will verify that DT range matching is allowed for a QR C-FIND-RQ.
        /// </summary>
        [Test]
        public void Ticket1282_1_1()
        {
            ThreadManager threadManager = new ThreadManager();

            DicomMessage cFindRq = new DicomMessage(DimseCommand.CFINDRQ);
            cFindRq.CommandSet.Set("0x00000002", VR.UI, "1.2.840.10008.5.1.4.1.2.1.1");
            cFindRq.DataSet.Set("0x00404005", VR.DT, "20090724000000.0000-20090724235959.9999");

            DicomThreadValideDicomMessage dicomThread = new DicomThreadValideDicomMessage(cFindRq);
            dicomThread.Initialize(threadManager);

            Config.SetOptions(dicomThread, "Ticket1282_1_1", "MainThread");
            dicomThread.Options.LoadDefinitionFile(Path.Combine(Paths.DefinitionsDirectoryFullPath, "PatientRootQueryRetrieve-FIND.def")); 

            dicomThread.Start();
            dicomThread.WaitForCompletion();

            Assert.That(dicomThread.NrOfErrors, Is.EqualTo(5));
        }

        /// <summary>
        /// Test for ticket 1282.
        /// 
        /// This test will verify that DT range matching is not allowed for a QR C-FIND-RSP.
        /// </summary>
        [Test]
        public void Ticket1282_2_1()
        {
            ThreadManager threadManager = new ThreadManager();

            DicomMessage cFindRq = new DicomMessage(DimseCommand.CFINDRSP);
            cFindRq.CommandSet.Set("0x00000002", VR.UI, "1.2.840.10008.5.1.4.1.2.1.1");
            cFindRq.DataSet.Set("0x00404005", VR.DT, "20090724000000.0000-20090724235959.9999");

            DicomThreadValideDicomMessage dicomThread = new DicomThreadValideDicomMessage(cFindRq);
            dicomThread.Initialize(threadManager);

            Config.SetOptions(dicomThread, "Ticket1282_2_1", "MainThread");
            dicomThread.Options.LoadDefinitionFile(Path.Combine(Paths.DefinitionsDirectoryFullPath, "PatientRootQueryRetrieve-FIND.def")); 

            dicomThread.Start();
            dicomThread.WaitForCompletion();

            Assert.That(dicomThread.NrOfErrors, Is.EqualTo(9));
        }

        /// <summary>
        /// Test for ticket 1282.
        /// 
        /// This test will verify that DT range matching is allowed for a Unified Procedure Step – Pull SOP Class C-FIND-RQ.
        /// </summary>
        [Test]
        public void Ticket1282_3_1()
        {
            ThreadManager threadManager = new ThreadManager();

            DicomMessage cFindRq = new DicomMessage(DimseCommand.CFINDRQ);
            cFindRq.CommandSet.Set("0x00000002", VR.UI, "1.2.840.10008.5.1.4.34.4.3");
            cFindRq.DataSet.Set("0x00404005", VR.DT, "20090724000000.0000-20090724235959.9999");

            DicomThreadValideDicomMessage dicomThread = new DicomThreadValideDicomMessage(cFindRq);
            dicomThread.Initialize(threadManager);

            Config.SetOptions(dicomThread, "Ticket1282_3_1", "MainThread");
            dicomThread.Options.LoadDefinitionFile(Path.Combine(Paths.DefinitionsDirectoryFullPath, "UnifiedProcedureStep-Pull.def")); 

            dicomThread.Start();
            dicomThread.WaitForCompletion();

            Assert.That(dicomThread.NrOfErrors, Is.EqualTo(4));
        }

        public class ScpAssociationOnly : DicomThread
        {
            private String sopClassUid = string.Empty;

            private String transferSyntaxUid = string.Empty;

            public ScpAssociationOnly(String sopClassUid, String transferSyntaxUid)
            {
                this.sopClassUid = sopClassUid;
                this.transferSyntaxUid = transferSyntaxUid;
            }

            protected override void Execute()
            {
                ReceiveAssociateRq();

                SendAssociateAc(new SopClasses(this.sopClassUid), new TransferSyntaxes(this.transferSyntaxUid));

                ReceiveReleaseRq();

                SendReleaseRp();
            }
        }

        public class ScuAssociationOnly : DicomThread
        {
            private String sopClassUid = string.Empty;

            private String transferSyntaxUid = string.Empty;

            public ScuAssociationOnly(String sopClassUid, String transferSyntaxUid)
            {
                this.sopClassUid = sopClassUid;
                this.transferSyntaxUid = transferSyntaxUid;
            }

            protected override void Execute()
            {
                SendAssociateRq(new PresentationContext(this.sopClassUid, this.transferSyntaxUid));

                ReceiveAssociateAc();

                SendReleaseRq();

                ReceiveReleaseRp();
            }
        }
    }
}
