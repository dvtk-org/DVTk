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
using System.IO;

namespace Dvtk.IheActors.IheFramework
{
    /// <summary>
    /// Summary of class FileSemaphore
    /// </summary>
    public class FileSemaphore
    {
        /// <summary>
        /// Signal that the test has completed.
        /// </summary>
        public static void SignalTestCompletion()
        {
            // fixed lock filename
            String lockFilename = String.Format("{0}fileLock.txt", System.AppDomain.CurrentDomain.BaseDirectory);

            try
            {
                // check if the lock file still exists
                FileInfo lockFileInfo = new FileInfo(lockFilename);
                if (lockFileInfo.Exists == true)
                {
                    // delete the lock file
                    lockFileInfo.Delete();
                }
            }
            catch (System.Exception)
            {
            }
        }

        /// <summary>
        /// Wait until told that the test has completed.
        /// </summary>
        public static void PendTestCompletion()
        {
            // blocking method call that pends this instance until another
            // instance indicates that the test is complete

            // fixed lock filename
            String lockFilename = String.Format("{0}fileLock.txt", System.AppDomain.CurrentDomain.BaseDirectory);

            try
            {
                // write the lock file
                StreamWriter sw = new StreamWriter(lockFilename);
                sw.WriteLine("DVTk IHE Framework lock file");
                sw.Flush();
                sw.Close();

                // block the method while the lock file exists
                bool pendingTestCompletion = true;
                while (pendingTestCompletion == true)
                {
                    // sleep for 5 seconds
                    System.Threading.Thread.Sleep(5000);

                    // check if the lock file still exists
                    FileInfo lockFileInfo = new FileInfo(lockFilename);
                    if (lockFileInfo.Exists == false)
                    {
                        pendingTestCompletion = false;
                    }
                }
            }
            catch (System.Exception)
            {
            }
        }
    }
}
