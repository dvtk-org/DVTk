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
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

using Tag = DvtkData.Dimse.Tag;
using VR = DvtkData.Dimse.VR;
using Dvtk.Sessions;
using DvtkHighLevelInterface.Dicom.Threads;
using DvtkHighLevelInterface.NUnit;



namespace DvtkHighLevelInterface.Dicom.Files
{
    /// <summary>
    /// Contains NUnit Test Cases.
    /// </summary>
    [TestFixture]
    public class DicomFile_NUnit
    {
        //
        // - Fields -
        //

        /// <summary>
        /// Test methods can use this field to reduce the method length.
        /// </summary>
        DicomFile dicomFile = null;



        //
        // - Methods containing common functionality for all test methods -
        //

        /// <summary>
        /// This method is performed just before each test method is called.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.dicomFile = new DicomFile();
        }

        /// <summary>
        /// This method is performed after each test method is run.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            this.dicomFile = null;
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
        ///     Test group length setting
        /// </summary>
        [Test]
        public void AddGroupLength()
        {
            // initial value
            Assert.That(dicomFile.AddGroupLength, Is.False);

            // test true
            dicomFile.AddGroupLength = true;
            Assert.That(dicomFile.AddGroupLength, Is.True);

            // test false
            dicomFile.AddGroupLength = false;
            Assert.That(dicomFile.AddGroupLength, Is.False);
        }

        /// <summary>
        ///     Testcase description...
        /// </summary>
        [Test]
        public void DataSet()
        {
            dicomFile.DataSet = new DvtkHighLevelInterface.Dicom.Other.DataSet(); 
            Assert.That(dicomFile.DataSet.Count, Is.EqualTo(0));
        }

        /// <summary>
        ///     Test store OB/OF/OW value setting
        /// </summary>
        [Test]
        public void StoreOBOFOWValuesWhenReading()
        {
            // initial value
            Assert.That(dicomFile.StoreOBOFOWValuesWhenReading, Is.True);

            // test false
            dicomFile.StoreOBOFOWValuesWhenReading = false;
            Assert.That(dicomFile.StoreOBOFOWValuesWhenReading, Is.False);

            // test true
            dicomFile.StoreOBOFOWValuesWhenReading = true;
            Assert.That(dicomFile.StoreOBOFOWValuesWhenReading, Is.True);
        }

        /// <summary>
        ///     Testcase description...
        /// </summary>
        [Test]
        public void Summary()
        {
            Assert.That(dicomFile.Summary, Is.TypeOf(typeof(string)) & Is.Not.Empty);
        }

        /// <summary>
        ///     Test Vr of UN definition look-up setting
        /// </summary>
        [Test]
        public void UnVrDefinitionLookUpWhenReading()
        {
            // initial value
            Assert.That(dicomFile.UnVrDefinitionLookUpWhenReading, Is.True);

            // test false
            dicomFile.UnVrDefinitionLookUpWhenReading = false;
            Assert.That(dicomFile.UnVrDefinitionLookUpWhenReading, Is.False);

            // test true
            dicomFile.UnVrDefinitionLookUpWhenReading = true;
            Assert.That(dicomFile.UnVrDefinitionLookUpWhenReading, Is.True);
        }

        // =======================
        // Public Instance Methods
        // =======================

        /// <summary>
        ///     Testcase description...
        /// </summary>
        [Test]
        public void DumpUsingVisualBasicNotation_objectName()
        {
            string objectName = "";

            Assert.That(dicomFile.DumpUsingVisualBasicNotation(objectName), Is.TypeOf(typeof(string)) & Is.Not.Empty);
        }

        /// <summary>
        ///     Testcase description...
        /// </summary>
        [Test]
        public void Equals_object()
        {
            Assert.That(dicomFile.Equals(dicomFile), Is.True);
        }

        /// <summary>
        ///     Testcase description...
        /// </summary>
        [Test]
        public void GetHashCode_()
        {
            int hashCode = dicomFile.GetHashCode();
        }

        // TODO: add extra check in Read method to test if parameters are non-empty.
        //       after this change, adjust this test.
        ///// <summary>
        /////     Testcase description...
        ///// </summary>
        //[Test]
        //public void Read_fullFileName_definitionFilesFullName()
        //{
        //    String fullFileName = "";
        //    String definitionFilesFullName = "";

        //    dicomFile.Read(fullFileName, definitionFilesFullName);
        //}

        // TODO: add extra check in Read method to test if parameters are non-empty.
        //       after this change, adjust this test.
        ///// <summary>
        /////     Testcase description...
        ///// </summary>
        //[Test]
        //public void Read_fullFileName_dicomThread()
        //{
        //    String fullFileName = "";
        //    DicomThread dicomThread = null;

        //    dicomFile.Read(fullFileName, dicomThread);
        //}

        // TODO: add extra check in Set method to check for uninitialized DvtkData.Dimse.Tag instance (if possible).
        ///// <summary>
        /////     Testcase description...
        ///// </summary>
        //[Test]
        //public void Set_dvtkDataTag_vR_parameters()
        //{
        //    DvtkData.Dimse.Tag dvtkDataTag = new DvtkData.Dimse.Tag();
        //    VR vR = VR.UN;
        //    Object[] parameters = null;

        //    dicomFile.Set(dvtkDataTag, vR, parameters);
        //}

        /// <summary>
        ///     Testcase description...
        /// </summary>
        [Test]
        public void Set_tagSequenceString_vR_parameters()
        {
            bool exceptionThrown = false;

            String tagSequenceString = "";
            VR vR = VR.UN;
            Object[] parameters = null;

            try
            {
                dicomFile.Set(tagSequenceString, vR, parameters);
            }
            catch
            {
                exceptionThrown = true;
            }

            Assert.That(exceptionThrown, Is.EqualTo(true));
        }

        /// <summary>
        ///     Testcase description...
        /// </summary>
        [Test]
        public void ToString_()
        {
            Assert.That(dicomFile.ToString(), Is.TypeOf(typeof(string)));
        }

        // TODO: change Write method to check for parameters. After this, change test.
        ///// <summary>
        /////     Testcase description...
        ///// </summary>
        //[Test]
        //public void Write_fullFileName()
        //{
        //    String fullFileName = "";

        //    dicomFile.Write(fullFileName);    // exception!
        //}

        /// <summary>
        /// Test the usage of an Byte array in the Set method, by comparing the size of two DICOM files that are written.
        /// </summary>
        [Test]
        public void Ticket740_1_1()
        {
            Byte[] value1 = new Byte[4] { 0, 1, 2, 3 };
            Byte[] value2 = new Byte[8] { 0, 1, 2, 3, 4, 5, 6, 7 };

            DicomFile dicomFile1 = new DicomFile();
            DicomFile dicomFile2 = new DicomFile();

            dicomFile1.DataSet.Set("0x00400040", VR.OW, value1);
            dicomFile2.DataSet.Set("0x00400040", VR.OW, value2);

            String dicomFile1FullPath = Path.Combine(Paths.DataDirectoryFullPath, "Ticket740_1_1_1.DCM");
            String dicomFile2FullPath = Path.Combine(Paths.DataDirectoryFullPath, "Ticket740_1_1_2.DCM");

            dicomFile1.Write(dicomFile1FullPath);
            dicomFile2.Write(dicomFile2FullPath);

            FileInfo fileInfo1 = new FileInfo(dicomFile1FullPath);
            FileInfo fileInfo2 = new FileInfo(dicomFile2FullPath);

            Assert.That(fileInfo1.Length + 4, Is.EqualTo(fileInfo2.Length));
        }

        /// <summary>
        /// Test the usage of an Byte array in the Set method, by comparing the size of two DICOM files that are written.
        /// </summary>
        [Test]
        public void Ticket740_2_1()
        {
            Byte[] value1 = new Byte[2] { 0, 1};
            Byte[] value2 = new Byte[8] { 0, 1, 2, 3, 4, 5, 6, 7 };

            DicomFile dicomFile1 = new DicomFile();
            DicomFile dicomFile2 = new DicomFile();

            dicomFile1.DataSet.Set(Tag.PIXEL_DATA, VR.OB, value1);
            dicomFile2.DataSet.Set(Tag.PIXEL_DATA, VR.OW, value2);

            String dicomFile1FullPath = Path.Combine(Paths.DataDirectoryFullPath, "Ticket740_2_1_1.DCM");
            String dicomFile2FullPath = Path.Combine(Paths.DataDirectoryFullPath, "Ticket740_2_1_2.DCM");

            dicomFile1.Write(dicomFile1FullPath);
            dicomFile2.Write(dicomFile2FullPath);

            FileInfo fileInfo1 = new FileInfo(dicomFile1FullPath);
            FileInfo fileInfo2 = new FileInfo(dicomFile2FullPath);

            Assert.That(fileInfo1.Length + 6, Is.EqualTo(fileInfo2.Length));
        }
    }
}
