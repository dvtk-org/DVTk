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



namespace DvtkHighLevelInterface.Common.Compare
{
    /// <summary>
    ///     Contains NUnit Test Cases.
    /// </summary>
    [TestFixture]
    public class CompareRule_NUnit
    {
        //
        // - Fields -
        //

        /// <summary>
        /// Test methods can use this field to reduce the method length.
        /// </summary>
        CompareRule compareRule = null;



        //
        // - Methods containing common functionality for all test methods -
        //

        /// <summary>
        /// This method is performed just before each test method is called.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.compareRule = new CompareRule();
        }

        /// <summary>
        /// This method is performed after each test method is run.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            this.compareRule = null;
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
        public void ArrayRepresentation()
        {
            Assert.That(compareRule.ArrayRepresentation, Is.TypeOf(typeof(ValidationRuleBase[])));
            Assert.That(compareRule.ArrayRepresentation, Is.Empty);

            // TODO: test code below fails. Improve interface (description)?
            //compareRule.ArrayRepresentation = null; // exception!
            //Assert.That(compareRule.ArrayRepresentation, Is.TypeOf(typeof(ValidationRuleBase)));
            //Assert.That(compareRule.ArrayRepresentation, Is.Null);
        }

        /// <summary>
        ///     Testcase description...
        /// </summary>
        [Test]
        public void CompareValueType()
        {
            Assert.That(compareRule.CompareValueType, Is.EqualTo(CompareValueTypes.Identical));

            compareRule.CompareValueType = CompareValueTypes.Date;
            Assert.That(compareRule.CompareValueType, Is.EqualTo(CompareValueTypes.Date));

            compareRule.CompareValueType = CompareValueTypes.ID;
            Assert.That(compareRule.CompareValueType, Is.EqualTo(CompareValueTypes.ID));

            compareRule.CompareValueType = CompareValueTypes.Name;
            Assert.That(compareRule.CompareValueType, Is.EqualTo(CompareValueTypes.Name));

            compareRule.CompareValueType = CompareValueTypes.String;
            Assert.That(compareRule.CompareValueType, Is.EqualTo(CompareValueTypes.String));

            compareRule.CompareValueType = CompareValueTypes.Time;
            Assert.That(compareRule.CompareValueType, Is.EqualTo(CompareValueTypes.Time));

            compareRule.CompareValueType = CompareValueTypes.UID;
            Assert.That(compareRule.CompareValueType, Is.EqualTo(CompareValueTypes.UID));
        }

        /// <summary>
        ///     Testcase description...
        /// </summary>
        [Test]
        public void ConditionText()
        {
            string conditionText = "Compare Rule Condition Text";

            Assert.That(compareRule.ConditionText, Is.Empty);

            compareRule.ConditionText = conditionText;
            Assert.That(compareRule.ConditionText, Is.EqualTo(conditionText));
        }

        /// <summary>
        ///     Test initial index Count
        /// </summary>
        [Test]
        public void Count()
        {
            Assert.That(compareRule.Count, Is.EqualTo(0));
        }

        /// <summary>
        ///     Testcase description...
        /// </summary>
        [Test]
        public void Add_validationRule()
        {
            ValidationRuleBase validationRule = null;
            int count = compareRule.Count;

            compareRule.Add(validationRule);
            Assert.That(count + 1, Is.EqualTo(compareRule.Count));
        }

        /// <summary>
        ///     Testcase description...
        /// </summary>
        [Test]
        public void Equals_object()
        {
            Assert.That(compareRule.Equals(compareRule), Is.True);
        }
    }
}
