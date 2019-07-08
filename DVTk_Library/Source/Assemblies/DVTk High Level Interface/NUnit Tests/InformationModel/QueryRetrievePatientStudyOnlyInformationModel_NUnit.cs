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

using Dimse = DvtkData.Dimse.DimseCommand;
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
    public class QueryRetrieveStudyRootInformationModel_NUnit
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
        /// Test list of UID matching on STUDY level.
        /// </summary>
        [Test]
        public void Ticket788_1_1()
        {
            DicomFile dicomFile = null;

            QueryRetrieveStudyRootInformationModel queryRetrieveStudyRootInformationModel = new QueryRetrieveStudyRootInformationModel();

            dicomFile = new DicomFile();
            dicomFile.DataSet.Set("0x00100020", VR.LO, "1");
            dicomFile.DataSet.Set("0x0020000D", VR.UI, "1.1");
            dicomFile.DataSet.Set("0x0020000E", VR.UI, "1.1.1");
            dicomFile.DataSet.Set("0x00080018", VR.UI, "1.1.1.1");
            queryRetrieveStudyRootInformationModel.AddToInformationModel(dicomFile);

            dicomFile = new DicomFile();
            dicomFile.DataSet.Set("0x00100020", VR.LO, "1");
            dicomFile.DataSet.Set("0x0020000D", VR.UI, "1.2");
            dicomFile.DataSet.Set("0x0020000E", VR.UI, "1.2.1");
            dicomFile.DataSet.Set("0x00080018", VR.UI, "1.2.1.1");
            queryRetrieveStudyRootInformationModel.AddToInformationModel(dicomFile);

            dicomFile = new DicomFile();
            dicomFile.DataSet.Set("0x00100020", VR.LO, "1");
            dicomFile.DataSet.Set("0x0020000D", VR.UI, "1.3");
            dicomFile.DataSet.Set("0x0020000E", VR.UI, "1.3.1");
            dicomFile.DataSet.Set("0x00080018", VR.UI, "1.3.1.1");
            queryRetrieveStudyRootInformationModel.AddToInformationModel(dicomFile);

            DicomMessage cMoveRequest = new DicomMessage(Dimse.CMOVERQ);

            cMoveRequest.Set("0x00000002", VR.UI, "1.2.840.10008.5.1.4.1.2.2.2");
            cMoveRequest.Set("0x00000600", VR.AE, "MOVE_DESTINATION");
            cMoveRequest.Set("0x00080052", VR.CS, "STUDY");
            cMoveRequest.Set("0x0020000D", VR.UI, "1.3", "1.1");

            DvtkData.Collections.StringCollection fileNames = queryRetrieveStudyRootInformationModel.RetrieveInformationModel(cMoveRequest);

            Assert.That(fileNames.Count, Is.EqualTo(2));
        }

        /// <summary>
        /// Test list of UID matching on SERIES level.
        /// </summary>
        [Test]
        public void Ticket788_2_1()
        {
            DicomFile dicomFile = null;

            QueryRetrieveStudyRootInformationModel queryRetrieveStudyRootInformationModel = new QueryRetrieveStudyRootInformationModel();

            dicomFile = new DicomFile();
            dicomFile.DataSet.Set("0x00100020", VR.LO, "1");
            dicomFile.DataSet.Set("0x0020000D", VR.UI, "1.1");
            dicomFile.DataSet.Set("0x0020000E", VR.UI, "1.1.1");
            dicomFile.DataSet.Set("0x00080018", VR.UI, "1.1.1.1");
            queryRetrieveStudyRootInformationModel.AddToInformationModel(dicomFile);

            dicomFile = new DicomFile();
            dicomFile.DataSet.Set("0x00100020", VR.LO, "1");
            dicomFile.DataSet.Set("0x0020000D", VR.UI, "1.1");
            dicomFile.DataSet.Set("0x0020000E", VR.UI, "1.1.2");
            dicomFile.DataSet.Set("0x00080018", VR.UI, "1.1.2.1");
            queryRetrieveStudyRootInformationModel.AddToInformationModel(dicomFile);

            dicomFile = new DicomFile();
            dicomFile.DataSet.Set("0x00100020", VR.LO, "1");
            dicomFile.DataSet.Set("0x0020000D", VR.UI, "1.1");
            dicomFile.DataSet.Set("0x0020000E", VR.UI, "1.1.3");
            dicomFile.DataSet.Set("0x00080018", VR.UI, "1.1.3.1");
            queryRetrieveStudyRootInformationModel.AddToInformationModel(dicomFile);

            DicomMessage cMoveRequest = new DicomMessage(Dimse.CMOVERQ);

            cMoveRequest.Set("0x00000002", VR.UI, "1.2.840.10008.5.1.4.1.2.2.2");
            cMoveRequest.Set("0x00000600", VR.AE, "MOVE_DESTINATION");
            cMoveRequest.Set("0x00080052", VR.CS, "SERIES");
            cMoveRequest.Set("0x0020000D", VR.UI, "1.1");
            cMoveRequest.Set("0x0020000E", VR.UI, "1.1.1", "1.1.3");

            DvtkData.Collections.StringCollection fileNames = queryRetrieveStudyRootInformationModel.RetrieveInformationModel(cMoveRequest);

            Assert.That(fileNames.Count, Is.EqualTo(2));
        }

        /// <summary>
        /// Test list of UID matching on IMAGE level.
        /// </summary>
        [Test]
        public void Ticket788_3_1()
        {
            DicomFile dicomFile = null;

            QueryRetrieveStudyRootInformationModel queryRetrieveStudyRootInformationModel = new QueryRetrieveStudyRootInformationModel();

            dicomFile = new DicomFile();
            dicomFile.Set("0x00100020", VR.LO, "1");
            dicomFile.Set("0x0020000D", VR.UI, "1.1");
            dicomFile.Set("0x0020000E", VR.UI, "1.1.1");
            dicomFile.Set("0x00080018", VR.UI, "1.1.1.1");
            queryRetrieveStudyRootInformationModel.AddToInformationModel(dicomFile);

            dicomFile = new DicomFile();
            dicomFile.Set("0x00100020", VR.LO, "1");
            dicomFile.Set("0x0020000D", VR.UI, "1.1");
            dicomFile.Set("0x0020000E", VR.UI, "1.1.1");
            dicomFile.Set("0x00080018", VR.UI, "1.1.1.2");
            queryRetrieveStudyRootInformationModel.AddToInformationModel(dicomFile);

            dicomFile = new DicomFile();
            dicomFile.Set("0x00100020", VR.LO, "1");
            dicomFile.Set("0x0020000D", VR.UI, "1.1");
            dicomFile.Set("0x0020000E", VR.UI, "1.1.1");
            dicomFile.Set("0x00080018", VR.UI, "1.1.1.3");
            queryRetrieveStudyRootInformationModel.AddToInformationModel(dicomFile);

            DicomMessage cMoveRequest = new DicomMessage(Dimse.CMOVERQ);

            cMoveRequest.Set("0x00000002", VR.UI, "1.2.840.10008.5.1.4.1.2.2.2");
            cMoveRequest.Set("0x00000600", VR.AE, "MOVE_DESTINATION");
            cMoveRequest.Set("0x00080052", VR.CS, "IMAGE");
            cMoveRequest.Set("0x0020000D", VR.UI, "1.1");
            cMoveRequest.Set("0x0020000E", VR.UI, "1.1.1");
            cMoveRequest.Set("0x00080018", VR.UI, "1.1.1.1", "1.1.1.3");

            DvtkData.Collections.StringCollection fileNames = queryRetrieveStudyRootInformationModel.RetrieveInformationModel(cMoveRequest);

            Assert.That(fileNames.Count, Is.EqualTo(2));
        }
    }
}
