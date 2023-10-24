using DvtkHighLevelInterface.Dicom.Threads;
using System.IO;

namespace DvtkHighLevelInterface.NUnit
{
    public class Config
    {
        public static void SetOptions(DicomThread dicomThread, string testName, string identifier)
        {
            string TestResultsDirectory = Path.Combine(Paths.ResultsDirectoryFullPath, testName);
            if (!Directory.Exists(TestResultsDirectory))
            {
                Directory.CreateDirectory(TestResultsDirectory);
            }

            dicomThread.Options.LocalPort = 104;
            dicomThread.Options.RemotePort = 104;
            dicomThread.Options.RemoteHostName = "localhost";
            dicomThread.Options.ResultsDirectory = Paths.ResultsDirectoryFullPath;
            dicomThread.Options.Identifier = identifier;
        }
    }
}