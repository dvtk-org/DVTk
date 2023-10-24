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



namespace DvtkHighLevelInterface.Dicom.Threads
{
	/// <summary>
	/// This abstract class contains the base functionality to implement a DicomThread that, once
	/// started, will do things when triggered from the outside.
	/// 
	/// The mechanism used for triggering is the polling mechanism. This object will use an external
	/// queue to determine when something needs to be done.
	/// 
	/// The abstract ProcessTrigger method needs to be overriden to determine what needs
	/// to be done when this object received a trigger.
	/// </summary>
	public abstract class DicomThreadPollingLoop: DicomThread
	{
		//
		// - Fields -
		//

		/// <summary>
		/// Indicates if looping needs to continue. Is set to false when the
		/// Stop method is called.
		/// </summary>
		private bool keepLooping = true;

		/// <summary>
		/// See property LoopDelay.
		/// </summary>
		private int loopDelay = 500;



		//
		// - Constructors -
		//

		public DicomThreadPollingLoop()
		{
			// Do nothing.
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

		/// <summary>
		/// This method implements the loop the the class.
		/// </summary>
		protected override void Execute()
		{
			// Calling the Stop method while break through this loop.
			while (this.keepLooping)
			{
				// Delay before checking if a new triggers are available
				System.Threading.Thread.Sleep(this.loopDelay);

				Object trigger = null;

				// Get the trigger from the overridden GetTrigger()
				while ((trigger = GetTrigger()) != null)
				{
					BeforeProcessTrigger(trigger);

					// Process the trigger
					ProcessTrigger(trigger);

					AfterProcessTrigger(trigger);

					CheckForNewResultsFile();
							
					// Wait awhile before processing new Trigger
					System.Threading.Thread.Sleep(500);
				}
			}
		}

		/// <summary>
		/// In a descendant class, this method must to be overriden to get a new trigger
		/// from some external queue.
		/// </summary>
		/// <returns>
		/// The next trigger from the external queue. 
		/// If the external queue is empty, the null value must be returned.
		/// </returns>
		protected abstract Object GetTrigger();

		/// <summary>
		/// Override this method to determine what should happen when a trigger is processed in the loop.
		/// 
		/// E.g. when the trigger is of type DicomMessage, send a DicomMessage to a SCP.
		/// </summary>
		/// <param name="trigger">The trigger.</param>
		public abstract void ProcessTrigger(Object trigger);

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






	}
}
