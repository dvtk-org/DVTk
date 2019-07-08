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
using DvtkHighLevelInterface.Dicom.Files;



namespace DvtkHighLevelInterface.Dicom.Other
{
    /// <summary>
    /// Contains NUnit Test Cases.
    /// </summary>
    [TestFixture]
    public class DataSet_NUnit
    {
        //
        // - Fields -
        //

        /// <summary>
        /// Test methods can use this field to reduce the method length.
        /// </summary>
        private DataSet dataSet1 = null;

        /// <summary>
        /// Test methods can use this field to reduce the method length.
        /// </summary>
        private DataSet dataSet2 = null;

        /// <summary>
        /// Test methods can use this field to reduce the method length.
        /// </summary>
        private DataSet dataSet3 = null;



        //
        // - Methods containing common functionality for all test methods -
        //

        /// <summary>
        /// This method is performed just before each test method is called.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.dataSet1 = new DataSet();
            this.dataSet2 = new DataSet();
            this.dataSet3 = new DataSet();
        }

        /// <summary>
        /// This method is performed after each test method is run.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            this.dataSet1 = null;
            this.dataSet2 = null;
            this.dataSet3 = null;
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
        /// Test the usage of a Values instance in the Set method.
        /// </summary>
        [Test]
        public void Ticket689_1_1()
        {
            dataSet1.Set("0x00400040", VR.LO, "Value 1", "Value 2");
            dataSet2.Set("0x00500050", VR.LO, dataSet1["0x00400040"].Values);

            Assert.That(dataSet2["0x00500050"].Values.Count, Is.EqualTo(2));
        }

        /// <summary>
        /// Test the usage of a Values instance in the Set method.
        /// </summary>
        [Test]
        public void Ticket689_1_2()
        {
            dataSet1.Set("0x00400040", VR.LO, "Value 1", "Value 2");
            dataSet2.Set("0x00500050", VR.LO, dataSet1["0x00400040"].Values);

            Assert.That(dataSet2["0x00500050"].Values[0], Is.EqualTo("Value 1"));
        }

        /// <summary>
        /// Test the usage of a Values instance in the Set method.
        /// </summary>
        [Test]
        public void Ticket689_1_3()
        {
            dataSet1.Set("0x00400040", VR.LO, "Value 1", "Value 2");
            dataSet2.Set("0x00500050", VR.LO, dataSet1["0x00400040"].Values);

            Assert.That(dataSet2["0x00500050"].Values[1], Is.EqualTo("Value 2"));
        }

        /// <summary>
        /// Test the usage of "normal" parameters in the Set method.
        /// </summary>
        [Test]
        public void Ticket689_2_1()
        {
            dataSet1.Set("0x00400040", VR.LO, "Value 1", "Value 2");

            Assert.That(dataSet1["0x00400040"].Values.Count, Is.EqualTo(2));
        }

        /// <summary>
        /// Test the usage of "normal" parameters in the Set method.
        /// </summary>
        [Test]
        public void Ticket689_2_2()
        {
            dataSet1.Set("0x00400040", VR.LO, "Value 1", "Value 2");

            Assert.That(dataSet1["0x00400040"].Values[0], Is.EqualTo("Value 1"));
        }

        /// <summary>
        /// Test the usage of "normal" parameters in the Set method.
        /// </summary>
        [Test]        
        public void Ticket689_2_3()
        {
            dataSet1.Set("0x00400040", VR.LO, "Value 1", "Value 2");

            Assert.That(dataSet1["0x00400040"].Values[1], Is.EqualTo("Value 2"));
        }
    }
}