//// ------------------------------------------------------
//// DVTk - The Healthcare Validation Toolkit (www.dvtk.org)
//// Copyright © 2009 DVTk
//// ------------------------------------------------------
//// This file is part of DVTk.
////
//// DVTk is free software; you can redistribute it and/or modify it under the terms of the GNU
//// Lesser General Public License as published by the Free Software Foundation; either version 3.0
//// of the License, or (at your option) any later version. 
//// 
//// DVTk is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even
//// the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser
//// General Public License for more details. 
//// 
//// You should have received a copy of the GNU Lesser General Public License along with this
//// library; if not, see <http://www.gnu.org/licenses/>



//using System;
//using NUnit.Framework;
//using NUnit.Framework.SyntaxHelpers;

//using DvtkHighLevelInterface.Common.Other;
//using DvtkHighLevelInterface.Dicom.Other;



//namespace DvtkHighLevelInterface.Common.Compare
//{
//    /// <summary>
//    ///  Contains NUnit Test Cases.
//    /// </summary>
//    [TestFixture]
//    public class CompareResults_NUnit
//    {
//        //
//        // - Fields -
//        //

//        /// <summary>
//        /// Test methods can use this field to reduce the method length.
//        /// </summary>
//        CompareResults compareResults = null;



//        //
//        // - Methods containing common functionality for all test methods -
//        //

//        /// <summary>
//        /// This method is performed just before each test method is called.
//        /// </summary>
//        [SetUp]
//        public void SetUp()
//        {
//            this.compareResults = new CompareResults(5);
//        }

//        /// <summary>
//        /// This method is performed after each test method is run.
//        /// </summary>
//        [TearDown]
//        public void TearDown()
//        {
//            this.compareResults = null;
//        }

//        /// <summary>
//        /// This method is performed once prior to executing any of the tests
//        /// in this class.
//        /// </summary>
//        [TestFixtureSetUp]
//        public void TestFixtureSetUp()
//        {
//            Dvtk.Setup.Initialize();
//        }

//        /// <summary>
//        /// This method is performed once after all tests are completed in this
//        /// class.
//        /// </summary>
//        [TestFixtureTearDown]
//        public void TestFixtureTearDown()
//        {
//            Dvtk.Setup.Terminate();
//        }



//        //
//        // - Test methods -
//        //

//        /// <summary>
//        ///     Testcase description...
//        /// </summary>
//        [Test]
//        public void DifferencesCount()
//        {
//            int maxDifferencesCount = 65536;

//            Assert.That(compareResults.DifferencesCount, Is.EqualTo(0));

//            compareResults.DifferencesCount = maxDifferencesCount;
//            Assert.That(compareResults.DifferencesCount, Is.EqualTo(maxDifferencesCount));
//        }

//        /// <summary>
//        ///     Testcase description...
//        /// </summary>
//        [Test]
//        public void Table()
//        {
//            Assert.That(compareResults.Table, Is.InstanceOfType(typeof(Table)));
//        }

//        /// <summary>
//        ///     Testcase description...
//        /// </summary>
//        [Test]
//        public void Equals_object()
//        {
//            Assert.That(compareResults.Equals(compareResults), Is.True);
//        }

//        /// <summary>
//        ///     Testcase description...
//        /// </summary>
//        [Test]
//        public void GetHashCode_()
//        {
//            int hashCode = compareResults.GetHashCode();
//        }

//        /// <summary>
//        ///     Testcase description...
//        /// </summary>
//        [Test]
//        public void ToString_()
//        {
//            Assert.That(compareResults.ToString(), Is.TypeOf(typeof(string)));
//        }
//    }
//}
