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
using NUnit.Framework.SyntaxHelpers;

using DvtkHighLevelInterface.Dicom.Other;



namespace DvtkHighLevelInterface.Common.Compare
{
    /// <summary>
    /// Contains NUnit Test Cases.
    /// </summary>
    [TestFixture]
    public class CompareBase_NUnit
    {
        //
        // - Fields -
        //

        /// <summary>
        /// Test methods can use this field to reduce the method length.
        /// </summary>
        CompareBase compareBase = null;



        //
        // - Methods containing common functionality for all test methods -
        //

        /// <summary>
        /// This method is performed just before each test method is called.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.compareBase = new CompareBase();
        }

        /// <summary>
        /// This method is performed after each test method is run.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            this.compareBase = null;
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
        ///     Testcase description...
        /// </summary>
        [Test]
        public void AddEmptyRowAfterEachDynamicComparedList()
        {
            compareBase.AddEmptyRowAfterEachDynamicComparedList = false;
            compareBase.AddEmptyRowAfterEachDynamicComparedList = true;
        }

        /// <summary>
        ///     Testcase description...
        /// </summary>
        [Test]
        public void DisplayAttributeName()
        {
            compareBase.DisplayAttributeName = true;
            Assert.That(compareBase.DisplayAttributeName, Is.True);

            compareBase.DisplayAttributeName = false;
            Assert.That(compareBase.DisplayAttributeName, Is.False);
        }

        /// <summary>
        ///     Testcase description...
        /// </summary>
        [Test]
        public void DisplayAttributePresent()
        {
            compareBase.DisplayAttributePresent = true;
            Assert.That(compareBase.DisplayAttributePresent, Is.True);

            compareBase.DisplayAttributePresent = false;
            Assert.That(compareBase.DisplayAttributePresent, Is.False);
        }

        /// <summary>
        ///     Testcase description...
        /// </summary>
        [Test]
        public void DisplayAttributeTag()
        {
            compareBase.DisplayAttributeTag = true;
            Assert.That(compareBase.DisplayAttributeTag, Is.True);

            compareBase.DisplayAttributeTag = false;
            Assert.That(compareBase.DisplayAttributeTag, Is.False);
        }

        /// <summary>
        ///     Testcase description...
        /// </summary>
        [Test]
        public void DisplayAttributeValues()
        {
            compareBase.DisplayAttributeValues = true;
            Assert.That(compareBase.DisplayAttributeValues, Is.True);

            compareBase.DisplayAttributeValues = false;
            Assert.That(compareBase.DisplayAttributeValues, Is.False);
        }

        /// <summary>
        ///     Testcase description...
        /// </summary>
        [Test]
        public void DisplayAttributeVR()
        {
            compareBase.DisplayAttributeVR = true;
            Assert.That(compareBase.DisplayAttributeVR, Is.True);

            compareBase.DisplayAttributeVR = false;
            Assert.That(compareBase.DisplayAttributeVR, Is.False);
        }

        /// <summary>
        ///     Testcase description...
        /// </summary>
        [Test]
        public void DisplayComments()
        {
            compareBase.DisplayComments = true;
            Assert.That(compareBase.DisplayComments, Is.True);

            compareBase.DisplayComments = false;
            Assert.That(compareBase.DisplayComments, Is.False);
        }

        /// <summary>
        ///     Testcase description...
        /// </summary>
        [Test]
        public void DisplayCommonName()
        {
            compareBase.DisplayCommonName = true;
            Assert.That(compareBase.DisplayCommonName, Is.True);

            compareBase.DisplayCommonName = false;
            Assert.That(compareBase.DisplayCommonName, Is.False);
        }

        /// <summary>
        ///     Testcase description...
        /// </summary>
        [Test]
        public void DisplayCommonTag()
        {
            compareBase.DisplayCommonTag = true;
            Assert.That(compareBase.DisplayCommonTag, Is.True);

            compareBase.DisplayCommonTag = false;
            Assert.That(compareBase.DisplayCommonTag, Is.False);
        }

        /// <summary>
        ///     Testcase description...
        /// </summary>
        [Test]
        public void DisplayCompareValueType()
        {
            compareBase.DisplayCompareValueType = true;
            Assert.That(compareBase.DisplayCompareValueType, Is.True);

            compareBase.DisplayCompareValueType = false;
            Assert.That(compareBase.DisplayCompareValueType, Is.False);
        }

        /// <summary>
        ///     Testcase description...
        /// </summary>
        [Test]
        public void DisplayFlags()
        {
            compareBase.DisplayFlags = true;
            Assert.That(compareBase.DisplayFlags, Is.True);

            compareBase.DisplayFlags = false;
            Assert.That(compareBase.DisplayFlags, Is.False);
        }

        /// <summary>
        ///     Testcase description...
        /// </summary>
        [Test]
        public void DisplayGroupLength()
        {
            compareBase.DisplayGroupLength = true;
            Assert.That(compareBase.DisplayGroupLength, Is.True);

            compareBase.DisplayGroupLength = false;
            Assert.That(compareBase.DisplayGroupLength, Is.False);
        }

        /// <summary>
        ///     Testcase description...
        /// </summary>
        [Test]
        public void Equals_object()
        {
            Assert.That(compareBase.Equals(compareBase), Is.True);
        }

        /// <summary>
        ///     Testcase description...
        /// </summary>
        [Test]
        public void GetHashCode_()
        {
            int hashCode = compareBase.GetHashCode();
        }

        /// <summary>
        ///     Testcase description...
        /// </summary>
        [Test]
        public void ToString_()
        {
            Assert.That(compareBase.ToString(), Is.TypeOf(typeof(string)));
        }
    }
}
