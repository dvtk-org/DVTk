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

using Dvtk.Hl7.Messages;
using DvtkHighLevelInterface.Dicom.Other;



namespace DvtkHighLevelInterface.Common.Compare
{
    /// <summary>
    /// Contains NUnit Test Cases.
    /// </summary>
    [TestFixture]
    public class AttributeCollections_NUnit
    {
        //
        // - Fields -
        //

        /// <summary>
        /// Test methods can use this field to reduce the method length.
        /// </summary>
        AttributeCollections attributeCollections = null;



        //
        // - Methods containing common functionality for all test methods -
        //

        /// <summary>
        /// This method is performed just before each test method is called.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.attributeCollections = new AttributeCollections();
        }

        /// <summary>
        /// This method is performed after each test method is run.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            this.attributeCollections = null;
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
        ///     Test initial index Count
        /// </summary>
        [Test]
        public void Count()
        {
            Assert.That(attributeCollections.Count, Is.EqualTo(0));
        }

        /// <summary>
        ///     Testcase description...
        /// </summary>
        [Test]
        public void NotNullCount()
        {
            Assert.That(attributeCollections.Count, Is.EqualTo(attributeCollections.NotNullCount));
        }

        /// <summary>
        ///     Testcase description...
        /// </summary>
        [Test]
        public void Add_attributeSet()
        {
            AttributeSet attributeSet = null;
            int count = attributeCollections.Count;

            attributeCollections.Add(attributeSet);
            Assert.That(count + 1, Is.EqualTo(attributeCollections.Count));
        }

        /// <summary>
        ///     Testcase description...
        /// </summary>
        [Test]
        public void Add_attributeSet_flags()
        {
            AttributeSet attributeSet = null;
            FlagsDicomAttribute flags = new FlagsDicomAttribute();
            int count = attributeCollections.Count;

            attributeCollections.Add(attributeSet, flags);
            Assert.That(count + 1, Is.EqualTo(attributeCollections.Count));
        }

        /// <summary>
        ///     Testcase description...
        /// </summary>
        [Test]
        public void Add_hl7Message()
        {
            Hl7Message hl7Message = new Hl7Message();
            int count = attributeCollections.Count;

            attributeCollections.Add(hl7Message);
            Assert.That(count + 1, Is.EqualTo(attributeCollections.Count));
        }

        /// <summary>
        ///     Testcase description...
        /// </summary>
        [Test]
        public void Add_hl7Message_flags()
        {
            FlagsHl7Attribute flags = new FlagsHl7Attribute();
            Hl7Message hl7Message = new Hl7Message();
            int count = attributeCollections.Count;

            attributeCollections.Add(hl7Message, flags);
            Assert.That(count + 1, Is.EqualTo(attributeCollections.Count));
        }

        /// <summary>
        ///     Testcase description...
        /// </summary>
        [Test]
        public void AddNull()
        {
            int count = attributeCollections.Count;

            attributeCollections.AddNull();
            Assert.That(count + 1, Is.EqualTo(attributeCollections.Count));
        }

        /// <summary>
        ///     Testcase description...
        /// </summary>
        [Test]
        public void Clear()
        {
            attributeCollections.Clear();
        }

        /// <summary>
        ///     Testcase description...
        /// </summary>
        [Test]
        public void DicomMakeAscending()
        {
            attributeCollections.DicomMakeAscending();
        }

        /// <summary>
        ///     Testcase description...
        /// </summary>
        [Test]
        public void Equals_object()
        {
            Assert.That(attributeCollections.Equals(attributeCollections), Is.True);
        }

        // Incorrect test, AttributeCollections is empty.
        ///// <summary>
        /////     Testcase description...
        ///// </summary>
        //[Test]
        //public void GetAttributeSet_index()
        //{
        //    int index = 0;
        //    AttributeSet attributeSet = AttributeCollections.GetAttributeSet(index);
        //    Assert.That(attributeSet, Is.InstanceOfType(typeof(AttributeSet)));
        //}

        // Incorrect test, AttributeCollections is empty.
        ///// <summary>
        /////     Testcase description...
        ///// </summary>
        //[Test]
        //public void GetHl7Message_index()
        //{
        //    int index = 0;
        //    Hl7Message hl7Message = AttributeCollections.GetHl7Message(index);
        //    Assert.That(hl7Message, Is.InstanceOfType(typeof(Hl7Message)));
        //}

        // Incorrect test, AttributeCollections is empty.
        ///// <summary>
        /////     Testcase description...
        ///// </summary>
        //[Test]
        //public void IsAttributeSet_index()
        //{
        //    int index = 0;

        //    Assert.That(AttributeCollections.IsAttributeSet(index), Is.False);
        //}

        // Incorrect test, AttributeCollections is empty.
        ///// <summary>
        /////     Testcase description...
        ///// </summary>
        //[Test]
        //public void IsHl7Message_index()
        //{
        //    int index = 0;

        //    Assert.That(AttributeCollections.IsHl7Message(index), Is.False);
        //}

        // Incorrect test, AttributeCollections is empty.
        ///// <summary>
        /////     Testcase description...
        ///// </summary>
        //[Test]
        //public void IsNull_index()
        //{
        //    int index = 0;

        //    Assert.That(AttributeCollections.IsNull(index), Is.True);
        //}
    }
}
