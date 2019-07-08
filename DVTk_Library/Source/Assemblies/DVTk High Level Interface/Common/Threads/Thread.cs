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
using System.Collections;

using DvtkHighLevelInterface;
using DvtkHighLevelInterface.Common.Messages;
using DvtkHighLevelInterface.Common.Other;
using DvtkHighLevelInterface.Common.UserInterfaces;
using DvtkHighLevelInterface.Dicom.Other;



namespace DvtkHighLevelInterface.Common.Threads
{
	/// <summary>
	/// This class contains the shared threading functionality of a DicomThread and Hl7Thread.
	/// 
	/// To create a descendant class, do at least the following (DicomThread class may be used as an example):
	/// - Implement the StopCurrentThread method.
	/// - Implement the WriteError, WriteInformation and WriteWarning methods.
	/// - Implement the method ThreadEntryPoint. Within this method, the method Execute must be called.
	/// - Call the methods TriggerErrorOutputEvent, TriggerInformationOutputEvent and TriggerWarningOutputEvent
	///   whenever some Error, Information or Warning output is available (this is needed whenever some UserInface
	///   like the HliConsole or HliForm needs to be attached to the descendant class).
	///   
	/// The user of the descendant class on its turn at least needs to implement the Execute method.
	/// </summary>
	public abstract class Thread
	{
		//
		// - Constant fields -
		//

		internal const String alreadyInitializedErrorText = "Initialize has already been called and may not be called again.";

		internal const String notInitializedErrorText = "Initialize must be called first before calling other methods and properties.";



		//
		// - Fields -
		//

		/// <summary>
		/// See property AttachedUserInterfaces.
		/// </summary>
		private ArrayList attachedUserInterfaces = ArrayList.Synchronized(new ArrayList());

		/// <summary>
		/// The child Threads.
		/// </summary>
		internal protected ThreadCollection childs = null;

		/// <summary>
		/// The .Net thread that performs the actual execution in a seperate thread.
		/// </summary>
		internal protected System.Threading.Thread dotNetThread = null;

		/// <summary>
		/// See property HasBeenStarted.
		/// </summary>
		private bool hasBeenStarted = false;

		/// <summary>
		/// See property HasExceptionOccured.
		/// </summary>
		internal protected bool hasExceptionOccured = false;

		/// <summary>
		/// Initial number of milliseconds to wait before executing code in the thread.
		/// </summary>
		internal protected int initialMillisecondsToWait = 0;

		/// <summary>
		/// Boolean indicating if one of the Initialize methods has already been called.
		/// </summary>
		internal protected bool isInitialized = false;

		/// <summary>
        /// See property IsStopCalled.
		/// </summary>
		private bool isStopCalled = false;

        /// <summary>
        /// All messages that have been send or received. When needed, messages may be removed 
        /// using the RemoveMessage or ClearMessages methods.
        /// 
        /// When using this private field within this class, always lock the MessageLock of the
        /// ThreadManager first.
        /// </summary>
        internal protected MessageCollection messages = new MessageCollection();

        /// <summary>
        /// The parent thread if existing. Otherwise null.
        /// </summary>
        internal protected Thread parent = null;

        /// <summary>
        /// Only use property ResultsGatheringStarted to access this field.
        /// </summary>
        private bool resultsGatheringStarted = false;

        /// <summary>
        /// Thread used to stop this object and sub Threads, in order to let the caller of the 
        /// Stop method not wait.
        /// </summary>
        private System.Threading.Thread stopDotNetThread = null;

        /// <summary>
        /// Use this object to lock fields that may be accessed simultaniously by multiple .Net threads.
        /// </summary>
        internal protected Object threadLock = new Object();

        /// <summary>
        /// See property ThreadManager.
        /// </summary>
        private ThreadManager threadManager = null;

        /// <summary>
        /// This field must be set by a descendant of this class,
        /// </summary>
        internal protected ThreadOptions threadOptions = null;

        /// <summary>
        /// See property threads.
        /// </summary>
        private static Hashtable threads = Hashtable.Synchronized(new Hashtable());

        /// <summary>
        /// The current state of the thread.
        /// 
        /// Don't use directly in this class but use the ThreadState property instead.
        /// </summary>
        private ThreadState threadState = ThreadState.UnStarted;

        /// <summary>
        /// See property TopmostThread.
        /// </summary>
        private Thread topmostThread = null;



        //
        // - Delegates -
        //

        /// <summary>
        /// The delegate used for the events ErrorOutputEvent, InformationOutputEvent and WarningOutputEvent.
        /// </summary>
        internal delegate void TextOutputEventHandler(Thread thread, String text);

        /// <summary>
        /// The delegate used for the ThreadStateChangeEvent.
        /// </summary>
        public delegate void ThreadStateChangeHandler(Thread thread, ThreadState oldThreadState, ThreadState newThreadState);



        //
        // - Events -
        //

        /// <summary>
        /// Event is fired when some error output is available (must be implemented by descendant of this
        /// class by calling the TriggerErrorOutputEvent method).
        /// </summary>
        internal event TextOutputEventHandler ErrorOutputEvent;

        /// <summary>
        /// Event is fired when some information output is available (must be implemented by descendant of this
        /// class by calling the TriggerInformationOutputEvent method).
        /// </summary>
        internal event TextOutputEventHandler InformationOutputEvent;

        /// <summary>
        /// Event is fired when some warning output is available (must be implemented by descendant of this
        /// class by calling the TriggerWarningOutputEvent method).
        /// </summary>
        internal event TextOutputEventHandler WarningOutputEvent;

        /// <summary>
        /// This event is triggered whenever the ThreadState changes for this object.
        /// </summary>
        public event ThreadStateChangeHandler ThreadStateChangeEvent;



        //
        // - Properties -
        //

        /// <summary>
        /// Gets the attached UserInterfaces of this object.
        /// </summary>
        /// <remarks>
		/// An example of a UserInterface is the HliConsole class, which just outputs the text available
		/// in the ErrorOutputEvent, InformationOutputEvent and WarningOutputEvent to the console.
		/// </remarks>
        internal ArrayList AttachedUserInterfaces
        {
            get
            {
                return (this.attachedUserInterfaces);
            }
        }

        /// <summary>
        /// Gets the current HLI Thread that is (indirectly) calling this property.
        /// If no HLI thread is (indirectly) calling this property, null is returned.
        /// </summary>
        public static Thread CurrentThread
        {
            get
            {
                int hashCode = System.Threading.Thread.CurrentThread.GetHashCode();
                return (threads[hashCode] as Thread);
            }
        }

		/// <summary>
		/// Indicates if this Thread has been started.
		/// </summary>
		/// <remarks>
		/// A Thread that has been started and stopped afterwards will also return true.
		/// </remarks>
		public bool HasBeenStarted
		{
			get
			{
				return (this.hasBeenStarted);
			}
		}

        /// <summary>
        /// Indicates if an unhandled exception has occured during execution of the Thread.
        /// </summary>
        public bool HasExceptionOccured
        {
            get
            {
                lock (this.threadLock)
                {
                    return (this.hasExceptionOccured);
                }
            }
        }

        /// <summary>
        /// Indicates if the Stop method of this object has been called.
        /// </summary>
        public bool IsStopCalled
        {
            get
            {
                lock (this.threadLock)
                {
                    return (this.isStopCalled);
                }
            }
        }

        public ThreadOptions Options
        {
            get
            {
                return (this.threadOptions);
            }
        }

        /// <summary>
        /// Gets the parent Thread.
        /// </summary>
        public Thread Parent
        {
            get
            {
                return (this.parent);
            }
        }

        /// <summary>
        /// Indicates if results gathering has started.
        /// </summary>
        public bool ResultsGatheringStarted
        {
            get
            {
                lock (this.threadLock)
                {
                    return (this.resultsGatheringStarted);
                }
            }
            set
            {
                lock (this.threadLock)
                {
                    this.resultsGatheringStarted = value;
                }
            }
        }

        /// <summary>
        /// Gets the ThreadManager that manages this Thread.
        /// </summary>
        public ThreadManager ThreadManager
        {
            get
            {
                return (this.threadManager);
            }
        }

		/// <summary>
		/// Gets the Thread options.
		/// </summary>
		internal ThreadOptions ThreadOptions
		{
			get
			{
				return (this.threadOptions);  
			}
		}

        /// <summary>
        /// Contains mapping between .Net threads and High Level Interface threads.
        /// </summary>
        /// <remarks>
		/// - Key of this hashtable: GetHashCode of the .Net thread.
		/// - Value of this hashtable: reference to a High Level Interface thread.
		/// </remarks>
        internal static Hashtable Threads
        {
            get
            {
                return (threads);
            }
        }

        /// <summary>
        /// Gets the current ThreadState of this object.
        /// </summary>
        public ThreadState ThreadState
        {
            get
            {
                ThreadState threadState = ThreadState.UnStarted;

                lock (this.threadManager.ThreadManagerLock)
                {
                    threadState = this.threadState;
                }

                return (threadState);
            }
        }

        /// <summary>
        /// Gets the topmost Thread, considering the parent relation.
        /// If this thread does not have a parent, this object itself is the topmost thread.
        /// </summary>
        public Thread TopmostThread
        {
            get
            {
                return this.topmostThread;
            }
        }



        //
        // - Methods -
        //

        /// <summary>
        /// Adds a message that has been received or send by this Thread.
        /// </summary>
        /// <param name="message"></param>
        internal void AddMessage(Message message)
        {
            lock (this.threadManager.MessageLock)
            {
                this.messages.Add(message);
                this.threadManager.AddMessage(message);
            }
        }

		/// <summary>
		/// Called after the Execute method has been called.
		/// </summary>
		/// <remarks>
		/// Gives the descendant of this class the possibility to perform
		/// extra actions compared to this base class.
		/// </remarks>
		internal virtual void AfterCallingExecute()
		{
			// Do nothing by default.
		}

		/// <summary>
		/// Called after the HandleException method has been called.
		/// </summary>
		/// <remarks>
		/// Gives the descendant of this class the possibility to perform
		/// extra actions compared to this base class.
		/// </remarks>
        /// <param name="exception">Exception that has been handled.</param>
        protected virtual void AfterHandlingException(System.Exception exception)
		{
            // Do nothing by default.
        }

		/// <summary>
		/// Called after the WaitForCompletionChildThreads method has been called.
		/// </summary>
		/// <remarks>
		/// Gives the descendant of this class the possibility to perform
		/// extra actions compared to this base class.
		/// </remarks>
		internal virtual void AfterWaitingForCompletionChildThreads()
		{
			// Do nothing by default.
		}

		/// <summary>
		/// Called before the Execute method will be called.
		/// </summary>
		/// <remarks>
		/// Gives the descendant of this class the possibility to perform
		/// extra actions compared to this base class.
		/// </remarks>
		internal virtual void BeforeCallingExecute()
		{
			// Do nothing by default.
		}

		/// <summary>
		/// Called before the HandleException method will be called.
		/// </summary>
		/// <remarks>
		/// Gives the descendant of this class the possibility to perform
		/// extra actions compared to this base class.
		/// </remarks>
        /// <param name="exception">Exception that is about to be handled.</param>
        protected virtual void BeforeHandlingException(System.Exception exception)
		{
			// Do nothing by default.
		}

		/// <summary>
		/// Called before the WaitForCompletionChildThreads method will be called.
		/// </summary>
		/// <remarks>
		/// Gives the descendant of this class the possibility to perform
		/// extra actions compared to this base class.
		/// </remarks>
		internal virtual void BeforeWaitingForCompletionChildThreads()
		{
			// Do nothing by default.
		}

        /// <summary>
        ///  Clears the list of received and send messages.
        /// </summary>
        public void ClearMessages()
        {
            lock (this.threadManager.MessageLock)
            {
                foreach (Message message in this.messages)
                {
                    this.threadManager.RemoveMessage(message);
                }

                this.messages.Clear();
            }
        }

        /// <summary>
        /// Determines the Identifier (property of the Options of this Thread).
        /// </summary>
        /// <remarks>
        /// Override this method to change the Options.Identifier property if needed.
        /// <br></br><br></br>
		/// Default implementation is that when the Identifier is not set, a unique Identifier
		/// (considering the identifiers of all threads belonging to this ThreadManager) will 
		/// automatically be determined, in which the Name of this object will be postfixed with
		/// a number to make it unique.
		/// </remarks>
        protected virtual void DetermineIdentifier()
        {
            // 
            if (this.threadOptions.Identifier == null)
            {
                this.threadOptions.Identifier = ThreadManager.GetUniqueIdentifier(this);
            }
        }

        /// <summary>
        /// Determines the Name of the instance.
        /// </summary>
        /// <remarks>
        /// Override this method to change the Options.Name property if needed.<br></br><br></br>
        /// 
		/// Default implementation is that when the Name is not set, the Type of this
		/// object will be used as name.
		/// </remarks>
        protected virtual void DetermineName()
        {
            // Determine the name.
            if (this.threadOptions.Name == null)
            {
                this.threadOptions.Name = this.GetType().Name;
            }
        }

		/// <summary>
		/// Determines the results file name only without the extension.
		/// </summary>
		/// <remarks>
		/// Override this method to change the Options.ResultsFileNameOnlyWithoutExtension if needed.<br></br><br></br>
		/// 
		/// Default implementation is that when the results file name only is not set it is set to
		/// String.Format("{0:000}_{1}_res", (int)Options.SessionId, Options.Identifier) + "{0}".
        /// </remarks>
		protected virtual void DetermineResultsFileNameOnlyWithoutExtension()
		{
			if (this.threadOptions.ResultsFileNameOnlyWithoutExtension == null)
			{
				this.threadOptions.ResultsFileNameOnlyWithoutExtension = String.Format("{0:000}_{1}_res", (int)this.threadOptions.SessionId, this.threadOptions.Identifier) + "{0}";
			}
		}

		/// <summary>
		/// Displays information about an unhandled exception occuring in this instance.
		/// </summary>
		/// <param name="exception">The unhandled exception that has been thrown.</param>
		protected virtual void DisplayException(System.Exception exception)
		{
			// Write the fatal error text.
			String errorText = "Fatal error of type " + exception.GetType().ToString() + "!";

			errorText += "\r\n\r\nFatal error description:\r\n";
			errorText += exception.Message;

			WriteError(errorText);

			// Write extra information about the fatal error.
			String extraInformation = "Extra information about the fatal error.";

			System.Exception innerException = exception.InnerException;

			while (innerException != null)
			{
				extraInformation = "\r\n\r\n" + "[F]\r\n" + innerException.Message;

				innerException = innerException.InnerException;
			}

			extraInformation += "\r\n\r\nStack trace:\r\n" + exception.StackTrace;

			WriteInformation(extraInformation);
		}

        /// <summary>
        /// The actual test code should be placed in this overriden method.
        /// </summary>
		/// <example>
		///		<b>VB.NETC#</b>
		///		<code>
        /// 			<include file='..\..\assemblies\DVTk High Level Interface\Doc\VisualBasicExamples.xml' path='Examples/Example[@name="IncludesDicomThreadScu"]' />
        /// 			<include file='..\..\assemblies\DVTk High Level Interface\Doc\VisualBasicExamples.xml' path='Examples/Example[@name="DicomThreadScu"]' />
        /// 			<include file='..\..\assemblies\DVTk High Level Interface\Doc\VisualBasicExamples.xml' path='Examples/Example[@name="Main"]' />		
		///		</code>
		/// </example>
        abstract protected void Execute();

        /// <summary>
        /// Handles an unhandled exception in the Execute method.
        /// 
        /// When the Stop method (which also indirectly causes an exception) was not called,
        /// information about the exception will be logged in the results.
        /// </summary>
        /// <param name="exception">The unhandled exception.</param>
        protected void HandleExeption(System.Exception exception)
        {
            bool isStopMethodCalled = false;

            lock (this.threadLock)
            {
                isStopMethodCalled = this.isStopCalled;
            }

            if (!isStopMethodCalled)
            {
                lock (this.threadLock)
                {
                    this.hasExceptionOccured = true;
                }

				DisplayException(exception);

                // If this is not the topmost thread, report the fatal error to it.
                if (TopmostThread != this)
                {
                    TopmostThread.WriteInformation("Fatal error in " + this.ToString() + " \"" + this.threadOptions.Identifier + "\"!");
                }
            }

            // If the exception is a ThreadAbortException, we have to cancel the abort, otherwise
            // the .Net runtime will again throw this exception after the cath block.
            if (exception is System.Threading.ThreadAbortException)
            {
                System.Threading.Thread.ResetAbort();
            }
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        /// <remarks>
		/// Call one of the Initialize methods directly after creating a Thread instance.
		/// 
		/// Code that normally would be present in the constructor.
		/// This code is however put in a separate method to be able to have only
		/// one constructor in DicomThread. This way, it is easier to derive from a
		/// DicomThread class.
		/// 
		/// Use this method if this object should have a parent thread.
		/// </remarks>
        /// <param name="parent">The parent Thread.</param>
        protected void Initialize(Thread parent)
        {
            this.parent = parent;
            this.dotNetThread = new System.Threading.Thread(new System.Threading.ThreadStart(this.ThreadEntryPoint));
            this.threadManager = this.parent.ThreadManager;
            this.topmostThread = this.parent.TopmostThread;

            // See property ThreadManagerLock when to use this lock.
            lock (this.threadManager.ThreadManagerLock)
            {
                this.childs = new ThreadCollection();
                this.parent.childs.Add(this);
                this.threadManager.AddThread(this);
            }

            if (this.parent.ThreadOptions.AttachChildsToUserInterfaces)
            {
                foreach (IThreadUserInterface threadUserInterface in this.parent.AttachedUserInterfaces)
                {
                    threadUserInterface.Attach(this);
                }
            }
        }

        /// <summary>
		/// Initializes this instance.
        /// </summary>
        /// <remarks>
		/// Call one of the Initialize methods directly after creating a Thread instance.
		/// 
		/// Code that normally would be present in the constructor.
		/// This code is however put in a separate method to be able to have only
		/// one constructor in DicomThread. This way, it is easier to derive from a
		/// DicomThread class.
		/// 
		/// Use this method if this threads should not have a parent thread.
		/// </remarks>
        /// <param name="threadManager">The ThreadManager that manages this object.</param>
        protected void Initialize(ThreadManager threadManager)
        {
            this.parent = null;
            this.dotNetThread = new System.Threading.Thread(new System.Threading.ThreadStart(this.ThreadEntryPoint));
            this.threadManager = threadManager;
            this.topmostThread = this;

            // See property ThreadManagerLock when to use this lock.
            lock (this.threadManager.ThreadManagerLock)
            {
                this.childs = new ThreadCollection();
                this.threadManager.ChildThreads.Add(this);
                this.threadManager.AddChildThread(this);
            }
        }

        /// <summary>
        /// Removes this message from the list of received and send messages.
        /// </summary>
        /// <param name="message"></param>
        public void RemoveMessage(Message message)
        {
            lock (this.threadManager.MessageLock)
            {
                this.messages.Remove(message);
                this.threadManager.RemoveMessage(message);
            }
        }

        /// <summary>
        /// Makes the thread sleep for a specified number of milliseconds.
        /// </summary>
        /// <param name="milliseconds">Number of milliseconds to sleep.</param>
        protected void Sleep(int milliseconds)
        {
            System.Threading.Thread.Sleep(milliseconds);
        }

        /// <summary>
        /// Sets the new ThreadState of this object and fire the ThreadStateChangeEvent.
        /// </summary>
        /// <param name="threadState">The new ThreadState.</param>
        internal void SetThreadState(ThreadState threadState)
        {
            ThreadState oldThreadState = ThreadState.UnStarted;
            ThreadState newThreadState = ThreadState.UnStarted;

            lock (this.threadManager.ThreadManagerLock)
            {
                oldThreadState = this.threadState;
                this.threadState = threadState;
                newThreadState = this.threadState;
                this.threadManager.ThreadStateChanged(oldThreadState, newThreadState);
            }

            if (ThreadStateChangeEvent != null)
            {
                ThreadStateChangeEvent(this, oldThreadState, newThreadState);
            }

            this.threadManager.TriggerThreadsStateChangeEvent(this, oldThreadState, newThreadState);
        }

		internal abstract void ShowResults();

        /// <summary>
        /// Starts the Thread without an initial number of milliseconds to wait.
        /// </summary>
        /// <remarks>
        /// The Thread will only start when it has not already been started.
        /// </remarks>
        public void Start()
        {
            Start(0);
        }

        /// <summary>
        /// Start the Thread and wait an initial number of milleseconds before
        /// executing the code that is defined in the overriden Execute method.
        /// </summary>
        /// <remarks>
        /// The Thread will only start when it has not already been started.
        /// </remarks>
        /// <param name="initialMillisecondsToWait">Number of milliseconds.</param>
        public void Start(int initialMillisecondsToWait)
        {
            lock (this.threadManager.ThreadManagerLock)
            {
                if (ThreadState == ThreadState.UnStarted)
                {
                    DetermineName();
                    DetermineIdentifier();
                    DetermineResultsFileNameOnlyWithoutExtension();

                    SetThreadState(ThreadState.Running);

                    this.initialMillisecondsToWait = initialMillisecondsToWait;

                    this.hasBeenStarted = true;

                    this.dotNetThread.Start();
                }
            }
        }

        /// <summary>
        /// Starts the results gathering.
        /// </summary>
        /// <remarks>
		/// This method will only perform the basic administration. The actual starting of the
		/// results gathering needs to be implemented in the derived class.
		/// 
		/// When overriding this method, make sure to call this base method first.
		/// </remarks>
        public virtual void StartResultsGathering()
        {
            if (this.threadOptions.StartAndStopResultsGatheringEnabled)
            {
                if (ResultsGatheringStarted)
                {
                    StopResultsGathering();
                }

                this.threadOptions.ResultsFileNameIndex++;
                ResultsGatheringStarted = true;
            }
        }

        /// <summary>
        /// Stops this object and all child threads.
        /// </summary>
        /// <remarks>
        /// The caller of this method will not wait until the threads have ended.
        /// </remarks>
        public virtual void Stop()
        {
            // Initialize must be called first, so check for this.
            if (!this.isInitialized)
            {
                throw new HliException(notInitializedErrorText);
            }

            lock (this.threadLock)
            {
                this.isStopCalled = true;
            }

            bool terminateConnectionAndAbort = false;

            lock (this.threadManager.ThreadManagerLock)
            {
                // Stop all child threads.
                foreach (Thread childThread in this.childs)
                {
                    childThread.Stop();
                }

                // While the child threads are stopping, stop this object also.
                // This object will wait in the method ThreadEntryPoint until all
                // child threads have ended.

                if (ThreadState == ThreadState.UnStarted)
                {
                    SetThreadState(ThreadState.Stopped);
                }
                else if (ThreadState == ThreadState.Running)
                {
                    terminateConnectionAndAbort = true;
                }
            }

            if (terminateConnectionAndAbort)
            {
                this.stopDotNetThread = new System.Threading.Thread(new System.Threading.ThreadStart(StopCurrentThread));

                this.stopDotNetThread.Start();
            }
        }

        /// <summary>
        /// A descendant class must override this method and do the following to stop the current thread only:
        /// - Terminate any open connecten (e.g. TCP/IP connection)
        /// - If that doesn't work, Abort the .netThread associated with this object.
        /// 
        /// This method will be called in a seperate .Net thread (not in the .Net thread of this object).
        /// A seperate thread is used to make sure other threads are not waiting while this code is executed.
        /// </summary>
        internal protected abstract void StopCurrentThread();

        /// <summary>
        /// Stops the results gathering.
        /// </summary>
        /// <remarks>
		/// This method will only perform the basic administration. The actual stopping of the
		/// results gathering needs to be implemented in the derived class.
		/// 
		/// When overriding this method, make sure to call this base method first.
		/// </remarks>
        public virtual void StopResultsGathering()
        {
            if (this.threadOptions.StartAndStopResultsGatheringEnabled)
            {
                ResultsGatheringStarted = false;
            }
        }

        /// <summary>
        /// The thread entry point used by the .Net thread.
        /// 
        /// This method contains the basic implementation of all Thread classes.
        /// Other code from a derived class needs to be implemented in the overriden
        /// ThreadCodeDescendantClass method.
        /// </summary>
        private void ThreadEntryPoint()
        {
            // Add this thread to the collection of HLI threads currently executed.
            int hashCode = System.Threading.Thread.CurrentThread.GetHashCode();
            threads[hashCode] = this;

			this.dotNetThread.Name = this.threadOptions.Identifier;

			// Do an initial wait if necessary.
			if (this.initialMillisecondsToWait > 0)
			{
				if (TopmostThread != this)
				{
					TopmostThread.WriteInformation(String.Format("Waiting {0} milliseconds before starting DicomThread \"{1}\".", this.initialMillisecondsToWait.ToString(), this.threadOptions.Identifier));
				}

				System.Threading.Thread.Sleep(this.initialMillisecondsToWait);
			}

			if (!ResultsGatheringStarted)
			{
				StartResultsGathering();
			}

			// Now execute the actual testing code.
			try
			{
				BeforeCallingExecute();

				Execute();

				AfterCallingExecute();

				SetThreadState(ThreadState.Stopping);
			}
			catch (System.Exception exception)
			{
				BeforeHandlingException(exception);

				// If an exception was thrown, the ThreadState is still running.
				SetThreadState(ThreadState.Stopping);

				HandleExeption(exception);

				AfterHandlingException(exception);
			}
			finally
			{
				BeforeWaitingForCompletionChildThreads();

				WaitForCompletionChildThreads();

				AfterWaitingForCompletionChildThreads();

				if (ResultsGatheringStarted)
				{
					StopResultsGathering();
				}

				SetThreadState(ThreadState.Stopped);

				if (this.threadOptions.ShowResults)
				{
					ShowResults();
				}
			}

            // Remove this thread from the collection of HLI threads currently executed because it
            // is going to end now.
            threads.Remove(hashCode);
        }

        /// <summary>
        /// Call this method when this object should output an error.
        /// 
        /// Must be called by a descendant of this class when some error output is available.
        /// </summary>
        /// <param name="text">The error text.</param>
        internal protected void TriggerErrorOutputEvent(String text)
        {
            if (ErrorOutputEvent != null)
            {
                ErrorOutputEvent(this, text);
            }
        }

        /// <summary>
        /// Call this method when this object should output information.
        /// 
        /// Must be called by a descendant of this class when some information output is available.
        /// </summary>
        /// <param name="text">The information text.</param>
        internal protected void TriggerInformationOutputEvent(String text)
        {
            if (InformationOutputEvent != null)
            {
                InformationOutputEvent(this, text);
            }
        }

        /// <summary>
        /// Call this method when this object should output a warning.
        /// 
        /// Must be called by a descendant of this class when some warning output is available.
        /// </summary>
        /// <param name="text">The warning text.</param>
        internal protected void TriggerWarningOutputEvent(String text)
        {
            if (WarningOutputEvent != null)
            {
                WarningOutputEvent(this, text);
            }
        }

        /// <summary>
        /// Waits until this object has the ThreadState UnStarted or Stopped.
        /// </summary>
        public void WaitForCompletion()
        {
            bool wait = true;

            while (wait)
            {
                wait = false;

                ThreadState theThreadState = ThreadState;

                if ((this.threadState == ThreadState.Running) || (this.threadState == ThreadState.Stopping))
                {
                    wait = true;
                }

                if (wait)
                {
                    System.Threading.Thread.Sleep(500);
                }
            }
        }

        /// <summary>
        /// Waits until all child Threads are either unstarted or stopped.
        /// </summary>
        public void WaitForCompletionChildThreads()
        {
            bool wait = true;

            while (wait)
            {
                wait = false;

                lock (this.threadManager.ThreadManagerLock)
                {
                    foreach (Thread childThreads in this.childs)
                    {
                        if ((childThreads.ThreadState == ThreadState.Running) || (childThreads.ThreadState == ThreadState.Stopping))
                        {
                            wait = true;
                            break;
                        }
                    }
                }

                if (wait)
                {
                    System.Threading.Thread.Sleep(500);
                }
            }
        }

        /// <summary>
        /// Writes an error text to the results and triggers an ErrorOutputEvent.
        /// </summary>
        /// <remarks>
        /// When this instance is e.g. attached to a HliForm, the supplied error text will be
        /// displayed in this Form (the triggered ErrorOutputEvent is used for this).<br></br><br></br>
        /// 
        /// This method needs to be overridden in a descendant class.
        /// </remarks>
        /// <param name="text">The error text.</param>
        public abstract void WriteError(String text);

		/// <summary>
		/// If called within a HLI Thread, calls the WriteError method of this Thread.
		/// Otherwise, writes the text to the console.
		/// </summary>
		/// <param name="text">The error text.</param>
		public static void WriteErrorCurrentThread(String text)
		{
			Thread thread = CurrentThread;

			if (thread == null)
				// Not called from within a HLI thread. Log it to the console.
			{
				Console.WriteLine(text);
			}
			else
			{
				// Called from within a HLI thread. Log it in the thread.
				thread.WriteError(text);
			}
		}

        /// <summary>
        /// Writes an information text to the results and triggers an InformationOutputEvent.
        /// </summary>
        /// <remarks>
        /// When this instance is e.g. attached to a HliForm, the supplied information text will be
        /// displayed in this Form (the triggered InformationOutputEvent is used for this).<br></br><br></br>
        /// 
        /// This method needs to be overridden in a descendant class.
        /// </remarks>
        /// <param name="text">The information text.</param>
        public abstract void WriteInformation(String text);

		/// <summary>
		/// If called within a HLI Thread, calls the WriteInformation method of this Thread.
		/// Otherwise, writes the text to the console.
		/// </summary>
		/// <param name="text">The information text.</param>
		public static void WriteInformationCurrentThread(String text)
		{
			Thread thread = CurrentThread;

			if (thread == null)
				// Not called from within a HLI thread. Log it to the console.
			{
				Console.WriteLine(text);
			}
			else
			{
				// Called from within a HLI thread. Log it in the thread.
				thread.WriteInformation(text);
			}
		}

        /// <summary>
        /// Writes a warning text to the results and triggers a WarningOutputEvent.
        /// </summary>
        /// <remarks>
        /// When this instance is e.g. attached to a HliForm, the supplied warning text will be
        /// displayed in this Form (the triggered WarningOutputEvent is used for this).<br></br><br></br>
        /// 
        /// This method needs to be overridden in a descendant class.
        /// </remarks>
        /// <param name="text">The warning text.</param>
        public abstract void WriteWarning(String text);

		/// <summary>
		/// If called within a HLI Thread, calls the WriteWarning method of this Thread.
		/// Otherwise, writes the text to the console.
		/// </summary>
		/// <param name="text">The warning text.</param>
		public static void WriteWarningCurrentThread(String text)
		{
			Thread thread = CurrentThread;

			if (thread == null)
				// Not called from within a HLI thread. Log it to the console.
			{
				Console.WriteLine(text);
			}
			else
			{
				// Called from within a HLI thread. Log it in the thread.
				thread.WriteWarning(text);
			}
		}
	}
}
