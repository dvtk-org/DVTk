using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;

namespace Dvtk.Xml.Transformation
{
    public class XsltProcessor
    {
        #region Internal Variables
        /// <summary>
        ///     Contains the list of transformation jobs.
        /// </summary>
        private Queue<WorkItem> transformationQueue;

        /// <summary>
        ///     Contains the queue length at the start, used for calculating the progress.
        /// </summary>
        private int totalQueueLength;

        /// <summary>
        ///     Used for tracking the progress.
        /// </summary>
        private float progressCount;

        /// <summary>
        ///     Contains the total number of running Transformations.
        /// </summary>
        private int runningCount;
        #endregion

        #region Properties
        /// <summary>
        ///     Get the number of elements currently in the queue.
        /// </summary>
        public int Count
        {
            get
            {
                return transformationQueue.Count;
            }
        }
        #endregion

        #region Delegates & Events
        /// <summary>
        ///     Delegate for making an Async call to <see cref="Start" />.
        /// </summary>;
        private delegate void StartDelegate();

        /// <summary>
        ///     Occurs when the status of an async action has changed.
        /// </summary>
        public event ProgressChangedEventHandler ProgressChanged;

        /// <summary>
        ///     Occurs when the loading of a Data Repository has finished.
        /// </summary>
        public event AsyncCompletedEventHandler ProcessingCompleted;

        #endregion

        #region Public Functions
        /// <summary>
        ///     Create a new instance of the <see cref="XSLTProcessor" /> class.
        /// </summary>
        public XsltProcessor()
        {
            transformationQueue = new Queue<WorkItem>();
            totalQueueLength = 0;
            progressCount = 0;
        }

        /// <summary>
        ///     Adds a workItem to the Queue
        /// </summary>
        /// <param name="workItem">
        ///     The <see cref="WorkItem" /> to add to the queue.
        /// </param>
        /// <remarks>
        ///     This function is not thread safe and should not be called after calling
        ///     <see cref="StartAsync" />.
        /// </remarks>
        public void AddWorkItem(WorkItem workItem)
        {
            transformationQueue.Enqueue(workItem);
            totalQueueLength++;
        }

        /// <summary>
        ///     Start transforming all work items in the queue.
        /// </summary>
        public void Start()
        {
            // Reset counters.
            progressCount = 0;
            runningCount = 0;
            
            // Start processing n work items, where n equals the number of logical CPUs.
            for (int i = 0; i < Environment.ProcessorCount; i++)
            {
                dequeue();
            }
            // Keep checking if the queue is empty and there are no more transformations running.
            while ((transformationQueue.Count > 0) || (runningCount > 0))
            {
                Thread.Sleep(10);
            }
            Console.WriteLine("Exiting...");
        }

        /// <summary>
        ///     Start transforming all work items in the queue async. The 
        ///     <see cref="ProcessingCompleted" /> event will fire when done.
        /// </summary>
        public void StartAsync()
        {
            StartDelegate startDelegate = new StartDelegate(Start);
            IAsyncResult asyncResult;
            AsyncCallback asyncCallback = new AsyncCallback(startCallback);
            asyncResult = startDelegate.BeginInvoke(asyncCallback, startDelegate);
        }
        #endregion

        #region Protected Functions
        /// <summary>
        ///     Raises the <see cref="ProcessingCompleted" /> event.
        /// </summary>
        /// <param name="e">
        ///     A <see cref="AsyncCompletedEventArgs" /> that contains the event data.
        /// </param>
        protected virtual void OnProcessingCompleted(AsyncCompletedEventArgs e)
        {
            totalQueueLength = 0;
            if (ProcessingCompleted != null)
            {
                ProcessingCompleted(this, e);
            }
        }

        /// <summary>
        ///     Raises the ProgressChanged event.
        /// </summary>
        /// <param name="e">
        ///     A <see cref="ProgressChangedEventArgs" /> that contains the event data. The UserState k
        /// </param>
        protected virtual void OnProgressChanged(ProgressChangedEventArgs e)
        {
            if (ProgressChanged != null)
            {
                ProgressChanged(this, e);
            }
        }
        #endregion

        #region Private Functions
        /// <summary>
        ///     Perform the XSL Transformation for the given Work Item.
        /// </summary>
        /// <param name="workItem">
        ///     A Work Item that specifies the files needed for the XSL Transformation.
        /// </param>
        private void process(WorkItem workItem)
        {
            // Check if the output directory exists, if not create it.
            string outputFolder = Path.GetDirectoryName(workItem.OutputFile);
            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            // Create, configure and start the process object.
            Process xsltTransformation = new Process();
            xsltTransformation.EnableRaisingEvents = true;
            xsltTransformation.Exited += xsltTransformation_Exited;
            xsltTransformation.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

            string msxsl = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            msxsl = Path.Combine(msxsl, "msxsl.exe");
            xsltTransformation.StartInfo.FileName = msxsl;
            xsltTransformation.StartInfo.Arguments = "\"" + workItem.XmlFile + "\" \"" + workItem.XsltFile + "\" -o \"" + workItem.OutputFile + "\"";
            xsltTransformation.Start();
        }

        /// <summary>
        ///     Dequeue the next item from the internal work item queue.
        /// </summary>
        private void dequeue()
        {
            progressCount++;
            
            // Check if there are still items in the queue.
            if (transformationQueue.Count > 0)
            {
                runningCount++;

                // Get the next work item from the queue.
                WorkItem workItem = transformationQueue.Dequeue();

                // Calculate progress.
                int progress = Convert.ToInt32((progressCount / totalQueueLength) * 100);

                // Limit the progress.
                if (progress < 1)
                {
                    progress = 1;
                }
                else if (progress > 99)
                {
                    progress = 99;
                }

                // Report the progress.
                string progressMessage = "Running transformation: " + Path.GetFileName(workItem.OutputFile);
                OnProgressChanged(new ProgressChangedEventArgs(progress, progressMessage));

                // Start the progress.
                process(workItem);
            }
        }

        /// <summary>
        ///     Callback function used when running StartAsync.
        /// </summary>
        /// <param name="asyncResult">
        ///     The AsyncResult.
        /// </param>
        private void startCallback(IAsyncResult asyncResult)
        {
            Exception e = null;
            StartDelegate startDelegate = (StartDelegate)asyncResult.AsyncState;
            try
            {
                startDelegate.EndInvoke(asyncResult);
            }
            catch (Exception ex)
            {
                e = ex;
            }
            OnProcessingCompleted(new AsyncCompletedEventArgs(e, asyncResult.IsCompleted, asyncResult.AsyncState));
        }

        /// <summary>
        ///     Eventhandler for the Exited event that triggers when a previous transformation
        ///     has finished.
        /// </summary>
        /// <param name="sender">
        ///     Sender of the event.
        /// </param>
        /// <param name="e">
        ///     Arguments for the event.
        /// </param>
        private void xsltTransformation_Exited(object sender, EventArgs e)
        {
            // TODO: Check for crashes based on the Exit Code.
            // Update counter.
            runningCount--;

            // Start a new process by dequeuing the next item.
            dequeue();
            Console.WriteLine("Transformation Done...");
        }
        #endregion
    }
}
