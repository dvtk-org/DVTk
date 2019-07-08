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

using DvtkHighLevelInterface.Common.Threads;
using Thread = DvtkHighLevelInterface.Common.Threads.Thread;



namespace DvtkHighLevelInterface.Common.UserInterfaces
{
	/// <summary>
	/// Encapsulates a .Net Form on which High Level Interface Threads, that are attached to this
    /// instance, display their output events.
	/// </summary>
    /// <remarks>
    /// The actual .Net form is executed on a seperate thread. Because of this, any calls to this instance are
    /// not blocking on the .Net Form.
    /// <br></br><br></br>
    /// The encapsulated .Net Form also offers a Stop button by which all attached High Level 
    /// Interface threads may be stopped.
    /// </remarks>
	public class HliForm: IThreadUserInterface
	{
		//
		// - Fields -
		//

		/// <summary>
		/// See property AutoExit. 
        /// Do not use this field outside the property AutoExit because of the locking applied.
        /// </summary>
		private bool autoExit = true;

		/// <summary>
		/// Use this to synchronize the reading and writing of the autoExit field.
		/// </summary>
        private System.Threading.ReaderWriterLock autoExitLock = new System.Threading.ReaderWriterLock();

		/// <summary>
		/// See property Closed.
        /// Do not use this field outside the property Closed because of the locking applied.
		/// </summary>
		private bool closed = false;

		/// <summary>
		/// Use this to synchronize the reading and writing of the closed field.
		/// </summary>
        private System.Threading.ReaderWriterLock closedLock = new System.Threading.ReaderWriterLock();

		/// <summary>
		/// The thread in which the "actual" .Net form will be started. 
		/// </summary>
		private System.Threading.Thread dotNetThread = null;

		/// <summary>
		/// The "actual" .Net form that is displayed.
		/// </summary>
		private HliInternalForm hliInternalForm = null;

		/// <summary>
		/// Use this to synchronize the reading and writing of the hliInternalForm field.
		/// </summary>
        private System.Threading.ReaderWriterLock hliInternalFormLock = new System.Threading.ReaderWriterLock();

		/// <summary>
		/// Used to store a singleton if needed by users of this class.
		/// </summary>
		private static HliForm singleton = null;

		/// <summary>
		/// Use this to synchronize the reading and writing of the singleton field.
		/// </summary>
		private static Object singletonLock = new Object();

        /// <summary>
        /// See property Text.
        /// </summary>
        private String text = null;



		//
		// - Constructors -
		//

		/// <summary>
		/// Default constructor.
		/// </summary>
		public HliForm()
		{
			// Create and display the actual .Net form in a seperate thread.
			this.dotNetThread = new System.Threading.Thread(new System.Threading.ThreadStart(this.ThreadEntryPoint));
			this.dotNetThread.Start();
		}



		//
		// - Properties -
		//

		/// <summary>
        /// Gets or sets a boolean indicating if the encapsulated .Net Form must automatically be closed when the total
        /// number of running and stopping Threads becomes 0.
		/// </summary>
		public bool AutoExit
		{
			get
			{
                bool autoExitCopy = false;

                this.autoExitLock.AcquireReaderLock(-1);
                autoExitCopy = this.autoExit;
                this.autoExitLock.ReleaseReaderLock();

                return (autoExitCopy);
			}
			set
			{
                this.autoExitLock.AcquireWriterLock(-1);
				this.autoExit = value;
				this.autoExitLock.ReleaseWriterLock();
			}
		}

        /// <summary>
        /// Gets or sets a boolean indicating if the encapsulated .Net Form has been closed.
        /// </summary>
		internal bool Closed
		{
			get
			{
                bool closedCopy = false;

                this.closedLock.AcquireReaderLock(-1);
                closedCopy = this.closed;
                this.closedLock.ReleaseReaderLock();

                return (closedCopy);
			}
			set
			{
                this.closedLock.AcquireWriterLock(-1);
				this.closed = value;
                this.closedLock.ReleaseWriterLock();
			}
		}

        /// <summary>
        /// Gets or sets the text (caption) of the encapsulated .Net form.
        /// </summary>
        public String Text
        {
            get 
            { 
                return this.text; 
            }
            set 
            { 
                this.text = value;
                WaitForHliInternalFormConstructed();


                // When the call to WaitForHliInternalFormConstructed() is completed, the field
                // this.hliInternalForm will not change anymore, so no lock required anymore.
                this.hliInternalForm.ActionQueue.Enqueue(new HliInternalForm.SetTextAction(value));
            }
        }



		//
		// - Methods -
		//

		/// <summary>
		/// Attach to a Thread.
        /// <br></br><br></br>
		/// By doing so, this object may react to information, warning and error output events
		/// from the Thread.
        /// <br></br><br></br>
		/// Threads may not be already started when attaching.
		/// </summary>
		/// <param name="thread">The Thread to attach to.</param>
		public void Attach(Thread thread)
		{
			thread.AttachedUserInterfaces.Add(this);

			// Wait until the internal Form used has been created in a seperate thread and
			// attach the Thread to it.
			WaitForHliInternalFormConstructed();


            // When the call to WaitForHliInternalFormConstructed() is completed, the field
            // this.hliInternalForm will not change anymore, so no lock required anymore.
			this.hliInternalForm.Attach(thread);
		}

        /// <summary>
        /// Clears all activity logging that is present in the encapsulated .Net Form.
        /// </summary>
        public void ClearActivityLogging()
        {
            WaitForHliInternalFormConstructed();

            // When the call to WaitForHliInternalFormConstructed() is completed, the field
            // this.hliInternalForm will not change anymore, so no lock required anymore.
            this.hliInternalForm.UserControlActivityLogging.AddClearActionToQueue();
        }

		/// <summary>
		/// When outside this class, there is a need to use one HliForm instance in different places,
		/// this method may provide this single instance.
		/// </summary>
		/// <returns></returns>
		public static HliForm GetSingleton()
		{
			lock(singletonLock)
			{
				if (singleton == null)
				{
					singleton = new HliForm();
				}

				return(singleton);
			}
		}
		
		/// <summary>
		/// Reset the singleton, i.e. when the GetSingleton method is called afterwards, a new
		/// HliForm object will be returned.
		/// </summary>
		public static void ResetSingleton()
		{
			lock(singletonLock)
			{
				singleton = null;
			}
		}

        /// <summary>
        /// Determines how much logging in the activity logging may be present.
        /// </summary>
        /// <param name="maximumNumberOfLines">
        /// Maximum number of lines displayed in this instance before before the oldest lines will
        /// be removed.</param>
        /// <param name="keepNumberOfLines">
        /// Number of newest lines that will be kept when the oldest lines are removed.
        /// </param>
        public void SetNumberOfLinesActivityLogging(UInt32 maximumNumberOfLines, UInt32 keepNumberOfLines)
        {
            WaitForHliInternalFormConstructed();

            // When the call to WaitForHliInternalFormConstructed() is completed, the field
            // this.hliInternalForm will not change anymore, so no lock required anymore.
            this.hliInternalForm.UserControlActivityLogging.SetNumberOfLines(maximumNumberOfLines, keepNumberOfLines);
        }

		/// <summary>
		/// The entry point of the .Net thread, in which the .Net form is started.
		/// </summary>
		private void ThreadEntryPoint()
		{
			System.Threading.Thread.CurrentThread.Name = "HliInternalForm";

            this.hliInternalFormLock.AcquireWriterLock(-1);
			this.hliInternalForm = new HliInternalForm(this);
            this.hliInternalFormLock.ReleaseWriterLock();

			// Show the form and wait until it is closed.
			this.hliInternalForm.ShowDialog();
		}

		/// <summary>
		/// Waits until the encapsulated .Net Form has been constructed.
		/// </summary>
		private void WaitForHliInternalFormConstructed()
		{
			bool wait = true;

			while (wait)
			{
                this.hliInternalFormLock.AcquireReaderLock(-1);
				wait = (this.hliInternalForm == null);
                this.hliInternalFormLock.ReleaseReaderLock();

				if (wait)
				{
					System.Threading.Thread.Sleep(100);
				}
			}
		}

        /// <summary>
        /// Waits until the encapsulated .Net Form is closed.
        /// </summary>
		public void WaitUntilClosed()
		{
			bool wait = true;

			while (wait)
			{
				if (Closed)
				{
					wait = false;
				}
				else
				{
					System.Threading.Thread.Sleep(250);
				}
			}
		}
	}
}
