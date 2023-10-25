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



namespace DvtkHighLevelInterface.Common.Threads
{
	/// <summary>
	/// Represents the different states of a Thread instance.
	/// </summary>
	public enum ThreadState
	{
        /// <summary>
        /// Start has not yet been called.
        /// </summary>
		UnStarted,
        /// <summary>
        /// Start has been called.
        /// </summary>
		Running,
        /// <summary>
        /// Execution is now in the final block of the ThreadEntryPoint method,
        /// or the Stop method has been called and execution of the thread has
        /// still not not finished.
        /// </summary>
		Stopping,
        /// <summary>
        /// Execution of the thread has finished.
        /// </summary>
		Stopped
	}
}
