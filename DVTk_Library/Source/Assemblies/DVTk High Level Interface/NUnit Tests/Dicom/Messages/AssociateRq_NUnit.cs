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
using System.Windows.Forms;

using NUnit.Framework;
using NUnit.Framework.Constraints;
using NUnit.Framework.SyntaxHelpers;

using VR = DvtkData.Dimse.VR;
using DimseCommand = DvtkData.Dimse.DimseCommand;
using DvtkHighLevelInterface.Common.Threads;
using DvtkHighLevelInterface.Dicom.Other;
using DvtkHighLevelInterface.Dicom.Threads;
using DvtkHighLevelInterface.NUnit;



namespace DvtkHighLevelInterface.Dicom.Messages
{
    /// <summary>
    /// Contains NUnit Test Cases.
    /// </summary>
    [TestFixture]
    public class AssociateRq_NUnit
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

        private AssociateRq ticket1283_1_ReceivedAssociateRq = null;

        private void Ticket1283_1()
        {
            if (ticket1283_1_ReceivedAssociateRq == null)
            {
                ThreadManager threadManager = new ThreadManager();

                DicomThread_NUnit.ScpAssociationOnly scpAssociationOnly = new DicomThread_NUnit.ScpAssociationOnly("1.2.840.10008.5.1.4.1.1.7", "1.2.840.10008.1.2.1");

                scpAssociationOnly.Initialize(threadManager);

                Config.SetOptions(scpAssociationOnly, "ticket1283_1", "scp");

                DicomThread_NUnit.ScuAssociationOnly scuAssociationOnly = new DicomThread_NUnit.ScuAssociationOnly("1.2.840.10008.5.1.4.1.1.7", "1.2.840.10008.1.2.1");

                scuAssociationOnly.Initialize(threadManager);

                Config.SetOptions(scuAssociationOnly, "ticket1283_1", "scu");
                scuAssociationOnly.Options.LocalAeTitle = "CALLINGAETITLE1";
                scuAssociationOnly.Options.RemoteAeTitle = "CALLEDAETITLE1";

                scpAssociationOnly.Start();
                scuAssociationOnly.Start(250);

                scpAssociationOnly.WaitForCompletion();
                scuAssociationOnly.WaitForCompletion();

                ticket1283_1_ReceivedAssociateRq = scpAssociationOnly.Messages.DulMessages[0] as AssociateRq;
            }
        }

        /// <summary>
        /// Test the calling and called AE title properties for the AssociateRq class.
        /// </summary>
        [Test]
        public void Ticket1283_1_1()
        {
            Ticket1283_1();

            Assert.That(this.ticket1283_1_ReceivedAssociateRq.CallingAETitle.TrimEnd(' '), Is.EqualTo("CALLINGAETITLE1"));
        }

        /// <summary>
        /// Test the calling and called AE title properties for the AssociateRq class.
        /// </summary>
        [Test]
        public void Ticket1283_1_2()
        {
            Ticket1283_1();

            Assert.That(this.ticket1283_1_ReceivedAssociateRq.CalledAETitle.TrimEnd(' '), Is.EqualTo("CALLEDAETITLE1"));
        }
    }
}