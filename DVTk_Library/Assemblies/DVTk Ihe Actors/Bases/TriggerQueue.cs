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

namespace Dvtk.IheActors.Bases
{
	/// <summary>
	/// Summary description for TriggerQueue.
	/// </summary>
	public class TriggerQueue
	{
		private System.Collections.Queue _queue = null;
 
		/// <summary>
		/// Class constructor.
		/// </summary>
		public TriggerQueue()
		{
			_queue = System.Collections.Queue.Synchronized(new System.Collections.Queue());
		}

		/// <summary>
		/// Enqueue a Trigger message.
		/// </summary>
		/// <param name="trigger">Trigger message.</param>
		public void Enqueue(BaseTrigger trigger)
		{
			_queue.Enqueue(trigger);
		}

		/// <summary>
		/// Dequeue a Trigger message.
		/// </summary>
		/// <returns>Trigger message.</returns>
		public BaseTrigger Dequeue()
		{
			BaseTrigger trigger = (BaseTrigger)_queue.Dequeue();
			return trigger;
		}

		/// <summary>
		/// Test if queue is empty.
		/// </summary>
		/// <returns>bool - true / false - queue empty.</returns>
		public bool IsEmpty()
		{
			bool isEmpty = (_queue.Count > 0) ? false : true;
			return isEmpty;
		}
	}
}
