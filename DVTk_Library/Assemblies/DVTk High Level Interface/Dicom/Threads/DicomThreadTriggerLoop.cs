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



namespace DvtkHighLevelInterface.Dicom.Threads
{
	/// <summary>
	/// This abstract class contains the base functionality to implement a DicomThread that, once
	/// started, will do things when triggered from the outside.
	/// 
	/// The mechanism used for triggering is calling the Trigger method.
	///   
	/// The abstract ProcessTrigger method needs to be overriden to determine what needs
	/// to be done when this object received a trigger.
	/// </summary>
	public abstract class DicomThreadTriggerLoop: DicomThread
	{
		//
		// - Fields -
		//

		/// <summary>
		/// Indicates if the last Trigger method call has been processed.
		/// </summary>
		private bool hasLastTriggerCallBeenProcessed = true;

		/// <summary>
		/// Lock object to safeguard the hasLastTriggerCallBeenProcessed field.
		/// </summary>
		private Object hasLastTriggerCallBeenProcessedLock = new Object();

		/// <summary>
		/// Indicates if looping needs to continue. Is set to false when the
		/// Stop method is called.
		/// </summary>
		private bool keepLooping = true;

		/// <summary>
		/// See property LoopDelay.
		/// </summary>
		private int loopDelay = 500;

		/// <summary>
		/// Used to store the triggers that are supplied when the Trigger method is called.
		/// </summary>
		private System.Collections.Queue triggerQueue = null;



		//
		// - Constructors -
		//

		/// <summary>
		/// Constructor.
		/// </summary>
		public DicomThreadTriggerLoop()
		{
			triggerQueue = System.Collections.Queue.Synchronized(new System.Collections.Queue());
		}



		//
		// - Properties -
		//

		/// <summary>
		/// Delay before checking if unprocessed triggers are available.
		/// </summary>
		public int LoopDelay
		{
			get
			{
				return(this.loopDelay);
			}
			set
			{
				this.loopDelay = value;
			}
		}



		//
		// - Methods -
		//

		/// <summary>
		/// May be overriden to implement things that need to be performed after
		/// processing a trigger.
		/// </summary>
		/// <param name="trigger">The trigger that will be processed.</param>
		public virtual void AfterProcessTrigger(Object trigger)
		{
		}

		/// <summary>
		/// May be overriden to implement things that need to be performed before
		/// processing a trigger.
		/// </summary>
		/// <param name="trigger">The trigger that will be processed.</param>
		public virtual void BeforeProcessTrigger(Object trigger)
		{
		}

		private bool resultsFilePerTrigger = false;

        /// <summary>
        /// Gets or sets a boolean indicating if seperate results will be written for each trigger.
        /// </summary>
		public bool ResultsFilePerTrigger
		{
			get
			{
				return(this.resultsFilePerTrigger);
			}
			set
			{
				this.resultsFilePerTrigger = value;
			}
		}

		/// <summary>
		/// This method implements the loop the the class.
		/// </summary>
		protected override void Execute()
		{
			// Calling the Stop method while break through this loop.
			while (this.keepLooping)
			{
				// Delay before checking if a new triggers are available.
				System.Threading.Thread.Sleep(this.loopDelay);

				// Check if anything has been queued
				while (this.triggerQueue.Count != 0)
				{
					// Get the trigger
					Object trigger = this.triggerQueue.Dequeue();

					if (trigger != null)
					{
						BeforeProcessTrigger(trigger);

						// Process the trigger
						ProcessTrigger(trigger);

						AfterProcessTrigger(trigger);

						if (this.resultsFilePerTrigger)
						{
							StopResultsGathering();
							StartResultsGathering();
						}

						// Wait awhile before processing new Trigger
						System.Threading.Thread.Sleep(500);
					}
				}

				lock(this.hasLastTriggerCallBeenProcessedLock)
				{
					if (this.triggerQueue.Count == 0)
					{
						this.hasLastTriggerCallBeenProcessed = true;
					}
				}
			}
		}

		/// <summary>
		/// Override this method to determine what should happen when a trigger is processed in the loop.
		/// 
		/// E.g. when the trigger is of type DicomMessage, send a DicomMessage to a SCP.
		/// </summary>
		/// <param name="trigger">The trigger.</param>
		protected abstract void ProcessTrigger(Object trigger);

		/// <summary>
		/// Stop the looping, and as a consequence, stop this DicomThread.
		/// </summary>
		public override void Stop()
		{
			this.keepLooping = false;

			// Give the loop in the Execute method time to stop.
			System.Threading.Thread.Sleep(1500);

			// In case the loop hasn't stopped by itself, stop the thread.
			base.Stop();
		}
	
		/// <summary>
		/// Put a new trigger at the end of the queue. FIFO is used as
		/// the processing order for the triggers.
		/// </summary>
		/// <param name="trigger">
		/// The actual trigger. In an overriden method ProcessTrigger of a descendant class,
		/// the actual implementation what to do with a trigger should be placed.
		/// </param>
		protected void Trigger(Object trigger)
		{
			lock(this.hasLastTriggerCallBeenProcessedLock)
			{
				this.hasLastTriggerCallBeenProcessed = false;
				this.triggerQueue.Enqueue(trigger);
			}
		}

		/// <summary>
		/// Wait until all trigger calls have been processed.
		/// </summary>
		public void WaitForLastTriggerCallProcessed()
		{
			bool wait = true;

			while (wait)
			{
				lock(this.hasLastTriggerCallBeenProcessedLock)
				{
					if (this.hasLastTriggerCallBeenProcessed == true)
					{
						wait = false;
					}

					// If the thread state is stopped, we may use the
					// HasExceptionOcurred property.
					if (ThreadState == ThreadState.Stopped)
					{
						if (HasExceptionOccured)
						{
							wait = false;
						}
					}
				}

				if (wait)
				{
					System.Threading.Thread.Sleep(500);
				}
			}
		}
	}
}
