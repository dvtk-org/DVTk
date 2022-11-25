// ------------------------------------------------------
// Original author: Marco Kemper
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



namespace DvtkHighLevelInterface.Common.UserInterfaces
{
	/// <summary>
	/// Represents a HLI User Interface that shows output events, from Threads attached to,
	/// on the console.
	/// </summary>
	public class HliConsole: IThreadUserInterface
	{	
		//
		// - Fields -
		//

		/// <summary>
		/// Event handler that can handle an error output event from a Thread.
		/// </summary>
		private Thread.TextOutputEventHandler errorOutputEventHandler = null;

		/// <summary>
		/// The identifier of the Thread, which output event has been handled the last time.
		/// If no output event has been handled yet, this contains String.Empty.
		/// </summary>
		private String identifierLastHandledThread = String.Empty;

		/// <summary>
		/// Event handler that can handle an information output event from a Thread.
		/// </summary>
		private Thread.TextOutputEventHandler informationOutputEventHandler = null;

		/// <summary>
		/// Event handler that can handle a warning output event from a Thread.
		/// </summary>
		private Thread.TextOutputEventHandler warningOutputEventHandler = null;



		//
		// - Constructors -
		//

		/// <summary>
		/// Default constructor.
		/// </summary>
		public HliConsole()
		{
			errorOutputEventHandler = new Thread.TextOutputEventHandler(this.HandleErrorOutputEvent);
			informationOutputEventHandler = new Thread.TextOutputEventHandler(this.HandleInformationOutputEvent);
			warningOutputEventHandler = new Thread.TextOutputEventHandler(this.HandleWarningOutputEvent);
		}



		//
		// - Methods -
		//

		/// <summary>
		/// Attach to a Thread.
        /// <br></br><br></br>
		/// By doing so, this object may react to information, warning and error output events
		/// from the Thread.
		/// </summary>
		/// <param name="thread">The Thread to attach to.</param>
		public void Attach(Thread thread)
		{
			thread.AttachedUserInterfaces.Add(this);
			thread.ErrorOutputEvent += this.errorOutputEventHandler;
			thread.InformationOutputEvent += this.informationOutputEventHandler;
			thread.WarningOutputEvent += this.warningOutputEventHandler;
		}

		/// <summary>
		/// Handle an error output event from a Thread by writing it to the console.
		/// </summary>
		/// <param name="thread">The Thread that generated the output event.</param>
		/// <param name="text">The text from the output event.</param>
		protected void HandleErrorOutputEvent(Thread thread, String text)
		{
			Write(thread, text, "Error");
		}

		/// <summary>
		/// Handle an information output event from a Thread by writing it to the console.
		/// </summary>
		/// <param name="thread">The Thread that generated the output event.</param>
		/// <param name="text">The text from the output event.</param>
		protected void HandleInformationOutputEvent(Thread thread, String text)
		{
			Write(thread, text, "Information");
		}

		/// <summary>
		/// Handle a warning output event from a Thread by writing it to the console.
		/// </summary>
		/// <param name="thread">The Thread that generated the output event.</param>
		/// <param name="text">The text from the output event.</param>
		protected void HandleWarningOutputEvent(Thread thread, String text)
		{
			Write(thread, text, "Warning");
		}

		/// <summary>
		/// Write text, from a Thread output event, to the console.
		/// </summary>
		/// <param name="thread">The Thread that generated the output event.</param>
		/// <param name="text">The text from the output event.</param>
		/// <param name="type">
		/// String that indicates if the is information, a warning or an error.
		/// </param>
		private void Write(Thread thread, String text, String type)
		{
			if (this.identifierLastHandledThread != thread.ThreadOptions.Identifier)
			{
				Console.WriteLine("-----");
				Console.WriteLine("----- [" + thread.ThreadOptions.Identifier + "]");
				this.identifierLastHandledThread = thread.ThreadOptions.Identifier;
			}

			Console.WriteLine("-----");
			Console.WriteLine("(" + type + ")");
			Console.WriteLine(text + "");
		}
	}
}
