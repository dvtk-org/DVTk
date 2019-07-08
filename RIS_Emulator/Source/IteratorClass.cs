using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DvtkHighLevelInterface.Dicom.Threads;
using DvtkHighLevelInterface.Common.Threads;
using System.IO;

namespace RIS_Emulator
{
    class IteratorClass : MessageIterator
    {
        private ThreadManager threadManager;

        public IteratorClass(string rootPath, string resultsDirectory, string sessionFileName, string identifier)
        {
            threadManager = new ThreadManager();
            this.Initialize(threadManager);
            this.Options.LoadFromFile(Path.Combine(rootPath, sessionFileName));
            this.Options.Identifier = identifier;
            this.Options.AttachChildsToUserInterfaces = true;
            this.Options.StorageMode = Dvtk.Sessions.StorageMode.NoStorage;
            this.Options.LogThreadStartingAndStoppingInParent = false;
            this.Options.LogWaitingForCompletionChildThreads = false;
            this.Options.ResultsDirectory = resultsDirectory;
        }
    }
}
