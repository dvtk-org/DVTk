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

using Dvtk.Hl7;
using Dvtk.Hl7.Messages;
using Dvtk.IheActors.Bases;
using Dvtk.IheActors.Actors;
using DvtkHighLevelInterface.Hl7.Threads;

namespace Dvtk.IheActors.Hl7
{
	/// <summary>
	/// Summary description for Hl7ThreadForHl7Server.
	/// </summary>
	public class Hl7ThreadForHl7Server: Hl7Thread
	{
		private Hl7Server _hl7Server = null;

		/// <summary>
		/// Class Constructor.
		/// </summary>
		/// <param name="hl7Server">HL7 Server.</param>
		public Hl7ThreadForHl7Server(Hl7Server hl7Server)
		{
			_hl7Server = hl7Server;
		}

		protected override void Execute()
		{
			// Process the HL7 requests and responses.
			_hl7Server.ProcessMessages();
		}
	}
}
