using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using DvtkHighLevelInterface.Dicom.Threads;

namespace DvtkHighLevelInterface.NUnit
{
    public class Config
    {
        public static void SetOptions(DicomThread dicomThread, String testName, String identifier)
        {
            String TestResultsDirectory = Path.Combine(Paths.ResultsDirectoryFullPath, testName);
            if (!Directory.Exists(TestResultsDirectory))
            {
                Directory.CreateDirectory(TestResultsDirectory);
            }

            dicomThread.Options.LocalPort = 104;
            dicomThread.Options.RemotePort = 104;
            dicomThread.Options.RemoteHostName = "localhost";
            dicomThread.Options.ResultsDirectory = TestResultsDirectory;
            dicomThread.Options.Identifier = identifier;
        }
    }
}
