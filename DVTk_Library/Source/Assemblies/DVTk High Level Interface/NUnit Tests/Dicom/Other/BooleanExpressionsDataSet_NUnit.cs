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
using BDataSet = DvtkHighLevelInterface.Common.Other.GenericBooleanExpression<DvtkHighLevelInterface.Dicom.Other.DataSet, DvtkHighLevelInterface.Dicom.Other.DataSetCollection>;
using DvtkHighLevelInterface.Common.Other;
using DvtkHighLevelInterface.Dicom.Messages;



namespace DvtkHighLevelInterface.Dicom.Other
{
    /// <summary>
    /// Contains NUnit Test Cases.
    /// </summary>
    [TestFixture]
    public class BooleanExpressionsDataSet_NUnit
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



        //
        // - Methods containing common functionality for all test methods -
        //

        /// <summary>
        /// This method is performed just before each test method is called.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            // Do nothing.
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

            this.dataSet1 = new DataSet();
            this.dataSet1.Set("0x00200020", VR.LT, "Long text value");

            this.dataSet2 = new DataSet();
            this.dataSet2.Set("0x00300030", VR.LT, "Long text value");
            this.dataSet2.Set("0x00400040", VR.LT, "Long text value");
        }

        /// <summary>
        /// This method is performed once after all tests are completed in this
        /// class.
        /// </summary>
        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            this.dataSet1 = null;
            this.dataSet2 = null;

            Dvtk.Setup.Terminate();
        }



        //
        // - Test methods -
        //

        /// <summary>
        /// Tests if the ContainsAttribute boolean expression works when the attribute is indeed present.
        /// </summary>
        [Test]
        public void ContainsAttribute_1_1()
        {
            Assert.That(BooleanExpressionDataSet.ContainsAttribute("0x00200020").Evaluate(this.dataSet1), Is.True);
        }

        /// <summary>
        /// Tests if the ContainsAttribute boolean expression works when the attribute is not present.
        /// </summary>
        [Test]
        public void ContainsAttribute_2_1()
        {
            Assert.That(BooleanExpressionDataSet.ContainsAttribute("0x00300030").Evaluate(this.dataSet1), Is.False);
        }

        /// <summary>
        /// Tests if the logical NOT operator on a boolean expression works when the attribute is present.
        /// </summary>
        [Test]
        public void Not_1_1()
        {
            Assert.That((!BooleanExpressionDataSet.ContainsAttribute("0x00200020")).Evaluate(this.dataSet1), Is.False);
        }

        /// <summary>
        /// Tests if the logical NOT operator on a boolean expression works when the attribute is not present.
        /// </summary>
        [Test]
        public void Not_2_1()
        {
            Assert.That((!BooleanExpressionDataSet.ContainsAttribute("0x00300030")).Evaluate(this.dataSet1), Is.True);
        }

        /// <summary>
        /// Tests if the logical OR operator on two boolean expression works when the first boolean expression is true,
        /// written in one line.
        /// </summary>
        [Test]
        public void Or_1_1()
        {
            Assert.That(( (BooleanExpressionDataSet.ContainsAttribute("0x00300030")) | (BooleanExpressionDataSet.ContainsAttribute("0x00500050"))).Evaluate(this.dataSet2), Is.True);
        }

        /// <summary>
        /// Tests if the logical OR operator on two boolean expression works when the second boolean expression is true,
        /// written in one line.
        /// </summary>
        [Test]
        public void Or_2_1()
        {
            Assert.That(((BooleanExpressionDataSet.ContainsAttribute("0x00500050")) | (BooleanExpressionDataSet.ContainsAttribute("0x00400040"))).Evaluate(this.dataSet2), Is.True);
        }

        /// <summary>
        /// Tests if the logical OR operator on two boolean expression works when both boolean expressions are true,
        /// written in one line.
        /// </summary>
        [Test]
        public void Or_3_1()
        {
            Assert.That(((BooleanExpressionDataSet.ContainsAttribute("0x00300030")) | (BooleanExpressionDataSet.ContainsAttribute("0x00400040"))).Evaluate(this.dataSet2), Is.True);
        }

        /// <summary>
        /// Tests if the logical OR operator on two boolean expression works when both boolean expressions are false,
        /// written in one line.
        /// </summary>
        [Test]
        public void Or_4_1()
        {
            Assert.That(((BooleanExpressionDataSet.ContainsAttribute("0x00500050")) | (BooleanExpressionDataSet.ContainsAttribute("0x00600060"))).Evaluate(this.dataSet2), Is.False);
        }

        /// <summary>
        /// Tests if the logical OR operator on two boolean expression works when the first boolean expression is true,
        /// written in multiple lines.
        /// </summary>
        [Test]
        public void OrMultipleLines_1_1()
        {
            BDataSet booleanExpressionDataSet1 = BooleanExpressionDataSet.ContainsAttribute("0x00300030");
            BDataSet booleanExpressionDataSet2 = BooleanExpressionDataSet.ContainsAttribute("0x00500050");

            Assert.That((booleanExpressionDataSet1 | booleanExpressionDataSet2).Evaluate(this.dataSet2), Is.True);
        }

        /// <summary>
        /// Tests if the logical OR operator on two boolean expression works when the second boolean expression is true,
        /// written in multiple lines.
        /// </summary>
        [Test]
        public void OrMultipleLines_2_1()
        {
            BDataSet booleanExpressionDataSet1 = BooleanExpressionDataSet.ContainsAttribute("0x00500050");
            BDataSet booleanExpressionDataSet2 = BooleanExpressionDataSet.ContainsAttribute("0x00400040");

            Assert.That((booleanExpressionDataSet1 | booleanExpressionDataSet2).Evaluate(this.dataSet2), Is.True);
        }

        /// <summary>
        /// Tests if the logical OR operator on two boolean expression works when both boolean expressions are true,
        /// written in multiple lines.
        /// </summary>
        [Test]
        public void OrMultipleLines_3_1()
        {
            BDataSet booleanExpressionDataSet1 = BooleanExpressionDataSet.ContainsAttribute("0x00300030");
            BDataSet booleanExpressionDataSet2 = BooleanExpressionDataSet.ContainsAttribute("0x00400040");

            Assert.That((booleanExpressionDataSet1 | booleanExpressionDataSet2).Evaluate(this.dataSet2), Is.True);
        }

        /// <summary>
        /// Tests if the logical OR operator on two boolean expression works when both boolean expressions are false,
        /// written in multiple lines.
        /// </summary>
        [Test]
        public void OrMultipleLines_4_1()
        {
            BDataSet bDataSet1 = BooleanExpressionDataSet.ContainsAttribute("0x00500050");
            BDataSet bDataSet2 = BooleanExpressionDataSet.ContainsAttribute("0x00600060");

            Assert.That((bDataSet1 | bDataSet2).Evaluate(this.dataSet2), Is.False);
        }

        /// <summary>
        /// Tests if the logical OR operator on three boolean expression works when all boolean expressions are false,
        /// written in multiple lines and using all possible combination.
        /// </summary>
        [Test]
        public void OrMultipleLines_5_1()
        {
            BDataSet bDataSet1 = BooleanExpressionDataSet.ContainsAttribute("0x00500050");
            BDataSet bDataSet2 = BooleanExpressionDataSet.ContainsAttribute("0x00600060");
            BDataSet bDataSet3 = BooleanExpressionDataSet.ContainsAttribute("0x00700070");

            BDataSet bDataSetOr = bDataSet1 | bDataSet2 | bDataSet3;

            Assert.That(bDataSetOr.Evaluate(this.dataSet2), Is.False);
        }

        /// <summary>
        /// Tests if the logical AND operator on two boolean expression works when the first boolean expression is true,
        /// written in one line.
        /// </summary>
        [Test]
        public void And_1_1()
        {
            Assert.That(((BooleanExpressionDataSet.ContainsAttribute("0x00300030")) & (BooleanExpressionDataSet.ContainsAttribute("0x00500050"))).Evaluate(this.dataSet2), Is.False);
        }

        /// <summary>
        /// Tests if the logical AND operator on two boolean expression works when the second boolean expression is true,
        /// written in one line.
        /// </summary>
        [Test]
        public void And_2_1()
        {
            Assert.That(((BooleanExpressionDataSet.ContainsAttribute("0x00500050")) & (BooleanExpressionDataSet.ContainsAttribute("0x00400040") )).Evaluate(this.dataSet2), Is.False);
        }

        /// <summary>
        /// Tests if the logical AND operator on two boolean expression works when both boolean expressions are true,
        /// written in one line.
        /// </summary>
        [Test]
        public void And_3_1()
        {
            Assert.That(((BooleanExpressionDataSet.ContainsAttribute("0x00300030")) & (BooleanExpressionDataSet.ContainsAttribute("0x00400040")) & (BooleanExpressionDataSet.ContainsAttribute("0x00400040"))).Evaluate(this.dataSet2), Is.True);
        }

        /// <summary>
        /// Tests if the logical AND operator on two boolean expression works when both boolean expressions are false,
        /// written in one line.
        /// </summary>
        [Test]
        public void And_4_1()
        {
            Assert.That(((BooleanExpressionDataSet.ContainsAttribute("0x00500050")) & (BooleanExpressionDataSet.ContainsAttribute("0x00600060"))).Evaluate(this.dataSet2), Is.False);
        }

        [Test]
        public void DataSetCollectionEvaluate_1_1()
        {
            //
            // SetUp the test data for this test case.
            //

            DicomMessageCollection dicomMessageCollection = new DicomMessageCollection();
            
            DicomMessage dicomMessage1 = new DicomMessage(DimseCommand.CSTORERQ);
            dicomMessage1.DataSet.Set("0x00200020", VR.LT, "Long text value");
            dicomMessageCollection.Add(dicomMessage1);

            DicomMessage dicomMessage2 = new DicomMessage(DimseCommand.CSTORERQ);
            dicomMessage2.DataSet.Set("0x00300030", VR.LT, "Long text value");
            dicomMessageCollection.Add(dicomMessage2);


            //
            // Perform the actual test.
            //

            DataSetCollection dataSetCollection = BooleanExpressionDataSet.ContainsAttribute("0x00200020").Evaluate(dicomMessageCollection.DataSets);

            Assert.That(dataSetCollection.Count, Is.EqualTo(1));
        }

        [Test]
        public void DataSetCollectionEvaluate_2_1()
        {
            //
            // SetUp the test data for this test case.
            //

            DicomMessageCollection dicomMessageCollection1 = new DicomMessageCollection();
            DicomMessageCollection dicomMessageCollection2 = new DicomMessageCollection();

            DicomMessage dicomMessage1 = new DicomMessage(DimseCommand.CSTORERQ);
            dicomMessage1.DataSet.Set("0x00200020", VR.UI, "1.1.1.1");
            dicomMessageCollection1.Add(dicomMessage1);

            DicomMessage dicomMessage2 = new DicomMessage(DimseCommand.CSTORERQ);
            dicomMessage2.DataSet.Set("0x00300030", VR.UI, "1.1.1.1");
            dicomMessageCollection2.Add(dicomMessage2);

            DicomMessage dicomMessage3 = new DicomMessage(DimseCommand.CSTORERQ);
            dicomMessage3.DataSet.Set("0x00300030", VR.UI, "2.2.2.2");
            dicomMessageCollection2.Add(dicomMessage3);

            DicomMessage dicomMessage4 = new DicomMessage(DimseCommand.CSTORERQ);
            dicomMessage4.DataSet.Set("0x00300030", VR.UI, "3.3.3.3");
            dicomMessageCollection2.Add(dicomMessage4);

            //
            // Perform the actual test.
            //

            GenericCollection<GenericPair<DataSet, DataSet>> collection = BooleanExpressionTwoDataSets.MapsAttributes("0x00200020", "0x00300030").Evaluate(dicomMessageCollection1.DataSets, dicomMessageCollection2.DataSets);

            Assert.That(collection.Count, Is.EqualTo(1));
        }

        [Test]
        public void DataSetCollectionEvaluate_2_2()
        {
            //
            // SetUp the test data for this test case.
            //

            DicomMessageCollection dicomMessageCollection1 = new DicomMessageCollection();
            DicomMessageCollection dicomMessageCollection2 = new DicomMessageCollection();

            DicomMessage dicomMessage1 = new DicomMessage(DimseCommand.CSTORERQ);
            dicomMessage1.DataSet.Set("0x00200020", VR.UI, "1.1.1.1");
            dicomMessageCollection1.Add(dicomMessage1);

            DicomMessage dicomMessage2 = new DicomMessage(DimseCommand.CSTORERQ);
            dicomMessage2.DataSet.Set("0x00300030", VR.UI, "1.1.1.1");
            dicomMessage2.DataSet.Set("0x00400040", VR.LT, "dicomMessage2");
            dicomMessageCollection2.Add(dicomMessage2);

            DicomMessage dicomMessage3 = new DicomMessage(DimseCommand.CSTORERQ);
            dicomMessage3.DataSet.Set("0x00300030", VR.UI, "2.2.2.2");
            dicomMessage3.DataSet.Set("0x00400040", VR.LT, "dicomMessage3");
            dicomMessageCollection2.Add(dicomMessage3);

            DicomMessage dicomMessage4 = new DicomMessage(DimseCommand.CSTORERQ);
            dicomMessage4.DataSet.Set("0x00300030", VR.UI, "3.3.3.3");
            dicomMessage4.DataSet.Set("0x00400040", VR.LT, "dicomMessage4");
            dicomMessageCollection2.Add(dicomMessage4);

            //
            // Perform the actual test.
            //

            GenericCollection<GenericPair<DataSet, DataSet>> collection = BooleanExpressionTwoDataSets.MapsAttributes("0x00200020", "0x00300030").Evaluate(dicomMessageCollection1.DataSets, dicomMessageCollection2.DataSets);

            Assert.That(collection[0].Element2["0x00400040"].Values[0], Is.EqualTo("dicomMessage2"));
        }        
    }
}
