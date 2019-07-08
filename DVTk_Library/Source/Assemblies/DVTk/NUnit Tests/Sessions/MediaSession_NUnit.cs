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
using NUnit.Framework;
using NUnit.Framework.Constraints;
using NUnit.Framework.SyntaxHelpers;

namespace Dvtk.Sessions
{
    using Dvtk;
    using DvtkData.Media;  
    
    /// <summary>
    /// Contains NUnit Test Cases
    /// </summary>
    [TestFixture]
    public class MediaSession_NUnit
    {
        MediaSession mediaSession = null;
        Thread thread = null;
        bool ok = false;
        
        [SetUp]
        public void Init()
        {
            mediaSession = MediaSession.LoadFromFile(System.Environment.CurrentDirectory + "\\Media.ses");
            if (mediaSession != null)
            {
                DirectoryInfo resultDir = new DirectoryInfo(mediaSession.ResultsRootDirectory);
                if (!resultDir.Exists)
                    resultDir.Create();
            }
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
            if (mediaSession != null)
            {
                // Perform the actual execution of the script.
                thread = new Thread(new System.Threading.ThreadStart(this.ThreadEntryPoint));
            }
        }

        private void ThreadEntryPoint()
        {
            string mediaFile = System.Environment.CurrentDirectory + "\\IM_0001";
            string[] mediaFiles = { mediaFile };
            try
            {
                ok = mediaSession.ValidateMediaFiles(mediaFiles);
            }
            catch (Exception)
            {
                // If an exception was thrown, the ThreadState is still running.
                if (thread.ThreadState == ThreadState.Running)
                    thread.Abort();
            }
            finally
            {
                Assert.IsTrue(ok);
            }
        }
    }
}