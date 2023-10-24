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

using DvtkHighLevelInterface.Common.Threads;



namespace DvtkHighLevelInterface.Common.UserInterfaces
{
	/// <summary>
	/// Abstract base class for all classes that implement a HLI User Interface.
	/// 
	/// A HLI User Interface at least shows output events and shows instructions 
	/// as a result of Threads calling ShowAndContinue from Threads attached to.
	/// </summary>
	public abstract class UserInterface
	{
		//
		// - Constructors -
		//

		/// <summary>
		/// Default constructor.
		/// </summary>
		public UserInterface()
		{
		}



		//
		// - Methods -
		//

		/// <summary>
		/// Attach to a Thread.
		/// 
		/// By doing so, this object may react to information, warning and error output events
		/// from the Thread.
		/// </summary>
		/// <param name="thread">The Thread to attach to.</param>
		public virtual void Attach(Thread thread)
		{
			thread.AttachedUserInterfaces.Add(this);
		}
	}
}
