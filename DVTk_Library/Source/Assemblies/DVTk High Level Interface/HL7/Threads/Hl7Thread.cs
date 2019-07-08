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

using DvtkHighLevelInterface.Common.Other;
using DvtkHighLevelInterface.Common.Threads;



namespace DvtkHighLevelInterface.Hl7.Threads
{
	/// <summary>
	/// This class represents a single thread in which Hl7 communication may be tested.
	/// </summary>
	public abstract class Hl7Thread: Thread
	{
        /// <summary>
        /// The HL7 logger.
        /// </summary>
		public Dvtk.Hl7.Hl7Logger hl7Logger = null;

        /// <summary>
        /// This method is called after all child threads are stopped.
        /// </summary>
		protected virtual void AfterChildThreadsFinished()
		{
			// Do nothing.
		}

		internal override void ShowResults()
		{

		}

        /// <summary>
        /// Start the results gathering.
        /// </summary>
		public override void StartResultsGathering()
		{
			if (ThreadOptions.StartAndStopResultsGatheringEnabled)
			{
				base.StartResultsGathering();

				this.hl7Logger = new Dvtk.Hl7.Hl7Logger(ThreadOptions.DetailResultsFullFileName);
				this.hl7Logger.Start();
			}
		}

		/// <summary>
		/// Stop the current thread only:
		/// - Terminate any open connection
		/// - If that doesn't work, Abort the .netThread associated with this object.
		/// 
		/// This method will be called in a seperate .Net thread (not in the .Net thread of this object).
		/// A seperate thread is used to make sure other threads are not waiting while this code is executed.
		/// </summary>
		internal protected override void StopCurrentThread()
		{
			System.Threading.Thread.CurrentThread.Name = "StopCurrentThread thread for Thread \"" + ThreadOptions.Identifier + "\"";

			bool terminateConnectionAndAbort = false;

			// Extra check to see if this Thread is still running.
			lock(ThreadManager.ThreadManagerLock)
			{
				if (ThreadState == ThreadState.Running)
				{
					terminateConnectionAndAbort = true;
				}
			}

			if (terminateConnectionAndAbort)
			{
				// TODO Close the connection!!!!

				lock(ThreadManager.ThreadManagerLock)
				{
					// If the TerminateConnection call did not make the thread stop or the thread
					// didn't end by itself, do this with a .Net Thread Abort.
					if (ThreadState == ThreadState.Running)
					{
						this.dotNetThread.Abort();
					}
				}
			}
		}

		/// <summary>
		/// This method is called when this object is stopping.
		/// </summary>
		private void Stopping()
		{
			if (ResultsGatheringStarted)
			{
				StopResultsGathering();
			}

			SetThreadState(ThreadState.Stopped);
		}

        /// <summary>
        /// Stop the results gathering.
        /// </summary>
		public override void StopResultsGathering()
		{
			if (ThreadOptions.StartAndStopResultsGatheringEnabled)
			{
				base.StopResultsGathering();

				if (this.hl7Logger != null)
				{
					this.hl7Logger.Stop();
					this.hl7Logger = null;
				}
			}
		}

//		/// <summary>
//		/// Thread code that is implemented in this class.
//		/// </summary>
//		protected override void ThreadCodeDescendantClass()
//		{
//			// Determine the name.
//			if (ThreadOptions.Name == null)
//			{
//				ThreadOptions.Name = this.GetType().Name;
//			}
//
//			// Determine the (unique) identifier.
//			if (ThreadOptions.Identifier == null)
//			{
//				ThreadOptions.Identifier = ThreadManager.GetUniqueIdentifier(this);
//			}
//
//			this.dotNetThread.Name = ThreadOptions.Identifier;
//			
//			// Do an initial wait if necessary.
//			if (this.initialMillisecondsToWait > 0)
//			{
//				if (TopmostThread != this)
//				{
//					TopmostThread.WriteInformation(String.Format("Waiting {0} milliseconds before starting DicomThread \"{1}\".", this.initialMillisecondsToWait.ToString(), ThreadOptions.Identifier));
//				}
//
//				System.Threading.Thread.Sleep(this.initialMillisecondsToWait);
//			}
//
//			if (!ResultsGatheringStarted)
//			{
//				StartResultsGathering();
//			}
//
//			// Now execute the actual testing code.
//			try
//			{
//				Execute();
//
//				SetThreadState(ThreadState.Stopping);
//			}
//			catch (System.Exception exception)
//			{
//				// If an exception was thrown, the ThreadState is still running.
//				SetThreadState(ThreadState.Stopping);
//
//				HandleExeption(exception);
//			}
//			finally
//			{	
//				WaitForCompletionChildThreads();
//
//				// The method may be overriden to specify what needs to be performed when this thread is stopping.
//				AfterChildThreadsFinished();
//
//				Stopping();
//			}
//		}

		/// <summary>
		/// Write an error to the results.
		/// </summary>
		/// <param name="text">The error text.</param>
		public override void WriteError(String text)
		{
			if (hl7Logger != null)
			{
				hl7Logger.LogError(text);
			}
			TriggerErrorOutputEvent(text);
		}

		/// <summary>
		/// Log error to the results.
		/// </summary>
		/// <param name="text">The error text.</param>
		public void LogError(String text)
		{
			this.WriteError(text);
		}

		/// <summary>
		/// Write information to the results.
		/// </summary>
		/// <param name="text">The information text.</param>
		public override void WriteInformation(String text)
		{
			if (hl7Logger != null)
			{
				hl7Logger.LogInfo(text);
			}
			TriggerInformationOutputEvent(text);			
		}

		/// <summary>
		/// Log information to the results.
		/// </summary>
		/// <param name="text">The information text.</param>
		public void LogInformation(String text)
		{
			this.WriteInformation(text);
		}

		/// <summary>
		/// Write a warning to the results.
		/// </summary>
		/// <param name="text">The warning text.</param>
		public override void WriteWarning(String text)
		{
			if (hl7Logger != null)
			{
				hl7Logger.LogWarning(text);
			}
			TriggerWarningOutputEvent(text);			
		}

		/// <summary>
		/// Log warning to the results.
		/// </summary>
		/// <param name="text">The warning text.</param>
		public void LogWarning(String text)
		{
			this.WriteWarning(text);
		}

		/// <summary>
		/// Write the given XML string to the results file without further interpretation.
		/// </summary>
		/// <param name="xmlString">XML string to be written to file.</param>
		public void WriteXmlStringToResults(System.String xmlString)
		{
			if (hl7Logger != null)
			{
				hl7Logger.LogXmlString(xmlString);
			}
		}

		/// <summary>
		/// Update the Validation Error count by the errorCount given.
		/// </summary>
		/// <param name="errorCount">Error count.</param>
		public void UpdateValidationErrorCount(int errorCount)
		{
			if (hl7Logger != null)
			{
				hl7Logger.UpdateValidationErrorCount(errorCount);
			}
		}

		/// <summary>
		/// Update the Validation Warning count by the warningCount given.
		/// </summary>
		/// <param name="warningCount">Warning count.</param>
		public void UpdateValidationWarningCount(int warningCount)
		{
			if (hl7Logger != null)
			{
				hl7Logger.UpdateValidationWarningCount(warningCount);
			}
		}

		/// <summary>
		/// Property - get the total number of errors reported to this thread.
		/// </summary>
		public int NrErrors
		{
			get
			{
				int nrErrors = 0;

				if (hl7Logger != null)
				{
					nrErrors = hl7Logger.NrErrors;
				}

				return nrErrors;
			}
		}

		/// <summary>
		/// Property - get the total number of warnings reported to this thread.
		/// </summary>
		public int NrWarnings
		{
			get
			{
				int nrWarnings = 0;

				if (hl7Logger != null)
				{
					nrWarnings = hl7Logger.NrWarnings;
				}

				return nrWarnings;
			}
		}

		/// <summary>
		/// Initialize this object as a DicomThread with no parent thread.
		/// </summary>
		/// <param name="threadManager">The threadManager.</param>
		public new void Initialize(ThreadManager threadManager)
		{
			// Initialize may only be called once, so check for this.
			if (this.isInitialized)
			{
				throw new HliException(alreadyInitializedErrorText);
			}

			base.Initialize(threadManager);
			this.isInitialized = true;
			this.threadOptions = new Hl7ThreadOptions();
		}

		/// <summary>
		/// Initialize this object as a DicomThread with a parent thread.
		/// </summary>
		/// <param name="parent">The parent thread.</param>
		public new void Initialize(Thread parent)
		{
			// Initialize may only be called once, so check for this.
			if (this.isInitialized)
			{
				throw new HliException(alreadyInitializedErrorText);
			}

			base.Initialize(parent);
			this.isInitialized = true;
			this.threadOptions = new Hl7ThreadOptions();
		}

        /// <summary>
        /// Gets the HL7 thread options.
        /// </summary>
		public new Hl7ThreadOptions Options
		{
			get
			{
				return(this.threadOptions as Hl7ThreadOptions);
			}
		}


	}
}
