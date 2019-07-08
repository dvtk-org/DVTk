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
	/// Summary description for Semaphore.
	/// </summary>
	public class Semaphore
	{
		private System.Collections.Queue _queue = null;
		private uint _pollInterval = 0; // milliseconds

		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="pollInterval">Millisecond interval to sleep between polls.</param>
		public Semaphore(uint pollInterval)
		{
            // Set up the semaphore
            _queue = System.Collections.Queue.Synchronized(new System.Collections.Queue());

            // Ensure that we have a sensible minimum poll interval
			if (pollInterval < 10)
			{
				pollInterval = 10;
			}
			_pollInterval = pollInterval;
		}

		/// <summary>
		/// Wait for the semaphore to be signalled.
		/// </summary>
        /// <param name="timeout">Millisecond time-out - return if time-out expires</param>
        /// <returns>bool - returns true if semaphore signalled within time-out or false otherwise.</returns>
        public bool Wait(uint timeout)
		{
            // Ensure that we have a sensible minimum timeout
            // 0 = no timeout
            if ((timeout != 0) &&
                (timeout < _pollInterval))
            {
                // timeout must be at least 1 poll interval
                timeout = _pollInterval;
            }
            uint localTimeout = timeout;

            bool signalled = true;
			while (_queue.Count == 0)
			{
				System.Threading.Thread.Sleep((int)_pollInterval);

                if (timeout != 0)
                {
                    // Decrement the time-out
                    localTimeout -= _pollInterval;
                    if (localTimeout <= 0)
                    {
                        // If timeout expired then return false
                        return false;
                    }
                }
			}
			_queue.Dequeue();

            return signalled;
		}

		/// <summary>
		/// Signal the semaphore.
		/// </summary>
		public void Signal()
		{
			int signal = 1;
			_queue.Enqueue(signal);
		}
	}
}
