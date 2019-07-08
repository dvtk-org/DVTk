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
using System.Text;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using NUnit.Framework.SyntaxHelpers;

using DimseCommand = DvtkData.Dimse.DimseCommand;
using VR = DvtkData.Dimse.VR;
using DvtkHighLevelInterface.Dicom.Files;
using DvtkHighLevelInterface.Dicom.Messages;
using DvtkHighLevelInterface.Dicom.Other;
using DvtkHighLevelInterface.InformationModel;
using DvtkHighLevelInterface.NUnit;



namespace DvtkHighLevelInterface.InformationModel
{
    /// <summary>
    /// Contains NUnit Test Cases. 
    /// </summary>
    [TestFixture]
    public class QueryRetrievePatientRootInformationModel_NUnit
    {
        //
        // - Fields -
        //



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
        /// Common code for the Ticket788_1_... test cases.
        /// </summary>
        private void Ticket788CommonCode1(String patientName, out QueryRetrievePatientRootInformationModel queryRetrievePatientRootInformationModel, out DicomMessage cFindRequest)
        {
            queryRetrievePatientRootInformationModel = new QueryRetrievePatientRootInformationModel();
            cFindRequest = new DicomMessage(DimseCommand.CFINDRQ);
            DicomFile dicomFile = null;

            queryRetrievePatientRootInformationModel.DataDirectory = Paths.DataDirectoryFullPath;

            cFindRequest.CommandSet.Set("0x00000002", VR.UI, "1.2.840.10008.5.1.4.1.2.1.1"); // Set Affected SOP class to Patient Root Query/Retrieve Information Model – FIND
            cFindRequest.DataSet.Set("0x00100010", VR.PN, patientName); // Patient name attribute.
            cFindRequest.DataSet.Set("0x00080052", VR.CS, "PATIENT"); // Query Retrieve Level.

            dicomFile = new DicomFile();
            dicomFile.DataSet.Set("0x00100010", VR.PN, @".$^{[(|)*+?\1");
            queryRetrievePatientRootInformationModel.AddToInformationModel(dicomFile);

            dicomFile = new DicomFile();
            dicomFile.DataSet.Set("0x00100010", VR.PN, @".$^{[(|)*+?\2");
            queryRetrievePatientRootInformationModel.AddToInformationModel(dicomFile);

            dicomFile = new DicomFile();
            dicomFile.DataSet.Set("0x00100010", VR.PN, @"Patient name containing no regular expression operators");
            queryRetrievePatientRootInformationModel.AddToInformationModel(dicomFile);
        }

        /// <summary>
        /// Test matching if DICOM matching string contains a regular expression operator.
        /// </summary>
        [Test]
        public void Ticket788_01_1()
        {
            QueryRetrievePatientRootInformationModel queryRetrievePatientRootInformationModel = null;
            DicomMessage cFindRequest = null;

            Ticket788CommonCode1(@"*.*", out queryRetrievePatientRootInformationModel, out cFindRequest);

            DicomMessageCollection dicomMessageCollection = queryRetrievePatientRootInformationModel.QueryInformationModel(cFindRequest);

            // Expect two messages with status pending and on message with status success.
            Assert.That(dicomMessageCollection.Count, Is.EqualTo(3));
        }

        /// <summary>
        /// Test matching if DICOM matching string contains a regular expression operator.
        /// </summary>
        [Test]
        public void Ticket788_02_1()
        {
            QueryRetrievePatientRootInformationModel queryRetrievePatientRootInformationModel = null;
            DicomMessage cFindRequest = null;

            Ticket788CommonCode1(@"*$*", out queryRetrievePatientRootInformationModel, out cFindRequest);

            DicomMessageCollection dicomMessageCollection = queryRetrievePatientRootInformationModel.QueryInformationModel(cFindRequest);

            // Expect two messages with status pending and one message with status success.
            Assert.That(dicomMessageCollection.Count, Is.EqualTo(3));
        }

        /// <summary>
        /// Test matching if DICOM matching string contains a regular expression operator.
        /// </summary>
        [Test]
        public void Ticket788_03_1()
        {
            QueryRetrievePatientRootInformationModel queryRetrievePatientRootInformationModel = null;
            DicomMessage cFindRequest = null;

            Ticket788CommonCode1(@"*^*", out queryRetrievePatientRootInformationModel, out cFindRequest);

            DicomMessageCollection dicomMessageCollection = queryRetrievePatientRootInformationModel.QueryInformationModel(cFindRequest);

            // Expect two messages with status pending and one message with status success.
            Assert.That(dicomMessageCollection.Count, Is.EqualTo(3));
        }

        /// <summary>
        /// Test matching if DICOM matching string contains a regular expression operator.
        /// </summary>
        [Test]
        public void Ticket788_04_1()
        {
            QueryRetrievePatientRootInformationModel queryRetrievePatientRootInformationModel = null;
            DicomMessage cFindRequest = null;

            Ticket788CommonCode1(@"*{*", out queryRetrievePatientRootInformationModel, out cFindRequest);

            DicomMessageCollection dicomMessageCollection = queryRetrievePatientRootInformationModel.QueryInformationModel(cFindRequest);

            // Expect two messages with status pending and on message with status success.
            Assert.That(dicomMessageCollection.Count, Is.EqualTo(3));
        }

        /// <summary>
        /// Test matching if DICOM matching string contains a regular expression operator.
        /// </summary>
        [Test]
        public void Ticket788_05_1()
        {
            QueryRetrievePatientRootInformationModel queryRetrievePatientRootInformationModel = null;
            DicomMessage cFindRequest = null;

            Ticket788CommonCode1(@"*[*", out queryRetrievePatientRootInformationModel, out cFindRequest);

            DicomMessageCollection dicomMessageCollection = queryRetrievePatientRootInformationModel.QueryInformationModel(cFindRequest);

            // Expect two messages with status pending and on message with status success.
            Assert.That(dicomMessageCollection.Count, Is.EqualTo(3));
        }

        /// <summary>
        /// Test matching if DICOM matching string contains a regular expression operator.
        /// </summary>
        [Test]
        public void Ticket788_06_1()
        {
            QueryRetrievePatientRootInformationModel queryRetrievePatientRootInformationModel = null;
            DicomMessage cFindRequest = null;

            Ticket788CommonCode1(@"*(*", out queryRetrievePatientRootInformationModel, out cFindRequest);

            DicomMessageCollection dicomMessageCollection = queryRetrievePatientRootInformationModel.QueryInformationModel(cFindRequest);

            // Expect two messages with status pending and on message with status success.
            Assert.That(dicomMessageCollection.Count, Is.EqualTo(3));
        }

        /// <summary>
        /// Test matching if DICOM matching string contains a regular expression operator.
        /// </summary>
        [Test]
        public void Ticket788_07_1()
        {
            QueryRetrievePatientRootInformationModel queryRetrievePatientRootInformationModel = null;
            DicomMessage cFindRequest = null;

            Ticket788CommonCode1(@"*|*", out queryRetrievePatientRootInformationModel, out cFindRequest);

            DicomMessageCollection dicomMessageCollection = queryRetrievePatientRootInformationModel.QueryInformationModel(cFindRequest);

            // Expect two messages with status pending and on message with status success.
            Assert.That(dicomMessageCollection.Count, Is.EqualTo(3));
        }

        /// <summary>
        /// Test matching if DICOM matching string contains a regular expression operator.
        /// </summary>
        [Test]
        public void Ticket788_08_1()
        {
            QueryRetrievePatientRootInformationModel queryRetrievePatientRootInformationModel = null;
            DicomMessage cFindRequest = null;

            Ticket788CommonCode1(@"*)*", out queryRetrievePatientRootInformationModel, out cFindRequest);

            DicomMessageCollection dicomMessageCollection = queryRetrievePatientRootInformationModel.QueryInformationModel(cFindRequest);

            // Expect two messages with status pending and on message with status success.
            Assert.That(dicomMessageCollection.Count, Is.EqualTo(3));
        }

        /// <summary>
        /// Test matching if DICOM matching string contains a regular expression operator.
        /// </summary>
        [Test]
        public void Ticket788_09_1()
        {
            QueryRetrievePatientRootInformationModel queryRetrievePatientRootInformationModel = null;
            DicomMessage cFindRequest = null;

            Ticket788CommonCode1(@"*", out queryRetrievePatientRootInformationModel, out cFindRequest);

            DicomMessageCollection dicomMessageCollection = queryRetrievePatientRootInformationModel.QueryInformationModel(cFindRequest);

            // Expect two messages with status pending and on message with status success.
            Assert.That(dicomMessageCollection.Count, Is.EqualTo(4));
        }

        /// <summary>
        /// Test matching if DICOM matching string contains a regular expression operator.
        /// </summary>
        [Test]
        public void Ticket788_10_1()
        {
            QueryRetrievePatientRootInformationModel queryRetrievePatientRootInformationModel = null;
            DicomMessage cFindRequest = null;

            Ticket788CommonCode1(@"*+*", out queryRetrievePatientRootInformationModel, out cFindRequest);

            DicomMessageCollection dicomMessageCollection = queryRetrievePatientRootInformationModel.QueryInformationModel(cFindRequest);

            // Expect two messages with status pending and on message with status success.
            Assert.That(dicomMessageCollection.Count, Is.EqualTo(3));
        }

        /// <summary>
        /// Test matching if DICOM matching string contains a regular expression operator.
        /// </summary>
        [Test]
        public void Ticket788_11_1()
        {
            QueryRetrievePatientRootInformationModel queryRetrievePatientRootInformationModel = null;
            DicomMessage cFindRequest = null;

            Ticket788CommonCode1(@"*?*", out queryRetrievePatientRootInformationModel, out cFindRequest);

            DicomMessageCollection dicomMessageCollection = queryRetrievePatientRootInformationModel.QueryInformationModel(cFindRequest);

            // Expect two messages with status pending and on message with status success.
            Assert.That(dicomMessageCollection.Count, Is.EqualTo(4));
        }

        /// <summary>
        /// Test matching if DICOM matching string contains a regular expression operator.
        /// </summary>
        [Test]
        public void Ticket788_12_1()
        {
            QueryRetrievePatientRootInformationModel queryRetrievePatientRootInformationModel = null;
            DicomMessage cFindRequest = null;

            Ticket788CommonCode1(@"*\*", out queryRetrievePatientRootInformationModel, out cFindRequest);

            DicomMessageCollection dicomMessageCollection = queryRetrievePatientRootInformationModel.QueryInformationModel(cFindRequest);

            // Expect two messages with status pending and on message with status success.
            Assert.That(dicomMessageCollection.Count, Is.EqualTo(3));
        }

        /// <summary>
        /// Test single UID matching on PATIENT level.
        /// </summary>
        [Test]
        public void Ticket788_13_1()
        {
            DicomFile dicomFile = null;

            QueryRetrievePatientRootInformationModel queryRetrievePatientRootInformationModel = new QueryRetrievePatientRootInformationModel();

            dicomFile = new DicomFile();
            dicomFile.DataSet.Set("0x00100020", VR.LO, "1");
            dicomFile.DataSet.Set("0x0020000D", VR.UI, "1.1");
            dicomFile.DataSet.Set("0x0020000E", VR.UI, "1.1.1");
            dicomFile.DataSet.Set("0x00080018", VR.UI, "1.1.1.1");
            queryRetrievePatientRootInformationModel.AddToInformationModel(dicomFile);

            dicomFile = new DicomFile();
            dicomFile.DataSet.Set("0x00100020", VR.LO, "2");
            dicomFile.DataSet.Set("0x0020000D", VR.UI, "2.1");
            dicomFile.DataSet.Set("0x0020000E", VR.UI, "2.1.1");
            dicomFile.DataSet.Set("0x00080018", VR.UI, "2.1.1.1");
            queryRetrievePatientRootInformationModel.AddToInformationModel(dicomFile);

            dicomFile = new DicomFile();
            dicomFile.DataSet.Set("0x00100020", VR.LO, "3");
            dicomFile.DataSet.Set("0x0020000D", VR.UI, "3.1");
            dicomFile.DataSet.Set("0x0020000E", VR.UI, "3.1.1");
            dicomFile.DataSet.Set("0x00080018", VR.UI, "3.1.1.1");
            queryRetrievePatientRootInformationModel.AddToInformationModel(dicomFile);

            DicomMessage cMoveRequest = new DicomMessage(DimseCommand.CMOVERQ);

            cMoveRequest.Set("0x00000002", VR.UI, "1.2.840.10008.5.1.4.1.2.1.2");
            cMoveRequest.Set("0x00000600", VR.AE, "MOVE_DESTINATION");
            cMoveRequest.Set("0x00080052", VR.CS, "PATIENT");
            cMoveRequest.Set("0x00100020", VR.LO, "2");

            DvtkData.Collections.StringCollection fileNames = queryRetrievePatientRootInformationModel.RetrieveInformationModel(cMoveRequest);

            Assert.That(fileNames.Count, Is.EqualTo(1));
        }

        /// <summary>
        /// Test list of UID matching on STUDY level.
        /// </summary>
        [Test]
        public void Ticket788_14_1()
        {
            DicomFile dicomFile = null;

            QueryRetrievePatientRootInformationModel queryRetrievePatientRootInformationModel = new QueryRetrievePatientRootInformationModel();

            dicomFile = new DicomFile();
            dicomFile.DataSet.Set("0x00100020", VR.LO, "1");
            dicomFile.DataSet.Set("0x0020000D", VR.UI, "1.1");
            dicomFile.DataSet.Set("0x0020000E", VR.UI, "1.1.1");
            dicomFile.DataSet.Set("0x00080018", VR.UI, "1.1.1.1");
            queryRetrievePatientRootInformationModel.AddToInformationModel(dicomFile);

            dicomFile = new DicomFile();
            dicomFile.DataSet.Set("0x00100020", VR.LO, "1");
            dicomFile.DataSet.Set("0x0020000D", VR.UI, "1.2");
            dicomFile.DataSet.Set("0x0020000E", VR.UI, "1.2.1");
            dicomFile.DataSet.Set("0x00080018", VR.UI, "1.2.1.1");
            queryRetrievePatientRootInformationModel.AddToInformationModel(dicomFile);

            dicomFile = new DicomFile();
            dicomFile.DataSet.Set("0x00100020", VR.LO, "1");
            dicomFile.DataSet.Set("0x0020000D", VR.UI, "1.3");
            dicomFile.DataSet.Set("0x0020000E", VR.UI, "1.3.1");
            dicomFile.DataSet.Set("0x00080018", VR.UI, "1.3.1.1");
            queryRetrievePatientRootInformationModel.AddToInformationModel(dicomFile);

            DicomMessage cMoveRequest = new DicomMessage(DimseCommand.CMOVERQ);

            cMoveRequest.Set("0x00000002", VR.UI, "1.2.840.10008.5.1.4.1.2.1.2");
            cMoveRequest.Set("0x00000600", VR.AE, "MOVE_DESTINATION");
            cMoveRequest.Set("0x00080052", VR.CS, "STUDY");
            cMoveRequest.Set("0x00100020", VR.LO, "1");
            cMoveRequest.Set("0x0020000D", VR.UI, "1.3", "1.1");

            DvtkData.Collections.StringCollection fileNames = queryRetrievePatientRootInformationModel.RetrieveInformationModel(cMoveRequest);

            Assert.That(fileNames.Count, Is.EqualTo(2));
        }

        /// <summary>
        /// Test list of UID matching on SERIES level.
        /// </summary>
        [Test]
        public void Ticket788_15_1()
        {
            DicomFile dicomFile = null;

            QueryRetrievePatientRootInformationModel queryRetrievePatientRootInformationModel = new QueryRetrievePatientRootInformationModel();

            dicomFile = new DicomFile();
            dicomFile.DataSet.Set("0x00100020", VR.LO, "1");
            dicomFile.DataSet.Set("0x0020000D", VR.UI, "1.1");
            dicomFile.DataSet.Set("0x0020000E", VR.UI, "1.1.1");
            dicomFile.DataSet.Set("0x00080018", VR.UI, "1.1.1.1");
            queryRetrievePatientRootInformationModel.AddToInformationModel(dicomFile);

            dicomFile = new DicomFile();
            dicomFile.DataSet.Set("0x00100020", VR.LO, "1");
            dicomFile.DataSet.Set("0x0020000D", VR.UI, "1.1");
            dicomFile.DataSet.Set("0x0020000E", VR.UI, "1.1.2");
            dicomFile.DataSet.Set("0x00080018", VR.UI, "1.1.2.1");
            queryRetrievePatientRootInformationModel.AddToInformationModel(dicomFile);

            dicomFile = new DicomFile();
            dicomFile.DataSet.Set("0x00100020", VR.LO, "1");
            dicomFile.DataSet.Set("0x0020000D", VR.UI, "1.1");
            dicomFile.DataSet.Set("0x0020000E", VR.UI, "1.1.3");
            dicomFile.DataSet.Set("0x00080018", VR.UI, "1.1.3.1");
            queryRetrievePatientRootInformationModel.AddToInformationModel(dicomFile);

            DicomMessage cMoveRequest = new DicomMessage(DimseCommand.CMOVERQ);

            cMoveRequest.Set("0x00000002", VR.UI, "1.2.840.10008.5.1.4.1.2.1.2");
            cMoveRequest.Set("0x00000600", VR.AE, "MOVE_DESTINATION");
            cMoveRequest.Set("0x00080052", VR.CS, "SERIES");
            cMoveRequest.Set("0x00100020", VR.LO, "1");
            cMoveRequest.Set("0x0020000D", VR.UI, "1.1");
            cMoveRequest.Set("0x0020000E", VR.UI, "1.1.1", "1.1.3");

            DvtkData.Collections.StringCollection fileNames = queryRetrievePatientRootInformationModel.RetrieveInformationModel(cMoveRequest);

            Assert.That(fileNames.Count, Is.EqualTo(2));
        }

        /// <summary>
        /// Test list of UID matching on IMAGE level.
        /// </summary>
        [Test]
        public void Ticket788_16_1()
        {
            DicomFile dicomFile = null;

            QueryRetrievePatientRootInformationModel queryRetrievePatientRootInformationModel = new QueryRetrievePatientRootInformationModel();

            dicomFile = new DicomFile();
            dicomFile.DataSet.Set("0x00100020", VR.LO, "1");
            dicomFile.DataSet.Set("0x0020000D", VR.UI, "1.1");
            dicomFile.DataSet.Set("0x0020000E", VR.UI, "1.1.1");
            dicomFile.DataSet.Set("0x00080018", VR.UI, "1.1.1.1");
            queryRetrievePatientRootInformationModel.AddToInformationModel(dicomFile);

            dicomFile = new DicomFile();
            dicomFile.DataSet.Set("0x00100020", VR.LO, "1");
            dicomFile.DataSet.Set("0x0020000D", VR.UI, "1.1");
            dicomFile.DataSet.Set("0x0020000E", VR.UI, "1.1.1");
            dicomFile.DataSet.Set("0x00080018", VR.UI, "1.1.1.2");
            queryRetrievePatientRootInformationModel.AddToInformationModel(dicomFile);

            dicomFile = new DicomFile();
            dicomFile.DataSet.Set("0x00100020", VR.LO, "1");
            dicomFile.DataSet.Set("0x0020000D", VR.UI, "1.1");
            dicomFile.DataSet.Set("0x0020000E", VR.UI, "1.1.1");
            dicomFile.DataSet.Set("0x00080018", VR.UI, "1.1.1.3");
            queryRetrievePatientRootInformationModel.AddToInformationModel(dicomFile);

            DicomMessage cMoveRequest = new DicomMessage(DimseCommand.CMOVERQ);

            cMoveRequest.Set("0x00000002", VR.UI, "1.2.840.10008.5.1.4.1.2.1.2");
            cMoveRequest.Set("0x00000600", VR.AE, "MOVE_DESTINATION");
            cMoveRequest.Set("0x00080052", VR.CS, "IMAGE");
            cMoveRequest.Set("0x00100020", VR.LO, "1");
            cMoveRequest.Set("0x0020000D", VR.UI, "1.1");
            cMoveRequest.Set("0x0020000E", VR.UI, "1.1.1");
            cMoveRequest.Set("0x00080018", VR.UI, "1.1.1.1", "1.1.1.3");

            DvtkData.Collections.StringCollection fileNames = queryRetrievePatientRootInformationModel.RetrieveInformationModel(cMoveRequest);

            Assert.That(fileNames.Count, Is.EqualTo(2));
        }

        /// <summary>
        /// Returns number of responses.
        /// </summary>
        /// <param name="studyTime"></param>
        /// <param name="StydyTimeRange"></param>
        /// <param name="?"></param>
        /// <returns></returns>
        private int Ticket788CommonCode2
            (String studyTime,
            String StydyTimeRange)
        {
            QueryRetrievePatientRootInformationModel queryRetrievePatientRootInformationModel = new QueryRetrievePatientRootInformationModel();

            DicomFile dicomFile = new DicomFile();
            dicomFile.DataSet.Set("0x00100020", VR.LO, "1");
            dicomFile.DataSet.Set("0x0020000D", VR.UI, "1.1");
            dicomFile.DataSet.Set("0x0020000E", VR.UI, "1.1.1");
            dicomFile.DataSet.Set("0x00080018", VR.UI, "1.1.1.1");

            dicomFile.DataSet.Set("0x00080020", VR.DA, "20090225"); // Study date.
            dicomFile.DataSet.Set("0x00080030", VR.TM, studyTime); // Study time.

            queryRetrievePatientRootInformationModel.AddToInformationModel(dicomFile);

            DicomMessage cFindRequest = new DicomMessage(DimseCommand.CFINDRQ);

            cFindRequest.CommandSet.Set("0x00000002", VR.UI, "1.2.840.10008.5.1.4.1.2.1.1"); // Set Affected SOP class to Patient Root Query/Retrieve Information Model – FIND
            cFindRequest.DataSet.Set("0x00100020", VR.LO, "1"); // Patient ID.
            cFindRequest.DataSet.Set("0x0020000D", VR.UI); // Study instance UID.
            cFindRequest.Set("0x00080030", VR.TM, StydyTimeRange); // Study time.
            cFindRequest.DataSet.Set("0x00080052", VR.CS, "STUDY"); // Query Retrieve Level.

            DicomMessageCollection cFindResponses = queryRetrievePatientRootInformationModel.QueryInformationModel(cFindRequest);

            return (cFindResponses.Count);
        }
    
        /// <summary>
        /// Test matching on a time range:
        /// - Time present in datatset without fraction of seconds.
        /// - Time range in query without fraction of seconds.
        /// </summary>
        [Test]
        public void Ticket788_17_1()
        {
            int numberOfResponses = Ticket788CommonCode2("091349", "091348-091350");

            // Expect one message with status pending and on message with status success.
            Assert.That(numberOfResponses, Is.EqualTo(2));
        }

        /// <summary>
        /// Test matching on a time range:
        /// - Time present in datatset without fraction of seconds.
        /// - Time range in query with fraction of seconds.
        /// </summary>
        [Test]
        public void Ticket788_18_1()
        {
            int numberOfResponses = Ticket788CommonCode2("091349", "091348.005100-091350.005100");

            // Expect one message with status pending and on message with status success.
            Assert.That(numberOfResponses, Is.EqualTo(2));
        }

        /// <summary>
        /// Test matching on a time range:
        /// - Time present in datatset with fraction of seconds.
        /// - Time range in query without fraction of seconds.
        /// </summary>
        [Test]
        public void Ticket788_19_1()
        {
            int numberOfResponses = Ticket788CommonCode2("091349.005100", "091348-091350");

            // Expect one message with status pending and on message with status success.
            Assert.That(numberOfResponses, Is.EqualTo(2));
        }

        /// <summary>
        /// Test matching on a time range:
        /// - Time present in datatset with fraction of seconds.
        /// - Time range in query with fraction of seconds.
        /// </summary>
        [Test]
        public void Ticket788_20_1()
        {
            int numberOfResponses = Ticket788CommonCode2("091349.005200", "091349.005100-091349.005300");

            // Expect one message with status pending and on message with status success.
            Assert.That(numberOfResponses, Is.EqualTo(2));
        }

        /// <summary>
        /// Test matching on a time range with format HHMMSS-.
        /// </summary>
        [Test]
        public void Ticket788_21_1()
        {
            int numberOfResponses = Ticket788CommonCode2("091349", "091349-");

            // Expect one message with status pending and on message with status success.
            Assert.That(numberOfResponses, Is.EqualTo(2));
        }

        /// <summary>
        /// Test matching on a time range with format -HHMMSS.
        /// </summary>
        [Test]
        public void Ticket788_22_1()
        {
            int numberOfResponses = Ticket788CommonCode2("091349", "-091349");

            // Expect one message with status pending and on message with status success.
            Assert.That(numberOfResponses, Is.EqualTo(2));
        }

        /// <summary>
        /// Test no matching on a time range with format HHMMSS-.
        /// </summary>
        [Test]
        public void Ticket788_23_1()
        {
            int numberOfResponses = Ticket788CommonCode2("091349", "091350-");

            // Expect one message with status success.
            Assert.That(numberOfResponses, Is.EqualTo(1));
        }

        /// <summary>
        /// Test no matching on a time range with format -HHMMSS.
        /// </summary>
        [Test]
        public void Ticket788_24_1()
        {
            int numberOfResponses = Ticket788CommonCode2("091349", "-091348");

            // Expect one message status success.
            Assert.That(numberOfResponses, Is.EqualTo(1));
        }

        /// <summary>
        /// Test matching when request does not contain seconds.
        /// </summary>
        [Test]
        public void Ticket788_25_1()
        {
            int numberOfResponses = Ticket788CommonCode2("091300", "0913");

            // Expect one message with status pending and on message with status success.
            Assert.That(numberOfResponses, Is.EqualTo(2));
        }

        /// <summary>
        /// Test matching when attribute does not contain seconds.
        /// </summary>
        [Test]
        public void Ticket788_26_1()
        {
            int numberOfResponses = Ticket788CommonCode2("0913", "091300");

            // Expect one message with status pending and on message with status success.
            Assert.That(numberOfResponses, Is.EqualTo(2));
        }

        /// <summary>
        /// Test matching when request does not contain second fraction.
        /// </summary>
        [Test]
        public void Ticket788_27_1()
        {
            int numberOfResponses = Ticket788CommonCode2("091349.000000", "091349");

            // Expect one message with status pending and on message with status success.
            Assert.That(numberOfResponses, Is.EqualTo(2));
        }

        /// <summary>
        /// Test matching when attribute does not contain second fraction.
        /// </summary>
        [Test]
        public void Ticket788_28_1()
        {
            int numberOfResponses = Ticket788CommonCode2("091349", "091349.000000");

            // Expect one message with status pending and on message with status success.
            Assert.That(numberOfResponses, Is.EqualTo(2));
        }

        /// <summary>
        /// Test matching on series number.
        /// </summary>
        [Test]
        public void Ticket788_29_1()
        {
            DicomFile dicomFile = null;

            QueryRetrievePatientRootInformationModel queryRetrievePatientRootInformationModel = new QueryRetrievePatientRootInformationModel();

            dicomFile = new DicomFile();
            dicomFile.DataSet.Set("0x00100020", VR.LO, "1");
            dicomFile.DataSet.Set("0x0020000D", VR.UI, "1.1");
            dicomFile.DataSet.Set("0x0020000E", VR.UI, "1.1.1");
            dicomFile.DataSet.Set("0x00200011", VR.IS, "-1234567894"); // # Series Number
            dicomFile.DataSet.Set("0x00080018", VR.UI, "1.1.1.1");
            queryRetrievePatientRootInformationModel.AddToInformationModel(dicomFile);

            dicomFile = new DicomFile();
            dicomFile.DataSet.Set("0x00100020", VR.LO, "2");
            dicomFile.DataSet.Set("0x0020000D", VR.UI, "2.1");
            dicomFile.DataSet.Set("0x0020000E", VR.UI, "2.1.1");
            dicomFile.DataSet.Set("0x00200011", VR.IS, "456"); // # Series Number
            dicomFile.DataSet.Set("0x00080018", VR.UI, "2.1.1.1");
            queryRetrievePatientRootInformationModel.AddToInformationModel(dicomFile);

            DicomMessage cFindRequest = new DicomMessage(DimseCommand.CFINDRQ);
            cFindRequest.CommandSet.Set("0x00000002", VR.UI, "1.2.840.10008.5.1.4.1.2.1.1"); // Set Affected SOP class to Patient Root Query/Retrieve Information Model – FIND
            cFindRequest.DataSet.Set("0x00080052", VR.CS, "SERIES"); // Query Retrieve Level.
            cFindRequest.DataSet.Set("0x00100020", VR.LO, "1");
            cFindRequest.DataSet.Set("0x0020000D", VR.UI, "1.1");
            cFindRequest.DataSet.Set("0x0020000E", VR.UI);
            cFindRequest.DataSet.Set("0x00200011", VR.IS, "-1234567894");

            DicomMessageCollection responses = queryRetrievePatientRootInformationModel.QueryInformationModel(cFindRequest);

            // Expect one message with status pending and on message with status success.
            Assert.That(responses.Count, Is.EqualTo(2));
        }

        /// <summary>
        /// Test no matching on series number.
        /// </summary>
        [Test]
        public void Ticket788_30_1()
        {
            DicomFile dicomFile = null;

            QueryRetrievePatientRootInformationModel queryRetrievePatientRootInformationModel = new QueryRetrievePatientRootInformationModel();

            dicomFile = new DicomFile();
            dicomFile.DataSet.Set("0x00100020", VR.LO, "1");
            dicomFile.DataSet.Set("0x0020000D", VR.UI, "1.1");
            dicomFile.DataSet.Set("0x0020000E", VR.UI, "1.1.1");
            dicomFile.DataSet.Set("0x00200011", VR.IS, "-1234567894"); // # Series Number
            dicomFile.DataSet.Set("0x00080018", VR.UI, "1.1.1.1");
            queryRetrievePatientRootInformationModel.AddToInformationModel(dicomFile);

            dicomFile = new DicomFile();
            dicomFile.DataSet.Set("0x00100020", VR.LO, "2");
            dicomFile.DataSet.Set("0x0020000D", VR.UI, "2.1");
            dicomFile.DataSet.Set("0x0020000E", VR.UI, "2.1.1");
            dicomFile.DataSet.Set("0x00200011", VR.IS, "456"); // # Series Number
            dicomFile.DataSet.Set("0x00080018", VR.UI, "2.1.1.1");
            queryRetrievePatientRootInformationModel.AddToInformationModel(dicomFile);

            DicomMessage cFindRequest = new DicomMessage(DimseCommand.CFINDRQ);
            cFindRequest.CommandSet.Set("0x00000002", VR.UI, "1.2.840.10008.5.1.4.1.2.1.1"); // Set Affected SOP class to Patient Root Query/Retrieve Information Model – FIND
            cFindRequest.DataSet.Set("0x00080052", VR.CS, "SERIES"); // Query Retrieve Level.
            cFindRequest.DataSet.Set("0x00100020", VR.LO, "1");
            cFindRequest.DataSet.Set("0x0020000D", VR.UI, "1.1");
            cFindRequest.DataSet.Set("0x0020000E", VR.UI);
            cFindRequest.DataSet.Set("0x00200011", VR.IS, "-999");

            DicomMessageCollection responses = queryRetrievePatientRootInformationModel.QueryInformationModel(cFindRequest);

            // Expect one message with status success.
            Assert.That(responses.Count, Is.EqualTo(1));
        }

        /// <summary>
        /// Test matching on modality.
        /// </summary>
        [Test]
        public void Ticket788_31_1()
        {
            DicomFile dicomFile = null;

            QueryRetrievePatientRootInformationModel queryRetrievePatientRootInformationModel = new QueryRetrievePatientRootInformationModel();

            dicomFile = new DicomFile();
            dicomFile.DataSet.Set("0x00100020", VR.LO, "1");
            dicomFile.DataSet.Set("0x0020000D", VR.UI, "1.1");
            dicomFile.DataSet.Set("0x0020000E", VR.UI, "1.1.1");
            dicomFile.DataSet.Set("0x00080060", VR.CS, "OT"); // # Modality
            dicomFile.DataSet.Set("0x00080018", VR.UI, "1.1.1.1");
            queryRetrievePatientRootInformationModel.AddToInformationModel(dicomFile);

            dicomFile = new DicomFile();
            dicomFile.DataSet.Set("0x00100020", VR.LO, "2");
            dicomFile.DataSet.Set("0x0020000D", VR.UI, "2.1");
            dicomFile.DataSet.Set("0x0020000E", VR.UI, "2.1.1");
            dicomFile.DataSet.Set("0x00080060", VR.CS, "XA"); // # Modality
            dicomFile.DataSet.Set("0x00080018", VR.UI, "2.1.1.1");
            queryRetrievePatientRootInformationModel.AddToInformationModel(dicomFile);

            DicomMessage cFindRequest = new DicomMessage(DimseCommand.CFINDRQ);
            cFindRequest.CommandSet.Set("0x00000002", VR.UI, "1.2.840.10008.5.1.4.1.2.1.1"); // Set Affected SOP class to Patient Root Query/Retrieve Information Model – FIND
            cFindRequest.DataSet.Set("0x00080052", VR.CS, "SERIES"); // Query Retrieve Level.
            cFindRequest.DataSet.Set("0x00100020", VR.LO, "1");
            cFindRequest.DataSet.Set("0x0020000D", VR.UI, "1.1");
            cFindRequest.DataSet.Set("0x0020000E", VR.UI);
            cFindRequest.DataSet.Set("0x00080060", VR.CS, "OT");

            DicomMessageCollection responses = queryRetrievePatientRootInformationModel.QueryInformationModel(cFindRequest);

            // Expect one message with status pending and on message with status success.
            Assert.That(responses.Count, Is.EqualTo(2));
        }

        /// <summary>
        /// Test no matching on modality.
        /// </summary>
        [Test]
        public void Ticket788_32_1()
        {
            DicomFile dicomFile = null;

            QueryRetrievePatientRootInformationModel queryRetrievePatientRootInformationModel = new QueryRetrievePatientRootInformationModel();

            dicomFile = new DicomFile();
            dicomFile.DataSet.Set("0x00100020", VR.LO, "1");
            dicomFile.DataSet.Set("0x0020000D", VR.UI, "1.1");
            dicomFile.DataSet.Set("0x0020000E", VR.UI, "1.1.1");
            dicomFile.DataSet.Set("0x00080060", VR.CS, "OT"); // # Modality
            dicomFile.DataSet.Set("0x00080018", VR.UI, "1.1.1.1");
            queryRetrievePatientRootInformationModel.AddToInformationModel(dicomFile);

            dicomFile = new DicomFile();
            dicomFile.DataSet.Set("0x00100020", VR.LO, "2");
            dicomFile.DataSet.Set("0x0020000D", VR.UI, "2.1");
            dicomFile.DataSet.Set("0x0020000E", VR.UI, "2.1.1");
            dicomFile.DataSet.Set("0x00080060", VR.CS, "XA"); // # Modality
            dicomFile.DataSet.Set("0x00080018", VR.UI, "2.1.1.1");
            queryRetrievePatientRootInformationModel.AddToInformationModel(dicomFile);

            DicomMessage cFindRequest = new DicomMessage(DimseCommand.CFINDRQ);
            cFindRequest.CommandSet.Set("0x00000002", VR.UI, "1.2.840.10008.5.1.4.1.2.1.1"); // Set Affected SOP class to Patient Root Query/Retrieve Information Model – FIND
            cFindRequest.DataSet.Set("0x00080052", VR.CS, "SERIES"); // Query Retrieve Level.
            cFindRequest.DataSet.Set("0x00100020", VR.LO, "1");
            cFindRequest.DataSet.Set("0x0020000D", VR.UI, "1.1");
            cFindRequest.DataSet.Set("0x0020000E", VR.UI);
            cFindRequest.DataSet.Set("0x00080060", VR.CS, "ECG");

            DicomMessageCollection responses = queryRetrievePatientRootInformationModel.QueryInformationModel(cFindRequest);

            // Expect one message with status success.
            Assert.That(responses.Count, Is.EqualTo(1));
        }

        /// <summary>
        /// Test single UID matching on PATIENT level.
        /// </summary>
        [Test]
        public void Ticket1361_1_1()
        {
            DicomFile dicomFile = null;

            QueryRetrievePatientRootInformationModel queryRetrievePatientRootInformationModel = new QueryRetrievePatientRootInformationModel();

            dicomFile = new DicomFile();
            dicomFile.DataSet.Set("0x00100020", VR.LO, "1");
            dicomFile.DataSet.Set("0x0020000D", VR.UI, "1.1");
            dicomFile.DataSet.Set("0x0020000E", VR.UI, "1.1.1");
            dicomFile.DataSet.Set("0x00080018", VR.UI, "1.1.1.1");
            queryRetrievePatientRootInformationModel.AddToInformationModel(dicomFile,false);

            dicomFile = new DicomFile();
            dicomFile.DataSet.Set("0x00100020", VR.LO, "2");
            dicomFile.DataSet.Set("0x0020000D", VR.UI, "2.1");
            dicomFile.DataSet.Set("0x0020000E", VR.UI, "2.1.1");
            dicomFile.DataSet.Set("0x00080018", VR.UI, "2.1.1.1");
            queryRetrievePatientRootInformationModel.AddToInformationModel(dicomFile, false);

            dicomFile = new DicomFile();
            dicomFile.DataSet.Set("0x00100020", VR.LO, "3");
            dicomFile.DataSet.Set("0x0020000D", VR.UI, "3.1");
            dicomFile.DataSet.Set("0x0020000E", VR.UI, "3.1.1");
            dicomFile.DataSet.Set("0x00080018", VR.UI, "3.1.1.1");
            queryRetrievePatientRootInformationModel.AddToInformationModel(dicomFile, false);

            DicomMessage cMoveRequest = new DicomMessage(DimseCommand.CMOVERQ);

            cMoveRequest.Set("0x00000002", VR.UI, "1.2.840.10008.5.1.4.1.2.1.2");
            cMoveRequest.Set("0x00000600", VR.AE, "MOVE_DESTINATION");
            cMoveRequest.Set("0x00080052", VR.CS, "PATIENT");
            cMoveRequest.Set("0x00100020", VR.LO, "2");

            DvtkData.Collections.StringCollection fileNames = queryRetrievePatientRootInformationModel.RetrieveInformationModel(cMoveRequest);

            Assert.That(fileNames.Count, Is.EqualTo(0));
        }

        /// <summary>
        /// Test list of UID matching on STUDY level.
        /// </summary>
        [Test]
        public void Ticket1361_2_1()
        {
            DicomFile dicomFile = null;

            QueryRetrievePatientRootInformationModel queryRetrievePatientRootInformationModel = new QueryRetrievePatientRootInformationModel();

            dicomFile = new DicomFile();
            dicomFile.DataSet.Set("0x00100020", VR.LO, "1");
            dicomFile.DataSet.Set("0x0020000D", VR.UI, "1.1");
            dicomFile.DataSet.Set("0x0020000E", VR.UI, "1.1.1");
            dicomFile.DataSet.Set("0x00080018", VR.UI, "1.1.1.1");
            queryRetrievePatientRootInformationModel.AddToInformationModel(dicomFile, false);

            dicomFile = new DicomFile();
            dicomFile.DataSet.Set("0x00100020", VR.LO, "1");
            dicomFile.DataSet.Set("0x0020000D", VR.UI, "1.2");
            dicomFile.DataSet.Set("0x0020000E", VR.UI, "1.2.1");
            dicomFile.DataSet.Set("0x00080018", VR.UI, "1.2.1.1");
            queryRetrievePatientRootInformationModel.AddToInformationModel(dicomFile, false);

            dicomFile = new DicomFile();
            dicomFile.DataSet.Set("0x00100020", VR.LO, "1");
            dicomFile.DataSet.Set("0x0020000D", VR.UI, "1.3");
            dicomFile.DataSet.Set("0x0020000E", VR.UI, "1.3.1");
            dicomFile.DataSet.Set("0x00080018", VR.UI, "1.3.1.1");
            queryRetrievePatientRootInformationModel.AddToInformationModel(dicomFile, false);

            DicomMessage cMoveRequest = new DicomMessage(DimseCommand.CMOVERQ);

            cMoveRequest.Set("0x00000002", VR.UI, "1.2.840.10008.5.1.4.1.2.1.2");
            cMoveRequest.Set("0x00000600", VR.AE, "MOVE_DESTINATION");
            cMoveRequest.Set("0x00080052", VR.CS, "STUDY");
            cMoveRequest.Set("0x00100020", VR.LO, "1");
            cMoveRequest.Set("0x0020000D", VR.UI, "1.3", "1.1");

            DvtkData.Collections.StringCollection fileNames = queryRetrievePatientRootInformationModel.RetrieveInformationModel(cMoveRequest);

            Assert.That(fileNames.Count, Is.EqualTo(0));
        }

        /// <summary>
        /// Test list of UID matching on SERIES level.
        /// </summary>
        [Test]
        public void Ticket1361_3_1()
        {
            DicomFile dicomFile = null;

            QueryRetrievePatientRootInformationModel queryRetrievePatientRootInformationModel = new QueryRetrievePatientRootInformationModel();

            dicomFile = new DicomFile();
            dicomFile.DataSet.Set("0x00100020", VR.LO, "1");
            dicomFile.DataSet.Set("0x0020000D", VR.UI, "1.1");
            dicomFile.DataSet.Set("0x0020000E", VR.UI, "1.1.1");
            dicomFile.DataSet.Set("0x00080018", VR.UI, "1.1.1.1");
            queryRetrievePatientRootInformationModel.AddToInformationModel(dicomFile, false);

            dicomFile = new DicomFile();
            dicomFile.DataSet.Set("0x00100020", VR.LO, "1");
            dicomFile.DataSet.Set("0x0020000D", VR.UI, "1.1");
            dicomFile.DataSet.Set("0x0020000E", VR.UI, "1.1.2");
            dicomFile.DataSet.Set("0x00080018", VR.UI, "1.1.2.1");
            queryRetrievePatientRootInformationModel.AddToInformationModel(dicomFile, false);

            dicomFile = new DicomFile();
            dicomFile.DataSet.Set("0x00100020", VR.LO, "1");
            dicomFile.DataSet.Set("0x0020000D", VR.UI, "1.1");
            dicomFile.DataSet.Set("0x0020000E", VR.UI, "1.1.3");
            dicomFile.DataSet.Set("0x00080018", VR.UI, "1.1.3.1");
            queryRetrievePatientRootInformationModel.AddToInformationModel(dicomFile, false);

            DicomMessage cMoveRequest = new DicomMessage(DimseCommand.CMOVERQ);

            cMoveRequest.Set("0x00000002", VR.UI, "1.2.840.10008.5.1.4.1.2.1.2");
            cMoveRequest.Set("0x00000600", VR.AE, "MOVE_DESTINATION");
            cMoveRequest.Set("0x00080052", VR.CS, "SERIES");
            cMoveRequest.Set("0x00100020", VR.LO, "1");
            cMoveRequest.Set("0x0020000D", VR.UI, "1.1");
            cMoveRequest.Set("0x0020000E", VR.UI, "1.1.1", "1.1.3");

            DvtkData.Collections.StringCollection fileNames = queryRetrievePatientRootInformationModel.RetrieveInformationModel(cMoveRequest);

            Assert.That(fileNames.Count, Is.EqualTo(0));
        }

        /// <summary>
        /// Test list of UID matching on IMAGE level.
        /// </summary>
        [Test]
        public void Ticket1361_4_1()
        {
            DicomFile dicomFile = null;

            QueryRetrievePatientRootInformationModel queryRetrievePatientRootInformationModel = new QueryRetrievePatientRootInformationModel();

            dicomFile = new DicomFile();
            dicomFile.DataSet.Set("0x00100020", VR.LO, "1");
            dicomFile.DataSet.Set("0x0020000D", VR.UI, "1.1");
            dicomFile.DataSet.Set("0x0020000E", VR.UI, "1.1.1");
            dicomFile.DataSet.Set("0x00080018", VR.UI, "1.1.1.1");
            queryRetrievePatientRootInformationModel.AddToInformationModel(dicomFile, false);

            dicomFile = new DicomFile();
            dicomFile.DataSet.Set("0x00100020", VR.LO, "1");
            dicomFile.DataSet.Set("0x0020000D", VR.UI, "1.1");
            dicomFile.DataSet.Set("0x0020000E", VR.UI, "1.1.1");
            dicomFile.DataSet.Set("0x00080018", VR.UI, "1.1.1.2");
            queryRetrievePatientRootInformationModel.AddToInformationModel(dicomFile, false);

            dicomFile = new DicomFile();
            dicomFile.DataSet.Set("0x00100020", VR.LO, "1");
            dicomFile.DataSet.Set("0x0020000D", VR.UI, "1.1");
            dicomFile.DataSet.Set("0x0020000E", VR.UI, "1.1.1");
            dicomFile.DataSet.Set("0x00080018", VR.UI, "1.1.1.3");
            queryRetrievePatientRootInformationModel.AddToInformationModel(dicomFile, false);

            DicomMessage cMoveRequest = new DicomMessage(DimseCommand.CMOVERQ);

            cMoveRequest.Set("0x00000002", VR.UI, "1.2.840.10008.5.1.4.1.2.1.2");
            cMoveRequest.Set("0x00000600", VR.AE, "MOVE_DESTINATION");
            cMoveRequest.Set("0x00080052", VR.CS, "IMAGE");
            cMoveRequest.Set("0x00100020", VR.LO, "1");
            cMoveRequest.Set("0x0020000D", VR.UI, "1.1");
            cMoveRequest.Set("0x0020000E", VR.UI, "1.1.1");
            cMoveRequest.Set("0x00080018", VR.UI, "1.1.1.1", "1.1.1.3");

            DvtkData.Collections.StringCollection fileNames = queryRetrievePatientRootInformationModel.RetrieveInformationModel(cMoveRequest);

            Assert.That(fileNames.Count, Is.EqualTo(0));
        }
    }
}
