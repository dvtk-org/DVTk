// ------------------------------------------------------
// DVTk - The Healthcare Validation Toolkit (www.dvtk.org)
// Copyright � 2009 DVTk
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

using Dvtk.IheActors.Bases;

namespace Dvtk.IheActors.UserInterfaces
{
	/// <summary>
	/// Summary description for Console.
	/// </summary>
    public class Console : DvtkHighLevelInterface.Common.UserInterfaces.HliConsole, IActorUserInterface, IIheFrameworkUserInterface
	{
		public void Attach(BaseActor actor)
		{
			actor.Attach(this);
		}

        public void Attach(Dvtk.IheActors.IheFramework.IheFramework iheFramework)
		{
            iheFramework.Attach(this);
		}
	}
}
