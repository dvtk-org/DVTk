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
using DvtkHighLevelInterface.Common.Threads;
using DvtkHighLevelInterface.Dicom.Threads;
using DvtkHighLevelInterface.NUnit;



namespace DvtkHighLevelInterface.Dicom.Threads
{
    /// <summary>
    /// Contains NUnit Test Cases.
    /// </summary>
    [TestFixture]
    public class DicomThreadOptions_NUnit
    {
        //
        // - Fields -
        //

        /// <summary>
        /// Used for all Ticket1210_1 tests.
        /// </summary>
        SCP ticket1210_1_DicomThread = null;



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

        private void Ticket1210_1()
        {
            if (this.ticket1210_1_DicomThread == null)
            {
                ThreadManager threadManager = new ThreadManager();
                SCP dicomThread = new SCP();
                dicomThread.Initialize(threadManager);

                dicomThread.Options.LocalAeTitle = "1";
                dicomThread.Options.LocalImplementationClassUid = "2";
                dicomThread.Options.LocalImplementationVersionName = "3";
                dicomThread.Options.LocalMaximumLength = 4;
                dicomThread.Options.LocalPort = 5;
                dicomThread.Options.RemoteAeTitle = "6";
                dicomThread.Options.RemoteHostName = "7";
                dicomThread.Options.RemoteImplementationClassUid = "8";
                dicomThread.Options.RemoteImplementationVersionName = "9";
                dicomThread.Options.RemoteMaximumLength = 10;
                dicomThread.Options.RemotePort = 11;
                dicomThread.Options.DefinitionFileApplicationEntityName = "12";
                dicomThread.Options.DefinitionFileApplicationEntityVersion = "13";

                String sessionFullPath = Path.Combine(Paths.DataDirectoryFullPath, "Ticket1210_1.ses");

                dicomThread.Options.SaveToFile(sessionFullPath);

                this.ticket1210_1_DicomThread = new SCP();
                this.ticket1210_1_DicomThread.Initialize(threadManager);

                this.ticket1210_1_DicomThread.Options.LoadFromFile(sessionFullPath);
            }
        }

        /// <summary>
        /// Test setting an option, saving to session file, loading from session file and getting the option.
        /// </summary>
        [Test]
        public void Ticket1210_1_01()
        {
            Ticket1210_1();

            Assert.That(this.ticket1210_1_DicomThread.Options.LocalAeTitle, Is.EqualTo("1"));
        }

        /// <summary>
        /// Test setting an option, saving to session file, loading from session file and getting the option.
        /// </summary>
        [Test]
        public void Ticket1210_1_02()
        {
            Ticket1210_1();

            Assert.That(this.ticket1210_1_DicomThread.Options.LocalImplementationClassUid, Is.EqualTo("2"));
        }

        /// <summary>
        /// Test setting an option, saving to session file, loading from session file and getting the option.
        /// </summary>
        [Test]
        public void Ticket1210_1_03()
        {
            Ticket1210_1();

            Assert.That(this.ticket1210_1_DicomThread.Options.LocalImplementationVersionName, Is.EqualTo("3"));
        }

        /// <summary>
        /// Test setting an option, saving to session file, loading from session file and getting the option.
        /// </summary>
        [Test]
        public void Ticket1210_1_04()
        {
            Ticket1210_1();

            Assert.That(this.ticket1210_1_DicomThread.Options.LocalMaximumLength, Is.EqualTo(4));
        }

        /// <summary>
        /// Test setting an option, saving to session file, loading from session file and getting the option.
        /// </summary>
        [Test]
        public void Ticket1210_1_05()
        {
            Ticket1210_1();

            Assert.That(this.ticket1210_1_DicomThread.Options.LocalPort, Is.EqualTo(5));
        }

        /// <summary>
        /// Test setting an option, saving to session file, loading from session file and getting the option.
        /// </summary>
        [Test]
        public void Ticket1210_1_06()
        {
            Ticket1210_1();

            Assert.That(this.ticket1210_1_DicomThread.Options.RemoteAeTitle, Is.EqualTo("6"));
        }

        /// <summary>
        /// Test setting an option, saving to session file, loading from session file and getting the option.
        /// </summary>
        [Test]
        public void Ticket1210_1_07()
        {
            Ticket1210_1();

            Assert.That(this.ticket1210_1_DicomThread.Options.RemoteHostName, Is.EqualTo("7"));
        }

        /// <summary>
        /// Test setting an option, saving to session file, loading from session file and getting the option.
        /// </summary>
        [Test]
        public void Ticket1210_1_08()
        {
            Ticket1210_1();

            Assert.That(this.ticket1210_1_DicomThread.Options.RemoteImplementationClassUid, Is.EqualTo("8"));
        }

        /// <summary>
        /// Test setting an option, saving to session file, loading from session file and getting the option.
        /// </summary>
        [Test]
        public void Ticket1210_1_09()
        {
            Ticket1210_1();

            Assert.That(this.ticket1210_1_DicomThread.Options.RemoteImplementationVersionName, Is.EqualTo("9"));
        }

        /// <summary>
        /// Test setting an option, saving to session file, loading from session file and getting the option.
        /// </summary>
        [Test]
        public void Ticket1210_1_10()
        {
            Ticket1210_1();

            Assert.That(this.ticket1210_1_DicomThread.Options.RemoteMaximumLength, Is.EqualTo(10));
        }

        /// <summary>
        /// Test setting an option, saving to session file, loading from session file and getting the option.
        /// </summary>
        [Test]
        public void Ticket1210_1_11()
        {
            Ticket1210_1();

            Assert.That(this.ticket1210_1_DicomThread.Options.RemotePort, Is.EqualTo(11));
        }

        /// <summary>
        /// Test setting an option, saving to session file, loading from session file and getting the option.
        /// </summary>
        [Test]
        public void Ticket1210_1_12()
        {
            Ticket1210_1();

            Assert.That(this.ticket1210_1_DicomThread.Options.DefinitionFileApplicationEntityName, Is.EqualTo("12"));
        }

        /// <summary>
        /// Test setting an option, saving to session file, loading from session file and getting the option.
        /// </summary>
        [Test]
        public void Ticket1210_1_13()
        {
            Ticket1210_1();

            Assert.That(this.ticket1210_1_DicomThread.Options.DefinitionFileApplicationEntityVersion, Is.EqualTo("13"));
        }
    }
}
