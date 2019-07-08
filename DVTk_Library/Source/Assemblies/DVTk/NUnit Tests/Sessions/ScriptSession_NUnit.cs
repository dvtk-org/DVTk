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
using System.Threading;
using System.ComponentModel;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using NUnit.Framework.SyntaxHelpers;

namespace Dvtk.Sessions
{
    using Dvtk;
    using DvtkData;
    
    /// <summary>
    /// Contains NUnit Test Cases
    /// </summary>
    [TestFixture]
    public class ScriptSession_NUnit
    {
        ScriptSession scriptSession = null;
        string appDirectory = System.Environment.CurrentDirectory;
        string scriptName = "";
        UInt32 nrOfErrors;
        UInt32 nrOfWarnings;
        bool wait = true;
        AsyncCallback theAsyncCallback = null;

        [SetUp]
        public void Init()
        {
            scriptSession = ScriptSession.LoadFromFile(appDirectory + @"\test_validation.ses");
            if (scriptSession != null)
            {
                DirectoryInfo resultDir = new DirectoryInfo(scriptSession.ResultsRootDirectory);
                if (!resultDir.Exists)
                    resultDir.Create();
            }

            theAsyncCallback = new AsyncCallback(this.ResultsFromExecutingScriptAsynchronously);
        }

        [TearDown]
        public void Dispose()
        {
        }

        /// <summary>
        /// Test if the Count property of the DataSet class works correct after adding and deleting attributes.
        /// </summary>
        [Test]
        public void TestCase1()
        {
            scriptName = appDirectory + @"\scripts\val_test_ranges.ds";
            FileInfo info = new FileInfo(scriptName);
            nrOfErrors = 22;
            nrOfWarnings = 23;
            if (scriptSession != null)
            {
                string resultFile = scriptSession.SessionId.ToString("000") + '_' + info.Name + "_res.xml";
                scriptSession.StartResultsGathering(resultFile);
                
                // Perform the actual execution of the script.
                scriptSession.BeginExecuteScript(scriptName, false, theAsyncCallback);
            }

            lock (this)
            {
                wait = true;

                while (wait)
                {
                    if (wait)
                    {
                        System.Threading.Thread.Sleep(500);
                    }
                }
            }

            Assert.That(scriptSession.NrOfValidationErrors, Is.EqualTo(nrOfErrors), "Validation Error:");
            Assert.That(scriptSession.NrOfValidationWarnings, Is.EqualTo(nrOfWarnings), "Validation Warning:");
        }

        [Test]
        public void TestCase2()
        {
            scriptName = appDirectory + @"\scripts\val_test_presence.ds";
            FileInfo info = new FileInfo(scriptName);
            nrOfErrors = 39;
            nrOfWarnings = 0;
            if (scriptSession != null)
            {
                string resultFile = scriptSession.SessionId.ToString("000") + '_' + info.Name + "_res.xml";
                scriptSession.StartResultsGathering(resultFile);

                // Perform the actual execution of the script.
                scriptSession.BeginExecuteScript(scriptName, false, theAsyncCallback);
            }

            lock (this)
            {
                wait = true;

                while (wait)
                {
                    if (wait)
                    {
                        System.Threading.Thread.Sleep(500);
                    }
                }
            }

            Assert.That(scriptSession.NrOfValidationErrors, Is.EqualTo(nrOfErrors), "Validation Error:");
            Assert.That(scriptSession.NrOfValidationWarnings, Is.EqualTo(nrOfWarnings), "Validation Warning:");
        }

        [Test]
        public void TestCase3()
        {
            scriptName = appDirectory + @"\scripts\val_test_conditions.ds";
            FileInfo info = new FileInfo(scriptName);
            nrOfErrors = 25;
            nrOfWarnings = 0;
            if (scriptSession != null)
            {
                string resultFile = scriptSession.SessionId.ToString("000") + '_' + info.Name + "_res.xml";
                scriptSession.StartResultsGathering(resultFile);

                // Perform the actual execution of the script.
                scriptSession.BeginExecuteScript(scriptName, false, theAsyncCallback);
            }

            lock (this)
            {
                wait = true;

                while (wait)
                {
                    if (wait)
                    {
                        System.Threading.Thread.Sleep(500);
                    }
                }
            }

            Assert.That(scriptSession.NrOfValidationErrors, Is.EqualTo(nrOfErrors), "Validation Error:");
            Assert.That(scriptSession.NrOfValidationWarnings, Is.EqualTo(nrOfWarnings), "Validation Warning:");
        }

        [Test]
        public void TestCase4()
        {
            scriptName = appDirectory + @"\scripts\val_test_ref.ds";
            FileInfo info = new FileInfo(scriptName);
            nrOfErrors = 74;
            nrOfWarnings = 0;
            if (scriptSession != null)
            {
                string resultFile = scriptSession.SessionId.ToString("000") + '_' + info.Name + "_res.xml";
                scriptSession.StartResultsGathering(resultFile);

                // Perform the actual execution of the script.
                scriptSession.BeginExecuteScript(scriptName, false, theAsyncCallback);
            }

            lock (this)
            {
                wait = true;

                while (wait)
                {
                    if (wait)
                    {
                        System.Threading.Thread.Sleep(500);
                    }
                }
            }

            Assert.That(scriptSession.NrOfValidationErrors, Is.EqualTo(nrOfErrors), "Validation Error:");
            Assert.That(scriptSession.NrOfValidationWarnings, Is.EqualTo(nrOfWarnings), "Validation Warning:");
        }

        [Test]
        public void TestCase5()
        {
            scriptName = appDirectory + @"\scripts\val_test_sq_nesting.ds";
            FileInfo info = new FileInfo(scriptName);
            nrOfErrors = 14;
            nrOfWarnings = 0;
            if (scriptSession != null)
            {
                string resultFile = scriptSession.SessionId.ToString("000") + '_' + info.Name + "_res.xml";
                scriptSession.StartResultsGathering(resultFile);

                // Perform the actual execution of the script.
                scriptSession.BeginExecuteScript(scriptName, false, theAsyncCallback);
            }

            lock (this)
            {
                wait = true;

                while (wait)
                {
                    if (wait)
                    {
                        System.Threading.Thread.Sleep(500);
                    }
                }
            }

            Assert.That(scriptSession.NrOfValidationErrors, Is.EqualTo(nrOfErrors), "Validation Error:");
            Assert.That(scriptSession.NrOfValidationWarnings, Is.EqualTo(nrOfWarnings), "Validation Warning:");
        }

        [Test]
        public void TestCase6()
        {
            scriptName = appDirectory + @"\scripts\val_test_vm_fail.ds";
            FileInfo info = new FileInfo(scriptName);
            nrOfErrors = 12;
            nrOfWarnings = 0;
            if (scriptSession != null)
            {
                string resultFile = scriptSession.SessionId.ToString("000") + '_' + info.Name + "_res.xml";
                scriptSession.StartResultsGathering(resultFile);

                // Perform the actual execution of the script.
                scriptSession.BeginExecuteScript(scriptName, false, theAsyncCallback);
            }

            lock (this)
            {
                wait = true;

                while (wait)
                {
                    if (wait)
                    {
                        System.Threading.Thread.Sleep(500);
                    }
                }
            }

            Assert.That(scriptSession.NrOfValidationErrors, Is.EqualTo(nrOfErrors), "Validation Error:");
            Assert.That(scriptSession.NrOfValidationWarnings, Is.EqualTo(nrOfWarnings), "Validation Warning:");
        }

        [Test]
        public void TestCase7()
        {
            scriptName = appDirectory + @"\scripts\val_test_vm_pass.ds";
            FileInfo info = new FileInfo(scriptName);
            nrOfErrors = 0;
            nrOfWarnings = 0;
            if (scriptSession != null)
            {
                string resultFile = scriptSession.SessionId.ToString("000") + '_' + info.Name + "_res.xml";
                scriptSession.StartResultsGathering(resultFile);

                // Perform the actual execution of the script.
                scriptSession.BeginExecuteScript(scriptName, false, theAsyncCallback);
            }

            lock (this)
            {
                wait = true;

                while (wait)
                {
                    if (wait)
                    {
                        System.Threading.Thread.Sleep(500);
                    }
                }
            }

            Assert.That(scriptSession.NrOfValidationErrors, Is.EqualTo(nrOfErrors), "Validation Error:");
            Assert.That(scriptSession.NrOfValidationWarnings, Is.EqualTo(nrOfWarnings), "Validation Warning:");
        }

        [Test]
        public void TestCase8()
        {
            scriptName = appDirectory + @"\scripts\val_test_vr_fail.ds";
            FileInfo info = new FileInfo(scriptName);
            nrOfErrors = 66;
            nrOfWarnings = 2;
            if (scriptSession != null)
            {
                string resultFile = scriptSession.SessionId.ToString("000") + '_' + info.Name + "_res.xml";
                scriptSession.StartResultsGathering(resultFile);

                // Perform the actual execution of the script.
                scriptSession.BeginExecuteScript(scriptName, false, theAsyncCallback);
            }

            lock (this)
            {
                wait = true;

                while (wait)
                {
                    if (wait)
                    {
                        System.Threading.Thread.Sleep(500);
                    }
                }
            }

            Assert.That(scriptSession.NrOfValidationErrors, Is.EqualTo(nrOfErrors), "Validation Error:");
            Assert.That(scriptSession.NrOfValidationWarnings, Is.EqualTo(nrOfWarnings), "Validation Warning:");
        }

        [Test]
        public void TestCase9()
        {
            scriptName = appDirectory + @"\scripts\val_test_vr_pass.ds";
            FileInfo info = new FileInfo(scriptName);
            nrOfErrors = 1;
            nrOfWarnings = 0;
            if (scriptSession != null)
            {
                string resultFile = scriptSession.SessionId.ToString("000") + '_' + info.Name + "_res.xml";
                scriptSession.StartResultsGathering(resultFile);

                // Perform the actual execution of the script.
                scriptSession.BeginExecuteScript(scriptName, false, theAsyncCallback);
            }

            lock (this)
            {
                wait = true;

                while (wait)
                {
                    if (wait)
                    {
                        System.Threading.Thread.Sleep(500);
                    }
                }
            }

            Assert.That(scriptSession.NrOfValidationErrors, Is.EqualTo(nrOfErrors), "Validation Error:");
            Assert.That(scriptSession.NrOfValidationWarnings, Is.EqualTo(nrOfWarnings), "Validation Warning:");
        }

        private void ResultsFromExecutingScriptAsynchronously(IAsyncResult theIAsyncResult)
        {
            try
            {
                // Obligated to call the following method according to the asynchronous design pattern.
                scriptSession.EndExecuteScript(theIAsyncResult);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            scriptSession.EndResultsGathering();

            wait = false;
        }
    }
}