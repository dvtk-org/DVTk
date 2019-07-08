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
using System.IO;

using DvtkHighLevelInterface.Common.Messages;



namespace DvtkHighLevelInterface.Common.Threads
{
	/// <summary>
	/// The ThreadManager holds the overview of a collection of Threads, some of which are directly
	/// linked to the ThreadManager as the childThreads, other that are indirectly linked. Each Thread
	/// must directly or indirectly be linked to an instance of a ThreadManager.
	/// </summary>
	public class ThreadManager
	{
		//
		// - Fields -
		//

		/// <summary>
		/// See property ChildThreads.
		/// </summary>
		private ThreadCollection childThreads = null;

		/// <summary>
		/// All messages that have been send or received by the Threads contained in this ThreadManager. 
		/// When needed, messages may be removed using the RemoveMessage or ClearMessages methods.
		/// </summary>
		private MessageCollection messages = new MessageCollection();

		/// <summary>
		/// See property MessageLock.
		/// </summary>
		private Object messageLock = new Object();

		/// <summary>
		/// See property ThreadManagerLock.
		/// </summary>
		private Object threadManagerLock = new Object();

		/// <summary>
		/// See property Threads.
		/// </summary>
		private ThreadCollection threads = null;

		/// <summary>
		/// Contains for each ThreadState the number of Threads in that state.
		/// </summary>
		private int[] threadStatesCount = new int[4];



		//
		// - Delegates -
		//

		/// <summary>
		/// The delegate used for the ThreadsStateChangeEvent.
		/// </summary>
		public delegate void ThreadsStateChangeEventHandler(Thread thread, ThreadState oldThreadState, ThreadState newThreadState, int numberOfUnStarted, int numberOfRunning, int numberOfStopping, int numberOfStopped);



		//
		// - Events -
		//

		/// <summary>
		/// Event is fired when the state of one of the Threads managed by this ThreadManager changes.
		/// </summary>
		public event ThreadsStateChangeEventHandler ThreadsStateChangeEvent;



		//
		// - Constructors -
		//

		/// <summary>
		/// Default coonstructor.
		/// </summary>
		public ThreadManager()
		{
			this.childThreads = new ThreadCollection();
			this.threads = new ThreadCollection();

			threadStatesCount[(int)ThreadState.UnStarted] = 0;
			threadStatesCount[(int)ThreadState.Running] = 0;
			threadStatesCount[(int)ThreadState.Stopping] = 0;
			threadStatesCount[(int)ThreadState.Stopped] = 0;
		}



		//
		// - Properties -
		//

		/// <summary>
		/// Get the collection of child Threads.
		/// </summary>
		internal ThreadCollection ChildThreads
		{
			get
			{
				return(this.childThreads);
			}
		}

		/// <summary>
		/// Use this lock when accessing the messages of the ThreadManager and Thread.
		/// Do this access in one step.
		/// </summary>
		internal Object MessageLock
		{
			get
			{
				return(this.messageLock);
			}
		}

		/// <summary>
		/// Get a copy of the list of received and send messages of the contained Threads. 
		/// Note that received and send messages may have been removed from this list using the
		/// RemoveMessage or ClearMessages methods of the contained Threads.
		/// </summary>
		public MessageCollection Messages
		{
			get
			{
				MessageCollection copyOfMessageList = new MessageCollection();

				lock(this.messageLock)
				{
					foreach(Message message in this.messages)
					{
						copyOfMessageList.Add(message);
					}
				}

				return(copyOfMessageList);
			}
		}

		/// <summary>
		/// Always use this lock in the ThreadManager and Thread (descendant) classes
		/// for the complete set of access to:
		/// - Thread State changes of Threads of this ThreadManager.
		/// - Childs of a Thread.
		/// - Childs of the ThreadManager.
		/// - Threads of the ThreadManager.
		/// </summary>
		internal Object ThreadManagerLock
		{
			get
			{
				return(this.threadManagerLock);
			}
		}

		/// <summary>
		/// Get the collection of all Threads managed by this ThreadManager.
		/// </summary>
		internal ThreadCollection Threads
		{
			get
			{
				return(this.threads);
			}
		}



		//
		// - Methods -
		//

		/// <summary>
		/// Add a child Thread to this ThreadManager.
		/// May only be called from one of the Thread.Initialize methods.
		/// </summary>
		/// <param name="thread">The Thread to add.</param>
		internal void AddChildThread(Thread thread)
		{
			this.childThreads.Add(thread);
			this.threads.Add(thread);
			this.threadStatesCount[(int)thread.ThreadState]++;
		}

		/// <summary>
		/// This method must be called when a message is received or send by a Thread contained
		/// in this ThreadManager.
		/// </summary>
		/// <param name="message"></param>
		internal void AddMessage(Message message)
		{
			lock(this.messageLock)
			{
				this.messages.Add(message);
			}
		}

		/// <summary>
		/// Add a Thread to this ThreadManager.
		/// May only be called from one of the Thread.Initialize methods.
		/// </summary>
		/// <param name="thread">The Thread to add.</param>
		internal void AddThread(Thread thread)
		{
			this.threads.Add(thread);
			this.threadStatesCount[(int)thread.ThreadState]++;
		}
	
		/// <summary>
		/// Use this method to determine a unique Identifier for a Thread, when the
		/// Identifier has not been explicitly set. Uniqueness can only be guaranteed
		/// for the Identifier returned for the Threads managed by this ThreadManager.
		/// </summary>
		/// <param name="thread">The Thread for which to create a unique identifier.</param>
		/// <returns>The unique identifier.</returns>
		internal String GetUniqueIdentifier(Thread thread)
		{
			String uniqueIdentifier = "";

			// Use the threadManagerLock because we are accessing the threads collection in
			// the IdentifierExists method.
			lock(this.threadManagerLock)
			{
				int index = 1;

				while (IdentifierExists(thread.ThreadOptions.Name + index.ToString()))
				{
					index++;
				}

				uniqueIdentifier = thread.ThreadOptions.Name + index.ToString();
			}

			return (uniqueIdentifier);
		}

		/// <summary>
		/// Returns a boolean indicating if the supplied Identifier is unique.
		/// </summary>
		/// <param name="identifier">The Identifier.</param>
		/// <returns>Indicates if it is unique.</returns>
		private bool IdentifierExists(String identifier)
		{
			bool identifierExists = false;

			foreach (Thread thread in this.threads)
			{	
				if (thread.ThreadOptions.Identifier != null)
				{
					if (thread.ThreadOptions.Identifier == identifier)
					{
						identifierExists = true;
						break;
					}
				}
			}

			return (identifierExists);
		}

		/// <summary>
		/// Remove a message fro the list maintained by the ThreadManager. This method may only
		/// be called from a Thread (descendant) class.
		/// </summary>
		/// <param name="message"></param>
		internal void RemoveMessage(Message message)
		{
			lock(this.messageLock)
			{
				this.messages.Remove(message);
			}
		}

		/// <summary>
		/// Stop all Threads that are managed by this ThreadManager.
		/// </summary>
		public void Stop()
		{
			lock(this.threadManagerLock)
			{
				foreach(Thread childThread in this.childThreads)
				{
					childThread.Stop();
				}
			}
		}

		/// <summary>
		/// This method may only be called from the Thread class. It method call
		/// signals that the ThreadState of the supplied Thread has changed.
		/// </summary>
		/// <param name="oldThreadState">The old ThreadState.</param>
		/// <param name="newThreadState">The new ThreadState.</param>
		internal void ThreadStateChanged(ThreadState oldThreadState, ThreadState newThreadState)
		{
			this.threadStatesCount[(int)oldThreadState]--;
			this.threadStatesCount[(int)newThreadState]++;

		}

		internal void TriggerThreadsStateChangeEvent(Thread thread, ThreadState oldThreadState, ThreadState newThreadState)
		{
			if (ThreadsStateChangeEvent != null)
			{
				ThreadsStateChangeEvent(
					thread, oldThreadState, 
					newThreadState, 
					this.threadStatesCount[(int)ThreadState.UnStarted], 
					this.threadStatesCount[(int)ThreadState.Running], 
					this.threadStatesCount[(int)ThreadState.Stopping], 
					this.threadStatesCount[(int)ThreadState.Stopped]);
			}
		}

		/// <summary>
		/// Wait until all child Threads are either unstarted or stopped.
		/// </summary>
		public void WaitForCompletionThreads()
		{
			bool wait = true;

			while(wait)
			{
				wait = false;

				lock(this.threadManagerLock)
				{
					foreach(Thread childThreads in this.childThreads)
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
	}
}
